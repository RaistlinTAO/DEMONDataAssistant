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
//        Module  Name:                   ArchiveOpenCallback.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion


#if MONO
using SevenZip.Mono;
using SevenZip.Mono.COM;
#endif

namespace SevenZip
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    #endregion

#if UNMANAGED
    /// <summary>
    ///   Callback to handle the archive opening
    /// </summary>
    internal sealed class ArchiveOpenCallback : CallbackBase, IArchiveOpenCallback, IArchiveOpenVolumeCallback,
                                                ICryptoGetTextPassword, IDisposable
    {
        private readonly List<string> _volumeFileNames = new List<string>();
        private FileInfo _fileInfo;

        private Dictionary<string, InStreamWrapper> _wrappers =
            new Dictionary<string, InStreamWrapper>();

        /// <summary>
        ///   Initializes a new instance of the ArchiveOpenCallback class.
        /// </summary>
        /// <param name = "fileName">The archive file name.</param>
        public ArchiveOpenCallback(string fileName)
        {
            Init(fileName);
        }

        /// <summary>
        ///   Initializes a new instance of the ArchiveOpenCallback class.
        /// </summary>
        /// <param name = "fileName">The archive file name.</param>
        /// <param name = "password">Password for the archive.</param>
        public ArchiveOpenCallback(string fileName, string password) : base(password)
        {
            Init(fileName);
        }

        /// <summary>
        ///   Gets the list of volume file names.
        /// </summary>
        public IList<string> VolumeFileNames
        {
            get { return _volumeFileNames; }
        }

        #region IArchiveOpenCallback Members

        public void SetTotal(IntPtr files, IntPtr bytes)
        {
        }

        public void SetCompleted(IntPtr files, IntPtr bytes)
        {
        }

        #endregion

        #region IArchiveOpenVolumeCallback Members

        public int GetProperty(ItemPropId propId, ref PropVariant value)
        {
            switch (propId)
            {
                case ItemPropId.Name:
                    value.VarType = VarEnum.VT_BSTR;
                    value.Value = Marshal.StringToBSTR(_fileInfo.FullName);
                    break;
                case ItemPropId.IsDirectory:
                    value.VarType = VarEnum.VT_BOOL;
                    value.UInt64Value = (byte) (_fileInfo.Attributes & FileAttributes.Directory);
                    break;
                case ItemPropId.Size:
                    value.VarType = VarEnum.VT_UI8;
                    value.UInt64Value = (UInt64) _fileInfo.Length;
                    break;
                case ItemPropId.Attributes:
                    value.VarType = VarEnum.VT_UI4;
                    value.UInt32Value = (uint) _fileInfo.Attributes;
                    break;
                case ItemPropId.CreationTime:
                    value.VarType = VarEnum.VT_FILETIME;
                    value.Int64Value = _fileInfo.CreationTime.ToFileTime();
                    break;
                case ItemPropId.LastAccessTime:
                    value.VarType = VarEnum.VT_FILETIME;
                    value.Int64Value = _fileInfo.LastAccessTime.ToFileTime();
                    break;
                case ItemPropId.LastWriteTime:
                    value.VarType = VarEnum.VT_FILETIME;
                    value.Int64Value = _fileInfo.LastWriteTime.ToFileTime();
                    break;
            }
            return 0;
        }

        public int GetStream(string name, out IInStream inStream)
        {
            if (!File.Exists(name))
            {
                name = Path.Combine(Path.GetDirectoryName(_fileInfo.FullName), name);
                if (!File.Exists(name))
                {
                    inStream = null;
                    AddException(
                        new FileNotFoundException("The volume \"" + name +
                                                  "\" was not found. Extraction can be impossible."));
                    return 1;
                }
            }
            _volumeFileNames.Add(name);
            if (_wrappers.ContainsKey(name))
            {
                inStream = _wrappers[name];
            }
            else
            {
                try
                {
                    var wrapper = new InStreamWrapper(
                        new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), true);
                    _wrappers.Add(name, wrapper);
                    inStream = wrapper;
                }
                catch (Exception)
                {
                    AddException(
                        new FileNotFoundException("Failed to open the volume \"" + name +
                                                  "\". Extraction is impossible."));
                    inStream = null;
                    return 1;
                }
            }
            return 0;
        }

        #endregion

        #region ICryptoGetTextPassword Members

        /// <summary>
        ///   Sets password for the archive
        /// </summary>
        /// <param name = "password">Password for the archive</param>
        /// <returns>Zero if everything is OK</returns>
        public int CryptoGetTextPassword(out string password)
        {
            password = Password;
            return 0;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_wrappers != null)
            {
                foreach (InStreamWrapper wrap in _wrappers.Values)
                {
                    wrap.Dispose();
                }
                _wrappers = null;
            }
#if MONO
			libp7zInvokerRaw.FreeObject(Handle);
#endif
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///   Performs the common initialization.
        /// </summary>
        /// <param name = "fileName">Volume file name.</param>
        private void Init(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                _fileInfo = new FileInfo(fileName);
                _volumeFileNames.Add(fileName);
                if (fileName.EndsWith("001"))
                {
                    int index = 2;
                    string baseName = fileName.Substring(0, fileName.Length - 3);
                    string volName = baseName + (index > 99
                                                     ? index.ToString()
                                                     : index > 9 ? "0" + index : "00" + index);
                    while (File.Exists(volName))
                    {
                        _volumeFileNames.Add(volName);
                        index++;
                        volName = baseName + (index > 99
                                                  ? index.ToString()
                                                  : index > 9 ? "0" + index : "00" + index);
                    }
                }
            }
        }
    }
#endif
}