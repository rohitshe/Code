﻿// <auto-generated>
// Do not edit this file yourself!
//
// This code was generated by Paradox Shader Mixin Code Generator.
// To generate it yourself, please install SiliconStudio.Paradox.VisualStudio.Package .vsix
// and re-save the associated .pdxfx.
// </auto-generated>

using System;
using SiliconStudio.Core;
using SiliconStudio.Paradox.Effects;
using SiliconStudio.Paradox.Graphics;
using SiliconStudio.Paradox.Shaders;
using SiliconStudio.Core.Mathematics;
using Buffer = SiliconStudio.Paradox.Graphics.Buffer;

using SiliconStudio.Paradox.Effects.Data;
using SiliconStudio.Paradox.Effects;
using SiliconStudio.Paradox.Effects.Renderers;
namespace SiliconStudio.Paradox.Effects.Cubemap
{
    internal static partial class ShaderMixins
    {
        internal partial class CubemapBlendEffect  : IShaderMixinBuilder
        {
            public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
            {
                context.Mixin(mixin, "ShaderBase");
                context.Mixin(mixin, "ImageEffectShader");
                mixin.Mixin.AddMacro("TEXTURECUBE_BLEND_COUNT", context.GetParam(CubemapBlendRenderer.CubemapCount));
                if (context.GetParam(CubemapBlendRenderer.UseMultipleRenderTargets))
                    context.Mixin(mixin, "CubemapBlenderMRT");
                else
                    context.Mixin(mixin, "CubemapBlender");
                foreach(var ____1 in context.GetParam(CubemapBlendRenderer.Cubemaps))

                {
                    context.PushParameters(____1);

                    {
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                        context.Mixin(__subMixin, "CubemapFace", context.GetParam(CubemapBlendRenderer.CubemapKey));
                        mixin.Mixin.AddCompositionToArray("Cubemaps", __subMixin.Mixin);
                    }
                    context.PopParameters();
                }
            }

            [ModuleInitializer]
            internal static void __Initialize__()

            {
                ShaderMixinManager.Register("CubemapBlendEffect", new CubemapBlendEffect());
            }
        }
    }
}
