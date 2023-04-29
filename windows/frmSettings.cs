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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NabuAdaptor
{
    /// <summary>
    /// Form to allow user to modify some settings
    /// </summary>
    public partial class frmSettings : Form
    {
        /// <summary>
        /// Get the default baud rate from settings
        /// </summary>
        string baudRate = Settings.defaultBaudRate;

        /// <summary>
        ///  Initializes a new instance of the <see cref="frmSettings"/> class. 
        /// </summary>
        public frmSettings()
        {
            InitializeComponent();
            this.Text = $"NabuNetwork.com Internet Adapter (v{Settings.majorVersion}.{Settings.minorVersion})";
        }

        /// <summary>
        /// Disallow people from being able to modify the baudrate
        /// </summary>
        public void DisableInput()
        {
            txtBaudRate.Enabled = false;
        }

        /// <summary>
        /// Gets or sets the baud rate
        /// </summary>
        public string BaudRate
        {
            get
            {
                return txtBaudRate.Text;
            }
            set
            {
                this.baudRate = value;
                txtBaudRate.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the save settings on exit checkbox
        /// </summary>
        public bool SaveSettingsOnExit
        {
            get
            {
                return this.chkSaveSettings.Checked;
            }
            set
            {
                this.chkSaveSettings.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets the disable flow control checkbox
        /// </summary>
        public bool DisableFlowControl
        {
            get
            {
                return this.chkFlowControl.Checked;
            }
            set
            {
                this.chkFlowControl.Checked = value;
            }
        }

        /// <summary>
        /// Event handler for the OK button
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            int baud;

            if (!int.TryParse(txtBaudRate.Text, out baud))
            {
                this.txtBaudRate.Text = Settings.defaultBaudRate;
            }
            else
            {
                if (baud < 1 || baud > 115200)
                {
                    this.txtBaudRate.Text = Settings.defaultBaudRate;
                }
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Event handler for Key Press on the txtBaudRate object
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        private void txtBaudRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }
    }
}
