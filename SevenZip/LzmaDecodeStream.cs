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
//        Module  Name:                   LzmaDecodeStream.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip
{
    #region

    using System;
    using System.IO;
    using SevenZip.Sdk.Compression.Lzma;

    #endregion

#if LZMA_STREAM
    /// <summary>
    ///   The stream which decompresses data with LZMA on the fly.
    /// </summary>
    public class LzmaDecodeStream : Stream
    {
        private readonly MemoryStream _buffer = new MemoryStream();
        private readonly Decoder _decoder = new Decoder();
        private readonly Stream _input;
        private byte[] _commonProperties;
        private bool _error;
        private bool _firstChunkRead;

        /// <summary>
        ///   Initializes a new instance of the LzmaDecodeStream class.
        /// </summary>
        /// <param name = "encodedStream">A compressed stream.</param>
        public LzmaDecodeStream(Stream encodedStream)
        {
            if (!encodedStream.CanRead)
            {
                throw new ArgumentException("The specified stream can not read.", "encodedStream");
            }
            _input = encodedStream;
        }

        /// <summary>
        ///   Gets the chunk size.
        /// </summary>
        public int ChunkSize
        {
            get { return (int) _buffer.Length; }
        }

        /// <summary>
        ///   Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        ///   Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        ///   Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        ///   Gets the length in bytes of the output stream.
        /// </summary>
        public override long Length
        {
            get
            {
                if (_input.CanSeek)
                {
                    return _input.Length;
                }
                return _buffer.Length;
            }
        }

        /// <summary>
        ///   Gets or sets the position within the output stream.
        /// </summary>
        public override long Position
        {
            get
            {
                if (_input.CanSeek)
                {
                    return _input.Position;
                }
                return _buffer.Position;
            }
            set { throw new NotSupportedException(); }
        }

        private void ReadChunk()
        {
            long size;
            byte[] properties;
            try
            {
                properties = SevenZipExtractor.GetLzmaProperties(_input, out size);
            }
            catch (LzmaException)
            {
                _error = true;
                return;
            }
            if (!_firstChunkRead)
            {
                _commonProperties = properties;
            }
            if (_commonProperties[0] != properties[0] ||
                _commonProperties[1] != properties[1] ||
                _commonProperties[2] != properties[2] ||
                _commonProperties[3] != properties[3] ||
                _commonProperties[4] != properties[4])
            {
                _error = true;
                return;
            }
            if (_buffer.Capacity < (int) size)
            {
                _buffer.Capacity = (int) size;
            }
            _buffer.SetLength(size);
            _decoder.SetDecoderProperties(properties);
            _buffer.Position = 0;
            _decoder.Code(
                _input, _buffer, 0, size, null);
            _buffer.Position = 0;
        }

        /// <summary>
        ///   Does nothing.
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        ///   Reads a sequence of bytes from the current stream and decompresses data if necessary.
        /// </summary>
        /// <param name = "buffer">An array of bytes.</param>
        /// <param name = "offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name = "count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_error)
            {
                return 0;
            }

            if (!_firstChunkRead)
            {
                ReadChunk();
                _firstChunkRead = true;
            }
            int readCount = 0;
            while (count > _buffer.Length - _buffer.Position && !_error)
            {
                var buf = new byte[_buffer.Length - _buffer.Position];
                _buffer.Read(buf, 0, buf.Length);
                buf.CopyTo(buffer, offset);
                offset += buf.Length;
                count -= buf.Length;
                readCount += buf.Length;
                ReadChunk();
            }
            if (!_error)
            {
                _buffer.Read(buffer, offset, count);
                readCount += count;
            }
            return readCount;
        }

        /// <summary>
        ///   Sets the position within the current stream.
        /// </summary>
        /// <param name = "offset">A byte offset relative to the origin parameter.</param>
        /// <param name = "origin">A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Sets the length of the current stream.
        /// </summary>
        /// <param name = "value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Writes a sequence of bytes to the current stream.
        /// </summary>
        /// <param name = "buffer">An array of bytes.</param>
        /// <param name = "offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name = "count">The maximum number of bytes to be read from the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
#endif
}