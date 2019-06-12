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
//        Module  Name:                   ArchiveEmulationStreamProxy.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip
{
    #region

    using System;
    using System.IO;

    #endregion

    /// <summary>
    ///   The Stream extension class to emulate the archive part of a stream.
    /// </summary>
    internal class ArchiveEmulationStreamProxy : Stream, IDisposable
    {
        /// <summary>
        ///   Initializes a new instance of the ArchiveEmulationStream class.
        /// </summary>
        /// <param name = "stream">The stream to wrap.</param>
        /// <param name = "offset">The stream offset.</param>
        public ArchiveEmulationStreamProxy(Stream stream, int offset)
        {
            Source = stream;
            Offset = offset;
            Source.Position = offset;
        }

        /// <summary>
        ///   Gets the file offset.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        ///   The source wrapped stream.
        /// </summary>
        public Stream Source { get; private set; }

        public override bool CanRead
        {
            get { return Source.CanRead; }
        }

        public override bool CanSeek
        {
            get { return Source.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return Source.CanWrite; }
        }

        public override long Length
        {
            get { return Source.Length - Offset; }
        }

        public override long Position
        {
            get { return Source.Position - Offset; }
            set { Source.Position = value; }
        }

        #region IDisposable Members

        public new void Dispose()
        {
            Source.Dispose();
        }

        #endregion

        public override void Flush()
        {
            Source.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Source.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return Source.Seek(origin == SeekOrigin.Begin ? offset + Offset : offset,
                               origin) - Offset;
        }

        public override void SetLength(long value)
        {
            Source.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Source.Write(buffer, offset, count);
        }

        public override void Close()
        {
            Source.Close();
        }
    }
}