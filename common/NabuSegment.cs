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

    /// <summary>
    /// Class to encapsulate a Nabu Segment
    /// A segment is a collection of packets.  IOS requests segments, either code to execute or data to provide.
    /// A segment number is analogous (comparable) to a filename.  It's just a compact way of making a directory entry.
    /// </summary>
    public class NabuSegment
    {
        /// <summary>
        /// List of all the packets in this segment
        /// </summary>
        public NabuPacket[] Packets
        {
            get; set;
        }

        /// <summary>
        /// Name of the segment
        /// </summary>
        public int Name
        {
            get; set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NabuSegment"/> class.
        /// </summary>
        /// <param name="packets">packets for this segment</param>
        /// <param name="name">name of the segment</param>
        public NabuSegment(NabuPacket[] packets, int name)
        {
            this.Packets = packets;
            this.Name = name;
        }
    }
}
