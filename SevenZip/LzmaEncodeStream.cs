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
//        Module  Name:                   LzmaEncodeStream.cs
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
#if COMPRESS
    /// <summary>
    ///   The stream which compresses data with LZMA on the fly.
    /// </summary>
    public class LzmaEncodeStream : Stream
    {
        private const int MAX_BUFFER_CAPACITY = 1 << 30; //1 Gb
        private readonly MemoryStream _buffer = new MemoryStream();
        private readonly int _bufferCapacity = 1 << 18; //256 kb
        private readonly bool _ownOutput;
        private bool _disposed;
        private Encoder _lzmaEncoder;
        private Stream _output;

        /// <summary>
        ///   Initializes a new instance of the LzmaEncodeStream class.
        /// </summary>
        public LzmaEncodeStream()
        {
            _output = new MemoryStream();
            _ownOutput = true;
            Init();
        }

        /// <summary>
        ///   Initializes a new instance of the LzmaEncodeStream class.
        /// </summary>
        /// <param name = "bufferCapacity">The buffer size. The bigger size, the better compression.</param>
        public LzmaEncodeStream(int bufferCapacity)
        {
            _output = new MemoryStream();
            _ownOutput = true;
            if (bufferCapacity > MAX_BUFFER_CAPACITY)
            {
                throw new ArgumentException("Too large capacity.", "bufferCapacity");
            }
            _bufferCapacity = bufferCapacity;
            Init();
        }

        /// <summary>
        ///   Initializes a new instance of the LzmaEncodeStream class.
        /// </summary>
        /// <param name = "outputStream">An output stream which supports writing.</param>
        public LzmaEncodeStream(Stream outputStream)
        {
            if (!outputStream.CanWrite)
            {
                throw new ArgumentException("The specified stream can not write.", "outputStream");
            }
            _output = outputStream;
            Init();
        }

        /// <summary>
        ///   Initializes a new instance of the LzmaEncodeStream class.
        /// </summary>
        /// <param name = "outputStream">An output stream which supports writing.</param>
        /// <param name = "bufferCapacity">A buffer size. The bigger size, the better compression.</param>
        public LzmaEncodeStream(Stream outputStream, int bufferCapacity)
        {
            if (!outputStream.CanWrite)
            {
                throw new ArgumentException("The specified stream can not write.", "outputStream");
            }
            _output = outputStream;
            if (bufferCapacity > 1 << 30)
            {
                throw new ArgumentException("Too large capacity.", "bufferCapacity");
            }
            _bufferCapacity = bufferCapacity;
            Init();
        }

        /// <summary>
        ///   Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return false; }
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
            get
            {
                DisposedCheck();
                return _buffer.CanWrite;
            }
        }

        /// <summary>
        ///   Gets the length in bytes of the output stream.
        /// </summary>
        public override long Length
        {
            get
            {
                DisposedCheck();
                if (_output.CanSeek)
                {
                    return _output.Length;
                }
                return _buffer.Position;
            }
        }

        /// <summary>
        ///   Gets or sets the position within the output stream.
        /// </summary>
        public override long Position
        {
            get
            {
                DisposedCheck();
                if (_output.CanSeek)
                {
                    return _output.Position;
                }
                return _buffer.Position;
            }
            set { throw new NotSupportedException(); }
        }

        private void Init()
        {
            _buffer.Capacity = _bufferCapacity;
            SevenZipCompressor.LzmaDictionarySize = _bufferCapacity;
            _lzmaEncoder = new Encoder();
            SevenZipCompressor.WriteLzmaProperties(_lzmaEncoder);
        }

        /// <summary>
        ///   Checked whether the class was disposed.
        /// </summary>
        /// <exception cref = "System.ObjectDisposedException" />
        private void DisposedCheck()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("SevenZipExtractor");
            }
        }

        private void WriteChunk()
        {
            _lzmaEncoder.WriteCoderProperties(_output);
            long streamSize = _buffer.Position;
            if (_buffer.Length != _buffer.Position)
            {
                _buffer.SetLength(_buffer.Position);
            }
            _buffer.Position = 0;
            for (int i = 0; i < 8; i++)
            {
                _output.WriteByte((byte) (streamSize >> (8*i)));
            }
            _lzmaEncoder.Code(_buffer, _output, -1, -1, null);
            _buffer.Position = 0;
        }

        /// <summary>
        ///   Converts the LzmaEncodeStream to the LzmaDecodeStream to read data.
        /// </summary>
        /// <returns></returns>
        public LzmaDecodeStream ToDecodeStream()
        {
            DisposedCheck();
            Flush();
            return new LzmaDecodeStream(_output);
        }

        /// <summary>
        ///   Clears all buffers for this stream and causes any buffered data to be compressed and written.
        /// </summary>
        public override void Flush()
        {
            DisposedCheck();
            WriteChunk();
        }

        /// <summary>
        ///   Releases all unmanaged resources used by LzmaEncodeStream.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Flush();
                    _buffer.Close();
                    if (_ownOutput)
                    {
                        _output.Dispose();
                    }
                    _output = null;
                }
                _disposed = true;
            }
        }

        /// <summary>
        ///   Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name = "buffer">An array of bytes.</param>
        /// <param name = "offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name = "count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            DisposedCheck();
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Sets the position within the current stream.
        /// </summary>
        /// <param name = "offset">A byte offset relative to the origin parameter.</param>
        /// <param name = "origin">A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            DisposedCheck();
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Sets the length of the current stream.
        /// </summary>
        /// <param name = "value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            DisposedCheck();
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Writes a sequence of bytes to the current stream and compresses it if necessary.
        /// </summary>
        /// <param name = "buffer">An array of bytes.</param>
        /// <param name = "offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name = "count">The maximum number of bytes to be read from the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            DisposedCheck();
            int dataLength = Math.Min(buffer.Length - offset, count);
            while (_buffer.Position + dataLength >= _bufferCapacity)
            {
                int length = _bufferCapacity - (int) _buffer.Position;
                _buffer.Write(buffer, offset, length);
                offset = length + offset;
                dataLength -= length;
                WriteChunk();
            }
            _buffer.Write(buffer, offset, dataLength);
        }
    }
#endif
#endif
}