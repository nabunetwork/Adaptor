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
    using System.IO;
    using System.Text;

    /// <summary>
    /// Some extensions to the stream to make reading and writing larger values easier
    /// </summary>
    public static class StreamExtensions
    {
        public static void WriteString(this Stream stream, string str)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                bw.Write(str);
            }
        }

        /// <summary>
        /// Read a string
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="length">the length of the string</param>
        /// <returns>The String</returns>
        public static string ReadString(this Stream stream, int length)
        {
            using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
            {
                return Encoding.ASCII.GetString(br.ReadBytes(length));
            }
        }

        /// <summary>
        /// Read a byte array
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="length">number of bytes to read</param>
        /// <returns>array of bytes</returns>
        public static byte[] ReadBytes(this Stream stream, int length)
        {
            using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
            {
                return br.ReadBytes(length);
            }
        }

        /// <summary>
        /// Read an unsigned int
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <returns>the unsigned int</returns>
        public static uint ReadUint(this Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
            {
                return br.ReadUInt32();
            }
        }

        /// <summary>
        /// Read an unsigned short
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <returns>the unsigned short</returns>
        public static ushort ReadUshort(this Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
            {
                return br.ReadUInt16();
            }
        }

        /// <summary>
        /// Read an int
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <returns>the int</returns>
        public static int ReadInt(this Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
            {
                return br.ReadInt32();
            }
        }

        /// <summary>
        /// Write an unsigned short
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="value">the short to write</param>
        public static void WriteUint(this Stream stream, uint value)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                bw.Write(value);
            }
        }

        /// <summary>
        /// Write an unsigned short
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="value">the short to write</param>
        public static void WriteUshort(this Stream stream, ushort value)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                bw.Write(value);
            }
        }

        /// <summary>
        /// Write the int
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="value">the int to write</param>
        public static void WriteInt(this Stream stream, int value)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                bw.Write(value);
            }
        }

        /// <summary>
        /// Write an array of bytes
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="bytes">the array of bytes</param>
        public static void WriteBytes(this Stream stream, params byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
