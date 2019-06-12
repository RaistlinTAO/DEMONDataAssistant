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
//        Module  Name:                   LzmaBase.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip.Sdk.Compression.Lzma
{
    internal abstract class Base
    {
        public const uint kAlignMask = (kAlignTableSize - 1);
        public const uint kAlignTableSize = 1 << kNumAlignBits;
        public const int kDicLogSizeMin = 0;
        public const uint kEndPosModelIndex = 14;
        public const uint kMatchMaxLen = kMatchMinLen + kNumLenSymbols - 1;
        // public const int kDicLogSizeMax = 30;
        // public const uint kDistTableSizeMax = kDicLogSizeMax * 2;

        public const uint kMatchMinLen = 2;

        public const int kNumAlignBits = 4;

        public const uint kNumFullDistances = 1 << ((int) kEndPosModelIndex/2);
        public const int kNumHighLenBits = 8;

        public const uint kNumLenSymbols = kNumLowLenSymbols + kNumMidLenSymbols +
                                           (1 << kNumHighLenBits);

        public const uint kNumLenToPosStates = 1 << kNumLenToPosStatesBits;
        public const int kNumLenToPosStatesBits = 2; // it's for speed optimization

        public const uint kNumLitContextBitsMax = 8;
        public const uint kNumLitPosStatesBitsEncodingMax = 4;

        public const int kNumLowLenBits = 3;
        public const uint kNumLowLenSymbols = 1 << kNumLowLenBits;
        public const int kNumMidLenBits = 3;
        public const uint kNumMidLenSymbols = 1 << kNumMidLenBits;
        public const uint kNumPosModels = kEndPosModelIndex - kStartPosModelIndex;
        public const int kNumPosSlotBits = 6;
        public const int kNumPosStatesBitsEncodingMax = 4;
        public const int kNumPosStatesBitsMax = 4;
        public const uint kNumPosStatesEncodingMax = (1 << kNumPosStatesBitsEncodingMax);
        public const uint kNumPosStatesMax = (1 << kNumPosStatesBitsMax);
        public const uint kNumRepDistances = 4;
        public const uint kNumStates = 12;
        public const uint kStartPosModelIndex = 4;

        public static uint GetLenToPosState(uint len)
        {
            len -= kMatchMinLen;
            if (len < kNumLenToPosStates)
                return len;
            return (kNumLenToPosStates - 1);
        }

        #region Nested type: State

        public struct State
        {
            public uint Index;

            public void Init()
            {
                Index = 0;
            }

            public void UpdateChar()
            {
                if (Index < 4) Index = 0;
                else if (Index < 10) Index -= 3;
                else Index -= 6;
            }

            public void UpdateMatch()
            {
                Index = (uint) (Index < 7 ? 7 : 10);
            }

            public void UpdateRep()
            {
                Index = (uint) (Index < 7 ? 8 : 11);
            }

            public void UpdateShortRep()
            {
                Index = (uint) (Index < 7 ? 9 : 11);
            }

            public bool IsCharState()
            {
                return Index < 7;
            }
        }

        #endregion
    }
}