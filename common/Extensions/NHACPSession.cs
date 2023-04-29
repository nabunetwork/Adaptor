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
    using FileTypes;

    /// <summary>
    /// Class to wrap a NHACP Session
    /// </summary>
    public class NHACPSession
    {
        /// <summary>
        /// Keep the start message, it has version and options.
        /// </summary>
        public NHACPStartMessage Settings { get; set; }

        /// <summary>
        /// When FileList() is called, we create a list of the files which can then be returned one at a time with a call to FileListItem()
        /// </summary>
        public Dictionary<byte, List<FileDetails>> FileDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<byte, int> FileDetailsIndex { get; set; }

        /// <summary>
        /// We keep track of the file handles opened by NABU with a quick dictionary lookup
        /// </summary>
        public Dictionary<byte, FileHandle> FileHandles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHACPSession"/> class. 
        /// </summary>
        /// <param name="settings">NHACP Session Settings</param>
        public NHACPSession(NHACPStartMessage settings)
        {
            this.Settings = settings;
            this.FileDetails = new Dictionary<byte, List<FileDetails>>();
            this.FileDetailsIndex = new Dictionary<byte, int>();
            this.FileHandles = new Dictionary<byte, FileHandle>();

            // Just make sure that all the file handles are set to null (unused) - this is a just in case
            for (byte b = 0; b < byte.MaxValue; b++)
            {
                this.FileHandles[b] = null;
            }

            this.FileHandles[byte.MaxValue] = null;
        }
    }
}
