﻿// Copyright(c) 2022 NabuNetwork.com
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
    /// Class to calculate the CRC-8/CDMA2000 for NHACP
    /// </summary>
    public static class CRCCDMA
    {
        private static ushort[] table = new ushort[256]
        {
            0, 155, 173, 54, 193, 90, 108, 247, 25, 130,
            180, 47, 216, 67, 117, 238, 50, 169, 159, 4,
            243, 104, 94, 197, 43, 176, 134, 29, 234, 113,
            71, 220, 100, 255, 201, 82, 165, 62, 8, 147,
            125, 230, 208, 75, 188, 39, 17, 138, 86, 205,
            251, 96, 151, 12, 58, 161, 79, 212, 226, 121,
            142, 21, 35, 184, 200, 83, 101, 254, 9, 146,
            164, 63, 209, 74, 124, 231, 16, 139, 189, 38,
            250, 97, 87, 204, 59, 160, 150, 13, 227, 120,
            78, 213, 34, 185, 143, 20, 172, 55, 1, 154,
            109, 246, 192, 91, 181, 46, 24, 131, 116, 239,
            217, 66, 158, 5, 51, 168, 95, 196, 242, 105,
            135, 28, 42, 177, 70, 221, 235, 112, 11, 144,
            166, 61, 202, 81, 103, 252, 18, 137, 191, 36,
            211, 72, 126, 229, 57, 162, 148, 15, 248, 99,
            85, 206, 32, 187, 141, 22, 225, 122, 76, 215,
            111, 244, 194, 89, 174, 53, 3, 152, 118, 237,
            219, 64, 183, 44, 26, 129, 93, 198, 240, 107,
            156, 7, 49, 170, 68, 223, 233, 114, 133, 30,
            40, 179, 195, 88, 110, 245, 2, 153, 175, 52,
            218, 65, 119, 236, 27, 128, 182, 45, 241, 106,
            92, 199, 48, 171, 157, 6, 232, 115, 69, 222,
            41, 178, 132, 31, 167, 60, 10, 145, 102, 253,
            203, 80, 190, 37, 19, 136, 127, 228, 210, 73,
            149, 14, 56, 163, 84, 207, 249, 98, 140, 23,
            33, 186, 77, 214, 224, 123
        };

        /// <summary>
        /// Calculate the CRC based on the passed in byte array
        /// </summary>
        /// <param name="bytes">data to calculate the CRC</param>
        /// <returns>CRC value</returns>
        public static byte CalculateCRC(byte[] bytes)
        {
            ushort seed = 0xFF;

            foreach (byte b in bytes)
            {
                int index = (seed ^ b) & 0xFF;
                seed = (ushort)(table[index] ^ (seed >> 8));
            }

            return (byte)seed;
        }
    }
}
