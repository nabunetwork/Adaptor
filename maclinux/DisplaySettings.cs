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
namespace NabuAdaptorLinux
{
    using System;
    using NabuAdaptor;

    /// <summary>
    /// Form to allow user to modify some settings
    /// </summary>
    public partial class DisplaySettings : Gtk.Dialog
    {
        /// <summary>
        /// Get the default baud rate from settings
        /// </summary>
        string baudRate = NabuAdaptor.Settings.defaultBaudRate;

        /// <summary>
        /// The settings.
        /// </summary>
        NabuAdaptor.Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NabuAdaptorLinux.Settings"/> class.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public DisplaySettings(NabuAdaptor.Settings settings)
        {
            this.settings = settings;
            this.Build();
            this.Title = $"NabuNetwork.com Internet Adapter (v{NabuAdaptor.Settings.majorVersion}.{NabuAdaptor.Settings.minorVersion})";
            this.SaveSettingsOnExit = this.settings.SaveSettingsOnExit;
            this.BaudRate = this.settings.BaudRate;
            this.DisableFlowControl = this.settings.DisableFlowControl;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:NabuAdaptorLinux.Settings"/> disable flow control.
        /// </summary>
        /// <value><c>true</c> if disable flow control; otherwise, <c>false</c>.</value>
        public bool DisableFlowControl
        {
            get
            {
                return this.chkFlowControl.Active;
            }
            set
            {
                this.chkFlowControl.Active = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:NabuAdaptorLinux.Settings"/> save settings on exit.
        /// </summary>
        /// <value><c>true</c> if save settings on exit; otherwise, <c>false</c>.</value>
        public bool SaveSettingsOnExit
        {
            get
            {
                return this.chkSave.Active;
            }
            set
            {
                this.chkSave.Active = value;
            }
        }

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        /// <value>The baud rate.</value>
        public string BaudRate
        {
            get
            {
                return this.txtbaudRate.Text;
            }
            set
            {
                this.baudRate = value;
                this.txtbaudRate.Text = value;
            }
        }

        /// <summary>
        /// Ons the button ok clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            int baud;

            if (!int.TryParse(this.txtbaudRate.Text, out baud))
            {
                this.txtbaudRate.Text = NabuAdaptor.Settings.defaultBaudRate;
            }
            else
            {
                if (baud < 1 || baud > 115200)
                {
                    this.txtbaudRate.Text = NabuAdaptor.Settings.defaultBaudRate;
                }
            }

            this.settings.SaveSettingsOnExit = this.chkSave.Active;
            this.settings.DisableFlowControl = this.chkFlowControl.Active;
            this.settings.BaudRate = this.txtbaudRate.Text;
        }

        /// <summary>
        /// Ons the txtbaud rate key release event.
        /// </summary>
        /// <param name="o">O.</param>
        /// <param name="args">Arguments.</param>
        protected void OnTxtbaudRateKeyReleaseEvent(object o, Gtk.KeyReleaseEventArgs args)
        {
            if (!char.IsDigit((char)args.Event.Key) && !char.IsControl((char)args.Event.Key))
            {
                args.RetVal = true;
            }
            else
            {
                args.RetVal = false;
            }
        }
    }
}
