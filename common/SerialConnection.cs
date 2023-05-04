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
    using System.IO.Ports;

    /// <summary>
    /// Class to talk to nabu over serial
    /// </summary>
    public class SerialConnection : IConnection
    {
        /// <summary>
        /// Serial port 
        /// </summary>
        private SerialPort serialPort;

        /// <summary>
        /// Logger
        /// </summary>
        private Logger logger;

        /// <summary>
        /// settings
        /// </summary>
        private Settings Settings { get; set; }

        /// <summary>
        /// Stream used to read/write to/from nabu
        /// </summary>
        public Stream NabuStream { get; private set; }

        /// <summary>
        /// Flag to determine if the nabu is connected
        /// </summary>
        public bool Connected
        {
            get
            {
                if (this.serialPort != null)
                {
                    return this.serialPort.IsOpen;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialConnection"/> class.
        /// </summary>
        /// <param name="comPort">Com port to use</param>
        public SerialConnection(Settings settings, Logger logger)
        {
            this.Settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public void StartServer()
        {
            this.logger.Log("Server running in Serial mode", Logger.Target.console);
            this.serialPort?.Dispose();
            this.serialPort = new SerialPort();
            this.serialPort.PortName = this.Settings.SerialPort;
            this.serialPort.BaudRate = int.Parse(this.Settings.BaudRate);
            this.serialPort.StopBits = StopBits.Two;
            this.serialPort.Parity = Parity.None;
            this.serialPort.DataBits = 8;

            try
            {
                this.serialPort.ReceivedBytesThreshold = 1;
            }
            catch (NotImplementedException)
            { }

            this.serialPort.ReadBufferSize = 8192;
            this.serialPort.WriteBufferSize = 8192;
            this.serialPort.ReadTimeout = -1;

            if (!this.Settings.DisableFlowControl)
            {
                this.serialPort.DtrEnable = true;
                this.serialPort.RtsEnable = true;
            }
            else
            {
                this.serialPort.DtrEnable = false;
                this.serialPort.RtsEnable = false;
            }

            this.serialPort.Handshake = Handshake.None;

            this.serialPort.Open();
            this.NabuStream = this.serialPort.BaseStream;
            this.logger.Log($"Connected to {this.Settings.SerialPort}", Logger.Target.console);
        }

        /// <summary>
        /// Stop the server and clean up
        /// </summary>
        public void StopServer()
        {
            this.serialPort.Close();
            this.serialPort.Dispose();
        }
    }
}
