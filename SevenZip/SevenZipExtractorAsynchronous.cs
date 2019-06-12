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
//        Module  Name:                   SevenZipExtractorAsynchronous.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip
{
    #region

    using System;
    using System.IO;

    #endregion

#if DOTNET20

#else
    using System.Windows.Threading;
#endif

    partial class SevenZipExtractor
    {
        #region Asynchronous core methods

        /// <summary>
        ///   Recreates the instance of the SevenZipExtractor class.
        ///   Used in asynchronous methods.
        /// </summary>
        private void RecreateInstanceIfNeeded()
        {
            if (NeedsToBeRecreated)
            {
                NeedsToBeRecreated = false;
                Stream backupStream = null;
                string backupFileName = null;
                if (String.IsNullOrEmpty(_fileName))
                {
                    backupStream = _inStream;
                }
                else
                {
                    backupFileName = _fileName;
                }
                CommonDispose();
                if (backupStream == null)
                {
                    Init(backupFileName);
                }
                else
                {
                    Init(backupStream);
                }
            }
        }

        internal override void SaveContext(
#if !DOTNET20
            DispatcherPriority eventPriority
#if CS4
            = DispatcherPriority.Normal
#endif
#endif
            )
        {
            DisposedCheck();
            _asynchronousDisposeLock = true;
            base.SaveContext(
#if !DOTNET20
                eventPriority
#endif
                );
        }

        internal override void ReleaseContext()
        {
            base.ReleaseContext();
            _asynchronousDisposeLock = false;
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   The delegate to use in BeginExtractArchive.
        /// </summary>
        /// <param name = "directory">The directory where the files are to be unpacked.</param>
        private delegate void ExtractArchiveDelegate(string directory);

        /// <summary>
        ///   The delegate to use in BeginExtractFile (by file name).
        /// </summary>
        /// <param name = "fileName">The file full name in the archive file table.</param>
        /// <param name = "stream">The stream where the file is to be unpacked.</param>
        private delegate void ExtractFileByFileNameDelegate(string fileName, Stream stream);

        /// <summary>
        ///   The delegate to use in BeginExtractFile (by index).
        /// </summary>
        /// <param name = "index">Index in the archive file table.</param>
        /// <param name = "stream">The stream where the file is to be unpacked.</param>
        private delegate void ExtractFileByIndexDelegate(int index, Stream stream);

        /// <summary>
        ///   The delegate to use in BeginExtractFiles(string directory, params int[] indexes).
        /// </summary>
        /// <param name = "indexes">indexes of the files in the archive file table.</param>
        /// <param name = "directory">Directory where the files are to be unpacked.</param>
        private delegate void ExtractFiles1Delegate(string directory, int[] indexes);

        /// <summary>
        ///   The delegate to use in BeginExtractFiles(string directory, params string[] fileNames).
        /// </summary>
        /// <param name = "fileNames">Full file names in the archive file table.</param>
        /// <param name = "directory">Directory where the files are to be unpacked.</param>
        private delegate void ExtractFiles2Delegate(string directory, string[] fileNames);

        /// <summary>
        ///   The delegate to use in BeginExtractFiles(ExtractFileCallback extractFileCallback).
        /// </summary>
        /// <param name = "extractFileCallback">The callback to call for each file in the archive.</param>
        private delegate void ExtractFiles3Delegate(ExtractFileCallback extractFileCallback);

        #endregion

#if !DOTNET20
        /// <summary>
        /// Unpacks the whole archive asynchronously to the specified directory name at the specified priority.
        /// </summary>
        /// <param name="directory">The directory where the files are to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
#else
        /// <summary>
        /// Unpacks the whole archive asynchronously to the specified directory name at the specified priority.
        /// </summary>
        /// <param name="directory">The directory where the files are to be unpacked.</param>
#endif

        public void BeginExtractArchive(string directory
#if !DOTNET20
            , DispatcherPriority eventPriority
#if CS4
            = DispatcherPriority.Normal
#endif
#endif
            )
        {
            SaveContext(
#if !DOTNET20
                eventPriority
#endif
                );
            (new ExtractArchiveDelegate(ExtractArchive)).BeginInvoke(directory, AsyncCallbackImplementation, this);
        }

#if !DOTNET20
        /// <summary>
        /// Unpacks the file asynchronously by its name to the specified stream.
        /// </summary>
        /// <param name="fileName">The file full name in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
#else
        /// <summary>
        /// Unpacks the file asynchronously by its name to the specified stream.
        /// </summary>
        /// <param name="fileName">The file full name in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
#endif

        public void BeginExtractFile(string fileName, Stream stream
#if !DOTNET20
            , DispatcherPriority eventPriority
#if CS4
            = DispatcherPriority.Normal
#endif
#endif
            )
        {
            SaveContext(
#if !DOTNET20
                eventPriority
#endif
                );
            (new ExtractFileByFileNameDelegate(ExtractFile)).BeginInvoke(fileName, stream, AsyncCallbackImplementation,
                                                                         this);
        }

#if !DOTNET20
        /// <summary>
        /// Unpacks the file asynchronously by its index to the specified stream.
        /// </summary>
        /// <param name="index">Index in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
#else
        /// <summary>
        /// Unpacks the file asynchronously by its index to the specified stream.
        /// </summary>
        /// <param name="index">Index in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
#endif

        public void BeginExtractFile(int index, Stream stream
#if !DOTNET20
            , DispatcherPriority eventPriority
#if CS4
            = DispatcherPriority.Normal
#endif
#endif
            )
        {
            SaveContext(
#if !DOTNET20
                eventPriority
#endif
                );
            (new ExtractFileByIndexDelegate(ExtractFile)).BeginInvoke(index, stream, AsyncCallbackImplementation, this);
        }

#if !DOTNET20
        /// <summary>
        /// Unpacks files asynchronously by their indices to the specified directory.
        /// </summary>
        /// <param name="indexes">indexes of the files in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
#else
        /// <summary>
        /// Unpacks files asynchronously by their indices to the specified directory.
        /// </summary>
        /// <param name="indexes">indexes of the files in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
#endif

        public void BeginExtractFiles(string directory
#if !DOTNET20
            , DispatcherPriority eventPriority 
#if CS4
            = DispatcherPriority.Normal
#endif
#endif
                                      , params int[] indexes)
        {
            SaveContext(
#if !DOTNET20
                eventPriority
#endif
                );
            (new ExtractFiles1Delegate(ExtractFiles)).BeginInvoke(directory, indexes, AsyncCallbackImplementation, this);
        }

#if !DOTNET20
        /// <summary>
        /// Unpacks files asynchronously by their full names to the specified directory.
        /// </summary>
        /// <param name="fileNames">Full file names in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
#else
        /// <summary>
        /// Unpacks files asynchronously by their full names to the specified directory.
        /// </summary>
        /// <param name="fileNames">Full file names in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
#endif

        public void BeginExtractFiles(string directory
#if !DOTNET20
            , DispatcherPriority eventPriority 
#if CS4
            = DispatcherPriority.Normal
#endif
#endif
                                      , params string[] fileNames)
        {
            SaveContext(
#if !DOTNET20
                eventPriority
#endif
                );
            (new ExtractFiles2Delegate(ExtractFiles)).BeginInvoke(directory, fileNames, AsyncCallbackImplementation,
                                                                  this);
        }

#if !DOTNET20
        /// <summary>
        /// Extracts files from the archive asynchronously, giving a callback the choice what
        /// to do with each file. The order of the files is given by the archive.
        /// 7-Zip (and any other solid) archives are NOT supported.
        /// </summary>
        /// <param name="extractFileCallback">The callback to call for each file in the archive.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
#else
        /// <summary>
        /// Extracts files from the archive asynchronously, giving a callback the choice what
        /// to do with each file. The order of the files is given by the archive.
        /// 7-Zip (and any other solid) archives are NOT supported.
        /// </summary>
        /// <param name="extractFileCallback">The callback to call for each file in the archive.</param>
#endif

        public void BeginExtractFiles(ExtractFileCallback extractFileCallback
#if !DOTNET20
            , DispatcherPriority eventPriority 
#if CS4
            = DispatcherPriority.Normal
#endif
#endif
            )
        {
            SaveContext(
#if !DOTNET20
                eventPriority
#endif
                );
            (new ExtractFiles3Delegate(ExtractFiles)).BeginInvoke(extractFileCallback, AsyncCallbackImplementation, this);
        }
    }
}