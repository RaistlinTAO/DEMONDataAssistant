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
//        Module  Name:                   ICoder.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip.Sdk
{
    #region

    using System;
    using System.IO;

    #endregion

    /// <summary>
    ///   The exception that is thrown when an error in input stream occurs during decoding.
    /// </summary>
    [Serializable]
    internal class DataErrorException : ApplicationException
    {
        public DataErrorException() : base("Data Error")
        {
        }
    }

    /// <summary>
    ///   The exception that is thrown when the value of an argument is outside the allowable range.
    /// </summary>
    internal class InvalidParamException : ApplicationException
    {
        public InvalidParamException() : base("Invalid Parameter")
        {
        }
    }

    /// <summary>
    ///   Callback progress interface.
    /// </summary>
    public interface ICodeProgress
    {
        /// <summary>
        ///   Callback progress.
        /// </summary>
        /// <param name = "inSize">
        ///   Processed input size. -1 if unknown.
        /// </param>
        /// <param name = "outSize">
        ///   Processed output size. -1 if unknown.
        /// </param>
        void SetProgress(Int64 inSize, Int64 outSize);
    };

    /// <summary>
    ///   Stream coder interface
    /// </summary>
    public interface ICoder
    {
        /// <summary>
        ///   Codes streams.
        /// </summary>
        /// <param name = "inStream">
        ///   input Stream.
        /// </param>
        /// <param name = "outStream">
        ///   output Stream.
        /// </param>
        /// <param name = "inSize">
        ///   input Size. -1 if unknown.
        /// </param>
        /// <param name = "outSize">
        ///   output Size. -1 if unknown.
        /// </param>
        /// <param name = "progress">
        ///   callback progress reference.
        /// </param>
        /// <exception cref = "SevenZip.Sdk.DataErrorException">
        ///   if input stream is not valid
        /// </exception>
        void Code(Stream inStream, Stream outStream,
                  Int64 inSize, Int64 outSize, ICodeProgress progress);
    };

    /*
	public interface ICoder2
	{
		 void Code(ISequentialInStream []inStreams,
				const UInt64 []inSizes, 
				ISequentialOutStream []outStreams, 
				UInt64 []outSizes,
				ICodeProgress progress);
	};
  */

    /// <summary>
    ///   Provides the fields that represent properties idenitifiers for compressing.
    /// </summary>
    public enum CoderPropId
    {
        /// <summary>
        ///   Specifies default property.
        /// </summary>
        DefaultProp = 0,

        /// <summary>
        ///   Specifies size of dictionary.
        /// </summary>
        DictionarySize,

        /// <summary>
        ///   Specifies size of memory for PPM*.
        /// </summary>
        UsedMemorySize,

        /// <summary>
        ///   Specifies order for PPM methods.
        /// </summary>
        Order,

        /// <summary>
        ///   Specifies Block Size.
        /// </summary>
        BlockSize,

        /// <summary>
        ///   Specifies number of postion state bits for LZMA (0 &lt;= x &lt;= 4).
        /// </summary>
        PosStateBits,

        /// <summary>
        ///   Specifies number of literal context bits for LZMA (0 &lt;= x &lt;= 8).
        /// </summary>
        LitContextBits,

        /// <summary>
        ///   Specifies number of literal position bits for LZMA (0 &lt;= x &lt;= 4).
        /// </summary>
        LitPosBits,

        /// <summary>
        ///   Specifies number of fast bytes for LZ*.
        /// </summary>
        NumFastBytes,

        /// <summary>
        ///   Specifies match finder. LZMA: "BT2", "BT4" or "BT4B".
        /// </summary>
        MatchFinder,

        /// <summary>
        ///   Specifies the number of match finder cyckes.
        /// </summary>
        MatchFinderCycles,

        /// <summary>
        ///   Specifies number of passes.
        /// </summary>
        NumPasses,

        /// <summary>
        ///   Specifies number of algorithm.
        /// </summary>
        Algorithm,

        /// <summary>
        ///   Specifies the number of threads.
        /// </summary>
        NumThreads,

        /// <summary>
        ///   Specifies mode with end marker.
        /// </summary>
        EndMarker = 0x490
    };

    /// <summary>
    ///   The ISetCoderProperties interface
    /// </summary>
    internal interface ISetCoderProperties
    {
        void SetCoderProperties(CoderPropId[] propIDs, object[] properties);
    };

    /// <summary>
    ///   The IWriteCoderProperties interface
    /// </summary>
    internal interface IWriteCoderProperties
    {
        void WriteCoderProperties(Stream outStream);
    }

    /// <summary>
    ///   The ISetDecoderPropertiesinterface
    /// </summary>
    internal interface ISetDecoderProperties
    {
        /// <summary>
        ///   Sets decoder properties
        /// </summary>
        /// <param name = "properties">Array of byte properties</param>
        void SetDecoderProperties(byte[] properties);
    }
}