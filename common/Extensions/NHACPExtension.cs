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
    using System.Threading;
    using FileTypes;

    /// <summary>
    /// Class to implement all of the NHACP Extensions as defined in
    /// https://github.com/thorpej/nabu-figforth/blob/dev/nhacp-draft-0.1/nabu-comms.md
    /// Tested enough to get ISHKUR CPM booting.
    /// </summary>
    public class NHACPExtension : IServerExtension
    {
        /// <summary>
        /// Error strings defined.
        /// </summary>
        public string[] ErrorStrings = new string[] {
        "undefined generic error",
        "Operation is not supported",
        "Operation is not permitted",
        "Requested file does not exist",
        "Input/output error",
        "Bad file descriptor",
        "Out of memory",
        "Access denied",
        "File is busy",
        "File already exists",
        "File is a directory",
        "Invalid argument/request",
        "Too many open files",
        "File is too large",
        "Out of space",
        "Seek on non-seekable file",
        "File is not a directory",
        "Directory is not empty",
        "No such process or session",
        "Too many sessions",
        "Try again later",
        "Storage object is write-protected" };

        /// <summary>
        /// Instance of the server
        /// </summary>
        private Server server;

        /// <summary>
        /// Collection of active NHACP sessions
        /// </summary>
        public Dictionary<byte, NHACPSession> sessions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHACPExtension"/> class. 
        /// </summary>
        /// <param name="server">Reference to the server</param>
        public NHACPExtension(Server server)
        {
            this.server = server;
            this.Initialize();
        }

        /// <summary>
        /// This extension implements several new op codes - This function maps those codes to the appropriate function call.
        /// </summary>
        /// <param name="opCode">OP code to process</param>
        /// <returns>true if we acted on this opCode, otherwise false.</returns>
        public bool TryProcessCommand(byte opCode, CancellationToken token)
        {
            if (opCode == 0x8F)
            {                
                NHACPFrame frame = new NHACPFrame(this.server.Connection.NabuStream);

                switch (frame.OpCode)
                {
                    case 0x0:
                        this.InitSession(frame);
                        break;
                    case 0x1:
                        this.StorageOpen(frame);
                        break;
                    case 0x2:
                        this.StorageGet(frame);
                        break;
                    case 0x3:
                        this.StoragePut(frame);
                        break;
                    case 0x4:
                        this.GetDateTime(frame);
                        break;
                    case 0x5:
                        this.FileClose(frame);
                        break;
                    case 0x6:
                        this.GetErrorDetails(frame);
                        break;
                    case 0x7:
                        this.StorageGetBlock(frame);
                        break;
                    case 0x8:
                        this.StoragePutBlock(frame);
                        break;
                    case 0x9:
                        this.FileRead(frame);
                        break;
                    case 0xa:
                        this.FileWrite(frame);
                        break;
                    case 0xb:
                        this.FileSeek(frame);
                        break;
                    case 0xc:
                        this.FileGetInfo(frame);
                        break;
                    case 0xd:
                        this.FileSetSize(frame);
                        break;
                    case 0xe:
                        this.ListDir(frame);
                        break;
                    case 0xf:
                        this.GetDirEntry(frame);
                        break;
                    case 0x10:
                        this.Remove(frame);
                        break;
                    case 0x11:
                        this.Rename(frame);
                        break;
                    case 0x12:
                        this.Mkdir(frame);
                        break;
                    case 0xef:
                        this.Goodbye(frame);
                        break;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Reset this extension.  If the Nabu starts over loading segment 0 and packet 0 - start over.
        /// </summary>
        public void Reset()
        {
            this.Initialize();
        }

        /// <summary>
        /// Initialize this NHACP Session
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void InitSession(NHACPFrame frame)
        {
            byte sessionId;

            NHACPSession session = new NHACPSession(new NHACPStartMessage(frame.Stream));

            if (frame.SessionId == 0x00)
            {
                this.Reset();
                this.sessions[0] = session;
                sessionId = 0x00;
            }
            else if (frame.SessionId == 0xff)
            {
                try
                {
                    // find the first unused session ID
                    KeyValuePair<byte, NHACPSession> kvp = this.sessions.First(handle => handle.Value == null);
                    this.sessions[kvp.Key] = session;
                    sessionId = kvp.Key;
                }
                catch (InvalidOperationException)
                {
                    this.SendError(session.Settings.CRC, Errors.EINVAL);
                    return;
                }
            }
            else
            {
                this.SendError(session.Settings.CRC, Errors.ENSESS);
                return;
            }

            NHACPStartResponse response = new NHACPStartResponse(session.Settings.Version, sessionId);
            response.Write(this.server.Connection.NabuStream);
        }

        /// <summary>
        /// Storage Open
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void StorageOpen(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            MemoryStream outgoingFrame = new MemoryStream();

            // Get the index
            byte fileHandle = (byte)frame.Stream.ReadByte();

            // Get the flags
            ushort flags = frame.Stream.ReadUshort();

            // get the filename Length
            byte fileNameLen = (byte)frame.Stream.ReadByte();

            // Get the filename
            string fileName = frame.Stream.ReadString(fileNameLen);

            fileName = this.SanitizeFilename(fileName);

            if (session.FileHandles[fileHandle] != null && fileHandle != byte.MaxValue)
            {
                // Filehandle is already in use, must return a failure.
                this.SendError(session.Settings.CRC, Errors.EBADF);
                return;
            }

            if (fileName.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                // not a valid url, send back 
                if (!this.ValidateUri(fileName))
                {
                    this.SendError(session.Settings.CRC, Errors.EPERM);
                    return;
                }

                // Download this file from wherever it is located to the curent directory.
                Uri uri = new Uri(fileName);
                fileName = Path.GetFileName(uri.LocalPath);

                string fullPathAndFilename = Path.Combine(this.server.GetWorkingDirectory(), fileName);
                if (!File.Exists(fullPathAndFilename))
                {
                    byte[] data;

                    WebLoader webLoader = new WebLoader();
                    webLoader.TryGetData(uri.AbsoluteUri, out data);
                    System.IO.File.WriteAllBytes(fullPathAndFilename, data);
                }
            }

            // If this handle is the max value, find the first unused handle
            if (fileHandle == byte.MaxValue)
            {
                KeyValuePair<byte, FileHandle> kvp = session.FileHandles.First(handle => handle.Value == null);
                fileHandle = kvp.Key;
            }

            FileHandle FileHandle = new FileHandle(this.server.GetWorkingDirectory(), fileName, flags, fileHandle);

            // Mark this handle as in use.
            session.FileHandles[fileHandle] = FileHandle;

            // Let the NABU know what we've done:
            outgoingFrame.WriteBytes(0x83);
            outgoingFrame.WriteBytes(fileHandle);

            // no data buffered
            outgoingFrame.WriteUint((uint)this.FileSize(FileHandle.FullFileName));

            this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
        }

        /// <summary>
        /// Storage Get
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void StorageGet(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            MemoryStream outgoingFrame = new MemoryStream();

            // Get the index
            byte fileHandle = (byte)frame.Stream.ReadByte();

            // Get the offset
            uint offset = frame.Stream.ReadUint();

            // get the length of data to read
            ushort length = frame.Stream.ReadUshort();

            if (length > 8192)
            {
                this.SendError(session.Settings.CRC, Errors.ENOTSUP);
                return;
            }

            // Retrieve this file handle from the file handle list.
            FileHandle FileHandle = session.FileHandles[fileHandle];

            if (FileHandle != null)
            {
                byte[] data = File.ReadAllBytes(FileHandle.FullFileName).Skip((int)offset).Take(length).ToArray();

                // write out the data buffer
                outgoingFrame.WriteByte(0x84);

                // write out the length
                outgoingFrame.WriteUshort((ushort)data.Length);

                // write out the data
                outgoingFrame.WriteBytes(data);

                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                server.Logger.Log($"StorageGet Requested file handle to read: {fileHandle:X06} but it was not found", Logger.Target.console);

                // send back error
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// Storage Put
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void StoragePut(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            MemoryStream outgoingFrame = new MemoryStream();

            // Get the index
            byte fileHandle = (byte)frame.Stream.ReadByte();

            // Get the offset
            uint offset = frame.Stream.ReadUint();

            // get the length of data to write to file
            ushort length = frame.Stream.ReadUshort();

            byte[] data = frame.Stream.ReadBytes(length);

            if (length > 8192)
            {
                this.SendError(session.Settings.CRC, Errors.ENOTSUP);
                return;
            }

            // Get the file handle
            FileHandle FileHandle = session.FileHandles[fileHandle];

            if (FileHandle != null && (
                FileHandle.GetFlagsAsNHACPFlags().HasFlag(NHACPFlags.O_RDONLY) ||
                FileHandle.GetFlagsAsNHACPFlags().HasFlag(NHACPFlags.O_RDWR)))
            {
                List<byte> bytes = File.ReadAllBytes(FileHandle.FullFileName).ToList();
                for (int i = 0; i < length; i++)
                {
                    bytes[(int)(i + offset)] = data[i];
                }

                File.WriteAllBytes(FileHandle.FullFileName, bytes.ToArray());

                outgoingFrame.WriteByte(0x81);
                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                server.Logger.Log($"Requested handle in HandleDeleteReplace {fileHandle:X06} but it was not found", Logger.Target.console);
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// Get the Date & Time
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void GetDateTime(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            MemoryStream outgoingFrame = new MemoryStream();

            string date = DateTime.Now.ToString("yyyyMMdd");
            string time = DateTime.Now.ToString("HHmmss");
            outgoingFrame.WriteByte(0x85);
            outgoingFrame.WriteBytes(System.Text.Encoding.ASCII.GetBytes(date));
            outgoingFrame.WriteBytes(System.Text.Encoding.ASCII.GetBytes(time));
            this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
        }

        /// <summary>
        /// File Close
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void FileClose(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            // first byte, the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();
            session.FileHandles[fileHandle] = null;
        }

        /// <summary>
        /// Get Error Details
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void GetErrorDetails(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            server.Logger.Log("GetErrorDetails", Logger.Target.console);

            ushort code = frame.Stream.ReadUshort();
            byte len = (byte)frame.Stream.ReadByte();
            this.SendError(session.Settings.CRC, Errors.ENOTSUP);
        }

        /// <summary>
        /// Storage Get Block
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void StorageGetBlock(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            MemoryStream outgoingFrame = new MemoryStream();

            byte fileHandle = (byte)frame.Stream.ReadByte();
            uint blockNumber = frame.Stream.ReadUint();
            ushort length = frame.Stream.ReadUshort();

            if (length > 8192)
            {
                this.SendError(session.Settings.CRC, Errors.ENOTSUP);
                return;
            }

            uint offset = blockNumber * length;

            // Retrieve this file handle from the file handle list.
            FileHandle FileHandle = session.FileHandles[fileHandle];

            if (FileHandle != null)
            {
                byte[] data = File.ReadAllBytes(FileHandle.FullFileName).Skip((int)offset).Take(length).ToArray();

                if (data.Length < length)
                {
                    byte[] returnData = new byte[length];
                    Array.Copy(data, returnData, data.Length);

                    outgoingFrame.WriteByte(0x84);

                    // write out the length
                    outgoingFrame.WriteUshort((ushort)returnData.Length);

                    // write out the data
                    outgoingFrame.WriteBytes(returnData);
                }
                else
                {
                    // write out the data buffer
                    outgoingFrame.WriteByte(0x84);

                    // write out the length
                    outgoingFrame.WriteUshort((ushort)data.Length);

                    // write out the data
                    outgoingFrame.WriteBytes(data);
                }

                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                server.Logger.Log($"StorageGet Requested file handle to read: {fileHandle:X06} but it was not found", Logger.Target.console);

                // send back error
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// Storage Put Block
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void StoragePutBlock(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            //server.Logger.Log("StoragePutBlock", Logger.Target.console);

            MemoryStream outgoingFrame = new MemoryStream();

            byte fileHandle = (byte)frame.Stream.ReadByte();
            uint blockNumber = frame.Stream.ReadUint();
            ushort length = frame.Stream.ReadUshort();

            if (length > 8192)
            {
                this.SendError(session.Settings.CRC, Errors.ENOTSUP);
                return;
            }

            byte[] data = frame.Stream.ReadBytes(length);

            uint offset = blockNumber * length;

            // Get the file handle
            FileHandle FileHandle = session.FileHandles[fileHandle];


            if (FileHandle != null && (
                FileHandle.GetFlagsAsNHACPFlags().HasFlag(NHACPFlags.O_RDONLY) ||
                FileHandle.GetFlagsAsNHACPFlags().HasFlag(NHACPFlags.O_RDWR)))
            {
                List<byte> bytes = File.ReadAllBytes(FileHandle.FullFileName).ToList();
                for (int i = 0; i < length; i++)
                {
                    bytes[(int)(i + offset)] = data[i];
                }

                File.WriteAllBytes(FileHandle.FullFileName, bytes.ToArray());

                outgoingFrame.WriteByte(0x81);
                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                server.Logger.Log($"Requested handle in HandleDeleteReplace {fileHandle:X06} but it was not found", Logger.Target.console);
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// File Handle Sequential Read
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void FileRead(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            server.Logger.Log("FileRead", Logger.Target.console);

            MemoryStream outgoingFrame = new MemoryStream();

            // Read the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();

            // Get the flags
            ushort flags = frame.Stream.ReadUshort();

            // Read the number of bytes to read
            ushort length = frame.Stream.ReadUshort();

            // Retrieve this file handle from the file handle list.
            FileHandle FileHandle = session.FileHandles[fileHandle];

            // if the file handle is null, what the heck?
            if (FileHandle != null)
            {
                byte[] data = File.ReadAllBytes(FileHandle.FullFileName).Skip((int)FileHandle.Index).Take(length).ToArray();
                FileHandle.Index += data.Length;

                // write out the data buffer
                outgoingFrame.WriteByte(0x84);

                // write how much data we got
                outgoingFrame.WriteUshort((ushort)data.Length);

                // write the data
                outgoingFrame.WriteBytes(data);

                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                server.Logger.Log($"Requested file handle for FileHandleReadSeq: {fileHandle:X06} but it was not found", Logger.Target.console);
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// File Write
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void FileWrite(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            server.Logger.Log("FileWrite", Logger.Target.console);

            MemoryStream outgoingFrame = new MemoryStream();

            // Read the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();

            // Get the flags
            ushort flags = frame.Stream.ReadUshort();

            // Read the number of bytes to read
            ushort length = frame.Stream.ReadUshort();

            byte[] data = frame.Stream.ReadBytes(length);

            if (length > 8192)
            {
                this.SendError(session.Settings.CRC, Errors.ENOTSUP);
                return;
            }

            // Retrieve this file handle from the file handle list.
            FileHandle FileHandle = session.FileHandles[fileHandle];

            if (FileHandle != null && (
                FileHandle.GetFlagsAsNHACPFlags().HasFlag(NHACPFlags.O_RDONLY) ||
                FileHandle.GetFlagsAsNHACPFlags().HasFlag(NHACPFlags.O_RDWR)))           
            {
                List<byte> bytes = File.ReadAllBytes(FileHandle.FullFileName).ToList();
                for (int i = 0; i < length; i++)
                {
                    bytes[(int)(i + FileHandle.Index)] = data[i];
                }

                File.WriteAllBytes(FileHandle.FullFileName, bytes.ToArray());

                outgoingFrame.WriteByte(0x81);
                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                server.Logger.Log($"Requested file handle for FileHandleReadSeq: {fileHandle:X06} but it was not found", Logger.Target.console);
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// File Seek
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        private void FileSeek(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            server.Logger.Log("FileSeek", Logger.Target.console);

            MemoryStream outgoingFrame = new MemoryStream();

            // read the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();

            // read the offset
            int offset = frame.Stream.ReadInt();

            // read the seek options
            byte seekOption = (byte)frame.Stream.ReadByte();

            SeekFlags seekFlags = (SeekFlags)seekOption;

            // Retrieve this file handle from the file handle list.
            FileHandle FileHandle = session.FileHandles[fileHandle];

            if (FileHandle != null)
            {
                FileInfo fileInfo = new FileInfo(FileHandle.FullFileName);

                if (seekFlags == SeekFlags.SET)
                {
                    // Seek from the start of the file
                    FileHandle.Index = offset;
                }
                else if (seekFlags == SeekFlags.CUR)
                {
                    // Seek from the current position in the file.
                    FileHandle.Index += offset;
                }
                else
                {
                    // Last option is from the end of the file.
                    FileHandle.Index = fileInfo.Length - offset;
                }

                if (FileHandle.Index < 0)
                {
                    FileHandle.Index = 0L;
                }
                else if (FileHandle.Index > fileInfo.Length)
                {
                    FileHandle.Index = fileInfo.Length;
                }

                outgoingFrame.WriteByte(0x89);
                outgoingFrame.WriteUint((uint)FileHandle.Index);
                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                server.Logger.Log($"Requested file handle for FileHandleSeek: {fileHandle:X06} but it was not found", Logger.Target.console);
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// File Get Info
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void FileGetInfo(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            // read the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();
        }

        /// <summary>
        /// File Set Sizxe
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void FileSetSize(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            // read the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();
        }

        /// <summary>
        /// List Directory
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void ListDir(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            server.Logger.Log("ListDir", Logger.Target.console);

            MemoryStream outgoingFrame = new MemoryStream();

            // read the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();

            // read the search pattern length
            byte length = (byte)frame.Stream.ReadByte();

            // read the search pattern
            string searchPattern = frame.Stream.ReadString(length);

            if (session.FileDetails.ContainsKey(fileHandle))
            {
                session.FileDetails[fileHandle].Clear();
                session.FileDetailsIndex[fileHandle] = 0;
            }
            else
            {
                session.FileDetails.Add(fileHandle, new List<FileDetails>());
                session.FileDetailsIndex.Add(fileHandle, 0);
            }

            // Retrieve this file handle from the file handle list.
            FileHandle FileHandle = session.FileHandles[fileHandle];

            if (FileHandle != null)
            {
                string[] files = Directory.GetFiles(Path.Combine(this.server.GetWorkingDirectory(), FileHandle.FileName), searchPattern);
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    session.FileDetails[fileHandle].Add(new FileDetails(fileInfo));
                }

                string[] directories = Directory.GetDirectories(Path.Combine(this.server.GetWorkingDirectory(), FileHandle.FileName), searchPattern);
                foreach (string directory in directories)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    session.FileDetails[fileHandle].Add(new FileDetails(directoryInfo));
                }

                outgoingFrame.WriteByte(0x81);
                this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
            }
            else
            {
                this.SendError(session.Settings.CRC, Errors.EBADF);
            }
        }

        /// <summary>
        /// Get Directory Entry
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void GetDirEntry(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            server.Logger.Log("GetDirEntry", Logger.Target.console);

            MemoryStream outgoingFrame = new MemoryStream();

            // read the file handle
            byte fileHandle = (byte)frame.Stream.ReadByte();

            byte length = (byte)frame.Stream.ReadByte();

            FileHandle FileHandle = session.FileHandles[fileHandle];

            if (FileHandle != null && session.FileDetails.ContainsKey(fileHandle))
            {
                int index = session.FileDetailsIndex[fileHandle];

                if (index < session.FileDetails[fileHandle].Count())
                {
                    FileDetails file = session.FileDetails[fileHandle][index];
                    session.FileDetailsIndex[fileHandle]++;

                    outgoingFrame.WriteByte(0x86);

                    // Write date time,
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    string time = DateTime.Now.ToString("HHmmss");
                    outgoingFrame.WriteBytes(System.Text.Encoding.ASCII.GetBytes(date));
                    outgoingFrame.WriteBytes(System.Text.Encoding.ASCII.GetBytes(time));

                    // write flags u16
                    if (file.FileType == FileDetails.fileType.Directory)
                    {
                        outgoingFrame.WriteByte(0x4);
                    }
                    else
                    {
                        outgoingFrame.WriteByte(0x3);
                    }

                    // write file size u32
                    outgoingFrame.WriteUint((uint)file.FileSize);
                    this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
                    return;
                }
                else
                {
                    outgoingFrame.WriteByte(0x81);
                    this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
                    return;
                }
            }

            this.SendError(session.Settings.CRC, Errors.EPERM);
        }

        /// <summary>
        /// Remove - Not Implemented
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void Remove(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            this.SendError(session.Settings.CRC, Errors.EACCES);

            //// Get the flags
            //ushort flags = frame.Stream.ReadUshort();

            //// get the filename Length
            //byte fileNameLen = (byte)frame.Stream.ReadByte();

            //// Get the filename
            //string fileName = frame.Stream.ReadString(fileNameLen);
            //fileName = this.SanitizeFilename(fileName);
        }

        /// <summary>
        /// Rename
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void Rename(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            // get the filename Length
            byte fileNameLen = (byte)frame.Stream.ReadByte();

            // Get the filename
            string oldName = frame.Stream.ReadString(fileNameLen);
            oldName = this.SanitizeFilename(oldName);

            // get the filename Length
            fileNameLen = (byte)frame.Stream.ReadByte();

            // Get the filename
            string newName = frame.Stream.ReadString(fileNameLen);
            newName = this.SanitizeFilename(newName);
        }

        /// <summary>
        /// Make Directory
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void Mkdir(NHACPFrame frame)
        {
            // Get the Session
            NHACPSession session = this.sessions[frame.SessionId];

            MemoryStream outgoingFrame = new MemoryStream();
            
            // get the directory name Length
            byte dirNameLen = (byte)frame.Stream.ReadByte();

            // Get the filename
            string directoryName = frame.Stream.ReadString(dirNameLen);

            directoryName = this.SanitizeFilename(directoryName);

            try
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }
            catch (Exception e)
            {
                this.server.Logger.Log(e.ToString(), Logger.Target.console);
                this.SendError(session.Settings.CRC, Errors.EACCES);
            }

            outgoingFrame.WriteByte(0x81);
            this.WriteFrame(session.Settings.CRC, outgoingFrame.ToArray());
        }

        /// <summary>
        /// End Session
        /// </summary>
        /// <param name="frame">the NHACPFrame</param>
        public void Goodbye(NHACPFrame frame)
        {
            if (frame.SessionId == 0)
            {
                this.Reset();
            }
            else if (this.sessions.ContainsKey(frame.SessionId))
            {
                this.sessions[frame.SessionId] = null;
            }
        }

        /// <summary>
        /// Initialize the extension - setup the member variables.
        /// </summary>
        private void Initialize()
        {
            this.sessions = new Dictionary<byte, NHACPSession>();

            for (int b = 0; b <= byte.MaxValue; b++)
            {
                this.sessions[(byte)b] = null;
            }
        }

        /// <summary>
        /// Sanitize Filename
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string SanitizeFilename(string path)
        {

            int index = path.IndexOf('\0');

            if (index > 0)
            {
                path = path.Substring(0, index);
            }

            // First, get the file name from path.
            string filename = Path.GetFileName(path);

            if (!Settings.AllowedExtensions.Contains(Path.GetExtension(filename), StringComparer.InvariantCultureIgnoreCase))
            {
                throw new ArgumentOutOfRangeException($"NABU requested a file extension which is not allowed: {Path.GetExtension(filename)}");
            }

            return filename;
        }


        /// <summary>
        /// Validate Url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private bool ValidateUri(string uri)
        {
            Uri outUri;

            if (Uri.TryCreate(uri, UriKind.Absolute, out outUri) && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
            {
                if (this.server.Settings.AllowedUri.Contains(outUri.Host, StringComparer.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// File Size
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file size</returns>
        private int FileSize(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                return (int)fileInfo.Length;
            }
            else
            {
                server.Logger.Log($"FileSize, unable to find file {fileName}, returing -1", Logger.Target.console);
                return -1;
            }
        }

        /// <summary>
        /// Send Error
        /// </summary>
        /// <param name="crc">Calculate CRC or not</param>
        /// <param name="error">Error value</param>
        private void SendError(bool crc, Errors error)
        {
            MemoryStream stream = new MemoryStream();

            stream.WriteByte(0x82);
            stream.WriteUshort((byte)error);
            stream.WriteString(ErrorStrings[(int)error]);

            this.WriteFrame(crc, stream.ToArray());
        }

        /// <summary>
        /// Write Frame
        /// </summary>
        /// <param name="crc">Calculate CRC or not</param>
        /// <param name="frame">Frame to send</param>
        private void WriteFrame(bool crc, byte[] frame)
        {
            if (crc)
            {
                List<Byte> frameList = new List<Byte>();
                frameList.AddRange(frame);
                frameList.Add(CRCCDMA.CalculateCRC(frame));
                frame = frameList.ToArray();
            }

            this.server.Connection.NabuStream.WriteUshort((ushort)frame.Length);
            this.server.Connection.NabuStream.WriteBytes(frame);
        }
    }
}
