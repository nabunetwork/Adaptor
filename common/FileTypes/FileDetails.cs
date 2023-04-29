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
namespace NabuAdaptor.FileTypes
{
    using System;
    using System.IO;

    /// <summary>
    /// Class to keep track of a FileDetails object used by the file extensions
    /// </summary>
    public class FileDetails
    {
        public enum fileType
        {
            File,
            Directory
        }

        /// <summary>
        /// The file type (directory or file)
        /// </summary>
        public fileType FileType { get; set; }

        /// <summary>
        /// Gets or sets the Created time
        /// </summary>
        private DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the Modified time
        /// </summary>
        private DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the Filename
        /// </summary>
        private string FileName { get; set; }

        /// <summary>
        /// Gets or sets the FileSize
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="FileDetails"/> class. 
        /// </summary>
        /// <param name="created">Created Time</param>
        /// <param name="modified">Last Modified Time</param>
        /// <param name="fileName">File Name</param>
        /// <param name="fileSize">File Size</param>
        public FileDetails(DateTime created, DateTime modified, string fileName, long fileSize)
        {
            this.Created = created;
            this.Modified = modified;
            this.FileName = FileName;
            this.FileSize = FileSize;
            this.FileType = fileType.File;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="FileDetails"/> class.
        /// </summary>
        /// <param name="directoryInfo">DirectoryInfo object</param>
        public FileDetails(DirectoryInfo directoryInfo)
        {
            this.Created = directoryInfo.CreationTime;
            this.Modified = directoryInfo.LastWriteTime;

            if (string.IsNullOrWhiteSpace(directoryInfo.Name))
            {
                this.FileName = @"\";
            }
            else
            {
                this.FileName = directoryInfo.Name;
            }

            this.FileSize = -1;
            this.FileType = fileType.Directory;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="FileDetails"/> class.
        /// </summary>
        /// <param name="fileInfo">FileInfo object</param>
        public FileDetails(FileInfo fileInfo)
        {
            this.Created = fileInfo.CreationTime;
            this.Modified = fileInfo.LastWriteTime;

            if (string.IsNullOrWhiteSpace(fileInfo.Name))
            {
                this.FileName = @"\";
            }
            else
            {
                this.FileName = fileInfo.Name;
            }

            this.FileSize = fileInfo.Length;
            this.FileType = fileType.File;
        }

        /// <summary>
        /// Byte array details defined here:
        /// https://github.com/DJSures/NABU-LIB/blob/main/NABULIB/RetroNET-FileStore.h
        /// </summary>
        /// <returns>Returns this FileDetails object in the necessary byte array format for NABU</returns>
        public byte[] GetFileDetails()
        {
            byte[] fileDetails = new byte[83];

            fileDetails[0] = (byte)((uint)this.FileSize & 0xFFu);
            fileDetails[1] = (byte)((uint)(this.FileSize >> 8) & 0xFFu);
            fileDetails[2] = (byte)((uint)(this.FileSize >> 16) & 0xFFu);
            fileDetails[3] = (byte)((uint)(this.FileSize >> 24) & 0xFFu);
            fileDetails[4] = (byte)((ushort)this.Created.Year & 0xFFu);
            fileDetails[5] = (byte)((uint)((ushort)this.Created.Year >> 8) & 0xFFu);
            fileDetails[6] = (byte)this.Created.Month;
            fileDetails[7] = (byte)this.Created.Day;
            fileDetails[8] = (byte)this.Created.Hour;
            fileDetails[9] = (byte)this.Created.Minute;
            fileDetails[10] = (byte)this.Created.Second;
            fileDetails[11] = (byte)((ushort)this.Modified.Year & 0xFFu);
            fileDetails[12] = (byte)((uint)((ushort)this.Modified.Year >> 8) & 0xFFu);
            fileDetails[13] = (byte)this.Modified.Month;
            fileDetails[14] = (byte)this.Modified.Day;
            fileDetails[15] = (byte)this.Modified.Hour;
            fileDetails[16] = (byte)this.Modified.Minute;
            fileDetails[17] = (byte)this.Modified.Second;
            fileDetails[18] = (byte)Math.Min(this.FileName.Length, 64);

            for (int i = 0; i < fileDetails[18]; i++)
            {
                fileDetails[19 + i] = (byte)this.FileName[i];
            }

            return fileDetails;
        }
    }
}
