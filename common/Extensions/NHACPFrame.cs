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
namespace NabuAdaptor.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class to wrap a NHACP Frame
    /// </summary>
    public class NHACPFrame
    {
        /// <summary>
        /// Gets or sets the Session ID
        /// </summary>
        public byte SessionId { get; set; }

        /// <summary>
        /// Gets or sets the Op Code
        /// </summary>
        public ushort OpCode { get; set; }
        
        /// <summary>
        /// Gets or sets the memory stream
        /// </summary>
        public MemoryStream Stream { get; set; }

        /// <summary>
        /// Gets or sets the Valid Frame flag
        /// </summary>
        public bool ValidFrame { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHACPFrame"/> class. 
        /// </summary>
        /// <param name="stream">stream from Nabu connection</param>
        public NHACPFrame(Stream stream)
        {
            this.ValidFrame = false;

            byte[] data = new byte[0];

            // Read the session ID
            this.SessionId = (byte)stream.ReadByte();

            // Read the frame length
            ushort length = stream.ReadUshort();

            if (length < 0x8383)
            {
                //Now, read the frame
                data = stream.ReadBytes(length);
                this.OpCode = data[0];
                ValidFrame = true;
                this.Stream = new MemoryStream(data.Skip(1).ToArray());
            }
        }
    }
}
