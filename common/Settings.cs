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
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Settings class
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Major Version
        /// </summary>
        public static string majorVersion = "2";

        /// <summary>
        /// Minor Version
        /// </summary>
        public static string minorVersion = "0";

        /// <summary>
        /// Default Baud Rate
        /// </summary>
        public static string defaultBaudRate = "111865";

        /// <summary>
        /// Name of Headless bootloader program
        /// </summary>
        public static string HeadlessBootLoader = "Bootloader.nabu";

        /// <summary>
        /// List of allowed extensions the file API's are allowed to access
        /// </summary>
        public static string[] AllowedExtensions = { ".bin", ".nabu", ".dsk", ".txt", ".sc2", ".fth", ".sys", ".grb", ".img"};

        /// <summary>
        /// Internal enum for parsing state
        /// </summary>
        private enum ParseState
        {
            start,
            port,
            mode,
            path,
        }

        /// <summary>
        /// server mode
        /// </summary>
        public enum Mode
        {
            Serial,
            TCPIP
        }

        /// <summary>
        /// Source Location
        /// </summary>
        public enum SourceLocation
        {
            NabuNetwork,
            HomeBrew,
            GameRoom,
            LocalDirectory,
            Headless
        }

        /// <summary>
        /// Load Local Targets - only exposed in settings, for scenarios where people want to craft their own targets.xml file for local non networked use.
        /// </summary>
        public bool LoadLocalTargets { get; set; }

        /// <summary>
        /// List of uri's that the file api's are allowed to access.   For general consumption, we limit this to reduce exposure.
        /// </summary>
        public List<string> AllowedUri { get; set; }

        /// <summary>
        /// source location
        /// </summary>
        public SourceLocation? Location { get; set; }

        /// <summary>
        /// Gets the ask for channel setting
        /// </summary>
        public bool AskForChannel { get; set; }

        /// <summary>
        /// Save settings on exit
        /// </summary>
        public bool SaveSettingsOnExit { get; set; }

        /// <summary>
        /// flow control
        /// </summary>
        public bool DisableFlowControl { get; set; }

        /// <summary>
        /// TCP Delay
        /// </summary>
        public bool TcpNoDelay { get; set; }

        /// <summary>
        /// Baud Rate
        /// </summary>
        public string BaudRate { get; set; }

        /// <summary>
        /// Serial Port
        /// </summary>
        public string SerialPort { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; set; }
 
        /// <summary>
        /// Selected Cycle Name
        /// </summary>
        public string SelectedCycleName { get; set; }

        /// <summary>
        /// Selected Fun Stuff
        /// </summary>
        public string SelectedHomeBrew { get; set; }

        /// <summary>
        /// Selected Game Room
        /// </summary>
        public string SelectedGameRoom { get; set; }

        /// <summary>
        /// TCPIP Port
        /// </summary>
        public string TcpipPort { get; set; }

        /// <summary>
        /// Send Buffer
        /// </summary>
        public int SendBuffer { get; set; }

        /// <summary>
        /// Receive Buffer
        /// </summary>
        public int ReceiveBuffer { get; set; }

        /// <summary>
        /// Ignore Certificate Errors (Configuration file option only)
        /// </summary>
        public bool IgnoreCertificateErrors { get; set; }

        /// <summary>
        /// Gets the port
        /// </summary>
        public string Port
        {
            get
            {
                switch (this.OperatingMode.Value)
                {
                    case Mode.Serial:
                        return this.SerialPort;
                    case Mode.TCPIP:
                        return this.TcpipPort;
                }

                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the operating mode
        /// </summary>
        public Mode? OperatingMode { get; set;  }

        /// <summary>
        /// Gets the current directory, this is where we expect the files to be
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Gets the loaded cycles
        /// </summary>
        public Target[] Cycles { get; set; }

        /// <summary>
        /// Manual creation of settings
        /// </summary>
        public Settings()
        {
            this.LoadSettings();
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="args"></param>
        public Settings(string[] args)
        {
            this.LoadSettings();

            ParseState parseState = ParseState.start;
            this.OperatingMode = null;

            try
            {
                if (!args.Any())
                {
                    this.DisplayHelp();
                }

                // Parse the arguments into settings
                foreach (string argument in args)
                {
                    switch (parseState)
                    {
                        case ParseState.mode:
                            switch (argument.ToLowerInvariant())
                            {
                                case "serial":
                                    this.OperatingMode = Mode.Serial;
                                    break;
                                case "tcpip":
                                    this.OperatingMode = Mode.TCPIP;
                                    break;                                
                                default:
                                    this.DisplayHelp();
                                    break;
                            }

                            parseState = ParseState.start;
                            break;

                        case ParseState.port:
                            switch (this.OperatingMode)
                            {
                                case Mode.Serial:
                                    this.SerialPort = argument;
                                    break;
                                case Mode.TCPIP:
                                    this.TcpipPort = argument;
                                    break;                                
                            }
                            parseState = ParseState.start;
                            break;

                        case ParseState.path:
                            this.LocalPath = argument;
                            if (this.LocalPath.Equals("headless", StringComparison.InvariantCultureIgnoreCase))
                            {
                                this.Location = Settings.SourceLocation.Headless;
                                this.Path = Settings.HeadlessBootLoader;
                            }
                            else
                            {
                                this.Path = this.LocalPath;
                            }
                            parseState = ParseState.start;
                            break;

                        case ParseState.start:
                            switch (argument.ToLowerInvariant())
                            {
                                case "-mode":
                                    parseState = ParseState.mode;
                                    break;
                                case "-port":
                                    parseState = ParseState.port;
                                    break;
                                case "-askforchannel":
                                    this.AskForChannel = true;
                                    break;
                                case "-path":
                                    parseState = ParseState.path;
                                    break;
                                default:
                                    this.DisplayHelp();
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                this.DisplayHelp();
            }

            if (this.OperatingMode == null || string.IsNullOrWhiteSpace(this.Port))
            {
                this.DisplayHelp();
            }
        }

        /// <summary>
        /// Load the settings from the app.config
        /// </summary>
        public void LoadSettings()
        {
            this.LoadLocalTargets = false;

            this.AllowedUri = new List<string>();

            // Add nabunetwork.com and nabu.ca
            this.AllowedUri.Add("cloud.nabu.ca");
            this.AllowedUri.Add("www.nabunetwork.com");
            this.AllowedUri.Add("www.nabu.ca");

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

            // files location (default to nabu network)
            this.Location = SourceLocation.NabuNetwork;
            if (settings["Location"] != null)
            {
                this.Location = (SourceLocation)Enum.Parse(typeof(SourceLocation), settings["Location"].Value);
            }

            if (settings["LocalPath"] != null)
            {
                this.LocalPath = settings["LocalPath"].Value;
            }

            // Default to serial
            this.OperatingMode = Mode.Serial;
            if (settings["Mode"] != null)
            {
                this.OperatingMode = (Mode)Enum.Parse(typeof(Mode), settings["Mode"].Value);
            }

            if (settings["SerialPort"] != null)
            {
                this.SerialPort = settings["SerialPort"].Value;
            }

            if (settings["TcpipPort"] != null)
            {
                this.TcpipPort = settings["TcpipPort"].Value;
            }
            else
            {
                this.TcpipPort = "5816";
            }

            if (settings["AskForChannel"] != null)
            {
                this.AskForChannel = bool.Parse(settings["AskForChannel"].Value);
            }
            else
            {
                this.AskForChannel = false;
            }

            if (settings["Cycle"] != null)
            {
                this.SelectedCycleName = settings["Cycle"].Value;
            }

            if (settings["SelectedHomeBrew"] != null)
            {
                this.SelectedHomeBrew = settings["SelectedHomeBrew"].Value;
            }

            if (settings["SelectedGameRoom"] != null)
            {
                this.SelectedGameRoom = settings["SelectedGameRoom"].Value;
            }

            if (settings["BaudRate"] != null)
            {
                this.BaudRate = settings["BaudRate"].Value;
            }
            else
            {
                this.BaudRate = "111865";
            }

            if (settings["SaveSettingsOnExit"] != null)
            {
                this.SaveSettingsOnExit = bool.Parse(settings["SaveSettingsOnExit"].Value);
            }
            else
            {
                this.SaveSettingsOnExit = true;
            }

            if (settings["DisableFlowControl"] != null)
            {
                this.DisableFlowControl = bool.Parse(settings["DisableFlowControl"].Value);
            }
            else
            {
                this.DisableFlowControl = true;
            }

            if (settings["TcpNoDelay"] != null)
            {
                this.TcpNoDelay = bool.Parse(settings["TcpNoDelay"].Value);
            }
            else
            {
                this.TcpNoDelay = true;
            }

            if (settings["ReceiveBuffer"] != null)
            {
                this.ReceiveBuffer = int.Parse(settings["ReceiveBuffer"].Value);
            }
            else
            {
                this.ReceiveBuffer = 8192;
            }

            if (settings["SendBuffer"] != null)
            {
                this.SendBuffer = int.Parse(settings["SendBuffer"].Value);
            }
            else
            {
                this.SendBuffer = 8192;
            }

            if (settings["IgnoreCertificateErrors"] != null)
            {
                this.IgnoreCertificateErrors = bool.Parse(settings["IgnoreCertificateErrors"].Value);
            }
            else
            {
                this.IgnoreCertificateErrors = false;
            }

            // Populate UI based on settings
            string xml = string.Empty;

                if (!this.LoadLocalTargets)
                {
                    try
                    {
                        xml = WebClientWrapper.DownloadString("https://www.nabunetwork.com/NabuNetwork.xml");
                        System.IO.File.WriteAllText("Targets.xml", xml);
                    }
                    catch (Exception)
                    {
                        xml = System.IO.File.ReadAllText("Targets.xml");
                    }
                }
                else
                {
                    xml = System.IO.File.ReadAllText("Targets.xml");
                }

            var serializer = new XmlSerializer(typeof(Target[]));

            using (TextReader reader = new StringReader(xml))
            {
                this.Cycles = (Target[])serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Save settings
        /// </summary>
        public void SaveSettings()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

            this.AddOrUpdateSettings(settings, "Location", this.Location.ToString());
            this.AddOrUpdateSettings(settings, "LocalPath", this.LocalPath);
            this.AddOrUpdateSettings(settings, "Mode", this.OperatingMode.ToString());
            this.AddOrUpdateSettings(settings, "SerialPort", this.SerialPort);
            this.AddOrUpdateSettings(settings, "TcpipPort", this.TcpipPort);
            this.AddOrUpdateSettings(settings, "AskForChannel", this.AskForChannel.ToString());
            this.AddOrUpdateSettings(settings, "BaudRate", this.BaudRate.ToString());
            this.AddOrUpdateSettings(settings, "Cycle", this.SelectedCycleName);
            this.AddOrUpdateSettings(settings, "SelectedHomeBrew", this.SelectedHomeBrew);
            this.AddOrUpdateSettings(settings, "SelectedGameRoom", this.SelectedGameRoom);
            this.AddOrUpdateSettings(settings, "SaveSettingsOnExit", this.SaveSettingsOnExit.ToString());
            this.AddOrUpdateSettings(settings, "DisableFlowControl", this.DisableFlowControl.ToString());
            this.AddOrUpdateSettings(settings, "TcpNoDelay", this.TcpNoDelay.ToString());
            this.AddOrUpdateSettings(settings, "ReceiveBuffer", this.ReceiveBuffer.ToString());
            this.AddOrUpdateSettings(settings, "SendBuffer", this.SendBuffer.ToString());
            this.AddOrUpdateSettings(settings, "IgnoreCertificateErrors", this.IgnoreCertificateErrors.ToString());

            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="setting"></param>
        /// <param name="value"></param>
        public void AddOrUpdateSettings(KeyValueConfigurationCollection settings, string setting, string value)
        {
            if (settings[setting] == null)
            {
                settings.Add(setting, value);
            }
            else
            {
                settings[setting].Value = value;
            }
        }

        /// <summary>
        /// Display help on error
        /// </summary>
        public void DisplayHelp()
        {
            Console.WriteLine("NABU console server usage");
            Console.WriteLine("");
            Console.WriteLine("Parameters:");
            Console.WriteLine("-mode -port -askforchannel -path");
            Console.WriteLine();
            Console.WriteLine("mode options: Serial, TCPIP - listen to serial port or TCPIP port");
            Console.WriteLine("port: Which serial port or TCPIP port to listen to, examples would be COM4 or 5816");
            Console.WriteLine("askforchannel: Sets the flag to prompt the nabu for a channel");
            Console.WriteLine("path: can be one of the following options");
            Console.WriteLine();
            Console.WriteLine("       Local path for files, defaults to current directory");
            Console.WriteLine("       url to cloud location, example https://www.mydomain.com/paklocation");
            Console.WriteLine("       headless, to run in headless mode");
            Console.WriteLine();
            Console.WriteLine("Serial Mode example:");
            Console.WriteLine("NabuAdaptor.exe -Mode Serial -Port COM4 -path headless");
            Console.WriteLine("");
            Console.WriteLine("TCPIP Mode example:");
            Console.WriteLine("NabuAdaptor.exe -Mode TCPIP -Port 5816 -path https://www.nabunetwork.com/cycle2022");
            Environment.Exit(0);
        }
    }
}
