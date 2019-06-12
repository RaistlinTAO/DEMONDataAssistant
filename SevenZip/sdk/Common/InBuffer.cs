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
//        Module  Name:                   InBuffer.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip.Sdk.Buffer
{
    #region

    using System.IO;

    #endregion

    /// <summary>
    ///   Implements the input buffer work
    /// </summary>
    internal class InBuffer
    {
        private readonly byte[] m_Buffer;
        private readonly uint m_BufferSize;
        private uint m_Limit;
        private uint m_Pos;
        private ulong m_ProcessedSize;
        private Stream m_Stream;
        private bool m_StreamWasExhausted;

        /// <summary>
        ///   Initializes the input buffer
        /// </summary>
        /// <param name = "bufferSize"></param>
        private InBuffer(uint bufferSize)
        {
            m_Buffer = new byte[bufferSize];
            m_BufferSize = bufferSize;
        }

        /// <summary>
        ///   Initializes the class
        /// </summary>
        /// <param name = "stream"></param>
        private void Init(Stream stream)
        {
            m_Stream = stream;
            m_ProcessedSize = 0;
            m_Limit = 0;
            m_Pos = 0;
            m_StreamWasExhausted = false;
        }

        /// <summary>
        ///   Reads the whole block
        /// </summary>
        /// <returns></returns>
        private bool ReadBlock()
        {
            if (m_StreamWasExhausted)
                return false;
            m_ProcessedSize += m_Pos;
            int aNumProcessedBytes = m_Stream.Read(m_Buffer, 0, (int) m_BufferSize);
            m_Pos = 0;
            m_Limit = (uint) aNumProcessedBytes;
            m_StreamWasExhausted = (aNumProcessedBytes == 0);
            return (!m_StreamWasExhausted);
        }

        /// <summary>
        ///   Releases the stream
        /// </summary>
        private void ReleaseStream()
        {
            // m_Stream.Close(); 
            m_Stream = null;
        }

        /// <summary>
        ///   Reads the byte to check it
        /// </summary>
        /// <param name = "b"></param>
        /// <returns></returns>
        private bool ReadByte(out byte b)
        {
            b = 0;
            if (m_Pos >= m_Limit)
                if (!ReadBlock())
                    return false;
            b = m_Buffer[m_Pos++];
            return true;
        }

        /// <summary>
        ///   Reads the next byte
        /// </summary>
        /// <returns></returns>
        private byte ReadByte()
        {
            // return (byte)m_Stream.ReadByte();
            if (m_Pos >= m_Limit)
                if (!ReadBlock())
                    return 0xFF;
            return m_Buffer[m_Pos++];
        }

        /// <summary>
        ///   Gets processed size
        /// </summary>
        /// <returns></returns>
        private ulong GetProcessedSize()
        {
            return m_ProcessedSize + m_Pos;
        }
    }
}