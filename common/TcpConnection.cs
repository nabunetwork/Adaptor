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
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Class to talk to nabu emulator over TCP/IP
    /// </summary>
    public class TcpConnection : IConnection
    {
        /// <summary>
        /// TCP/IP Client
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// TCP/IP Listener
        /// </summary>
        private static TcpListener tcpListener = null;

        /// <summary>
        /// settings
        /// </summary>
        private Settings Settings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Logger logger;

        /// <summary>
        /// Stream used to read/write from/to the nabu
        /// </summary>
        public Stream NabuStream { get; private set; }

        private static TcpListener GetListener(int port)
        {
            if (tcpListener == null)
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
            }

            return tcpListener;
        }

        /// <summary>
        /// Flag to determine if the port is connected - TODO - doesn't work.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (this.tcpClient != null)
                {
                    return this.tcpClient.Connected;
                }
                if (tcpListener != null)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpConnection"/> class.
        /// </summary>
        /// <param name="settings">Settings object</param>
        /// <param name="logger">Logger</param>
        public TcpConnection(Settings settings, Logger logger)
        {
            this.Settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Start the Server - open the socket and wait for a connection
        /// </summary>
        public void StartServer()
        {
            this.logger.Log("Server running in TCP/IP mode", Logger.Target.console);
            this.logger.Log("Waiting for connection", Logger.Target.console);
            this.tcpClient = GetListener(Int32.Parse(this.Settings.Port)).AcceptTcpClient();

            if (this.Settings.TcpNoDelay)
            {
                this.tcpClient.NoDelay = true;
            }

            this.tcpClient.ReceiveBufferSize = this.Settings.ReceiveBuffer;
            this.tcpClient.SendBufferSize = this.Settings.SendBuffer;
            this.tcpClient.LingerState = new LingerOption(false, 0);

            this.logger.Log("Connected", Logger.Target.console);
            this.NabuStream = tcpClient.GetStream();
        }

        /// <summary>
        /// Stop and clean up
        /// </summary>
        public void StopServer()
        {
            GetListener(Int32.Parse(this.Settings.Port)).Stop();
            tcpListener = null;

            if (this.tcpClient != null)
            {
                this.tcpClient.GetStream().Close();
                this.tcpClient.Close();
            }
        }
    }
}
