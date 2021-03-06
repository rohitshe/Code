// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
namespace SiliconStudio.Assets.Templates
{
    public delegate bool RunGeneratorDelegate();

    /// <summary>
    /// The interface to represent a template generator.
    /// </summary>
    public interface ITemplateGenerator
    {
        /// <summary>
        /// Determines whether this generator is supporting the specified template
        /// </summary>
        /// <param name="templateDescription">The template description.</param>
        /// <returns><c>true</c> if this generator is supporting the specified template; otherwise, <c>false</c>.</returns>
        bool IsSupportingTemplate(TemplateDescription templateDescription);
    }

    /// <summary>
    /// The interface to represent a template generator.
    /// </summary>
    /// <typeparam name="TParameters">The type of parameters this generator uses.</typeparam>
    public interface ITemplateGenerator<in TParameters> : ITemplateGenerator where TParameters : TemplateGeneratorParameters
    {
        /// <summary>
        /// Prepares this generator with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the template generator.</param>
        /// <remarks>This method should be used to verify that the parameters are correct, and to ask user for additional
        /// information before running the template.
        /// </remarks>
        /// <returns><c>True</c> if the preparation was successful, <c>false</c> otherwise.</returns>
        bool PrepareForRun(TParameters parameters);

        /// <summary>
        /// Runs the generator with the given parameter.
        /// </summary>
        /// <param name="parameters">The parameters for the template generator.</param>
        /// <remarks>
        /// This method should work in unattended mode and should not ask user for information anymore.
        /// </remarks>
        /// <returns><c>True</c> if the generation was successful, <c>false</c> otherwise.</returns>
        bool Run(TParameters parameters);
    }
}
