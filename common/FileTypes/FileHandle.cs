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

    [Flags]
    public enum FileFlags
    {
        ReadOnly = 0,
        ReadWrite = 1
    }

    [Flags]
    public enum NHACPFlags
    {
        O_RDONLY = 0,
        O_RDWR = 1,
        O_RDWP = 2,
        O_DIRECTORY = 8,
        O_CREAT = 16,
        O_EXCL = 32,
        O_TRUNC = 64
    }

    [Flags]
    public enum CopyMoveFlags
    {
        NoReplace = 0,
        YesReplace = 1
    }

    [Flags]
    public enum FileListFlags
    {
        IncludeFiles = 1,
        IncludeDirectories = 2
    }

    /// <summary>
    /// Seek flags for access into a file
    /// </summary>
    public enum SeekFlags
    {  
        SET = 0,
        CUR = 1,
        END = 2
    }

    /// <summary>
    /// NHACP Errors
    /// </summary>
    public enum Errors
    {
        undefined = 0,
        ENOTSUP = 1,
        EPERM = 2,
        ENOENT = 3,
        EIO = 4,
        EBADF = 5,
        ENOMEM = 6,
        EACCES = 7,
        EBUSY = 8,
        EEXIST = 9,
        EISDIR = 10,
        EINVAL = 11,
        ENFILE = 12,
        EFBIG = 13,
        ENOSPC = 14,
        ESEEK = 15, 
        ENOTDIR = 16,
        ENOTEMPTY = 17,
        ESRCH = 18,
        ENSESS = 19,
        EAGAIN = 20,
        EROFS = 21
    }

    /// <summary>
    /// Class to describe what the server needs to keep in memory for a file handle opened by the Nabu 
    /// </summary>
    public class FileHandle
    {
        /// <summary>
        /// Internal holder for index
        /// </summary>
        private long index;

        /// <summary>
        /// Gets the file handle assigned to this file
        /// </summary>
        public byte Handle { get; private set; }

        /// <summary>
        /// Gets the flags used when the file was opened
        /// </summary>
        public ushort Flags { get; private set; }

        /// <summary>
        /// Gets the filename
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the local working directory
        /// </summary>
        public string WorkingDirectory { get; private set; }

        /// <summary>
        /// The index into the file where we are currently reading/writing
        /// </summary>
        public long Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;

                // Make sure that Index is valid
                if (this.index < 0)
                {
                    this.index = 0L;
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(this.FullFileName);

                    if (this.index > fileInfo.Length)
                    {
                        this.Index = fileInfo.Length;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the file working filename (path and filename)
        /// </summary>
        public string FullFileName
        {
            get
            {
                return Path.Combine(this.WorkingDirectory, this.FileName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandle"/> class. 
        /// </summary>
        /// <param name="workingDirectory">Working Directory</param>
        /// <param name="fileName">Filename</param>
        /// <param name="flags">File Flags</param>
        /// <param name="fileHandle">File Handle</param>
        public FileHandle(string workingDirectory, string fileName, ushort flags, byte fileHandle)
        {
            this.WorkingDirectory = workingDirectory;
            this.Flags = flags;
            this.Handle = fileHandle;
            this.FileName = fileName;
            this.Index = 0;
        }

        /// <summary>
        /// Gets the Flags as File Flags
        /// </summary>
        /// <returns></returns>
        public FileFlags GetFlagsAsFileFlags()
        {
            return (FileFlags)this.Flags;
        }

        /// <summary>
        /// Gets the flags as NHACP Flags
        /// </summary>
        /// <returns></returns>
        public NHACPFlags GetFlagsAsNHACPFlags()
        {
            return (NHACPFlags)this.Flags;
        }
    }
}
