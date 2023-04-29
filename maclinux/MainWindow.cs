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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Gtk;
using NabuAdaptor;

/// <summary>
/// Main UI for the adaptor
/// </summary>
public partial class MainWindow : Gtk.Window
{
    /// <summary>
    /// The settings.
    /// </summary>
    private global::NabuAdaptor.Settings settings = null;

    /// <summary>
    /// The server.
    /// </summary>
    private Server server = null;

    /// <summary>
    /// The task.
    /// </summary>
    private Task task = null;

    /// <summary>
    /// The source.
    /// </summary>
    CancellationTokenSource source;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MainWindow"/> class.
    /// </summary>
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        this.settings = new NabuAdaptor.Settings();

        Build();

        if (this.settings.IgnoreCertificateErrors)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        this.Title = $"NabuNetwork.com Internet Adapter (v{NabuAdaptor.Settings.majorVersion}.{NabuAdaptor.Settings.minorVersion})";

        string xml = string.Empty;

        try
        {
            // Get all the Cycles from the list (sort)
            List<NabuAdaptor.Target> nabunetwork = (from item in this.settings.Cycles where item.TargetType == NabuAdaptor.Target.TargetEnum.NabuNetwork select item).ToList();
            List<NabuAdaptor.Target> homebrew = (from item in this.settings.Cycles where item.TargetType == NabuAdaptor.Target.TargetEnum.Homebrew select item).OrderBy(x => x.Name).ToList();
            List<NabuAdaptor.Target> gameroom = (from item in this.settings.Cycles where item.TargetType == NabuAdaptor.Target.TargetEnum.Gameroom select item).OrderBy(x => x.Name).ToList();

            // Populate Cycles
            for (int i = 0; i < nabunetwork.Count; i++)
            {
                comboNabuNetwork.AppendText(nabunetwork[i].Name);
                if (nabunetwork[i].Name.Equals(this.settings.SelectedCycleName))
                {
                    comboNabuNetwork.Active = i;
                }
            }

            if (string.IsNullOrWhiteSpace(comboNabuNetwork.ActiveText))
            {
                comboNabuNetwork.Active = 0;
            }

            for (int i = 0; i < homebrew.Count; i++)
            {
                comboHomeBrew.AppendText(homebrew[i].Name);
                if (homebrew[i].Name.Equals(this.settings.SelectedHomeBrew))
                {
                    comboHomeBrew.Active = i;
                }
            }

            if (string.IsNullOrWhiteSpace(comboHomeBrew.ActiveText))
            {
                comboHomeBrew.Active = 0;
            }

            for (int i = 0; i < gameroom.Count; i++)
            {
                comboGameRoom.AppendText(gameroom[i].Name);
                if (gameroom[i].Name.Equals(this.settings.SelectedGameRoom))
                {
                    comboGameRoom.Active = i;
                }
            }

            if (string.IsNullOrWhiteSpace(comboGameRoom.ActiveText))
            {
                comboGameRoom.Active = 0;
            }

            txtLocalPath.Text = this.settings.LocalPath;

            this.PopulatePorts(this.settings.SerialPort);
            chkChannel.Active = this.settings.AskForChannel;

            switch (this.settings.Location)
            {
                case NabuAdaptor.Settings.SourceLocation.NabuNetwork:
                    this.radioButtonNabuNetwork.Active = true;
                    this.comboNabuNetwork.Show();
                    this.comboHomeBrew.Hide();
                    this.comboGameRoom.Hide();
                    this.txtLocalPath.Hide();
                    this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboNabuNetwork.ActiveText)).First()).Url;
                    break;
                case NabuAdaptor.Settings.SourceLocation.HomeBrew:
                    this.radiobuttonHomebrew.Active = true;
                    this.comboNabuNetwork.Hide();
                    this.comboHomeBrew.Show();
                    this.comboGameRoom.Hide();
                    this.txtLocalPath.Hide();
                    this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboHomeBrew.ActiveText)).First()).Url;
                    break;
                case NabuAdaptor.Settings.SourceLocation.GameRoom:
                    this.radiobuttonGameRoom.Active = true;
                    this.comboNabuNetwork.Hide();
                    this.comboHomeBrew.Hide();
                    this.comboGameRoom.Show();
                    this.txtLocalPath.Hide();
                    this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboGameRoom.ActiveText)).First()).Url;
                    break;
                case NabuAdaptor.Settings.SourceLocation.LocalDirectory:
                    this.radioButtonLocalDirectory.Active = true;
                    this.comboNabuNetwork.Hide();
                    this.comboHomeBrew.Hide();
                    this.comboGameRoom.Hide();
                    this.txtLocalPath.Show();
                    this.settings.Path = txtLocalPath.Text;
                    break;
                case NabuAdaptor.Settings.SourceLocation.Headless:
                    this.radioButtonHeadless.Active = true;
                    this.comboNabuNetwork.Hide();
                    this.comboHomeBrew.Hide();
                    this.comboGameRoom.Hide();
                    this.txtLocalPath.Hide();
                    this.settings.Path = NabuAdaptor.Settings.HeadlessBootLoader;
                    break;
            }

            switch (this.settings.OperatingMode)
            {
                case NabuAdaptor.Settings.Mode.Serial:
                    this.radioSerial.Active = true;
                    this.comboSerialPorts.Show();
                    txtTcpipPort.Hide();
                    break;
                case NabuAdaptor.Settings.Mode.TCPIP:
                    this.radioTcpip.Active = true;
                    this.comboSerialPorts.Hide();
                    txtTcpipPort.Show();
                    break;
            }
            txtTcpipPort.Text = this.settings.TcpipPort;

            txtLog.ModifyBase(StateType.Normal, new Gdk.Color(0x00, 0x00, 0x00));
            txtLog.ModifyText(StateType.Normal, new Gdk.Color(0xFF, 0xFF, 0xFF));
        }
        catch (Exception)
        { }
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        if (this.settings.SaveSettingsOnExit)
        {
            this.settings.SaveSettings();
        }

        Application.Quit();
        a.RetVal = true;
    }

    /// <summary>
    /// Ons the about action activated.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnAboutActionActivated(object sender, EventArgs e)
    {
        NabuAdaptorLinux.About about = new NabuAdaptorLinux.About();
        about.Run();
        about.Destroy();
    }

    /// <summary>
    /// Populates the ports.
    /// </summary>
    /// <param name="savedPort">Saved port.</param>
    private void PopulatePorts(string savedPort)
    {
        string[] ports = SerialPort.GetPortNames();
        //comboSerialPorts.Clear();

        foreach (string port in ports)
        {
            comboSerialPorts.AppendText(port);
            if (port.Equals(savedPort))
            {
                //comboSerialPorts.SelectedItem = port;
            }
        }

        if (string.IsNullOrWhiteSpace(comboSerialPorts.ActiveText))
        {
            comboSerialPorts.Active = 0;
        }
    }

    /// <summary>
    /// Ons the radiobutton game room clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRadiobuttonGameRoomClicked(object sender, EventArgs e)
    {
        this.comboNabuNetwork.Hide();
        this.comboHomeBrew.Hide();
        this.comboGameRoom.Show();
        this.txtLocalPath.Hide();
        this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboGameRoom.ActiveText)).First()).Url;
        this.settings.Location = NabuAdaptor.Settings.SourceLocation.GameRoom;

        if (!string.IsNullOrWhiteSpace(this.comboGameRoom.ActiveText))
        {
            // Get the URL for the active text.
            NabuAdaptor.Target cycle = this.settings.Cycles.Where(x => x.Name.Equals(this.comboGameRoom.ActiveText)).First();
            this.settings.Path = cycle.Url;
        }

        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the radiobutton nabu network clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRadiobuttonNabuNetworkClicked(object sender, EventArgs e)
    {
        this.comboNabuNetwork.Show();
        this.comboHomeBrew.Hide();
        this.comboGameRoom.Hide();
        this.txtLocalPath.Hide();
        this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboNabuNetwork.ActiveText)).First()).Url;
        this.settings.Location = NabuAdaptor.Settings.SourceLocation.NabuNetwork;

        if (!string.IsNullOrWhiteSpace(this.comboGameRoom.ActiveText))
        {
            // Get the URL for the active text.
            NabuAdaptor.Target cycle = this.settings.Cycles.Where(x => x.Name.Equals(this.comboNabuNetwork.ActiveText)).First();
            this.settings.Path = cycle.Url;
        }

        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the radiobutton home brew clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRadiobuttonHomeBrewClicked(object sender, EventArgs e)
    {
        this.comboNabuNetwork.Hide();
        this.comboHomeBrew.Show();
        this.comboGameRoom.Hide();
        this.txtLocalPath.Hide();
        this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboHomeBrew.ActiveText)).First()).Url;
        this.settings.Location = NabuAdaptor.Settings.SourceLocation.HomeBrew;

        if (!string.IsNullOrWhiteSpace(this.comboGameRoom.ActiveText))
        {
            // Get the URL for the active text.
            NabuAdaptor.Target cycle = this.settings.Cycles.Where(x => x.Name.Equals(this.comboHomeBrew.ActiveText)).First();
            this.settings.Path = cycle.Url;
        }

        Server.cache.Clear();
   }

    /// <summary>
    /// Ons the radiobutton headless clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRadiobuttonHeadlessClicked(object sender, EventArgs e)
    {
        this.comboNabuNetwork.Hide();
        this.comboHomeBrew.Hide();
        this.comboGameRoom.Hide();
        this.txtLocalPath.Hide();
        this.settings.Path = NabuAdaptor.Settings.HeadlessBootLoader;
        this.settings.Location = NabuAdaptor.Settings.SourceLocation.Headless;

        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the radiobutton local directory clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRadiobuttonLocalDirectoryClicked(object sender, EventArgs e)
    {
        this.comboNabuNetwork.Hide();
        this.comboHomeBrew.Hide();
        this.comboGameRoom.Hide();
        this.txtLocalPath.Show();

        this.settings.Location = NabuAdaptor.Settings.SourceLocation.LocalDirectory;
        this.settings.Path = txtLocalPath.Text;

        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the radio tcpip clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRadioTcpipClicked(object sender, EventArgs e)
    {
        this.settings.OperatingMode = NabuAdaptor.Settings.Mode.TCPIP;
        comboSerialPorts.Hide();
        txtTcpipPort.Show();
    }

    /// <summary>
    /// Ons the radio serial clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnRadioSerialClicked(object sender, EventArgs e)
    {
        this.settings.OperatingMode = NabuAdaptor.Settings.Mode.Serial;
        comboSerialPorts.Show();
        txtTcpipPort.Hide();
    }

    /// <summary>
    /// Ons the button start clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnBtnStartClicked(object sender, EventArgs e)
    {    
        if (this.settings.Location == NabuAdaptor.Settings.SourceLocation.NabuNetwork && string.IsNullOrWhiteSpace(comboNabuNetwork.ActiveText))
        {
            this.Log("Must specify Cycle for NabuNetwork");
            return;
        }

        if (this.settings.Location == NabuAdaptor.Settings.SourceLocation.HomeBrew && string.IsNullOrWhiteSpace(comboHomeBrew.ActiveText))
        {
            this.Log("Must specify a valid option for the HomeBrew source");
            return;
        }

        if (this.settings.Location == NabuAdaptor.Settings.SourceLocation.GameRoom && string.IsNullOrWhiteSpace(comboGameRoom.ActiveText))
        {
            this.Log("Must specify a valid option for the GameRoom source");
            return;
        }

        if (this.settings.Location == NabuAdaptor.Settings.SourceLocation.LocalDirectory && !string.IsNullOrWhiteSpace(this.settings.LocalPath) && (!(System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(this.settings.LocalPath)) || System.IO.File.Exists(this.settings.LocalPath))))
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
            radioTcpip.Sensitive = true;
            radioSerial.Sensitive = true;
            chkChannel.Sensitive = true;
            comboSerialPorts.Sensitive = true;
            txtTcpipPort.Sensitive = true;
            StopServer();
            btnStart.Label = "Start Server";
        }
        else
        {
            radioTcpip.Sensitive = false;
            radioSerial.Sensitive = false;
            chkChannel.Sensitive = false;
            comboSerialPorts.Sensitive = false;
            txtTcpipPort.Sensitive = false;
            Server.cache.Clear();
            StartServer();
            txtLog.Buffer.Text = "";
            btnStart.Label = "Stop Server";
        }
    }

    /// <summary>
    /// Starts the server.
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
    /// Stops the server.
    /// </summary>
    private void StopServer()
    {
        this.server.StopServer();
        this.source.Cancel();
        this.task.Wait(1000);
        this.server = null;
    }

    /// <summary>
    /// Logs the event handler.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="message">Message.</param>
    private void LogEventHandler(object obj, string message)
    {
        Gtk.Application.Invoke(delegate
        { Log(message); }
        );
    }

    /// <summary>
    /// Progresses the event handler.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="args">Arguments.</param>
    private void ProgressEventHandler(object obj, ProgressEventArgs args)
    {
        Gtk.Application.Invoke(delegate
        { SetProgress(args); }
        );
    }

    /// <summary>
    /// Sets the progress.
    /// </summary>
    /// <param name="args">Arguments.</param>
    private void SetProgress(ProgressEventArgs args)
    {
        try
        {
            progressbar1.Fraction = (double)args.Curr / (double)args.Max;
            progressbar1.Text = $"Transfering Segment: {args.Segment.ToString("X6")}";
            Main.IterationDo(false);
        }
        catch (ObjectDisposedException)
        {
        }
    }

    /// <summary>
    /// Log the specified message.
    /// </summary>
    /// <param name="message">Message.</param>
    private void Log(string message)
    {
        try
        {
            TextIter textIter = txtLog.Buffer.EndIter;
            txtLog.Buffer.Insert(ref textIter, message + Environment.NewLine);

            TextMark endMark = txtLog.Buffer.CreateMark("end-mark", txtLog.Buffer.EndIter, false);
            txtLog.ScrollToMark(endMark, 0, false, 0, 0);
        }
        catch (ObjectDisposedException)
        {
        }
    }

    /// <summary>
    /// Ons the combo serial ports changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnComboSerialPortsChanged(object sender, EventArgs e)
    {
        this.settings.SerialPort = comboSerialPorts.ActiveText;
    }

    /// <summary>
    /// Ons the combo game room changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnComboGameRoomChanged(object sender, EventArgs e)
    {
        this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboGameRoom.ActiveText)).First()).Url;
        this.settings.SelectedGameRoom = this.comboGameRoom.ActiveText;
        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the combo home brew changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnComboHomeBrewChanged(object sender, EventArgs e)
    {
        this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboHomeBrew.ActiveText)).First()).Url;
        this.settings.SelectedHomeBrew = this.comboHomeBrew.ActiveText;
        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the combo nabu network changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnComboNabuNetworkChanged(object sender, EventArgs e)
    {
        this.settings.Path = ((NabuAdaptor.Target)this.settings.Cycles.Where(x => x.Name.Equals(this.comboNabuNetwork.ActiveText)).First()).Url;
        this.settings.SelectedCycleName = this.comboNabuNetwork.ActiveText;
        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the text local path changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnTxtLocalPathChanged(object sender, EventArgs e)
    {
        this.settings.Path = ((Gtk.Entry)sender).Text;
        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the text local directory changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnTxtLocalDirectoryChanged(object sender, EventArgs e)
    {
        this.settings.LocalPath = ((Gtk.Entry)sender).Text;
        this.settings.Path = this.settings.LocalPath;
        Server.cache.Clear();
    }

    /// <summary>
    /// Ons the chk channel clicked.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnChkChannelClicked(object sender, EventArgs e)
    {
        this.settings.AskForChannel = chkChannel.Active;
    }

    /// <summary>
    /// Ons the text tcpip port changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnTxtTcpipPortChanged(object sender, EventArgs e)
    {
        int port;

        if (int.TryParse(this.txtTcpipPort.Text, out port))
        {
            this.settings.TcpipPort = this.txtTcpipPort.Text;
        }
    }

    /// <summary>
    /// Ons the exit action activated.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnExitActionActivated(object sender, EventArgs e)
    {
        if (this.settings.SaveSettingsOnExit)
        {
            this.settings.SaveSettings();
        }

        if (server != null)
        {
            StopServer();
        }

        Application.Quit();
    }

    /// <summary>
    /// Ons the text tcpip port key press event.
    /// </summary>
    /// <param name="o">O.</param>
    /// <param name="args">Arguments.</param>
    protected void OnTxtTcpipPortKeyPressEvent(object o, KeyPressEventArgs args)
    {
        args.RetVal = (!char.IsDigit((char)args.Event.Key) && !char.IsControl((char)args.Event.Key));
    }

    /// <summary>
    /// Ons the settings action2 activated.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnSettingsAction2Activated(object sender, EventArgs e)
    {
        NabuAdaptorLinux.DisplaySettings frmsettings = new NabuAdaptorLinux.DisplaySettings(this.settings);
        frmsettings.Run();
        frmsettings.Destroy();
    }
}
