﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiliconStudio.Paradox.Shaders.Parser.Ast;
using SiliconStudio.Shaders.Ast;
using SiliconStudio.Shaders.Ast.Hlsl;
using SiliconStudio.Shaders.Utility;
using SiliconStudio.Shaders.Visitor;
using SiliconStudio.Shaders.Writer;

namespace SiliconStudio.Paradox.Shaders.Parser.Mixins
{
    /// <summary>
    /// This class is responsible to generate associated C# code from an effect file (extension: pdxfx).
    /// </summary>
    public class ShaderMixinCodeGen : ShaderKeyGeneratorBase//ShaderWriter
    {
        private const string DefaultNameSpace = "SiliconStudio.Paradox.Effects.Modules";

        private const string BlockContextTag = "BlockContextTag";
        private readonly LoggerResult logging;
        private readonly Shader shader;
        private ShaderBlock currentBlock;
        private int localVariableCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderMixinCodeGen" /> class.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <param name="logging">The logging.</param>
        /// <exception cref="System.ArgumentNullException">shader or logging</exception>
        /// <exception cref="System.InvalidOperationException">Cannot process shaders having already parsing errors</exception>
        public ShaderMixinCodeGen(Shader shader, LoggerResult logging)
        {
            if (shader == null)
                throw new ArgumentNullException("shader");

            if (logging == null)
                throw new ArgumentNullException("logging");

            this.shader = shader;
            this.logging = logging;
            EnablePreprocessorLine = true;
        }

        /// <summary>
        /// Generates the csharp code from a pdxfx file.
        /// </summary>
        /// <param name="pdxfxShaderCode">The PDXFX shader code.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static string GenerateCsharp(string pdxfxShaderCode, string filePath)
        {
            // Compile
            var shader = ParadoxShaderParser.PreProcessAndParse(pdxfxShaderCode, filePath);

            // Try to generate a mixin code.
            var loggerResult = new LoggerResult();
            var shaderKeyGenerator = new ShaderMixinCodeGen(shader, loggerResult);

            if (shaderKeyGenerator.Run())
            {
                return shaderKeyGenerator.Text;
            }
            throw new InvalidOperationException(loggerResult.ToString());
        }

        /// <summary>
        /// Runs the code generation. Results is accessible from <see cref="ShaderWriter.Text"/> property.
        /// </summary>
        public override bool Run()
        {
            // If there are any errors, report them in the file as well
            // but return immediately as we can't really process the shader object
            if (logging.HasErrors)
            {
                LogErrors();
                return false;
            }

            // Add namespace for shader class type
            FixShaderClassTypeWithNoNameSpace();

            var blockVisitor = new ShaderBlockVisitor(this, logging);
            blockVisitor.Run(shader);

            // If there are any errors, generated by the visitor report them immediately
            if (logging.HasErrors)
            {
                LogErrors();
                return false;
            }

            WriteLine("// <auto-generated>");
            WriteLine("// Do not edit this file yourself!");
            WriteLine("//");
            WriteLine("// This code was generated by Paradox Shader Mixin Code Generator.");
            WriteLine("// To generate it yourself, please install SiliconStudio.Paradox.VisualStudio.Package .vsix");
            WriteLine("// and re-save the associated .pdxfx.");
            WriteLine("// </auto-generated>");
            WriteLine();

            // No mixin found, just return
            if (!blockVisitor.HasMixin && !blockVisitor.HasShaderClassType)
            {
                WriteLine("// Nothing to generate");
                return true;
            }

            // Header of usings declaration
            // TODO: Should probably be better to use fully qualified name of types to avoid conflicts.

            WriteLine("using System;");
            WriteLine("using SiliconStudio.Core;");
            WriteLine("using SiliconStudio.Paradox.Effects;");
            WriteLine("using SiliconStudio.Paradox.Graphics;");
            WriteLine("using SiliconStudio.Paradox.Shaders;");
            WriteLine("using SiliconStudio.Core.Mathematics;");
            WriteLine("using Buffer = SiliconStudio.Paradox.Graphics.Buffer;");
            WriteLine();

            // Visit the shader and generate the code
            VisitDynamic(shader);

            // If there are any errors log them into the shader
            if (logging.HasErrors)
            {
                LogErrors();
                return false;
            }

            return true;
        }

        public override void Visit(AssignmentExpression assignmentExpression)
        {
            Identifier typeTarget;
            Identifier typeMember;
            if (TryParameters(assignmentExpression.Target, out typeTarget, out typeMember))
            {
                Write("context.SetParam(").Write(typeTarget).Write(".").Write(typeMember).Write(", ");
                VisitDynamic(assignmentExpression.Value);
                Write(")");
            }
            else
            {
                base.Visit(assignmentExpression);
            }
        }

        public override void Visit(MemberReferenceExpression memberReferenceExpression)
        {
            Identifier typeTarget;
            Identifier typeMember;
            if (TryParameters(memberReferenceExpression, out typeTarget, out typeMember))
            {
                Write("context.GetParam(").Write(typeTarget).Write(".").Write(typeMember).Write(")");
            }
            else
            {
                base.Visit(memberReferenceExpression);
            }
        }

        /// <summary>
        /// Visits the specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        [Visit]
        protected virtual void Visit(EnumType enumType)
        {
            Write("[DataContract]");
            WriteLinkLine(enumType);
            Write("public enum");
            Write(" ");
            Write(enumType.Name);
            WriteSpace();

            OpenBrace();

            foreach (Expression fieldDeclaration in enumType.Values)
            {
                WriteLinkLine(fieldDeclaration);
                VisitDynamic(fieldDeclaration);
                WriteLine(",");
            }

            CloseBrace(false).Write(";").WriteLine();
        }

        /// <summary>
        /// Visits the specified params block.
        /// </summary>
        /// <param name="paramsBlock">The params block.</param>
        [Visit]
        protected virtual void Visit(ParametersBlock paramsBlock)
        {
            Write("[DataContract]");
            WriteLinkLine(paramsBlock);
            Write("public partial class");
            Write(" ");
            Write(paramsBlock.Name);
            WriteSpace();
            Write(": ShaderMixinParameters");

            OpenBrace();

            foreach (DeclarationStatement parameter in paramsBlock.Body.Statements.OfType<DeclarationStatement>())
            {
                var variable = parameter.Content as Variable;
                if (variable == null)
                    continue;

                WriteLinkLine(parameter);
                VisitDynamic(variable);
            }

            CloseBrace(false).Write(";").WriteLine();
        }

        /// <summary>
        /// Visits the specified keyword expression.
        /// </summary>
        /// <param name="keywordExpression">The keyword expression.</param>
        [Visit]
        protected virtual void Visit(KeywordExpression keywordExpression)
        {
            // A discard will be transformed to 'return'
            if (keywordExpression.Name.Text == "discard")
            {
                Write("return");
            }
            else
            {
                base.Visit(keywordExpression);
            }
        }

        [Visit]
        protected virtual void Visit(ShaderClassType shader)
        {
            Write("public static partial class ");
            Write(shader.Name);
            Write("Keys");
            OpenBrace();
            foreach (var decl in shader.Members.OfType<Variable>())
            {
                VisitDynamic(decl);
            }
            CloseBrace();
        }

        [Visit]
        protected virtual void Visit(GenericType<ObjectType> type)
        {
            if (IsStringInList(type.Name, "StructuredBuffer", "RWStructuredBuffer", "ConsumeStructuredBuffer", "AppendStructuredBuffer"))
            {
                Write("Buffer");
            }
            ProcessInitialValueStatus = false;
        }

        /// <summary>
        /// Visits the specified for each statement.
        /// </summary>
        /// <param name="forEachStatement">For each statement.</param>
        [Visit]
        protected virtual void Visit(ForEachStatement forEachStatement)
        {
            WriteLinkLine(forEachStatement);

            if (forEachStatement.Variable == null)
            {
                localVariableCount++;

                Identifier parameterType;
                Identifier parameterMember;
                if (!TryParameters(forEachStatement.Collection, out parameterType, out parameterMember))
                {
                    Write(@"#error ""Unexpected parameter for 'foreach params' [");
                    VisitDynamic(forEachStatement.Collection);
                    WriteLine(@"]. Expecting single property access""");
                    return;
                }

                string variable = "____" + localVariableCount;
                Write("foreach(").Write("var ").Write(variable).Write(" in ");
                VisitDynamic(forEachStatement.Collection);
                WriteLine(")");

                var statement = forEachStatement.Body as BlockStatement;
                if (statement == null)
                {
                    statement = new BlockStatement {Span = forEachStatement.Body.Span};
                    statement.Statements.Add(forEachStatement.Body);
                }
                AddPushPopParameters(statement, parameterType, parameterMember, new VariableReferenceExpression(variable), forEachStatement.Span);

                VisitDynamic(statement);

                localVariableCount--;
            }
            else
            {
                Write("foreach(");
                VisitDynamic(forEachStatement.Variable);
                Write(" in ");
                VisitDynamic(forEachStatement.Collection);
                Write(")");
                WriteLine();
                VisitDynamic(forEachStatement.Body);
            }
        }

        /// <summary>
        /// Visits the specified shader block.
        /// </summary>
        /// <param name="shaderBlock">The shader block.</param>
        [Visit]
        protected virtual void Visit(ShaderBlock shaderBlock)
        {
            WriteLinkLine(shaderBlock);
            currentBlock = shaderBlock;

            VariableAsParameterKey = false;

            Write("internal partial class");
            Write(" ");
            Write(shaderBlock.Name);
            WriteSpace();
            Write(" : IShaderMixinBuilder");
            OpenBrace();

            // Generate the main generate method for each shader block
            Write("public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)");
            OpenBrace();

            // Create a context associated with ShaderBlock
            foreach (Statement statement in shaderBlock.Body.Statements)
            {
                VisitDynamic(statement);
            }
            CloseBrace();

            WriteLine();
            WriteLine("[ModuleInitializer]");
            WriteLine("internal static void __Initialize__()");
            OpenBrace();
            Write("ShaderMixinManager.Register(\"").Write(shaderBlock.Name).Write("\", new ").Write(shaderBlock.Name).WriteLine("());");
            CloseBrace();

            CloseBrace();

            VariableAsParameterKey = true;
            currentBlock = null;
        }

        /// <summary>
        /// Visits the specified mixin statement.
        /// </summary>
        /// <param name="mixinStatement">The mixin statement.</param>
        [Visit]
        protected virtual void Visit(MixinStatement mixinStatement)
        {
            Expression mixinName;
            AssignmentExpression assignExpression;
            var genericParameters = new List<Expression>();

            switch (mixinStatement.Type)
            {
                case MixinStatementType.Default:
                    ExtractGenericParameters(mixinStatement.Target, out mixinName, genericParameters);

                    WriteLinkLine(mixinStatement);
                    Write("context.Mixin(mixin, ");
                    WriteMixinName(mixinName);
                    WriteGenericParameters(genericParameters);
                    WriteLine(");");
                    break;

                case MixinStatementType.Child:
                    ExtractGenericParameters(mixinStatement.Target, out mixinName, genericParameters);

                    OpenBrace();
                    WriteLinkLine(mixinStatement);
                    Write("var __subMixin = new ShaderMixinSourceTree() { Name = ");
                    WriteMixinName(mixinName);
                    WriteLine(", Parent = mixin };");
                    WriteLine("mixin.Children.Add(__subMixin);");
                    
                    WriteLinkLine(mixinStatement);
                    WriteLine("context.BeginChild(__subMixin);");
                    
                    WriteLinkLine(mixinStatement);
                    Write("context.Mixin(__subMixin, ");
                    WriteMixinName(mixinName);
                    WriteGenericParameters(genericParameters);
                    WriteLine(");");

                    WriteLinkLine(mixinStatement);
                    WriteLine("context.EndChild();");

                    CloseBrace();
                    break;

                case MixinStatementType.Remove:
                    ExtractGenericParameters(mixinStatement.Target, out mixinName, genericParameters);

                    WriteLinkLine(mixinStatement);
                    Write("context.RemoveMixin(mixin, ");
                    WriteMixinName(mixinName);
                    if (genericParameters.Count > 0)
                    {
                        logging.Error("Removing with generic parameters is not supported", mixinStatement.Span);
                    }
                    WriteLine(");");
                    break;

                case MixinStatementType.Clone:
                    WriteLinkLine(mixinStatement);
                    WriteLine("context.CloneProperties();");

                    WriteLinkLine(mixinStatement);
                    WriteLine("mixin.Mixin.CloneFrom(mixin.Parent.Mixin);");
                    break;

                case MixinStatementType.Macro:
                    WriteLinkLine(mixinStatement);
                    var context = (ShaderBlockContext)currentBlock.GetTag(BlockContextTag);
                    assignExpression = mixinStatement.Target as AssignmentExpression;
                    Expression macroName;
                    Expression macroValue;

                    if (assignExpression != null)
                    {
                        macroName = assignExpression.Target;
                        if (macroName is VariableReferenceExpression)
                        {
                            macroName = new LiteralExpression(macroName.ToString());
                        }
                        macroValue = assignExpression.Value;
                    }
                    else
                    {
                        var variableReference = mixinStatement.Target as MemberReferenceExpression;
                        if (variableReference == null || !(variableReference.Target is VariableReferenceExpression) || !context.DeclaredParameters.Contains((((VariableReferenceExpression)variableReference.Target).Name.Text)))
                        {
                            logging.Error("Invalid syntax. Expecting: mixin macro Parameters.NameOfProperty or mixin macro nameOfProperty = value", mixinStatement.Span);
                            macroName = new LiteralExpression("#INVALID_MACRO_NAME");
                            macroValue = mixinStatement.Target;
                        }
                        else
                        {
                            macroName = new LiteralExpression(variableReference.Member.Text);
                            macroValue = mixinStatement.Target;
                        }
                    }

                    Write("mixin.Mixin.AddMacro(");
                    VisitDynamic(macroName);
                    Write(", ");
                    VisitDynamic(macroValue);
                    WriteLine(");");
                    break;

                case MixinStatementType.Compose:
                    assignExpression = mixinStatement.Target as AssignmentExpression;
                    if (assignExpression == null)
                    {
                        logging.Error("Expecting assign expression for composition", mixinStatement.Target.Span);
                        return;
                    }

                    var addCompositionFunction = "AddComposition";

                    // If it's a +=, let's create or complete a ShaderArraySource
                    if (assignExpression.Operator == AssignmentOperator.Addition)
                    {
                        addCompositionFunction = "AddCompositionToArray";
                    }

                    ExtractGenericParameters(assignExpression.Value, out mixinName, genericParameters);

                    OpenBrace();
                    WriteLinkLine(mixinStatement);
                    WriteLine("var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };");

                    WriteLinkLine(mixinStatement);
                    Write("context.Mixin(__subMixin, ");
                    WriteMixinName(mixinName);
                    WriteGenericParameters(genericParameters);
                    WriteLine(");");

                    Write("mixin.Mixin.");
                    Write(addCompositionFunction);
                    Write("(");
                    WriteMixinName(assignExpression.Target);
                    Write(", __subMixin.Mixin");
                    WriteLine(");");
                    CloseBrace();
                    break;
            }
        }

        /// <summary>
        /// Visits the specified using statement.
        /// </summary>
        /// <param name="usingStatement">The using statement.</param>
        [Visit]
        protected virtual void Visit(UsingStatement usingStatement)
        {
            WriteLinkLine(usingStatement);
            Write("using ").Write(usingStatement.Name).WriteLine(";");
        }

        /// <summary>
        /// Visits the specified using parameters statement.
        /// </summary>
        /// <param name="usingParametersStatement">The using parameters statement.</param>
        [Visit]
        protected virtual void Visit(UsingParametersStatement usingParametersStatement)
        {
            if (usingParametersStatement.Body == null)
                return;

            Identifier parameterType;
            Identifier parameterMember;
            if (!TryParameters(usingParametersStatement.Name, out parameterType, out parameterMember))
            {
                Write(@"#error ""Unexpected parameter for 'using params' [");
                VisitDynamic(usingParametersStatement.Name);
                WriteLine(@"]. Expecting single property access""");
                return;
            }

            AddPushPopParameters(usingParametersStatement.Body, parameterType, parameterMember, usingParametersStatement.Name, usingParametersStatement.Span);

            Visit(usingParametersStatement.Body);
        }

        private void AddPushPopParameters(BlockStatement blockStatement, Identifier parameterType, Identifier parameterMember, Expression paramValue, SourceSpan span)
        {
            var pushStatement = new ExpressionStatement(new MethodInvocationExpression(new MemberReferenceExpression(new VariableReferenceExpression("context"), "PushParameters"), paramValue)) {Span = span};
            var popStatement = new ExpressionStatement(new MethodInvocationExpression(new MemberReferenceExpression(new VariableReferenceExpression("context"), "PopParameters"))) {Span = span};
            blockStatement.Statements.Insert(0, pushStatement);
            ;
            blockStatement.Statements.Add(popStatement);
        }

        private bool TryParameters(Expression expression, out Identifier type, out Identifier member)
        {
            type = null;
            member = null;
            var memberReferenceExpression = expression as MemberReferenceExpression;
            if (memberReferenceExpression == null)
                return false;

            var name = memberReferenceExpression.Target as VariableReferenceExpression;

            bool foundDeclaredParameters = false;
            if (currentBlock != null)
            {
                var context = (ShaderBlockContext)currentBlock.GetTag(BlockContextTag);
                HashSet<string> usings = context.DeclaredParameters;

                if (name != null && usings.Contains(name.Name))
                {
                    type = name.Name;
                    member = memberReferenceExpression.Member;
                    foundDeclaredParameters = true;
                }
            }

            return foundDeclaredParameters;
        }

        private void ExtractGenericParameters(Expression expression, out Expression mixinName, List<Expression> genericParametersOut)
        {
            if (genericParametersOut == null)
            {
                throw new ArgumentNullException("genericParametersOut");
            }

            mixinName = expression;
            genericParametersOut.Clear();

            var varExp = expression as VariableReferenceExpression;
            if (varExp != null)
            {
                Identifier identifier = varExp.Name;
                var identifierGeneric = identifier as IdentifierGeneric;
                if (identifierGeneric != null)
                {
                    mixinName = new VariableReferenceExpression(identifierGeneric.Text);

                    foreach (Identifier subIdentifier in identifierGeneric.Identifiers)
                    {
                        var identifierDot = subIdentifier as IdentifierDot;
                        if (identifierDot != null)
                        {
                            if (identifierDot.Identifiers.Count == 2)
                            {
                                genericParametersOut.Add(new MemberReferenceExpression(new VariableReferenceExpression(identifierDot.Identifiers[0]), identifierDot.Identifiers[1]));
                            }
                            else
                            {
                                logging.Error("Unsupported identifier in generic used for mixin", identifierDot.Span);
                            }
                        }
                        else if (subIdentifier is LiteralIdentifier)
                        {
                            var literalIdentifier = (LiteralIdentifier)subIdentifier;

                            genericParametersOut.Add(new LiteralExpression(literalIdentifier.Value));
                        }
                        else if (subIdentifier.GetType() == typeof(Identifier))
                        {
                            genericParametersOut.Add(new VariableReferenceExpression(subIdentifier));
                        }
                        else
                        {
                            logging.Error("Unsupported identifier in generic used for mixin", subIdentifier.Span);
                        }
                    }
                }
            }
        }
        
        private void WriteGenericParameters(IEnumerable<Expression> genericParameters)
        {
            foreach (Expression genericParameter in genericParameters)
            {
                Write(", ");
                VisitDynamic(genericParameter);
            }
        }

        private void WriteMixinName(Expression mixinName)
        {
            // Output between "" only if the mixin name is only a variable
            if (mixinName is VariableReferenceExpression)
            {
                Write("\"");
            }
            VisitDynamic(mixinName);
            if (mixinName is VariableReferenceExpression)
            {
                Write("\"");
            }
        }

        private void LogErrors()
        {
            foreach (var reportMessage in logging.Messages)
            {
                if (reportMessage.Level == ReportMessageLevel.Error)
                {
                    Write("#error ").WriteLine(reportMessage.ToString());
                }
            }
        }

        private class ShaderBlockContext
        {
            public readonly HashSet<string> DeclaredParameters = new HashSet<string>();
        }

        private void FixShaderClassTypeWithNoNameSpace()
        {
            for (int i = 0; i < shader.Declarations.Count; i++)
            {
                var node = shader.Declarations[i];
                if (node is ShaderClassType)
                {
                    var nameSpaceBlock = new NamespaceBlock(DefaultNameSpace);
                    nameSpaceBlock.Body.Add(node);
                    shader.Declarations[i] = nameSpaceBlock;
                }
            }
        }

        /// <summary>
        /// Internal visitor to precalculate all available Parameters in the context
        /// </summary>
        private sealed class ShaderBlockVisitor : ShaderVisitor
        {
            private readonly LoggerResult logging;
            private ShaderBlockContext currentContext;

            private readonly ShaderKeyGeneratorBase parent;

            private bool isVisitingShaderClassType;

            public ShaderBlockVisitor(ShaderKeyGeneratorBase parent, LoggerResult logging)
                : base(false, false)
            {
                this.parent = parent;
                this.logging = logging;
            }

            public bool HasMixin { get; private set; }

            public bool HasShaderClassType { get; private set; }

            public void Run(Shader shader)
            {
                VisitDynamic(shader);
            }

            [Visit]
            private void Visit(ParametersBlock paramsBlock)
            {
                HasMixin = true;
            }

            [Visit]
            private void Visit(ShaderClassType shaderClassType)
            {
                isVisitingShaderClassType = true;
                foreach (var variable in shaderClassType.Members.OfType<Variable>())
                {
                    if (isVisitingShaderClassType && !HasShaderClassType)
                    {
                        if (parent.IsParameterKey(variable))
                        {
                            HasShaderClassType = true;
                        }
                    }
                }
                isVisitingShaderClassType = false;
            }

            [Visit]
            private void Visit(ShaderBlock shaderBlock)
            {
                HasMixin = true;

                // Create a context associated with ShaderBlock
                currentContext = new ShaderBlockContext();
                shaderBlock.SetTag(BlockContextTag, currentContext);

                foreach (Statement statement in shaderBlock.Body.Statements)
                {
                    VisitDynamic(statement);
                }
                currentContext = null;
            }

            [Visit]
            private void Visit(UsingParametersStatement usingParametersStatement)
            {
                if (currentContext == null)
                {
                    logging.Error("Unexpected 'using params' outside of shader block declaration", usingParametersStatement.Span);
                    return;
                }

                HashSet<string> usings = currentContext.DeclaredParameters;

                // If this is a using params without a body, it is a simple reference of a ParameterBlock
                if (usingParametersStatement.Body == null)
                {
                    var simpleName = usingParametersStatement.Name as VariableReferenceExpression;
                    if (simpleName != null)
                    {
                        string typeName = simpleName.Name.Text;

                        if (usings.Contains(typeName))
                        {
                            logging.Error("Unexpected declaration of using params. This variable is already declared in this scope", usingParametersStatement.Span);
                            return;
                        }

                        usings.Add(typeName);
                    }
                }
                else
                {
                    // using params with a body is to enter the context of the parameters passed to the using statements
                    VisitDynamic(usingParametersStatement.Body);
                }
            }
        }
    }
}