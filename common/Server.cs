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
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;
    using Extensions;

    /// <summary>
    /// Main implementation of the Nabu server, sits and waits for the nabu to request something and then fulfills that request.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Cache of loaded segments
        /// If you don't cache this, you'll be loading in the file and parsing everything for every individual packet.
        /// </summary>       
        public static ConcurrentDictionary<int, NabuSegment> cache = new ConcurrentDictionary<int, NabuSegment>();

        /// <summary>
        /// Logger
        /// </summary>
        public Logger Logger
        {
            get;
            set;
        }

        /// <summary>
        /// Server settings
        /// </summary>
        public Settings Settings
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private EventHandler<ProgressEventArgs> progress;

        /// <summary>
        /// Server Connection
        /// </summary>
        public IConnection Connection
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IServerExtension> Extensions
        {
            get; set;
        }

        /// <summary>
        /// Cycle Count - used in headless mode
        /// </summary>
        public int CycleCount
        {
            get;
            set;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="settings">server settings</param>
        public Server(Settings settings, EventHandler<string> logger = null, EventHandler<ProgressEventArgs> progress = null)
        {
            this.Settings = settings;

            if (logger != null)
            {
                this.Logger = new Logger(logger);
            }
            else
            {
                this.Logger = new Logger();
            }

            this.progress = progress;
        }

        /// <summary>
        /// Start the server
        /// </summary>
        private void StartServer()
        {
            this.StopServer();
            
            switch (this.Settings.OperatingMode)
            {
                case Settings.Mode.Serial:
                    this.Connection = new SerialConnection(this.Settings, this.Logger);
                    break;
                case Settings.Mode.TCPIP:
                    this.Connection = new TcpConnection(this.Settings, this.Logger);
                    break;                
            }

            this.Connection.StartServer();
            this.Extensions = new List<IServerExtension>();
            this.Extensions.Add(new FileStoreExtension(this));
            this.Extensions.Add(new HeadlessExtension(this));
            this.Extensions.Add(new NHACPExtension(this));
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void StopServer()
        {
            if (this.Connection != null && this.Connection.Connected)
            {
                this.Connection.StopServer();
            }
        }

        /// <summary>
        /// Simple server to handle Nabu requests
        /// </summary>
        public void RunServer(CancellationToken token)
        {
            do
            {
                try
                {
                    //bool initialized = false;
                    bool err = false;

                    // Start the server first
                    try
                    {
                        this.StartServer();
                        this.Logger.Log("Listening for NABU", Logger.Target.file);
                    }
                    catch (Exception e)
                    {
                        if (e is System.IO.IOException || e is System.UnauthorizedAccessException)
                        {
                            //this.Logger.Log("Invalid PORT settings, Stop the server and select the correct port", Logger.Target.console);
                            this.Logger.Log(e.ToString(), Logger.Target.console);
                            return;
                        }

                        err = true;
                    }

                    while (this.Connection != null && this.Connection.Connected && !err && !token.IsCancellationRequested)
                    {
                        byte b = this.ReadByte();
                        switch (b)
                        {
                            case 0x85: // Channel
                                this.WriteBytes(0x10, 0x6);
                                int channel = this.ReadByte() + (this.ReadByte() << 8);
                                this.Logger.Log($"Received Channel {channel:X8}", Logger.Target.file);
                                this.WriteBytes(0xE4);
                                break;
                            case 0x84: // File Transfer
                                this.HandleFileRequest();
                                break;
                            case 0x83:
                                this.WriteBytes(0x10, 0x6, 0xE4);
                                break;
                            case 0x82:
                                this.ConfigureChannel(this.Settings.AskForChannel);
                                break;
                            case 0x81:
                                this.WriteBytes(0x10, 0x6);
                                this.ReadByte();
                                this.ReadByte();
                                this.WriteBytes(0xE4);
                                break;
                            case 0x1E:
                                this.WriteBytes(0x10, 0xE1);
                                break;
                            case 0x5:
                                this.WriteBytes(0xE4);
                                break;
                            case 0xF:
                                break;
                            case 0xFF:
                                // Well, we are reading garbage, socket has probably closed, quit this loop
                                err = true;
                                break;
                            default:
                                bool completed = false;

                                foreach (IServerExtension extension in this.Extensions)
                                {
                                    if (extension.TryProcessCommand(b, token))
                                    {
                                        completed = true;
                                        break;
                                    }
                                }

                                if (!completed)
                                {
                                    this.Logger.Log($"Unknown command {b:X8}", Logger.Target.console);
                                    this.WriteBytes(0x10, 0x6);
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Log($"Exception {ex.Message}", Logger.Target.file);
                }
                finally
                {
                    this.StopServer();
                }
            } while (!token.IsCancellationRequested);

        }

        /// <summary>
        /// Get the current working directory
        /// </summary>
        /// <returns></returns>
        public string GetWorkingDirectory()
        {
            ILoader loader;

            if (this.Settings.Path.ToLowerInvariant().StartsWith("http"))
            {
                loader = new WebLoader();
            }
            else
            {
                loader = new LocalLoader();
            }

            string directory = string.Empty;

            loader.TryGetDirectory(this.Settings.Path, out directory);

            return directory;
        }

        /// <summary>
        /// Handle the Nabu's file request
        /// </summary>
        private void HandleFileRequest()
        {
            this.WriteBytes(0x10, 0x6);

            // Ok, get the requested packet and segment info
            int packetNumber = this.GetRequestedPacket();          
            int segmentNumber = this.GetRequestedSegment();

            string segmentName = $"{segmentNumber:X06}";
            this.Logger.Log($"NABU requesting segment {segmentNumber:X06} and packet {packetNumber:X06}", Logger.Target.file);

            // ok
            this.WriteBytes(0xE4);
            NabuSegment segment = null;

            if (segmentNumber == 0x1 && packetNumber == 0x0)
            {
                foreach (IServerExtension extension in this.Extensions)
                {
                    //this.Logger.Log($"Resetting Registered Extension: {extension.GetType().Name}", Logger.Target.file);
                    extension.Reset();
                }

                if (this.Settings.Location == Settings.SourceLocation.Headless)
                {
                    // We are headless, and a cycle, need two loads of segment & packet in a row to reset
                    this.CycleCount++;
                    if (this.CycleCount > 1)
                    {
                        this.ResetHeadless();
                    }
                }

                if (this.Settings.Location == Settings.SourceLocation.LocalDirectory)
                {
                    cache.Clear();
                }
            }

            if (segmentNumber != 0x1 && segmentNumber != 0x2 && segmentNumber != 0x7FFFFF && this.Settings.Location == Settings.SourceLocation.Headless)
            {
                this.CycleCount = 0;
            }


            if (segmentNumber == 0x7FFFFF)
            {
                segment = SegmentManager.CreateTimeSegment();
            }
            else
            {
                if (cache.ContainsKey(segmentNumber))
                {
                    segment = cache[segmentNumber];
                }

                if (segment == null)
                {
                    // If the path starts with http, go cloud - otherwise local
                    ILoader loader;
                    if (this.Settings.Path.ToLowerInvariant().StartsWith("http"))
                    {
                        loader = new WebLoader();
                    }
                    else
                    {
                        loader = new LocalLoader();
                    }

                    // if the path ends with .nabu:
                    byte[] data = null;

                    if (this.Settings.Path.ToLowerInvariant().EndsWith(".nabu") && segmentNumber == 1)
                    {
                        this.Logger.Log($"Loading NABU segment {segmentNumber:X06} from {this.Settings.Path}", Logger.Target.console);

                        if (loader.TryGetData($"{this.Settings.Path}", out data))
                        {
                            segment = SegmentManager.CreatePackets(segmentNumber, data);
                        }
                    }
                    else if (this.Settings.Path.ToLowerInvariant().EndsWith(".pak") && segmentNumber == 1)
                    {
                        this.Logger.Log($"Creating NABU segment {segmentNumber:X06} from {this.Settings.Path}", Logger.Target.console);

                        if (loader.TryGetData($"{this.Settings.Path}", out data))
                        {
                            segment = SegmentManager.LoadPackets(segmentNumber, data, this.Logger);
                        }
                    }
                    else
                    {
                        string directory;

                        if (loader.TryGetDirectory(this.Settings.Path, out directory))
                        {
                            string fileName = $"{directory}/{segmentName}.nabu";

                            this.Logger.Log($"Creating NABU segment {segmentNumber:X06} from {fileName}", Logger.Target.console);

                            if (loader.TryGetData(fileName, out data))
                            {
                                segment = SegmentManager.CreatePackets(segmentNumber, data);
                            }
                            else if (loader.TryGetData($"{directory}/{segmentName}.pak", out data))
                            {
                                this.Logger.Log($"loading NABU segment {segmentNumber:X06} from {directory}/{segmentName}.pak", Logger.Target.console);
                                segment = SegmentManager.LoadPackets(segmentNumber, data, this.Logger);
                            }
                        }
                    }
                    
                    if (segment == null)
                    {
                        if (this.Settings.Location == Settings.SourceLocation.Headless)
                        {
                            this.Logger.Log($"Could not load requested headless target, reloading menu", Logger.Target.console);

                            loader = new LocalLoader();
                            if (loader.TryGetData(Settings.HeadlessBootLoader, out data))
                            {
                                segment = SegmentManager.CreatePackets(segmentNumber, data);
                            }

                            cache.TryAdd(segmentNumber, segment);
                        }
                        else
                        {
                            if (segmentNumber == 1)
                            {
                                // Nabu can't do anything without an initial pack - throw and be done.
                                throw new FileNotFoundException($"Initial NABU file of {segmentName} was not found, fix this");
                            }

                            // File not found, write unauthorized
                            this.WriteBytes(0x90);
                            this.ReadByte(0x10);
                            this.ReadByte(0x6);
                        }
                    }
                    else
                    {
                        cache.TryAdd(segmentNumber, segment);
                    }
                }                
            }

            // Send the requested segment of the pack
            if (segment != null && packetNumber <= segment.Packets.Length)
            {
                this.WriteBytes(0x91);
                byte b = this.ReadByte();
                if (b != 0x10)
                {
                    this.WriteBytes(0x10, 0x6, 0xE4);
                    return;
                }

                this.ReadByte(0x6);
                this.SendPacket(segment.Packets[packetNumber]);
                this.WriteBytes(0x10, 0xE1);

                if (this.progress != null)
                {
                    ProgressEventArgs args = new ProgressEventArgs(segmentNumber, packetNumber, segment.Packets.Length);
                    progress(this, args);
                }
                else
                {
                    Spinner.Turn(segmentNumber);
                }
            }
        }

        /// <summary>
        /// Write the byte array to the stream
        /// </summary>
        /// <param name="bytes">bytes to send</param>
        public void WriteBytes(params byte[] bytes)
        {
            this.Connection.NabuStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Get the requested packet
        /// </summary>
        /// <returns>requested packet</returns>
        private byte GetRequestedPacket()
        {
            return this.ReadByte();
        }

        /// <summary>
        /// Send the packet to the nabu
        /// </summary>
        /// <param name="packet">packet to send</param>
        private void SendPacket(NabuPacket packet)
        {
            byte[] array = packet.EscapedData;

            this.Connection.NabuStream.Write(array, 0, array.Length);
        }

        /// <summary>
        /// Get the requested segment name
        /// </summary>
        /// <returns>requested segment file</returns>
        private int GetRequestedSegment()
        {
            byte b1 = this.ReadByte();
            byte b2 = this.ReadByte();
            byte b3 = this.ReadByte();
            int packFile = b1 + (b2 << 8) + (b3 << 16);
            return packFile;
        }

        /// <summary>
        /// Read Byte - but throw if the byte we read is not what we expect (passed in)
        /// </summary>
        /// <param name="expectedByte">This is the value we expect to read</param>
        /// <returns>The read byte, or throw</returns>
        private byte ReadByte(byte expectedByte)
        {
            byte num = this.ReadByte();
           
            if (num != expectedByte)
            {
                throw new Exception($"Read {num:X02} but expected {expectedByte:X02}");
            }

            return num;
        }

        /// <summary>
        /// Read a single byte from the stream
        /// </summary>
        /// <returns>read byte</returns>
        internal byte ReadByte()
        {
            return (byte)this.Connection.NabuStream.ReadByte();
        }

        /// <summary>
        /// Read a sequence of bytes from the stream
        /// </summary>
        /// <returns></returns>
        private byte[] ReadBytes()
        {
            byte[] buffer = new byte[1024];

            this.Connection.NabuStream.Read(buffer, 0, 1024);
            return buffer;
        }

        /// <summary>
        /// Handle NABU channel negotiation
        /// </summary>
        /// <param name="askForChannel"></param>
        private void ConfigureChannel(bool askForChannel)
        {
            this.WriteBytes(0x10, 0x6);
            byte[] temp = this.ReadBytes();

            if (!askForChannel)
            {
                this.WriteBytes(0x1F, 0x10, 0xE1);
            }
            else
            {
                this.Logger.Log("Asking for channel", Logger.Target.file);
                this.WriteBytes(0xFF, 0x10, 0xE1);
            }
        }

        /// <summary>
        /// Reset the server to load headless
        /// </summary>
        private void ResetHeadless()
        {
            // reset the cache
            cache.Clear();
            this.CycleCount = 0;
            // set the headless app
            this.Settings.Path = Settings.HeadlessBootLoader;
        }
    }
}
