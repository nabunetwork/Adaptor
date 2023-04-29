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

    public class HeadlessExtension : IServerExtension
    {
        /// <summary>
        /// Instance of the server
        /// </summary>
        private Server server;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlessExtension"/> class. 
        /// </summary>
        /// <param name="server"></param>
        public HeadlessExtension(Server server)
        {
            this.server = server;
        }

        /// <summary>
        /// Reset this extension.  If the Nabu starts over loading segment 0 and packet 0 - start over.
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// This extension implements several new op codes - This function maps those codes to the appropriate function call.
        /// </summary>
        /// <param name="opCode">OP code to process</param>
        /// <returns>true if we acted on this opCode, otherwise false.</returns>
        public bool TryProcessCommand(byte opCode, CancellationToken token)
        {
            switch (opCode)
            {
                case 0x20:
                    // Send the main menu
                    this.SendMenuCount();
                    return true;
                case 0x21:
                    // send specified menu item
                    this.SendMenuItem();
                    return true;
                case 0x22:
                    // Select the specified cycle
                    this.SetCycle();
                    return true;
                case 0x23:
                    // set the specified local path
                    this.SetPath();
                    return true;
            }

            // Op code not serviced by this extension
            return false;
        }

        /// <summary>
        /// The NABU will set the cycle
        /// </summary>
        private void SetCycle()
        {
            // Read the value
            byte menu = this.server.ReadByte();

            // Read the menu option
            // Get the item number:
            byte menuItem = this.server.ReadByte();

            List<string> names = new List<string>();

            if (this.server.Settings.Location != Settings.SourceLocation.Headless)
            {
                // Don't set location unless in headless mode.
                return;
            }

            switch (menu)
            {
                case 1:
                    IEnumerable<Target> nabunetwork = from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.NabuNetwork select item;                    
                    names = nabunetwork.Select(cycle => cycle.Name).ToList();
                    break;
                case 2:
                    IEnumerable<Target> homebrew = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Homebrew select item).OrderBy(x => x.Name).ToList();
                    names = homebrew.Select(cycle => cycle.Name).ToList();
                    break;
                case 3:
                    IEnumerable<Target> gameroom = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Gameroom select item).OrderBy(x => x.Name).ToList();
                    names = gameroom.Select(cycle => cycle.Name).ToList();
                    break;
            }

            Target selected = (from item in this.server.Settings.Cycles where item.Name == names[menuItem] select item).First();

            // Set the URL in the software and the NABU will reboot.
            this.server.Settings.Path = selected.Url;
            this.server.CycleCount = 0;
            Server.cache.Clear();
        }

        /// <summary>
        /// The NABU will set the path
        /// </summary>
        private void SetPath()
        {
            int strlen = this.server.Connection.NabuStream.ReadByte();
            
            string path = this.server.Connection.NabuStream.ReadString(strlen);

            if (this.server.Settings.Location != Settings.SourceLocation.Headless)
            {
                // Don't set location unless in headless mode.
                return;
            }

            if (path.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                Uri url = new Uri(path);

                //is this Url valid?
                if (this.server.Settings.AllowedUri.Contains(url.Host, StringComparer.InvariantCultureIgnoreCase))
                {
                    this.server.Settings.Path = path;
                    this.server.CycleCount = 0;
                    Server.cache.Clear();
                }
            }
            else
            {
                // two things about headless - First, must be in the current working directory and must either be a directory or .nabu
                string extension = Path.GetExtension(path);
                if (extension == "" || extension == ".nabu" || extension == ".pak")
                {
                    string fullPath = Path.GetFullPath(path);
                    if (fullPath.StartsWith(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.server.Settings.Path = fullPath;
                        this.server.CycleCount = 0;
                        Server.cache.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Send the specified submenu to the NABU
        /// </summary>
        private void SendSubMenu()
        {
            // Get the menu number
            byte menu = this.server.ReadByte();

            List<string> names = new List<string>();

            // send down the specified menu
            switch (menu)
            {
                case 1:
                    IEnumerable<Target> nabunetwork = from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.NabuNetwork select item;
                    names = nabunetwork.Select(cycle => cycle.Name).ToList();
                    break;
                case 2:
                    IEnumerable<Target> homebrew = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Homebrew select item).OrderBy(x => x.Name).ToList();
                    names = homebrew.Select(cycle => cycle.Name).ToList();
                    break;
                case 3:
                    IEnumerable<Target> gameroom = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Gameroom select item).OrderBy(x => x.Name).ToList();
                    names = gameroom.Select(cycle => cycle.Name).ToList();
                    break;
            }

            if (names.Any())
            {
                // Write # of strings
                foreach (string name in names)
                {
                    this.server.Logger.Log(name, Logger.Target.console);
                    this.server.Connection.NabuStream.WriteString($"{name}");
                }
            }
        }

        /// <summary>
        /// Send the number of menus to the NABU
        /// </summary>
        private void SendMenuCount()
        {
            // Get the menu number
            byte menu = this.server.ReadByte();

            List<string> names = new List<string>();

            switch (menu)
            {
                case 0:
                    names.Add("NabuNetwork.com");
                    names.Add("Homebrew");
                    names.Add("Game Room");
                    names.Add("Local Path");
                    break;
                case 1:
                    IEnumerable<Target> nabunetwork = from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.NabuNetwork select item;
                    names = nabunetwork.Select(cycle => cycle.Name).ToList();
                    break;
                case 2:
                    IEnumerable<Target> homebrew = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Homebrew select item).OrderBy(x => x.Name).ToList();
                    names = homebrew.Select(cycle => cycle.Name).ToList();
                    break;
                case 3:
                    IEnumerable<Target> gameroom = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Gameroom select item).OrderBy(x => x.Name).ToList();
                    names = gameroom.Select(cycle => cycle.Name).ToList();
                    break;
            }

            this.server.Connection.NabuStream.WriteByte((byte)names.Count);
        }

        /// <summary>
        /// Send the specified menu item to the NABU
        /// </summary>
        private void SendMenuItem()
        {
            // Get the menu number
            byte menu = this.server.ReadByte();

            // Get the item number:
            byte menuItem = this.server.ReadByte();

            List<string> names = new List<string>();

            switch (menu)
            {
                case 0:
                    names.Add("NabuNetwork.com");
                    names.Add("Homebrew Software");
                    names.Add("Game Room");
                    names.Add("Local Path");
                    break;
                case 1:
                    IEnumerable<Target> nabunetwork = from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.NabuNetwork select item;
                    names = nabunetwork.Select(cycle => cycle.Name).ToList();
                    break;
                case 2:
                    IEnumerable<Target> homebrew = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Homebrew select item).OrderBy(x => x.Name).ToList();
                    names = homebrew.Select(cycle => cycle.Name).ToList();
                    break;
                case 3:
                    IEnumerable<Target> gameroom = (from item in this.server.Settings.Cycles where item.TargetType == Target.TargetEnum.Gameroom select item).OrderBy(x => x.Name).ToList();
                    names = gameroom.Select(cycle => cycle.Name).ToList();
                    break;
            }

            this.server.Connection.NabuStream.WriteString(names[menuItem]);
        }
    }
}
