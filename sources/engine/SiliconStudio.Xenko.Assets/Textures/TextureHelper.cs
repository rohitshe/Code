﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using System.Threading;

using SiliconStudio.BuildEngine;
using SiliconStudio.Core;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Core.Serialization.Assets;
using SiliconStudio.Xenko.Assets.Sprite;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Graphics.Data;
using SiliconStudio.TextureConverter;

namespace SiliconStudio.Xenko.Assets.Textures
{
    /// <summary>
    /// An helper for the compile commands that needs to process textures.
    /// </summary>
    public static class TextureHelper
    {
        /// <summary>
        /// Parameters used to import/convert a texture.
        /// </summary>
        public class ImportParameters
        {
            public string OutputUrl;

            public bool IsSRgb;

            public Size2 DesiredSize;

            public bool IsSizeInPercentage;

            public TextureFormat DesiredFormat;

            public AlphaFormat DesiredAlpha;

            public TextureHint TextureHint;

            public bool GenerateMipmaps;

            public bool PremultiplyAlpha;

            public Color ColorKeyColor;

            public bool ColorKeyEnabled;

            public TextureQuality TextureQuality;

            public GraphicsPlatform GraphicsPlatform;

            public GraphicsProfile GraphicsProfile;

            public PlatformType Platform;

            public ImportParameters(TextureConvertParameters textureParameters)
            {
                var asset = textureParameters.Texture;

                // Compute SRgb usage
                // If Texture is in auto mode, use the global settings, else use the settings overridden by the texture asset. 
                IsSRgb = textureParameters.Texture.ColorSpace.ToColorSpace(textureParameters.ColorSpace, asset.Hint) == ColorSpace.Linear;

                DesiredSize = new Size2((int)asset.Width, (int)asset.Height);
                IsSizeInPercentage = asset.IsSizeInPercentage;
                DesiredFormat = asset.Format;
                DesiredAlpha = asset.Alpha;
                TextureHint = asset.Hint;
                GenerateMipmaps = asset.GenerateMipmaps;
                PremultiplyAlpha = asset.PremultiplyAlpha;
                ColorKeyColor  = asset.ColorKeyColor;
                ColorKeyEnabled = asset.ColorKeyEnabled;
                TextureQuality = textureParameters.TextureQuality;
                GraphicsPlatform = textureParameters.GraphicsPlatform;
                GraphicsProfile = textureParameters.GraphicsProfile;
                Platform = textureParameters.Platform;
            }

            public ImportParameters(SpriteSheetAssetCompiler.SpriteSheetParameters spriteSheetParameters)
            {
                var asset = spriteSheetParameters.SheetAsset;

                // Compute SRgb usage
                // If Texture is in auto mode, use the global settings, else use the settings overridden by the texture asset. 
                IsSRgb = asset.ColorSpace.ToColorSpace(spriteSheetParameters.ColorSpace, TextureHint.Color) == ColorSpace.Linear;

                DesiredSize = new Size2(100, 100);
                IsSizeInPercentage = true;
                DesiredFormat = asset.Format;
                DesiredAlpha = asset.Alpha;
                TextureHint = TextureHint.Color;
                GenerateMipmaps = asset.GenerateMipmaps;
                PremultiplyAlpha = asset.PremultiplyAlpha;
                ColorKeyColor = asset.ColorKeyColor;
                ColorKeyEnabled = asset.ColorKeyEnabled;
                TextureQuality = spriteSheetParameters.TextureQuality;
                GraphicsPlatform = spriteSheetParameters.GraphicsPlatform;
                GraphicsProfile = spriteSheetParameters.GraphicsProfile;
                Platform = spriteSheetParameters.Platform;
            }
        }

        /// <summary>
        /// Returns true if the PVRTC can be used for the provided texture size.
        /// </summary>
        /// <param name="textureSize">the size of the texture</param>
        /// <returns>true if PVRTC is supported</returns>
        public static bool SupportPVRTC(Int2 textureSize)
        {
            return textureSize.X == textureSize.Y && MathUtil.IsPow2(textureSize.X);
        }

        /// <summary>
        /// Utility function to check that the texture size is supported on the graphics platform for the provided graphics profile.
        /// </summary>
        /// <param name="parameters">The import parameters</param>
        /// <param name="textureSizeRequested">The texture size requested.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>true if the texture size is supported</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">graphicsProfile</exception>
        public static Size2 FindBestTextureSize(ImportParameters parameters, Size2 textureSizeRequested, ILogger logger = null)
        {
            var textureSize = textureSizeRequested;

            // compressed DDS files has to have a size multiple of 4.
            if (parameters.GraphicsPlatform == GraphicsPlatform.Direct3D11 && parameters.DesiredFormat == TextureFormat.Compressed
                && ((textureSizeRequested.Width % 4) != 0 || (textureSizeRequested.Height % 4) != 0))
            {
                textureSize.Width = unchecked((int)(((uint)(textureSizeRequested.Width + 3)) & ~(uint)3));
                textureSize.Height = unchecked((int)(((uint)(textureSizeRequested.Height + 3)) & ~(uint)3));
            }

            var maxTextureSize = 0;

            // determine if the desired size if valid depending on the graphics profile
            switch (parameters.GraphicsProfile)
            {
                case GraphicsProfile.Level_9_1:
                case GraphicsProfile.Level_9_2:
                case GraphicsProfile.Level_9_3:
                    if (parameters.GenerateMipmaps && (!MathUtil.IsPow2(textureSize.Width) || !MathUtil.IsPow2(textureSize.Height)))
                    {
                        // TODO: TEMPORARY SETUP A MAX TEXTURE OF 1024. THIS SHOULD BE SPECIFIED DONE IN THE ASSET INSTEAD
                        textureSize.Width = Math.Min(MathUtil.NextPowerOfTwo(textureSize.Width), 1024);
                        textureSize.Height = Math.Min(MathUtil.NextPowerOfTwo(textureSize.Height), 1024);
                        logger?.Warning("Graphic profiles 9.1/9.2/9.3 do not support mipmaps with textures that are not power of 2. Asset is automatically resized to " + textureSize);
                    }
                    maxTextureSize = parameters.GraphicsProfile >= GraphicsProfile.Level_9_3 ? 4096 : 2048;
                    break;
                case GraphicsProfile.Level_10_0:
                case GraphicsProfile.Level_10_1:
                    maxTextureSize = 8192;
                    break;
                case GraphicsProfile.Level_11_0:
                case GraphicsProfile.Level_11_1:
                case GraphicsProfile.Level_11_2:
                    maxTextureSize = 16384;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("graphicsProfile");
            }

            if (textureSize.Width > maxTextureSize || textureSize.Height > maxTextureSize)
            {
                logger?.Error("Graphic profile {0} do not support texture with resolution {2} x {3} because it is larger than {1}. " +
                             "Please reduce texture size or upgrade your graphic profile.", parameters.GraphicsProfile, maxTextureSize, textureSize.Width, textureSize.Height);
                return new Size2(Math.Min(textureSize.Width, maxTextureSize), Math.Min(textureSize.Height, maxTextureSize));
            }

            return textureSize;
        }

        /// <summary>
        /// Determine the output format of the texture depending on the platform and asset properties.
        /// </summary>
        /// <param name="parameters">The conversion request parameters</param>
        /// <param name="imageSize">The texture output size</param>
        /// <param name="inputImageFormat">The pixel format of the input image</param>
        /// <returns>The pixel format to use as output</returns>
        public static PixelFormat DetermineOutputFormat(ImportParameters parameters, Int2 imageSize, PixelFormat inputImageFormat)
        {
            var hint = parameters.TextureHint;
            var alphaMode = parameters.DesiredAlpha;

            // Default output format
            var outputFormat = PixelFormat.R8G8B8A8_UNorm;
            switch (parameters.DesiredFormat)
            {
                case TextureFormat.Compressed:
                    switch (parameters.Platform)
                    {
                        case PlatformType.Android:
                            if (inputImageFormat.IsHDR())
                            {
                                outputFormat = inputImageFormat;
                            }
                            else
                            {
                                switch (parameters.GraphicsProfile)
                                {
                                    case GraphicsProfile.Level_9_1:
                                    case GraphicsProfile.Level_9_2:
                                    case GraphicsProfile.Level_9_3:
                                        outputFormat = alphaMode == AlphaFormat.None && !parameters.IsSRgb ? PixelFormat.ETC1 : parameters.IsSRgb ? PixelFormat.R8G8B8A8_UNorm_SRgb : PixelFormat.R8G8B8A8_UNorm;
                                        break;
                                    case GraphicsProfile.Level_10_0:
                                    case GraphicsProfile.Level_10_1:
                                    case GraphicsProfile.Level_11_0:
                                    case GraphicsProfile.Level_11_1:
                                    case GraphicsProfile.Level_11_2:
                                        // GLES3.0 starting from Level_10_0, this profile enables ETC2 compression on Android
                                        outputFormat = alphaMode == AlphaFormat.None && !parameters.IsSRgb ? PixelFormat.ETC1 : parameters.IsSRgb ? PixelFormat.ETC2_RGBA_SRgb : PixelFormat.ETC2_RGBA;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException("GraphicsProfile");
                                }
                            }
                            break;
                        case PlatformType.iOS:
                            // PVRTC works only for square POT textures
                            if (inputImageFormat.IsHDR())
                            {
                                outputFormat = inputImageFormat;
                            }
                            else if (SupportPVRTC(imageSize))
                            {
                                switch (alphaMode)
                                {
                                    case AlphaFormat.None:
                                        outputFormat = parameters.IsSRgb ? PixelFormat.PVRTC_4bpp_RGB_SRgb : PixelFormat.PVRTC_4bpp_RGB;
                                        break;
                                    case AlphaFormat.Mask:
                                        // DXT1 handles 1-bit alpha channel
                                        // TODO: Not sure about the equivalent here?
                                        outputFormat = parameters.IsSRgb ? PixelFormat.PVRTC_4bpp_RGBA_SRgb : PixelFormat.PVRTC_4bpp_RGBA;
                                        break;
                                    case AlphaFormat.Explicit:
                                    case AlphaFormat.Interpolated:
                                        // DXT3 is good at sharp alpha transitions
                                        // TODO: Not sure about the equivalent here?
                                        outputFormat = parameters.IsSRgb ? PixelFormat.PVRTC_4bpp_RGBA_SRgb : PixelFormat.PVRTC_4bpp_RGBA;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            else
                            {
                                outputFormat = parameters.IsSRgb ? PixelFormat.R8G8B8A8_UNorm_SRgb : PixelFormat.R8G8B8A8_UNorm;
                            }
                            break;
                        case PlatformType.Windows:
                        case PlatformType.WindowsPhone:
                        case PlatformType.WindowsStore:
                        case PlatformType.Windows10:
                            switch (parameters.GraphicsPlatform)
                            {
                                case GraphicsPlatform.Direct3D11:
                                case GraphicsPlatform.Direct3D12:
                                case GraphicsPlatform.OpenGL:


                                    // https://msdn.microsoft.com/en-us/library/windows/desktop/hh308955%28v=vs.85%29.aspx
                                    // http://www.reedbeta.com/blog/2012/02/12/understanding-bcn-texture-compression-formats/
                                    // ----------------------------------------------    ----------------------------------------------------                        ---    ---------------------------------
                                    // Source data                                       Minimum required data compression resolution 	                  Recommended format	Minimum supported feature level
                                    // ----------------------------------------------    ----------------------------------------------------                        ---    ---------------------------------
                                    // Three-channel color with alpha channel            Three color channels (5 bits:6 bits:5 bits), with 0 or 1 bit(s) of alpha    BC1    Direct3D 9.1     (color maps, cutout color maps - 1 bit alpha, normal maps if memory is tight)
                                    // Three-channel color with alpha channel            Three color channels (5 bits:6 bits:5 bits), with 4 bits of alpha           BC2    Direct3D 9.1     (idem)
                                    // Three-channel color with alpha channel            Three color channels (5 bits:6 bits:5 bits) with 8 bits of alpha            BC3    Direct3D 9.1     (color maps with alpha, packing color and mono maps together)
                                    // One-channel color                                 One color channel (8 bits)                                                  BC4    Direct3D 10      (Height maps, gloss maps, font atlases, any gray scales image)
                                    // Two-channel color	                             Two color channels (8 bits:8 bits)                                          BC5    Direct3D 10      (Tangent space normal maps)
                                    // Three-channel high dynamic range (HDR) color      Three color channels (16 bits:16 bits:16 bits) in "half" floating point*    BC6H   Direct3D 11      (HDR images)
                                    // Three-channel color, alpha channel optional       Three color channels (4 to 7 bits per channel) with 0 to 8 bits of alpha    BC7    Direct3D 11      (High quality color maps, Color maps with full alpha)

                                    switch (alphaMode)
                                    {
                                        case AlphaFormat.None:
                                        case AlphaFormat.Mask:
                                            // DXT1 handles 1-bit alpha channel
                                            outputFormat = parameters.IsSRgb ? PixelFormat.BC1_UNorm_SRgb : PixelFormat.BC1_UNorm;
                                            break;
                                        case AlphaFormat.Explicit:
                                            // DXT3 is good at sharp alpha transitions
                                            outputFormat = parameters.IsSRgb ? PixelFormat.BC2_UNorm_SRgb : PixelFormat.BC2_UNorm;
                                            break;
                                        case AlphaFormat.Interpolated:
                                            // DXT5 is good at alpha gradients
                                            outputFormat = parameters.IsSRgb ? PixelFormat.BC3_UNorm_SRgb : PixelFormat.BC3_UNorm;
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }

                                    // Overrides the format when profile is >= 10.0
                                    // Support some specific optimized formats based on the hint or input type
                                    if (parameters.GraphicsProfile >= GraphicsProfile.Level_10_0)
                                    {
                                        if (parameters.GraphicsPlatform != GraphicsPlatform.OpenGL && hint == TextureHint.NormalMap)
                                        {
                                            outputFormat = PixelFormat.BC5_SNorm;
                                        }
                                        else if (parameters.GraphicsPlatform != GraphicsPlatform.OpenGL && hint == TextureHint.Grayscale)
                                        {
                                            outputFormat = PixelFormat.BC4_UNorm;
                                        }
                                        else if (inputImageFormat.IsHDR())
                                        {
                                            // BC6H is too slow to compile
                                            //outputFormat = parameters.GraphicsProfile >= GraphicsProfile.Level_11_0 && alphaMode == AlphaFormat.None ? PixelFormat.BC6H_Uf16 : inputImageFormat;
                                            outputFormat = inputImageFormat;
                                        }
                                        // TODO support the BC6/BC7 but they are so slow to compile that we can't use them right now
                                    }
                                    break;
                                case GraphicsPlatform.OpenGLES: // OpenGLES on Windows
                                    if (inputImageFormat.IsHDR())
                                    {
                                        outputFormat = inputImageFormat;
                                    }
                                    else if (parameters.IsSRgb)
                                    {
                                        outputFormat = PixelFormat.R8G8B8A8_UNorm_SRgb;
                                    }
                                    else
                                    {
                                        switch (parameters.GraphicsProfile)
                                        {
                                            case GraphicsProfile.Level_9_1:
                                            case GraphicsProfile.Level_9_2:
                                            case GraphicsProfile.Level_9_3:
                                                outputFormat = alphaMode == AlphaFormat.None ? PixelFormat.ETC1 : PixelFormat.R8G8B8A8_UNorm;
                                                break;
                                            case GraphicsProfile.Level_10_0:
                                            case GraphicsProfile.Level_10_1:
                                            case GraphicsProfile.Level_11_0:
                                            case GraphicsProfile.Level_11_1:
                                            case GraphicsProfile.Level_11_2:
                                                // GLES3.0 starting from Level_10_0, this profile enables ETC2 compression on Android
                                                outputFormat = alphaMode == AlphaFormat.None ? PixelFormat.ETC1 : PixelFormat.ETC2_RGBA;
                                                break;
                                            default:
                                                throw new ArgumentOutOfRangeException("GraphicsProfile");
                                        }
                                    }
                                    break;
                                default:
                                    // OpenGL on Windows
                                    // TODO: Need to handle OpenGL Desktop compression
                                    outputFormat = parameters.IsSRgb ? PixelFormat.R8G8B8A8_UNorm_SRgb : PixelFormat.R8G8B8A8_UNorm;
                                    break;
                            }
                            break;
                        case PlatformType.Linux:
                                // OpenGL on Linux
                                // TODO: Need to handle OpenGL Desktop compression
                                outputFormat = parameters.IsSRgb ? PixelFormat.R8G8B8A8_UNorm_SRgb : PixelFormat.R8G8B8A8_UNorm;
                                break;
                        default:
                            throw new NotSupportedException("Platform " + parameters.Platform + " is not supported by TextureTool");
                    }
                    break;
                case TextureFormat.Color16Bits:
                    if (parameters.IsSRgb)
                    {
                        outputFormat = PixelFormat.R8G8B8A8_UNorm_SRgb;
                    }
                    else
                    {
                        if (alphaMode == AlphaFormat.None)
                        {
                            outputFormat = PixelFormat.B5G6R5_UNorm;
                        }
                        else if (alphaMode == AlphaFormat.Mask)
                        {
                            outputFormat = PixelFormat.B5G5R5A1_UNorm;
                        }
                    }
                    break;
                case TextureFormat.Color32Bits:
                    if (parameters.IsSRgb)
                    {
                        outputFormat = PixelFormat.R8G8B8A8_UNorm_SRgb;
                    }
                    break;
                case TextureFormat.AsIs:
                    outputFormat = inputImageFormat;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return outputFormat;
        }

        public static ResultStatus ImportTextureImage(TextureTool textureTool, TexImage texImage, ImportParameters parameters, CancellationToken cancellationToken, Logger logger)
        {
            var assetManager = new ContentManager();

            // Apply transformations
            textureTool.Decompress(texImage, parameters.IsSRgb);

            // Special case when the input texture is monochromatic but it is supposed to be a color and we are working in SRGB
            // In that case, we need to transform it to a supported SRGB format (R8G8B8A8_UNorm_SRgb)
            // TODO: As part of a conversion phase, this code may be moved to a dedicated method in this class at some point
            if (parameters.TextureHint == TextureHint.Color && parameters.IsSRgb && (texImage.Format == PixelFormat.R8_UNorm || texImage.Format == PixelFormat.A8_UNorm))
            {
                textureTool.Convert(texImage, PixelFormat.R8G8B8A8_UNorm_SRgb);
            }

            if (cancellationToken.IsCancellationRequested) // abort the process if cancellation is demanded
                return ResultStatus.Cancelled;

            var fromSize =  new Size2(texImage.Width, texImage.Height);
            var targetSize = parameters.DesiredSize;

            // Resize the image
            if (parameters.IsSizeInPercentage)
            {
                targetSize = new Size2((int)(fromSize.Width * targetSize.Width / 100.0f), (int)(fromSize.Height * targetSize.Height / 100.0f));
            }

            // Find the target size
            targetSize = FindBestTextureSize(parameters, targetSize, logger);

            // Resize the image only if needed
            if (targetSize != fromSize)
            {
                textureTool.Resize(texImage, targetSize.Width, targetSize.Height, Filter.Rescaling.Lanczos3);
            }

            if (cancellationToken.IsCancellationRequested) // abort the process if cancellation is demanded
                return ResultStatus.Cancelled;

            // texture size is now determined, we can cache it
            var textureSize = new Int2(texImage.Width, texImage.Height);

            // determine the alpha format of the texture when set to Auto
            // Note: this has to be done before the ColorKey transformation in order to be able to take advantage of image file AlphaDepth information
            if(parameters.DesiredAlpha == AlphaFormat.Auto)
            {
                var colorKey = parameters.ColorKeyEnabled? (Color?)parameters.ColorKeyColor : null;
                var alphaLevel = textureTool.GetAlphaLevels(texImage, new Rectangle(0, 0, textureSize.X, textureSize.Y), colorKey, logger);
                parameters.DesiredAlpha = alphaLevel.ToAlphaFormat();
            }

            // Apply the color key
            if (parameters.ColorKeyEnabled)
                textureTool.ColorKey(texImage, parameters.ColorKeyColor);

            if (cancellationToken.IsCancellationRequested) // abort the process if cancellation is demanded
                return ResultStatus.Cancelled;

            // Pre-multiply alpha only for relevant formats 
            if (parameters.PremultiplyAlpha && texImage.Format.HasAlpha32Bits())
                textureTool.PreMultiplyAlpha(texImage);

            if (cancellationToken.IsCancellationRequested) // abort the process if cancellation is demanded
                return ResultStatus.Cancelled;


            // Generate mipmaps
            if (parameters.GenerateMipmaps)
            {
                var boxFilteringIsSupported = !texImage.Format.IsSRgb() || (MathUtil.IsPow2(textureSize.X) && MathUtil.IsPow2(textureSize.Y));
                textureTool.GenerateMipMaps(texImage, boxFilteringIsSupported? Filter.MipMapGeneration.Box: Filter.MipMapGeneration.Linear);
            }
                
            if (cancellationToken.IsCancellationRequested) // abort the process if cancellation is demanded
                return ResultStatus.Cancelled;


            // Convert/Compress to output format
            // TODO: Change alphaFormat depending on actual image content (auto-detection)?
            var outputFormat = DetermineOutputFormat(parameters, textureSize, texImage.Format);
            textureTool.Compress(texImage, outputFormat, (TextureConverter.Requests.TextureQuality)parameters.TextureQuality);

            if (cancellationToken.IsCancellationRequested) // abort the process if cancellation is demanded
                return ResultStatus.Cancelled;

            // Save the texture
            using (var outputImage = textureTool.ConvertToXenkoImage(texImage))
            {
                if (cancellationToken.IsCancellationRequested) // abort the process if cancellation is demanded
                    return ResultStatus.Cancelled;

                assetManager.Save(parameters.OutputUrl, outputImage.ToSerializableVersion());

                logger.Verbose("Compression successful [{3}] to ({0}x{1},{2})", outputImage.Description.Width, outputImage.Description.Height, outputImage.Description.Format, parameters.OutputUrl);
            }

            return ResultStatus.Successful;
        }

        private static AlphaFormat ToAlphaFormat(this AlphaLevels alphaLevels)
        {
            switch (alphaLevels)
            {
                case AlphaLevels.NoAlpha:
                    return AlphaFormat.None;
                case AlphaLevels.MaskAlpha:
                    return AlphaFormat.Mask;
                case AlphaLevels.InterpolatedAlpha:
                    return AlphaFormat.Interpolated;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alphaLevels), alphaLevels, null);
            }
        }
    }
}
