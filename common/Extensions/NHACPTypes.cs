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
    /// Class to wrap the NHACP Session Start Message
    /// </summary>
    public class NHACPStartMessage
    {
        /// <summary>
        /// Magic "ACP"
        /// </summary>
        private char[] magic = new char[3];

        /// <summary>
        /// Gets or sets the Magic 3 byte char array
        /// </summary>
        public char[] Magic
        {
            get
            {
                return this.magic;
            }
            set
            {
                this.magic = value;
            }
        }

        /// <summary>
        /// Gets or sets NHACP version
        /// </summary>
        public ushort Version { get; set; }

        /// <summary>
        /// Gets or sets NHACP Options
        /// </summary>
        public ushort Options { get; set; }

        /// <summary>
        /// Gets or sets if this NHACP session wants CRC
        /// </summary>
        public bool CRC { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHACPStartMessage"/> class. 
        /// </summary>
        public NHACPStartMessage()
        {
            this.magic = "ACP".ToCharArray();
            this.Version = 0x0;
            this.Options = 0x0;
            this.CRC = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHACPStartMessage"/> class. 
        /// </summary>
        /// <param name="stream">Nabu transport stream</param>
        public NHACPStartMessage(Stream stream)
        {
            byte[] chars = stream.ReadBytes(3);
            this.magic = System.Text.Encoding.ASCII.GetString(chars).ToCharArray();
            this.Version = stream.ReadUshort();
            this.Options = stream.ReadUshort();
            if (this.Options == 1)
            {
                this.CRC = true;
                byte crc = (byte)stream.ReadByte();
            }
            else
            {
                this.CRC = false;
            }
        }
    }

    /// <summary>
    /// Class to wrap the NHACP Session Start response Message
    /// </summary>
    public class NHACPStartResponse
    {
        /// <summary>
        /// Response message type is 0x80
        /// </summary>
        public byte Type = 0x80;

        /// <summary>
        /// Gets or sets the Session ID
        /// </summary>
        public byte SessionId { get; set; }

        /// <summary>
        /// Gets or sets the NHACP Version
        /// </summary>
        public ushort Version { get; set; }

        /// <summary>
        /// Gets or sets the Adaptor Name
        /// </summary>
        public string AdaptorName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHACPStartResponse"/> class. 
        /// </summary>
        /// <param name="version">NHACP Version</param>
        /// <param name="sessionId">Session ID</param>
        public NHACPStartResponse(ushort version, byte sessionId)
        {
            this.AdaptorName = $"NabuNetwork.Com Adapter v{Settings.majorVersion}.{Settings.minorVersion}";
            this.Version = version;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Write the response
        /// </summary>
        /// <param name="stream">Stream to use to write the response.</param>
        public void Write(Stream stream)
        {
            // 1 byte for the type
            // 1 byte for the session ID
            // 2 bytes for the version
            // 1 byte for the length of the string
            // string
            int length = 1 + 1 + 2 + 1 + AdaptorName.Length;

            stream.WriteUshort((ushort)length);
            stream.WriteByte(Type);
            stream.WriteByte(SessionId);
            stream.WriteUshort(Version);
            stream.WriteString(AdaptorName);
        }
    }
}
