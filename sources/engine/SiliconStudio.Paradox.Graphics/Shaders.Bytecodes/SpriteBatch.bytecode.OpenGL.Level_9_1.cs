﻿#if SILICONSTUDIO_PARADOX_GRAPHICS_API_OPENGLCORE
//------------------------------------------------------------------------------
// <auto-generated>
//     Paradox Effect Compiler File Generated:
//     Effect [SpriteBatch]
//
//     Command Line: C:\Code\Paradox\sources\engine\SiliconStudio.Paradox.Graphics\Shaders.Bytecodes\..\..\..\..\Bin\Windows-Direct3D11\SiliconStudio.Assets.CompilerApp.exe --platform=Windows --profile=Windows --output-path=C:\Code\Paradox\sources\engine\SiliconStudio.Paradox.Graphics\Shaders.Bytecodes\obj\app_data --build-path=C:\Code\Paradox\sources\engine\SiliconStudio.Paradox.Graphics\Shaders.Bytecodes\obj\build_app_data --package-file=Graphics.pdxpkg
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiliconStudio.Paradox.Graphics 
{
    public partial class SpriteBatch
    {
        private static readonly byte[] binaryBytecode = new byte[] {
0, 4, 0, 0, 0, 25, 115, 104, 97, 100, 101, 114, 115, 47, 83, 112, 114, 105, 116, 101, 66, 97, 116, 99, 104, 46, 112, 100, 120, 115, 108, 1, 52, 95, 227, 40, 173, 190, 28, 56, 167, 210, 32, 96, 26, 17, 238, 47, 24, 115, 104, 97, 100, 101, 114, 115, 47, 83, 112, 114, 105, 116, 101, 66, 97, 
115, 101, 46, 112, 100, 120, 115, 108, 1, 29, 34, 58, 164, 137, 37, 104, 112, 102, 62, 171, 91, 222, 240, 184, 141, 24, 115, 104, 97, 100, 101, 114, 115, 47, 83, 104, 97, 100, 101, 114, 66, 97, 115, 101, 46, 112, 100, 120, 115, 108, 1, 106, 12, 112, 201, 155, 82, 245, 182, 20, 34, 125, 144, 52, 
118, 27, 28, 23, 115, 104, 97, 100, 101, 114, 115, 47, 84, 101, 120, 116, 117, 114, 105, 110, 103, 46, 112, 100, 120, 115, 108, 1, 5, 30, 63, 244, 238, 42, 195, 237, 244, 224, 18, 82, 140, 193, 239, 143, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 34, 83, 104, 97, 100, 101, 114, 
66, 97, 115, 101, 46, 80, 97, 114, 97, 100, 111, 120, 70, 108, 105, 112, 82, 101, 110, 100, 101, 114, 116, 97, 114, 103, 101, 116, 0, 0, 0, 0, 0, 23, 80, 97, 114, 97, 100, 111, 120, 70, 108, 105, 112, 82, 101, 110, 100, 101, 114, 116, 97, 114, 103, 101, 116, 0, 0, 0, 0, 3, 0, 0, 
0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 11, 79, 112, 101, 110, 103, 108, 70, 108, 97, 103, 115, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 26, 83, 112, 114, 105, 116, 
101, 66, 97, 115, 101, 46, 77, 97, 116, 114, 105, 120, 84, 114, 97, 110, 115, 102, 111, 114, 109, 0, 0, 0, 0, 0, 20, 77, 97, 116, 114, 105, 120, 84, 114, 97, 110, 115, 102, 111, 114, 109, 95, 105, 100, 53, 57, 3, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 
0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 4, 0, 0, 0, 1, 0, 7, 71, 108, 111, 98, 97, 108, 115, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 17, 84, 101, 120, 116, 117, 114, 105, 110, 103, 46, 83, 97, 109, 112, 108, 101, 114, 0, 0, 0, 
0, 0, 12, 83, 97, 109, 112, 108, 101, 114, 95, 105, 100, 51, 55, 8, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 84, 101, 120, 116, 117, 114, 105, 110, 103, 46, 84, 101, 120, 116, 117, 114, 101, 48, 0, 0, 0, 0, 0, 13, 84, 101, 120, 
116, 117, 114, 101, 48, 95, 105, 100, 49, 51, 9, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 79, 112, 101, 110, 103, 108, 70, 108, 97, 103, 115, 0, 0, 0, 0, 0, 11, 79, 112, 101, 110, 103, 108, 70, 108, 97, 103, 115, 10, 0, 0, 0, 
25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 71, 108, 111, 98, 97, 108, 115, 0, 0, 0, 0, 0, 7, 71, 108, 111, 98, 97, 108, 115, 10, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 
21, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 127, 255, 255, 255, 127, 127, 0, 17, 84, 101, 120, 116, 117, 114, 105, 110, 103, 46, 
83, 97, 109, 112, 108, 101, 114, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0, 234, 6, 0, 0, 35, 118, 101, 114, 115, 105, 111, 110, 32, 52, 50, 48, 13, 10, 13, 10, 115, 116, 114, 117, 99, 116, 32, 86, 83, 95, 83, 84, 82, 69, 65, 77, 
83, 32, 13, 10, 123, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 53, 56, 59, 13, 10, 32, 32, 32, 32, 102, 108, 111, 97, 116, 32, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 
50, 32, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 83, 104, 97, 100, 105, 110, 103, 80, 111, 115, 105, 116, 105, 111, 110, 
95, 105, 100, 48, 59, 13, 10, 125, 59, 13, 10, 115, 116, 114, 117, 99, 116, 32, 86, 83, 95, 79, 85, 84, 80, 85, 84, 32, 13, 10, 123, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 83, 104, 97, 100, 105, 110, 103, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 48, 59, 13, 10, 
32, 32, 32, 32, 102, 108, 111, 97, 116, 32, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 50, 32, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 67, 111, 108, 111, 114, 
95, 105, 100, 54, 48, 59, 13, 10, 125, 59, 13, 10, 115, 116, 114, 117, 99, 116, 32, 86, 83, 95, 73, 78, 80, 85, 84, 32, 13, 10, 123, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 53, 56, 59, 13, 10, 32, 32, 32, 32, 102, 108, 
111, 97, 116, 32, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 50, 32, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 59, 
13, 10, 125, 59, 13, 10, 115, 116, 100, 49, 52, 48, 32, 117, 110, 105, 102, 111, 114, 109, 32, 79, 112, 101, 110, 103, 108, 70, 108, 97, 103, 115, 32, 123, 13, 10, 32, 32, 32, 32, 102, 108, 111, 97, 116, 32, 80, 97, 114, 97, 100, 111, 120, 70, 108, 105, 112, 82, 101, 110, 100, 101, 114, 116, 
97, 114, 103, 101, 116, 59, 13, 10, 125, 59, 13, 10, 115, 116, 100, 49, 52, 48, 32, 117, 110, 105, 102, 111, 114, 109, 32, 71, 108, 111, 98, 97, 108, 115, 32, 123, 13, 10, 32, 32, 32, 32, 109, 97, 116, 52, 32, 77, 97, 116, 114, 105, 120, 84, 114, 97, 110, 115, 102, 111, 114, 109, 95, 105, 
100, 53, 57, 59, 13, 10, 125, 59, 13, 10, 111, 117, 116, 32, 102, 108, 111, 97, 116, 32, 118, 95, 66, 65, 84, 67, 72, 95, 83, 87, 73, 90, 90, 76, 69, 48, 59, 13, 10, 111, 117, 116, 32, 118, 101, 99, 50, 32, 118, 95, 84, 69, 88, 67, 79, 79, 82, 68, 48, 59, 13, 10, 111, 117, 
116, 32, 118, 101, 99, 52, 32, 118, 95, 67, 79, 76, 79, 82, 48, 59, 13, 10, 105, 110, 32, 118, 101, 99, 52, 32, 97, 95, 80, 79, 83, 73, 84, 73, 79, 78, 48, 59, 13, 10, 105, 110, 32, 102, 108, 111, 97, 116, 32, 97, 95, 66, 65, 84, 67, 72, 95, 83, 87, 73, 90, 90, 76, 69, 
48, 59, 13, 10, 105, 110, 32, 118, 101, 99, 50, 32, 97, 95, 84, 69, 88, 67, 79, 79, 82, 68, 48, 59, 13, 10, 105, 110, 32, 118, 101, 99, 52, 32, 97, 95, 67, 79, 76, 79, 82, 48, 59, 13, 10, 118, 111, 105, 100, 32, 109, 97, 105, 110, 40, 41, 13, 10, 123, 13, 10, 32, 32, 32, 
32, 86, 83, 95, 73, 78, 80, 85, 84, 32, 95, 105, 110, 112, 117, 116, 95, 59, 13, 10, 32, 32, 32, 32, 95, 105, 110, 112, 117, 116, 95, 46, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 32, 61, 32, 97, 95, 67, 79, 76, 79, 82, 48, 59, 13, 10, 32, 32, 32, 32, 95, 105, 110, 112, 
117, 116, 95, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 32, 61, 32, 97, 95, 84, 69, 88, 67, 79, 79, 82, 68, 48, 59, 13, 10, 32, 32, 32, 32, 95, 105, 110, 112, 117, 116, 95, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 32, 61, 32, 97, 95, 66, 
65, 84, 67, 72, 95, 83, 87, 73, 90, 90, 76, 69, 48, 59, 13, 10, 32, 32, 32, 32, 95, 105, 110, 112, 117, 116, 95, 46, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 53, 56, 32, 61, 32, 97, 95, 80, 79, 83, 73, 84, 73, 79, 78, 48, 59, 13, 10, 32, 32, 32, 32, 86, 83, 
95, 83, 84, 82, 69, 65, 77, 83, 32, 115, 116, 114, 101, 97, 109, 115, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 115, 46, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 53, 56, 32, 61, 32, 95, 105, 110, 112, 117, 116, 95, 46, 80, 111, 115, 105, 116, 105, 111, 110, 95, 
105, 100, 53, 56, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 115, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 32, 61, 32, 95, 105, 110, 112, 117, 116, 95, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 
101, 97, 109, 115, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 32, 61, 32, 95, 105, 110, 112, 117, 116, 95, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 115, 46, 67, 111, 108, 111, 114, 95, 105, 
100, 54, 48, 32, 61, 32, 95, 105, 110, 112, 117, 116, 95, 46, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 115, 46, 83, 104, 97, 100, 105, 110, 103, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 48, 32, 61, 32, 40, 115, 116, 
114, 101, 97, 109, 115, 46, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 53, 56, 32, 42, 32, 77, 97, 116, 114, 105, 120, 84, 114, 97, 110, 115, 102, 111, 114, 109, 95, 105, 100, 53, 57, 41, 59, 13, 10, 32, 32, 32, 32, 86, 83, 95, 79, 85, 84, 80, 85, 84, 32, 95, 111, 117, 116, 
112, 117, 116, 95, 59, 13, 10, 32, 32, 32, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 83, 104, 97, 100, 105, 110, 103, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 48, 32, 61, 32, 115, 116, 114, 101, 97, 109, 115, 46, 83, 104, 97, 100, 105, 110, 103, 80, 111, 115, 105, 116, 105, 111, 
110, 95, 105, 100, 48, 59, 13, 10, 32, 32, 32, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 32, 61, 32, 115, 116, 114, 101, 97, 109, 115, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 32, 32, 32, 95, 
111, 117, 116, 112, 117, 116, 95, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 32, 61, 32, 115, 116, 114, 101, 97, 109, 115, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 67, 111, 108, 
111, 114, 95, 105, 100, 54, 48, 32, 61, 32, 115, 116, 114, 101, 97, 109, 115, 46, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 59, 13, 10, 32, 32, 32, 32, 103, 108, 95, 80, 111, 115, 105, 116, 105, 111, 110, 32, 61, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 83, 104, 97, 100, 105, 110, 
103, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 48, 59, 13, 10, 32, 32, 32, 32, 118, 95, 66, 65, 84, 67, 72, 95, 83, 87, 73, 90, 90, 76, 69, 48, 32, 61, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 
32, 32, 32, 118, 95, 84, 69, 88, 67, 79, 79, 82, 68, 48, 32, 61, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 118, 95, 67, 79, 76, 79, 82, 48, 32, 61, 32, 95, 111, 117, 116, 112, 117, 116, 
95, 46, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 59, 13, 10, 32, 32, 32, 32, 103, 108, 95, 80, 111, 115, 105, 116, 105, 111, 110, 46, 122, 32, 61, 32, 103, 108, 95, 80, 111, 115, 105, 116, 105, 111, 110, 46, 122, 32, 42, 32, 50, 46, 48, 32, 45, 32, 103, 108, 95, 80, 111, 115, 105, 
116, 105, 111, 110, 46, 119, 59, 13, 10, 32, 32, 32, 32, 103, 108, 95, 80, 111, 115, 105, 116, 105, 111, 110, 46, 121, 32, 61, 32, 80, 97, 114, 97, 100, 111, 120, 70, 108, 105, 112, 82, 101, 110, 100, 101, 114, 116, 97, 114, 103, 101, 116, 32, 42, 32, 103, 108, 95, 80, 111, 115, 105, 116, 105, 
111, 110, 46, 121, 59, 13, 10, 125, 13, 10, 1, 119, 183, 11, 222, 71, 10, 100, 159, 179, 27, 116, 85, 210, 67, 36, 4, 0, 5, 0, 0, 0, 0, 159, 5, 0, 0, 35, 118, 101, 114, 115, 105, 111, 110, 32, 52, 50, 48, 13, 10, 13, 10, 111, 117, 116, 32, 118, 101, 99, 52, 32, 103, 108, 
95, 70, 114, 97, 103, 68, 97, 116, 97, 91, 49, 93, 59, 13, 10, 13, 10, 115, 116, 114, 117, 99, 116, 32, 80, 83, 95, 83, 84, 82, 69, 65, 77, 83, 32, 13, 10, 123, 13, 10, 32, 32, 32, 32, 102, 108, 111, 97, 116, 32, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 
10, 32, 32, 32, 32, 118, 101, 99, 50, 32, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 67, 111, 108, 111, 114, 84, 97, 
114, 103, 101, 116, 95, 105, 100, 49, 59, 13, 10, 125, 59, 13, 10, 115, 116, 114, 117, 99, 116, 32, 80, 83, 95, 79, 85, 84, 80, 85, 84, 32, 13, 10, 123, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 67, 111, 108, 111, 114, 84, 97, 114, 103, 101, 116, 95, 105, 100, 49, 59, 13, 10, 
125, 59, 13, 10, 115, 116, 114, 117, 99, 116, 32, 86, 83, 95, 79, 85, 84, 80, 85, 84, 32, 13, 10, 123, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 83, 104, 97, 100, 105, 110, 103, 80, 111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 48, 59, 13, 10, 32, 32, 32, 32, 102, 108, 111, 
97, 116, 32, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 50, 32, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 59, 13, 
10, 125, 59, 13, 10, 115, 116, 100, 49, 52, 48, 32, 117, 110, 105, 102, 111, 114, 109, 32, 79, 112, 101, 110, 103, 108, 70, 108, 97, 103, 115, 32, 123, 13, 10, 32, 32, 32, 32, 102, 108, 111, 97, 116, 32, 80, 97, 114, 97, 100, 111, 120, 70, 108, 105, 112, 82, 101, 110, 100, 101, 114, 116, 97, 
114, 103, 101, 116, 59, 13, 10, 125, 59, 13, 10, 117, 110, 105, 102, 111, 114, 109, 32, 115, 97, 109, 112, 108, 101, 114, 50, 68, 32, 84, 101, 120, 116, 117, 114, 101, 48, 95, 105, 100, 49, 51, 95, 83, 97, 109, 112, 108, 101, 114, 95, 105, 100, 51, 55, 59, 13, 10, 105, 110, 32, 102, 108, 111, 
97, 116, 32, 118, 95, 66, 65, 84, 67, 72, 95, 83, 87, 73, 90, 90, 76, 69, 48, 59, 13, 10, 105, 110, 32, 118, 101, 99, 50, 32, 118, 95, 84, 69, 88, 67, 79, 79, 82, 68, 48, 59, 13, 10, 105, 110, 32, 118, 101, 99, 52, 32, 118, 95, 67, 79, 76, 79, 82, 48, 59, 13, 10, 118, 
101, 99, 52, 32, 83, 104, 97, 100, 105, 110, 103, 95, 105, 100, 50, 40, 105, 110, 111, 117, 116, 32, 80, 83, 95, 83, 84, 82, 69, 65, 77, 83, 32, 115, 116, 114, 101, 97, 109, 115, 41, 13, 10, 123, 13, 10, 32, 32, 32, 32, 114, 101, 116, 117, 114, 110, 32, 116, 101, 120, 116, 117, 114, 101, 
40, 84, 101, 120, 116, 117, 114, 101, 48, 95, 105, 100, 49, 51, 95, 83, 97, 109, 112, 108, 101, 114, 95, 105, 100, 51, 55, 44, 32, 115, 116, 114, 101, 97, 109, 115, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 41, 59, 13, 10, 125, 13, 10, 118, 101, 99, 52, 32, 83, 104, 
97, 100, 105, 110, 103, 95, 105, 100, 51, 40, 105, 110, 111, 117, 116, 32, 80, 83, 95, 83, 84, 82, 69, 65, 77, 83, 32, 115, 116, 114, 101, 97, 109, 115, 41, 13, 10, 123, 13, 10, 32, 32, 32, 32, 118, 101, 99, 52, 32, 115, 119, 105, 122, 122, 108, 101, 67, 111, 108, 111, 114, 32, 61, 32, 
115, 116, 114, 101, 97, 109, 115, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 32, 61, 61, 32, 102, 108, 111, 97, 116, 40, 48, 41, 32, 63, 32, 83, 104, 97, 100, 105, 110, 103, 95, 105, 100, 50, 40, 115, 116, 114, 101, 97, 109, 115, 41, 32, 58, 32, 83, 104, 97, 100, 105, 110, 
103, 95, 105, 100, 50, 40, 115, 116, 114, 101, 97, 109, 115, 41, 46, 114, 114, 114, 114, 59, 13, 10, 32, 32, 32, 32, 114, 101, 116, 117, 114, 110, 32, 115, 119, 105, 122, 122, 108, 101, 67, 111, 108, 111, 114, 32, 42, 32, 115, 116, 114, 101, 97, 109, 115, 46, 67, 111, 108, 111, 114, 95, 105, 100, 
54, 48, 59, 13, 10, 125, 13, 10, 118, 111, 105, 100, 32, 109, 97, 105, 110, 40, 41, 13, 10, 123, 13, 10, 32, 32, 32, 32, 86, 83, 95, 79, 85, 84, 80, 85, 84, 32, 95, 105, 110, 112, 117, 116, 95, 59, 13, 10, 32, 32, 32, 32, 95, 105, 110, 112, 117, 116, 95, 46, 67, 111, 108, 111, 
114, 95, 105, 100, 54, 48, 32, 61, 32, 118, 95, 67, 79, 76, 79, 82, 48, 59, 13, 10, 32, 32, 32, 32, 95, 105, 110, 112, 117, 116, 95, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 32, 61, 32, 118, 95, 84, 69, 88, 67, 79, 79, 82, 68, 48, 59, 13, 10, 32, 32, 
32, 32, 95, 105, 110, 112, 117, 116, 95, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 32, 61, 32, 118, 95, 66, 65, 84, 67, 72, 95, 83, 87, 73, 90, 90, 76, 69, 48, 59, 13, 10, 32, 32, 32, 32, 95, 105, 110, 112, 117, 116, 95, 46, 83, 104, 97, 100, 105, 110, 103, 80, 
111, 115, 105, 116, 105, 111, 110, 95, 105, 100, 48, 32, 61, 32, 103, 108, 95, 70, 114, 97, 103, 67, 111, 111, 114, 100, 59, 13, 10, 32, 32, 32, 32, 80, 83, 95, 83, 84, 82, 69, 65, 77, 83, 32, 115, 116, 114, 101, 97, 109, 115, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 
115, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 32, 61, 32, 95, 105, 110, 112, 117, 116, 95, 46, 83, 119, 105, 122, 122, 108, 101, 95, 105, 100, 54, 49, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 115, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 
53, 32, 61, 32, 95, 105, 110, 112, 117, 116, 95, 46, 84, 101, 120, 67, 111, 111, 114, 100, 95, 105, 100, 53, 53, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 115, 46, 67, 111, 108, 111, 114, 95, 105, 100, 54, 48, 32, 61, 32, 95, 105, 110, 112, 117, 116, 95, 46, 67, 111, 108, 
111, 114, 95, 105, 100, 54, 48, 59, 13, 10, 32, 32, 32, 32, 115, 116, 114, 101, 97, 109, 115, 46, 67, 111, 108, 111, 114, 84, 97, 114, 103, 101, 116, 95, 105, 100, 49, 32, 61, 32, 83, 104, 97, 100, 105, 110, 103, 95, 105, 100, 51, 40, 115, 116, 114, 101, 97, 109, 115, 41, 59, 13, 10, 32, 
32, 32, 32, 80, 83, 95, 79, 85, 84, 80, 85, 84, 32, 95, 111, 117, 116, 112, 117, 116, 95, 59, 13, 10, 32, 32, 32, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 67, 111, 108, 111, 114, 84, 97, 114, 103, 101, 116, 95, 105, 100, 49, 32, 61, 32, 115, 116, 114, 101, 97, 109, 115, 46, 67, 
111, 108, 111, 114, 84, 97, 114, 103, 101, 116, 95, 105, 100, 49, 59, 13, 10, 32, 32, 32, 32, 103, 108, 95, 70, 114, 97, 103, 68, 97, 116, 97, 91, 48, 93, 32, 61, 32, 95, 111, 117, 116, 112, 117, 116, 95, 46, 67, 111, 108, 111, 114, 84, 97, 114, 103, 101, 116, 95, 105, 100, 49, 59, 13, 
10, 125, 13, 10, 1, 31, 29, 37, 179, 3, 196, 211, 191, 228, 175, 112, 82, 206, 57, 210, 206, 111, 18, 224, 208, 139, 218, 209, 8, 
        };
    }
}
#endif
