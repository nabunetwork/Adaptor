// Copyright(c) 2022 NabuNetwork.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace NabuAdaptor
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class to define a nabu packet
    /// </summary>
    public class NabuPacket
    {
        /// <summary>
        /// This is the size of a nabu header
        /// </summary>
        public const int PacketHeaderLength = 0x10;

        /// <summary>
        /// This is the maximum size of data that can be in a nabu packet
        /// </summary>
        public const int PacketDataLength = 0x3E1;

        /// <summary>
        /// This is the size of the CRC at the end of the packet
        /// </summary>
        public const int CrcLength = 0x2;

        /// <summary>
        /// The maximum size of a nabu packet, header + data + crc
        /// </summary>
        public static int MaxPacketSize
        {
            get
            {
                return PacketHeaderLength + PacketDataLength + CrcLength;
            }
        }

        /// <summary>
        /// Gets this segment's sequence number
        /// </summary>
        public byte SequenceNumber
        {
            get; private set;
        }

        /// <summary>
        /// Gets this segments data (what actually gets sent to the nabu
        /// </summary>
        public byte[] Data
        {
            get; private set;
        }

        /// <summary>
        /// Gets this segments data in escaped format
        /// </summary>
        public byte[] EscapedData
        {
            get
            {
                List<byte> escapedData = new List<byte>();

                foreach (byte b in this.Data)
                {
                    // need to escape 0x10
                    if (b == 0x10)
                    {
                        escapedData.Add(0x10);
                    }

                    escapedData.Add(b);
                }

                return escapedData.ToArray();
            }
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="NabuPacket"/> class.
        /// </summary>
        /// <param name="sequenceNumber">Sequence number of this segment</param>
        /// <param name="data">data for this segment</param>
        public NabuPacket(byte sequenceNumber, byte[] data)
        {
            this.SequenceNumber = sequenceNumber;
            this.Data = data;
        }
    }
}
