﻿#region SOURCE INFORMATION

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
//        Module  Name:                   Formats.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;

    #endregion

#if UNMANAGED
    /// <summary>
    ///   Readable archive format enumeration.
    /// </summary>
    public enum InArchiveFormat
    {
        /// <summary>
        ///   Open 7-zip archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/7-zip">Wikipedia information</a>
        /// </remarks>
        SevenZip,

        /// <summary>
        ///   Proprietary Arj archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/ARJ">Wikipedia information</a>
        /// </remarks>
        Arj,

        /// <summary>
        ///   Open Bzip2 archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Bzip2">Wikipedia information</a>
        /// </remarks>
        BZip2,

        /// <summary>
        ///   Microsoft cabinet archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Cabinet_(file_format)">Wikipedia information</a>
        /// </remarks>
        Cab,

        /// <summary>
        ///   Microsoft Compiled HTML Help file format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Microsoft_Compiled_HTML_Help">Wikipedia information</a>
        /// </remarks>
        Chm,

        /// <summary>
        ///   Microsoft Compound file format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Compound_File_Binary_Format">Wikipedia information</a>
        /// </remarks>
        Compound,

        /// <summary>
        ///   Open Cpio archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Cpio">Wikipedia information</a>
        /// </remarks>
        Cpio,

        /// <summary>
        ///   Open Debian software package format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Deb_(file_format)">Wikipedia information</a>
        /// </remarks>
        Deb,

        /// <summary>
        ///   Open Gzip archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Gzip">Wikipedia information</a>
        /// </remarks>
        GZip,

        /// <summary>
        ///   Open ISO disk image format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/ISO_image">Wikipedia information</a>
        /// </remarks>
        Iso,

        /// <summary>
        ///   Open Lzh archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Lzh">Wikipedia information</a>
        /// </remarks>
        Lzh,

        /// <summary>
        ///   Open core 7-zip Lzma raw archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Lzma">Wikipedia information</a>
        /// </remarks>
        Lzma,

        /// <summary>
        ///   Nullsoft installation package format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/NSIS">Wikipedia information</a>
        /// </remarks>
        Nsis,

        /// <summary>
        ///   RarLab Rar archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Rar">Wikipedia information</a>
        /// </remarks>
        Rar,

        /// <summary>
        ///   Open Rpm software package format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/RPM_Package_Manager">Wikipedia information</a>
        /// </remarks>
        Rpm,

        /// <summary>
        ///   Open split file format.
        /// </summary>
        /// <remarks>
        ///   <a href = "?">Wikipedia information</a>
        /// </remarks>
        Split,

        /// <summary>
        ///   Open Tar archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Tar_(file_format)">Wikipedia information</a>
        /// </remarks>
        Tar,

        /// <summary>
        ///   Microsoft Windows Imaging disk image format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Windows_Imaging_Format">Wikipedia information</a>
        /// </remarks>
        Wim,

        /// <summary>
        ///   Open LZW archive format; implemented in "compress" program; also known as "Z" archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Compress">Wikipedia information</a>
        /// </remarks>
        Lzw,

        /// <summary>
        ///   Open Zip archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/ZIP_(file_format)">Wikipedia information</a>
        /// </remarks>
        Zip,

        /// <summary>
        ///   Open Udf disk image format.
        /// </summary>
        Udf,

        /// <summary>
        ///   Xar open source archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Xar_(archiver)">Wikipedia information</a>
        /// </remarks>
        Xar,

        /// <summary>
        ///   Mub
        /// </summary>
        Mub,

        /// <summary>
        ///   Macintosh Disk Image on CD.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/HFS_Plus">Wikipedia information</a>
        /// </remarks>
        Hfs,

        /// <summary>
        ///   Apple Mac OS X Disk Copy Disk Image format.
        /// </summary>
        Dmg,

        /// <summary>
        ///   Open Xz archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Xz">Wikipedia information</a>
        /// </remarks>
        XZ,

        /// <summary>
        ///   MSLZ archive format.
        /// </summary>
        Mslz,

        /// <summary>
        ///   Flash video format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Flv">Wikipedia information</a>
        /// </remarks>
        Flv,

        /// <summary>
        ///   Shockwave Flash format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Swf">Wikipedia information</a>
        /// </remarks>
        Swf,

        /// <summary>
        ///   Windows PE executable format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Portable_Executable">Wikipedia information</a>
        /// </remarks>
        PE,

        /// <summary>
        ///   Linux executable Elf format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Executable_and_Linkable_Format">Wikipedia information</a>
        /// </remarks>
        Elf,

        /// <summary>
        ///   Windows Installer Database.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Windows_Installer">Wikipedia information</a>
        /// </remarks>
        Msi,

        /// <summary>
        ///   Microsoft virtual hard disk file format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/VHD_%28file_format%29">Wikipedia information</a>
        /// </remarks>
        Vhd
    }

#if COMPRESS
    /// <summary>
    ///   Writable archive format enumeration.
    /// </summary>
    public enum OutArchiveFormat
    {
        /// <summary>
        ///   Open 7-zip archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/7-zip">Wikipedia information</a>
        /// </remarks>
        SevenZip,

        /// <summary>
        ///   Open Zip archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/ZIP_(file_format)">Wikipedia information</a>
        /// </remarks>
        Zip,

        /// <summary>
        ///   Open Gzip archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Gzip">Wikipedia information</a>
        /// </remarks>
        GZip,

        /// <summary>
        ///   Open Bzip2 archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Bzip2">Wikipedia information</a>
        /// </remarks>
        BZip2,

        /// <summary>
        ///   Open Tar archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Tar_(file_format)">Wikipedia information</a>
        /// </remarks>
        Tar,

        /// <summary>
        ///   Open Xz archive format.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Xz">Wikipedia information</a>
        /// </remarks>
        XZ
    }

    /// <summary>
    ///   Compression level enumeration
    /// </summary>
    public enum CompressionLevel
    {
        /// <summary>
        ///   No compression
        /// </summary>
        None,

        /// <summary>
        ///   Very low compression level
        /// </summary>
        Fast,

        /// <summary>
        ///   Low compression level
        /// </summary>
        Low,

        /// <summary>
        ///   Normal compression level (default)
        /// </summary>
        Normal,

        /// <summary>
        ///   High compression level
        /// </summary>
        High,

        /// <summary>
        ///   The best compression level (slow)
        /// </summary>
        Ultra
    }

    /// <summary>
    ///   Compression method enumeration.
    /// </summary>
    /// <remarks>
    ///   Some methods are applicable only to Zip format, some - only to 7-zip.
    /// </remarks>
    public enum CompressionMethod
    {
        /// <summary>
        ///   Zip or 7-zip|no compression method.
        /// </summary>
        Copy,

        /// <summary>
        ///   Zip|Deflate method.
        /// </summary>
        Deflate,

        /// <summary>
        ///   Zip|Deflate64 method.
        /// </summary>
        Deflate64,

        /// <summary>
        ///   Zip or 7-zip|Bzip2 method.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Cabinet_(file_format)">Wikipedia information</a>
        /// </remarks>
        BZip2,

        /// <summary>
        ///   Zip or 7-zip|LZMA method based on Lempel-Ziv algorithm, it is default for 7-zip.
        /// </summary>
        Lzma,

        /// <summary>
        ///   7-zip|LZMA version 2, LZMA with improved multithreading and usually slight archive size decrease.
        /// </summary>
        Lzma2,

        /// <summary>
        ///   Zip or 7-zip|PPMd method based on Dmitry Shkarin's PPMdH source code, very efficient for compressing texts.
        /// </summary>
        /// <remarks>
        ///   <a href = "http://en.wikipedia.org/wiki/Prediction_by_Partial_Matching">Wikipedia information</a>
        /// </remarks>
        Ppmd,

        /// <summary>
        ///   No method change.
        /// </summary>
        Default
    }
#endif

    /// <summary>
    ///   Archive format routines
    /// </summary>
    public static class Formats
    {
        /*/// <summary>
        /// Gets the max value of the specified enum type.
        /// </summary>
        /// <param name="type">Type of the enum</param>
        /// <returns>Max value</returns>
        internal static int GetMaxValue(Type type)
        {
            List<int> enumList = new List<int>((IEnumerable<int>)Enum.GetValues(type));
            enumList.Sort();
            return enumList[enumList.Count - 1];
        }*/

        /// <summary>
        ///   List of readable archive format interface guids for 7-zip COM interop.
        /// </summary>
        internal static readonly Dictionary<InArchiveFormat, Guid> InFormatGuids =
            new Dictionary<InArchiveFormat, Guid>(20) 
                #region InFormatGuids initialization

                {
                    {InArchiveFormat.SevenZip, new Guid("23170f69-40c1-278a-1000-000110070000")},
                    {InArchiveFormat.Arj, new Guid("23170f69-40c1-278a-1000-000110040000")},
                    {InArchiveFormat.BZip2, new Guid("23170f69-40c1-278a-1000-000110020000")},
                    {InArchiveFormat.Cab, new Guid("23170f69-40c1-278a-1000-000110080000")},
                    {InArchiveFormat.Chm, new Guid("23170f69-40c1-278a-1000-000110e90000")},
                    {InArchiveFormat.Compound, new Guid("23170f69-40c1-278a-1000-000110e50000")},
                    {InArchiveFormat.Cpio, new Guid("23170f69-40c1-278a-1000-000110ed0000")},
                    {InArchiveFormat.Deb, new Guid("23170f69-40c1-278a-1000-000110ec0000")},
                    {InArchiveFormat.GZip, new Guid("23170f69-40c1-278a-1000-000110ef0000")},
                    {InArchiveFormat.Iso, new Guid("23170f69-40c1-278a-1000-000110e70000")},
                    {InArchiveFormat.Lzh, new Guid("23170f69-40c1-278a-1000-000110060000")},
                    {InArchiveFormat.Lzma, new Guid("23170f69-40c1-278a-1000-0001100a0000")},
                    {InArchiveFormat.Nsis, new Guid("23170f69-40c1-278a-1000-000110090000")},
                    {InArchiveFormat.Rar, new Guid("23170f69-40c1-278a-1000-000110030000")},
                    {InArchiveFormat.Rpm, new Guid("23170f69-40c1-278a-1000-000110eb0000")},
                    {InArchiveFormat.Split, new Guid("23170f69-40c1-278a-1000-000110ea0000")},
                    {InArchiveFormat.Tar, new Guid("23170f69-40c1-278a-1000-000110ee0000")},
                    {InArchiveFormat.Wim, new Guid("23170f69-40c1-278a-1000-000110e60000")},
                    {InArchiveFormat.Lzw, new Guid("23170f69-40c1-278a-1000-000110050000")},
                    {InArchiveFormat.Zip, new Guid("23170f69-40c1-278a-1000-000110010000")},
                    {InArchiveFormat.Udf, new Guid("23170f69-40c1-278a-1000-000110E00000")},
                    {InArchiveFormat.Xar, new Guid("23170f69-40c1-278a-1000-000110E10000")},
                    {InArchiveFormat.Mub, new Guid("23170f69-40c1-278a-1000-000110E20000")},
                    {InArchiveFormat.Hfs, new Guid("23170f69-40c1-278a-1000-000110E30000")},
                    {InArchiveFormat.Dmg, new Guid("23170f69-40c1-278a-1000-000110E40000")},
                    {InArchiveFormat.XZ, new Guid("23170f69-40c1-278a-1000-0001100C0000")},
                    {InArchiveFormat.Mslz, new Guid("23170f69-40c1-278a-1000-000110D50000")},
                    {InArchiveFormat.PE, new Guid("23170f69-40c1-278a-1000-000110DD0000")},
                    {InArchiveFormat.Elf, new Guid("23170f69-40c1-278a-1000-000110DE0000")},
                    {InArchiveFormat.Swf, new Guid("23170f69-40c1-278a-1000-000110D70000")},
                    {InArchiveFormat.Vhd, new Guid("23170f69-40c1-278a-1000-000110DC0000")}
                };

        #endregion

#if COMPRESS
        /// <summary>
        ///   List of writable archive format interface guids for 7-zip COM interop.
        /// </summary>
        internal static readonly Dictionary<OutArchiveFormat, Guid> OutFormatGuids =
            new Dictionary<OutArchiveFormat, Guid>(2)
                #region OutFormatGuids initialization

                {
                    {OutArchiveFormat.SevenZip, new Guid("23170f69-40c1-278a-1000-000110070000")},
                    {OutArchiveFormat.Zip, new Guid("23170f69-40c1-278a-1000-000110010000")},
                    {OutArchiveFormat.BZip2, new Guid("23170f69-40c1-278a-1000-000110020000")},
                    {OutArchiveFormat.GZip, new Guid("23170f69-40c1-278a-1000-000110ef0000")},
                    {OutArchiveFormat.Tar, new Guid("23170f69-40c1-278a-1000-000110ee0000")},
                    {OutArchiveFormat.XZ, new Guid("23170f69-40c1-278a-1000-0001100C0000")},
                };

        #endregion

        internal static readonly Dictionary<CompressionMethod, string> MethodNames =
            new Dictionary<CompressionMethod, string>(6)
                #region MethodNames initialization

                {
                    {CompressionMethod.Copy, "Copy"},
                    {CompressionMethod.Deflate, "Deflate"},
                    {CompressionMethod.Deflate64, "Deflate64"},
                    {CompressionMethod.Lzma, "LZMA"},
                    {CompressionMethod.Lzma2, "LZMA2"},
                    {CompressionMethod.Ppmd, "PPMd"},
                    {CompressionMethod.BZip2, "BZip2"}
                };

        #endregion

        internal static readonly Dictionary<OutArchiveFormat, InArchiveFormat> InForOutFormats =
            new Dictionary<OutArchiveFormat, InArchiveFormat>(6)
                #region InForOutFormats initialization

                {
                    {OutArchiveFormat.SevenZip, InArchiveFormat.SevenZip},
                    {OutArchiveFormat.GZip, InArchiveFormat.GZip},
                    {OutArchiveFormat.BZip2, InArchiveFormat.BZip2},
                    {OutArchiveFormat.Tar, InArchiveFormat.Tar},
                    {OutArchiveFormat.XZ, InArchiveFormat.XZ},
                    {OutArchiveFormat.Zip, InArchiveFormat.Zip}
                };

        #endregion

#endif

        /// <summary>
        ///   List of archive formats corresponding to specific extensions
        /// </summary>
        private static readonly Dictionary<string, InArchiveFormat> InExtensionFormats =
            new Dictionary<string, InArchiveFormat>
                #region InExtensionFormats initialization

                {
                    {"7z", InArchiveFormat.SevenZip},
                    {"gz", InArchiveFormat.GZip},
                    {"tar", InArchiveFormat.Tar},
                    {"rar", InArchiveFormat.Rar},
                    {"zip", InArchiveFormat.Zip},
                    {"lzma", InArchiveFormat.Lzma},
                    {"lzh", InArchiveFormat.Lzh},
                    {"arj", InArchiveFormat.Arj},
                    {"bz2", InArchiveFormat.BZip2},
                    {"cab", InArchiveFormat.Cab},
                    {"chm", InArchiveFormat.Chm},
                    {"deb", InArchiveFormat.Deb},
                    {"iso", InArchiveFormat.Iso},
                    {"rpm", InArchiveFormat.Rpm},
                    {"wim", InArchiveFormat.Wim},
                    {"udf", InArchiveFormat.Udf},
                    {"mub", InArchiveFormat.Mub},
                    {"xar", InArchiveFormat.Xar},
                    {"hfs", InArchiveFormat.Hfs},
                    {"dmg", InArchiveFormat.Dmg},
                    {"Z", InArchiveFormat.Lzw},
                    {"xz", InArchiveFormat.XZ},
                    {"flv", InArchiveFormat.Flv},
                    {"swf", InArchiveFormat.Swf},
                    {"exe", InArchiveFormat.PE},
                    {"dll", InArchiveFormat.PE},
                    {"vhd", InArchiveFormat.Vhd}
                };

        #endregion

        /// <summary>
        ///   List of archive formats corresponding to specific signatures
        /// </summary>
        /// <remarks>
        ///   Based on the information at <a href = "http://www.garykessler.net/library/file_sigs.html">this site.</a>
        /// </remarks>
        internal static readonly Dictionary<string, InArchiveFormat> InSignatureFormats =
            new Dictionary<string, InArchiveFormat>
                #region InSignatureFormats initialization

                {
                    {"37-7A-BC-AF-27-1C", InArchiveFormat.SevenZip},
                    {"1F-8B-08", InArchiveFormat.GZip},
                    {"75-73-74-61-72", InArchiveFormat.Tar},
                    //257 byte offset
                    {"52-61-72-21-1A-07-00", InArchiveFormat.Rar},
                    {"50-4B-03-04", InArchiveFormat.Zip},
                    {"5D-00-00-40-00", InArchiveFormat.Lzma},
                    {"2D-6C-68", InArchiveFormat.Lzh},
                    //^ 2 byte offset
                    {"1F-9D-90", InArchiveFormat.Lzw},
                    {"60-EA", InArchiveFormat.Arj},
                    {"42-5A-68", InArchiveFormat.BZip2},
                    {"4D-53-43-46", InArchiveFormat.Cab},
                    {"49-54-53-46", InArchiveFormat.Chm},
                    {"21-3C-61-72-63-68-3E-0A-64-65-62-69-61-6E-2D-62-69-6E-61-72-79", InArchiveFormat.Deb},
                    {"43-44-30-30-31", InArchiveFormat.Iso},
                    //^ 0x8001, 0x8801 or 0x9001 byte offset
                    {"ED-AB-EE-DB", InArchiveFormat.Rpm},
                    {"4D-53-57-49-4D-00-00-00", InArchiveFormat.Wim},
                    {"udf", InArchiveFormat.Udf},
                    {"mub", InArchiveFormat.Mub},
                    {"78-61-72-21", InArchiveFormat.Xar},
                    //0x400 byte offset
                    {"48-2B", InArchiveFormat.Hfs},
                    {"FD-37-7A-58-5A", InArchiveFormat.XZ},
                    {"46-4C-56", InArchiveFormat.Flv},
                    {"46-57-53", InArchiveFormat.Swf},
                    {"4D-5A", InArchiveFormat.PE},
                    {"7F-45-4C-46", InArchiveFormat.Elf},
                    {"78", InArchiveFormat.Dmg},
                    {"63-6F-6E-65-63-74-69-78", InArchiveFormat.Vhd}
                };

        #endregion

        internal static Dictionary<InArchiveFormat, string> InSignatureFormatsReversed;

        static Formats()
        {
            InSignatureFormatsReversed = new Dictionary<InArchiveFormat, string>(InSignatureFormats.Count);
            foreach (KeyValuePair<string, InArchiveFormat> pair in InSignatureFormats)
            {
                InSignatureFormatsReversed.Add(pair.Value, pair.Key);
            }
        }

        /// <summary>
        ///   Gets InArchiveFormat for specified archive file name
        /// </summary>
        /// <param name = "fileName">Archive file name</param>
        /// <param name = "reportErrors">Indicates whether to throw exceptions</param>
        /// <returns>InArchiveFormat recognized by the file name extension</returns>
        /// <exception cref = "System.ArgumentException" />
        public static InArchiveFormat FormatByFileName(string fileName, bool reportErrors)
        {
            if (String.IsNullOrEmpty(fileName) && reportErrors)
            {
                throw new ArgumentException("File name is null or empty string!");
            }
            string extension = Path.GetExtension(fileName).Substring(1);
            if (!InExtensionFormats.ContainsKey(extension) && reportErrors)
            {
                throw new ArgumentException("Extension \"" + extension +
                                            "\" is not a supported archive file name extension.");
            }
            return InExtensionFormats[extension];
        }
    }
#endif
}