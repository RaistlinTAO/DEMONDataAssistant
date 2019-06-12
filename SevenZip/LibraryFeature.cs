#region SOURCE INFORMATION

// COPYRIGHT LICENCE
// 
//  Copyright (c) 2011, D.E.M.O.N Organization
//  All rights reserved.
// 
//  Redistribution and use in source and binary forms, with or without modification,
//  are permitted provided that the following conditions are met:
// 
//      * Redistributions of source code must retain the above copyright notice,
//      this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright notice,
//      this list of conditions and the following disclaimer in the documentation
//      and/or other materials provided with the distribution.
//      * Neither D.E.M.O.N Organization nor its contributors
//      may be used to endorse or promote products derived from this
//      software without specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
//  FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
//  CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
//  OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//  THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// 
// 
// CODE DESCRIPTION
// 
//        Created by Raistlin.K @ D.E.M.O.N at  1:10  18/12/2011 .
//        E-Mail:                         DemonVK@Gmail.com
// 
//        Project Name:                   SevenZip
//        Module  Name:                   LibraryFeature.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip
{
    #region

    using System;

    #endregion

    /// <summary>
    ///   The set of features supported by the library.
    /// </summary>
    [Flags]
    [CLSCompliant(false)]
    public enum LibraryFeature : uint
    {
        /// <summary>
        ///   Default feature.
        /// </summary>
        None = 0,

        /// <summary>
        ///   The library can extract 7zip archives compressed with LZMA method.
        /// </summary>
        Extract7z = 0x1,

        /// <summary>
        ///   The library can extract 7zip archives compressed with LZMA2 method.
        /// </summary>
        Extract7zLZMA2 = 0x2,

        /// <summary>
        ///   The library can extract 7z archives compressed with all known methods.
        /// </summary>
        Extract7zAll = Extract7z | Extract7zLZMA2 | 0x4,

        /// <summary>
        ///   The library can extract zip archives.
        /// </summary>
        ExtractZip = 0x8,

        /// <summary>
        ///   The library can extract rar archives.
        /// </summary>
        ExtractRar = 0x10,

        /// <summary>
        ///   The library can extract gzip archives.
        /// </summary>
        ExtractGzip = 0x20,

        /// <summary>
        ///   The library can extract bzip2 archives.
        /// </summary>
        ExtractBzip2 = 0x40,

        /// <summary>
        ///   The library can extract tar archives.
        /// </summary>
        ExtractTar = 0x80,

        /// <summary>
        ///   The library can extract xz archives.
        /// </summary>
        ExtractXz = 0x100,

        /// <summary>
        ///   The library can extract all types of archives supported.
        /// </summary>
        ExtractAll = Extract7zAll | ExtractZip | ExtractRar | ExtractGzip | ExtractBzip2 | ExtractTar | ExtractXz,

        /// <summary>
        ///   The library can compress data to 7zip archives with LZMA method.
        /// </summary>
        Compress7z = 0x200,

        /// <summary>
        ///   The library can compress data to 7zip archives with LZMA2 method.
        /// </summary>
        Compress7zLZMA2 = 0x400,

        /// <summary>
        ///   The library can compress data to 7zip archives with all methods known.
        /// </summary>
        Compress7zAll = Compress7z | Compress7zLZMA2 | 0x800,

        /// <summary>
        ///   The library can compress data to tar archives.
        /// </summary>
        CompressTar = 0x1000,

        /// <summary>
        ///   The library can compress data to gzip archives.
        /// </summary>
        CompressGzip = 0x2000,

        /// <summary>
        ///   The library can compress data to bzip2 archives.
        /// </summary>
        CompressBzip2 = 0x4000,

        /// <summary>
        ///   The library can compress data to xz archives.
        /// </summary>
        CompressXz = 0x8000,

        /// <summary>
        ///   The library can compress data to zip archives.
        /// </summary>
        CompressZip = 0x10000,

        /// <summary>
        ///   The library can compress data to all types of archives supported.
        /// </summary>
        CompressAll = Compress7zAll | CompressTar | CompressGzip | CompressBzip2 | CompressXz | CompressZip,

        /// <summary>
        ///   The library can modify archives.
        /// </summary>
        Modify = 0x20000
    }
}