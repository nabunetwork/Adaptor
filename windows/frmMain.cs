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
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO.Ports;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Main UI for the adaptor
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>
        /// Server object
        /// </summary>
        private Server server = null;

        /// <summary>
        /// Settings object
        /// </summary>
        private Settings settings = null;

        /// <summary>
        /// Task for Server execution
        /// </summary>
        private Task task = null;

        /// <summary>
        /// Cancellation token source
        /// </summary>
        CancellationTokenSource source;

        /// <summary>
        ///  Initializes a new instance of the <see cref="frmMain"/> class. 
        /// </summary>
        public frmMain()
        {
            this.settings = new Settings();
            InitializeComponent();

            if (this.settings.IgnoreCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }

            // just hide the channel check box for now
            chkChannel.Hide();

            this.Text = $"NabuNetwork.com Internet Adapter (v{Settings.majorVersion}.{Settings.minorVersion})";

            try
            {
                // Get all the Cycles from the list (sort)
                IEnumerable<Target> nabunetwork = from item in this.settings.Cycles where item.TargetType == Target.TargetEnum.NabuNetwork select item;
                IEnumerable<Target> homebrew = (from item in this.settings.Cycles where item.TargetType == Target.TargetEnum.Homebrew select item).OrderBy(x => x.Name).ToList();
                IEnumerable<Target> gameroom = (from item in this.settings.Cycles where item.TargetType == Target.TargetEnum.Gameroom select item).OrderBy(x => x.Name).ToList();

                // Populate Cycles
                foreach (Target cycle in nabunetwork)
                {
                    comboNabuNetwork.Items.Add(cycle);
                    if (cycle.Name.Equals(this.settings.SelectedCycleName))
                    {
                        comboNabuNetwork.SelectedItem = cycle;
                    }
                }

                if (comboNabuNetwork.Items.Count > 0 && comboNabuNetwork.SelectedItem == null)
                {
                    comboNabuNetwork.SelectedItem = comboNabuNetwork.Items[0];
                }

                foreach (Target cycle in homebrew)
                {
                    comboHomeBrew.Items.Add(cycle);
                    if (cycle.Name.Equals(this.settings.SelectedHomeBrew))
                    {
                        comboHomeBrew.SelectedItem = cycle;
                    }
                }

                if (comboHomeBrew.Items.Count > 0 && comboHomeBrew.SelectedItem == null)
                {
                    comboHomeBrew.SelectedItem = comboHomeBrew.Items[0];
                }

                foreach (Target cycle in gameroom)
                {
                    comboGameRoom.Items.Add(cycle);
                    if (cycle.Name.Equals(this.settings.SelectedGameRoom))
                    {
                        comboGameRoom.SelectedItem = cycle;
                    }
                }

                if (comboGameRoom.Items.Count > 0 && comboGameRoom.SelectedItem == null)
                {
                    comboGameRoom.SelectedItem = comboGameRoom.Items[0];
                }
            }
            catch (Exception e)
            {
                this.Log(e.ToString());
            }

            if (comboHomeBrew.Items.Count > 0 && comboHomeBrew.SelectedItem == null)
            {
                comboHomeBrew.SelectedItem = comboHomeBrew.Items[0];
            }

            txtLocalPath.Text = this.settings.LocalPath;

            // Disable all panels: - TODO
            this.HidePanel(panelNabuNetwork);
            this.HidePanel(panelHomeBrew);
            this.HidePanel(panelLocalDirectory);
            this.HidePanel(panelGameRoom);
            this.HidePanel(panelHeadless);
            //this.HidePanel(panelLog);

            switch (this.settings.Location)
            {
                case Settings.SourceLocation.NabuNetwork:
                    this.ShowPanel(panelNabuNetwork);
                    this.settings.Path = comboNabuNetwork.SelectedItem != null ? ((Target)comboNabuNetwork.SelectedItem).Url : string.Empty;
                    break;
                case Settings.SourceLocation.HomeBrew:
                    this.ShowPanel(panelHomeBrew);
                    this.settings.Path = comboHomeBrew.SelectedItem != null ? ((Target)comboHomeBrew.SelectedItem).Url : string.Empty;
                    break;
                case Settings.SourceLocation.LocalDirectory:
                    this.ShowPanel(panelLocalDirectory);
                    this.settings.Path = txtLocalPath.Text;
                    break;
                case Settings.SourceLocation.GameRoom:
                    this.ShowPanel(panelGameRoom);
                    this.settings.Path = comboGameRoom.SelectedItem != null ? ((Target)comboGameRoom.SelectedItem).Url : string.Empty;
                    break;
                case Settings.SourceLocation.Headless:
                    this.ShowPanel(panelHeadless);
                    this.settings.Path = Settings.HeadlessBootLoader;
                    break;
            }

            // Disable events for initial setting:
            this.txtLocalPath.TextChanged -= this.txtFileLocation_TextChanged;
            txtLocalPath.Text = this.settings.LocalPath;
            
            // Re-Enable events
            this.txtLocalPath.TextChanged += new EventHandler(this.txtFileLocation_TextChanged);

            switch (this.settings.OperatingMode)
            {
                case Settings.Mode.Serial:
                    radioSerial.Checked = true;
                    txtTcpipPort.Enabled = false;
                    txtTcpipPort.Hide();
                    break;
                case Settings.Mode.TCPIP:
                    radioTcpip.Checked = true;
                    comboSerialPorts.Enabled = false;
                    comboSerialPorts.Hide();
                    break;
            }

            txtTcpipPort.Text = this.settings.TcpipPort;

            // Populate COM ports
            PopulatePorts(this.settings.SerialPort);
            comboSerialPorts.Refresh();

            chkChannel.Checked = this.settings.AskForChannel;
        }

        /// <summary>
        /// Populate the Ports - Event handler for the ports dropdown
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void comboPorts_DropDown(object sender, EventArgs e)
        {
            PopulatePorts(string.Empty);
        }

        /// <summary>
        /// Populate the serial port list, and highlight the saved value
        /// </summary>
        /// <param name="savedPort"></param>
        private void PopulatePorts(string savedPort)
        {
            try
            {
                if (!base.InvokeRequired)
                {
                    string[] ports = SerialPort.GetPortNames();
                    comboSerialPorts.Items.Clear();
                    foreach (string port in ports)
                    {
                        comboSerialPorts.Items.Add(port);
                        if (port.Equals(savedPort))
                        {
                            comboSerialPorts.SelectedItem = port;
                        }
                    }
                }
                else
                {
                    Invoke((Action)delegate
                    {
                        PopulatePorts(savedPort);
                    });
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Start the Server
        /// </summary>
        private void StartServer()
        {
            if (this.server == null)
            {
               this.source = new CancellationTokenSource();
               this.server = new Server(this.settings, this.LogEventHandler, this.ProgressEventHandler);
               this.task = Task.Factory.StartNew(() => this.server.RunServer(this.source.Token));
            }
        }

        /// <summary>
        /// Stop the Server
        /// </summary>
        private void StopServer()
        {
            this.server.StopServer();
            this.source.Cancel();
            this.task.Wait(1000);
            this.server = null;
        }

        /// <summary>
        /// Show download progress
        /// </summary>
        /// <param name="curr">current value</param>
        /// <param name="max">Maximum Value</param>
        private void SetProgress(ProgressEventArgs args)
        {
            try
            {
                if (!base.InvokeRequired)
                {
                    toolStripSegmentName.Text = $"Transfering Segment: {args.Segment.ToString("X6")}";
                    toolStripProgressBar.Minimum = 0;
                    toolStripProgressBar.Maximum = args.Max;
                    toolStripProgressBar.Value = args.Curr + 1;
                }
                else
                {
                    Invoke((Action)delegate
                    {
                        SetProgress(args);
                    });
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Log message to the log window
        /// </summary>
        /// <param name="message">Message to log</param>
        private void Log(string message)
        {
            try
            {
                if (!base.InvokeRequired)
                {
                    txtLog.AppendText(message);
                    txtLog.AppendText(Environment.NewLine);
                }
                else
                {
                    Invoke((Action)delegate
                    {
                        Log(message);
                    });
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Progress Event Handler, used for multi-threading
        /// </summary>
        /// <param name="obj">sender object</param>
        /// <param name="args">Progress Event Args</param>
        private void ProgressEventHandler(object obj, ProgressEventArgs args)
        {
            SetProgress(args);
        }

        /// <summary>
        /// Log Event Handler, used for multi-threading
        /// </summary>
        /// <param name="obj">sender object</param>
        /// <param name="message">log message</param>
        private void LogEventHandler(object obj, string message)
        {
            Log(message);
        }

        /// <summary>
        /// Choose Folder button event handler
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void btnChooseFolder_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "Folder";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName.EndsWith("Folder.nabu"))
                {
                    txtLocalPath.Text = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                }
                else
                {
                    txtLocalPath.Text = openFileDialog1.FileName;
                }
            }
        }

        /// <summary>
        /// Save Setting Menu event handler
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.settings.LocalPath = txtLocalPath.Text;
            this.settings.TcpipPort = txtTcpipPort.Text;
            this.settings.SaveSettings();
        }

        /// <summary>
        /// Selected Item Changed event handler for Serial Ports
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void comboSerialPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.SerialPort = comboSerialPorts.SelectedItem.ToString();
        }

        /// <summary>
        /// Exit menu event handler
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Start Server event handler
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.settings.Location == Settings.SourceLocation.NabuNetwork && comboNabuNetwork.SelectedItem == null)
            {
                this.Log("Must specify Cycle for NabuNetwork");
                return;
            }

            if (this.settings.Location == Settings.SourceLocation.HomeBrew && comboHomeBrew.SelectedItem == null)
            {
                this.Log("Must specify a valid option for the FunStuff source");
                return;
            }

            if (this.settings.Location == Settings.SourceLocation.GameRoom && comboGameRoom.SelectedItem == null)
            {
                this.Log("Must specify a valid option for the Arcade source");
                return;
            }

            if (this.settings.Location == Settings.SourceLocation.LocalDirectory && !string.IsNullOrWhiteSpace(this.settings.LocalPath) && (!(System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(this.settings.LocalPath)) || System.IO.File.Exists(this.settings.LocalPath))))
            {
                if (!this.settings.LocalPath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    this.Log($"Directory specified does not exist {this.settings.LocalPath}");
                    return;
                }
            }

            this.settings.LocalPath = txtLocalPath.Text;

            if (server != null)
            {
                this.EnableModePanel();
                StopServer();
                btnStart.Text = "Start Server";
                btnStart.BackColor = SystemColors.Control;
            }
            else
            {
                this.DisableModePanel();
                Server.cache.Clear();
                StartServer();
                txtLog.Text = "";
                btnStart.Text = "Stop Server";
                btnStart.BackColor = Color.Red;
            }
        }

        /// <summary>
        /// Enable all controls in mode panel
        /// </summary>
        private void EnableModePanel()
        {
            foreach (Control control in groupBoxMode.Controls)
            {
                control.Enabled = true;
            }
        }

        /// <summary>
        /// Disable all controls in mode panel
        /// </summary>
        private void DisableModePanel()
        {
            foreach (Control control in groupBoxMode.Controls)
            {
                control.Enabled = false;
            }
        }

        /// <summary>
        /// Event handler for ask for channel check box
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void chkChannel_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.AskForChannel = chkChannel.Checked;
        }

        /// <summary>
        /// Hide all controls in specified panel
        /// </summary>
        /// <param name="panel">winforms panel</param>
        private void HidePanel(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is RadioButton)
                {
                    ((RadioButton)control).Checked = false;
                }
                else
                {
                    control.Hide();
                }
            }
        }

        /// <summary>
        /// Show all controls in specified panel
        /// </summary>
        /// <param name="panel">winforms panel</param>
        private void ShowPanel(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is RadioButton)
                {
                    ((RadioButton)control).Checked = true;
                }
                if (!(control is RadioButton))
                {
                    control.Show();
                }
            }
        }

        /// <summary>
        /// Event handler for Game Room
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void radioButtonGameRoom_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGameRoom.Checked)
            {
                this.ShowPanel(panelGameRoom);
                this.HidePanel(panelNabuNetwork);
                this.HidePanel(panelHomeBrew);
                this.HidePanel(panelLocalDirectory);
                this.HidePanel(panelHeadless);
                this.settings.Location = Settings.SourceLocation.GameRoom;
                if (this.comboGameRoom.SelectedItem != null)
                {
                    this.settings.Path = ((Target)this.comboGameRoom.SelectedItem).Url;
                }
            }

            Server.cache.Clear();
        }

        /// <summary>
        /// Event Handler for Homebrew
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void radioButtonHomeBrew_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHomeBrew.Checked)
            {
                this.HidePanel(panelNabuNetwork);
                this.ShowPanel(panelHomeBrew);
                this.HidePanel(panelLocalDirectory);
                this.HidePanel(panelGameRoom);
                this.HidePanel(panelHeadless);
                this.settings.Location = Settings.SourceLocation.HomeBrew;
                if (this.comboHomeBrew.SelectedItem != null)
                {
                    this.settings.Path = ((Target)this.comboHomeBrew.SelectedItem).Url;
                }
            }

            Server.cache.Clear();
        }

        /// <summary>
        /// Event Handler for Local Directory
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void radioButtonLocalDirectory_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLocalDirectory.Checked)
            {
                this.HidePanel(panelNabuNetwork);
                this.HidePanel(panelHomeBrew);
                this.ShowPanel(panelLocalDirectory);
                this.HidePanel(panelGameRoom);
                this.HidePanel(panelHeadless);
                this.settings.Location = Settings.SourceLocation.LocalDirectory;
                this.settings.Path = txtLocalPath.Text;
            }

            Server.cache.Clear();
        }

        /// <summary>
        /// Event Handler for NabuNetwork
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void radioButtonNabuNetwork_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNabuNetwork.Checked)
            {
                this.ShowPanel(panelNabuNetwork);
                this.HidePanel(panelHomeBrew);
                this.HidePanel(panelLocalDirectory);
                this.HidePanel(panelGameRoom);
                this.HidePanel(panelHeadless);
                this.settings.Location = Settings.SourceLocation.NabuNetwork;
                if (this.comboNabuNetwork.SelectedItem != null)
                {
                    this.settings.Path = ((Target)this.comboNabuNetwork.SelectedItem).Url;
                }
            }

            Server.cache.Clear();
        }

        /// <summary>
        /// Event Handler for Headless
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void radioButtonHeadless_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHeadless.Checked)
            {
                this.HidePanel(panelNabuNetwork);
                this.HidePanel(panelHomeBrew);
                this.HidePanel(panelLocalDirectory);
                this.HidePanel(panelGameRoom);
                this.ShowPanel(panelHeadless);
                this.settings.Location = Settings.SourceLocation.Headless;

                this.settings.Path = Settings.HeadlessBootLoader;
            }

            Server.cache.Clear();
        }

        /// <summary>
        /// show about menu
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        /// <summary>
        /// Event handler for serial radio button
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void radioSerial_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSerial.Checked)
            {
                this.settings.OperatingMode = Settings.Mode.Serial;
                toolStripMode.Text = "Mode: Serial RS422";
                comboSerialPorts.Enabled = true;
                comboSerialPorts.Show();
                comboSerialPorts.SelectedValue = this.settings.SerialPort;
                txtTcpipPort.Enabled = false;
                txtTcpipPort.Hide();
            }
        }

        /// <summary>
        /// Event handler for TCPIP radio button
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void radioTcpip_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTcpip.Checked)
            {
                this.settings.OperatingMode = Settings.Mode.TCPIP;
                toolStripMode.Text = "Mode: TCP/IP";
                comboSerialPorts.Enabled = false;
                comboSerialPorts.Hide();
                txtTcpipPort.Enabled = true;
                txtTcpipPort.Show();
            }
        }

        /// <summary>
        /// Event handler for changing the option in the nabunetwork.com combo box
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void comboCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.Path = ((Target)comboNabuNetwork.SelectedItem).Url;
            this.settings.SelectedCycleName = ((Target)comboNabuNetwork.SelectedItem).ToString();
            Server.cache.Clear();
        }

        /// <summary>
        /// Event handler for changing the option in the GameRoom combo box
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void comboGameRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.Path = ((Target)comboGameRoom.SelectedItem).Url;
            this.settings.SelectedGameRoom = ((Target)comboGameRoom.SelectedItem).ToString();
            Server.cache.Clear();
        }

        /// <summary>
        /// Event handler for changing the option in the HomeBrew combo box
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void comboHomeBrew_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.Path = ((Target)comboHomeBrew.SelectedItem).Url;
            this.settings.SelectedHomeBrew = ((Target)comboHomeBrew.SelectedItem).ToString();
            Server.cache.Clear();
        }

        /// <summary>
        /// Event handler for changing the text in the location text box
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void txtFileLocation_TextChanged(object sender, EventArgs e)
        {
            this.settings.LocalPath = ((TextBox)sender).Text;
            this.settings.Path = this.settings.LocalPath;
            Server.cache.Clear();
        }

        /// <summary>
        /// Event handler for closing the form
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.settings.SaveSettingsOnExit)
            {
                this.settings.SaveSettings();
            }
        }

        /// <summary>
        /// Event handler for loading the settings menu
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings settings = new frmSettings();

            if (this.server != null)
            {
                settings.DisableInput();
            }

            settings.BaudRate = this.settings.BaudRate;
            settings.SaveSettingsOnExit = this.settings.SaveSettingsOnExit;
            settings.DisableFlowControl = this.settings.DisableFlowControl;

            if (settings.ShowDialog() == DialogResult.OK)
            {
                this.settings.SaveSettingsOnExit = settings.SaveSettingsOnExit;
                this.settings.DisableFlowControl = settings.DisableFlowControl;
                this.settings.BaudRate = settings.BaudRate;
                this.settings.SaveSettings();
            }
        }

        /// <summary>
        /// Event handler for changing the tcpip port in the tcpip port text box
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void txtTcpipPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }
    }
}
