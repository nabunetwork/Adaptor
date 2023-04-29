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
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class to manage nabu segments (programs in a cycle).
    /// Code to create the time segment on the fly using the local computers clock
    /// </summary>
    public static class SegmentManager
    {
        /// <summary>
        /// Create the time segment that the nabu can parse.
        /// </summary>
        /// <returns></returns>
        public static NabuSegment CreateTimeSegment()
        {
            DateTime dateTime = DateTime.Now;

            List<byte> list = new List<byte>();
            list.Add(0x7F);
            list.Add(0xFF);
            list.Add(0xFF);
            list.Add(0x0);
            list.Add(0x0);
            list.Add(0x7F);
            list.Add(0xFF);
            list.Add(0xFF);
            list.Add(0xFF);
            list.Add(0x7F);
            list.Add(0x80);
            list.Add(0x30);
            list.Add(0x0);
            list.Add(0x0);
            list.Add(0x0);
            list.Add(0x0);
            list.Add(0x2);
            list.Add(0x2);
            list.Add((byte)(dateTime.DayOfWeek + 1));
            list.Add(0x54);
            list.Add((byte)dateTime.Month);
            list.Add((byte)dateTime.Day);
            list.Add((byte)dateTime.Hour);
            list.Add((byte)dateTime.Minute);
            list.Add((byte)dateTime.Second);
            list.Add(0x0);
            list.Add(0x0);

            byte[] crcData = CRC.CalculateCRC(list.ToArray());

            list.Add(crcData[0]);
            list.Add(crcData[1]);

            return new NabuSegment(new List<NabuPacket> { new NabuPacket(0, list.ToArray()) }.ToArray(), 0x7FFFFF);
        }

        /// <summary>
        /// Load the packets inside of the segment file (original Nabu cycle packet)
        /// </summary>
        /// <param name="segmentNumber">Name of the segment file</param>
        /// <param name="data">contents of the file as a byte array</param>
        /// <returns>Nabu Segment object which contains the packets</returns>
        public static NabuSegment LoadPackets(int segmentNumber, byte[] data, Logger logger)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                if (memoryStream.Length > 0xFFFFL)
                {
                    throw new ArgumentOutOfRangeException("data", "File too large");
                }

                List<NabuPacket> list = new List<NabuPacket>();

                byte packetNumber = 0;

                // Ok, read in the segment file into it's constituent packets
                while (memoryStream.Position < memoryStream.Length)
                {
                    // Get the first two bytes, this is the length of this segment
                    int segmentLength = memoryStream.ReadByte() + (memoryStream.ReadByte() << 8);

                    if (segmentLength > 0 && segmentLength <= NabuPacket.MaxPacketSize)
                    {
                        // ok, Read this segment
                        byte[] segmentData = new byte[segmentLength];
                        memoryStream.Read(segmentData, 0, segmentLength);
                        NabuPacket packet = new NabuPacket(packetNumber, segmentData);
                        ValidatePacket(packet.Data, logger);
                        list.Add(packet);
                        packetNumber++;
                    }
                }

                return new NabuSegment(list.ToArray(), segmentNumber);
            }
        }

        /// <summary>
        /// Create packets object for a compiled program
        /// </summary>
        /// <param name="segmentNumber">name of segment file</param>
        /// <param name="data">binary data to make into segments</param>
        /// <returns>Nabu Segment object which contains the packets</returns>
        public static NabuSegment CreatePackets(int segmentNumber, byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                if (memoryStream.Length > 0xFFFFL)
                {
                   throw new ArgumentOutOfRangeException("data", "File too large");
                }

                List<NabuPacket> packets = new List<NabuPacket>();

                byte packetNumber = 0;
                while (true)
                {
                    long offset = memoryStream.Position;
                    byte[] buffer = new byte[NabuPacket.PacketDataLength];
                    int bytesRead = memoryStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // We're done
                        break;
                    }

                    // If we are at the EOF, then this is the last segment
                    bool lastSegment = memoryStream.Position == memoryStream.Length;

                    // Create the segment
                    packets.Add(new NabuPacket(packetNumber, CreatePacket(segmentNumber, packetNumber, (ushort)offset, lastSegment, buffer.Take(bytesRead).ToArray()))); 
                } 

                return new NabuSegment(packets.ToArray(), segmentNumber);
            }
        }

        /// <summary>
        /// Create an individual segment
        /// </summary>
        /// <param name="segmentNumber">Segment Number</param>
        /// <param name="packetNumber">Packet Number</param>
        /// <param name="offset">offset </param>
        /// <param name="lastSegment"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] CreatePacket(int segmentNumber, byte packetNumber, ushort offset, bool lastSegment, byte[] data)
        {
            List<byte> list = new List<byte>();

            // Cobble together the header
            list.Add((byte)(segmentNumber >> 16 & 0xFF));
            list.Add((byte)(segmentNumber >> 8 & 0xFF));
            list.Add((byte)(segmentNumber >> 0 & 0xFF));
            list.Add((byte)(packetNumber & 0xFF));

            // Owner
            list.Add(0x1);

            // Tier
            list.Add(0x7F);
            list.Add(0xFF);
            list.Add(0xFF);
            list.Add(0xFF);

            // Mystery bytes
            list.Add(0x7F);
            list.Add(0x80);

            // Packet Type
            byte type = 0x20;

            if (lastSegment)
            {
                // Set the 4th bit to mark end of segment
                type = (byte)(type | 0x10u);
            }
            else if (packetNumber == 0)
            {
                type = 0xA1;
            }

            list.Add(type);

            list.Add((byte)(packetNumber >> 0 & 0xFF));
            list.Add(0x0);

            list.Add((byte)(offset >> 8 & 0xFF));
            list.Add((byte)(offset >> 0 & 0xFF));

            // Payload
            list.AddRange(data);

            // CRC
            byte[] crcData = CRC.CalculateCRC(list.ToArray());

            list.Add(crcData[0]);
            list.Add(crcData[1]);
            return list.ToArray();
        }

        /// <summary>
        /// Validate the packet CRC
        /// </summary>
        /// <param name="packetData">segment data</param>
        private static void ValidatePacket(byte[] packetData, Logger logger)
        {
            byte[] data = new byte[packetData.Length - 2];
            Array.Copy(packetData, data, packetData.Length - 2);
            byte[] crcData = CRC.CalculateCRC(data);

            if (packetData[packetData.Length - 2] != crcData[0] || packetData[packetData.Length - 1] != crcData[1])
            {
                logger.Log($"CRC Bad, Calculated 0x{crcData[0]}, 0x{crcData[1]}, but read 0x{packetData[packetData.Length - 2]:X02}, 0x{packetData[packetData.Length - 1]:X02}", Logger.Target.file);

                // Fix the CRC so that the nabu will load.
                packetData[packetData.Length - 2] = crcData[0];
                packetData[packetData.Length - 1] = crcData[1];
            }
        }
    }
}
