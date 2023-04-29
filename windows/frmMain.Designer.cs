namespace NabuAdaptor
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripSegmentName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStart = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.radioTcpip = new System.Windows.Forms.RadioButton();
            this.radioSerial = new System.Windows.Forms.RadioButton();
            this.comboSerialPorts = new System.Windows.Forms.ComboBox();
            this.chkChannel = new System.Windows.Forms.CheckBox();
            this.txtTcpipPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panelHeadless = new System.Windows.Forms.Panel();
            this.radioButtonHeadless = new System.Windows.Forms.RadioButton();
            this.panelGameRoom = new System.Windows.Forms.Panel();
            this.comboGameRoom = new System.Windows.Forms.ComboBox();
            this.radioButtonGameRoom = new System.Windows.Forms.RadioButton();
            this.panelNabuNetwork = new System.Windows.Forms.Panel();
            this.radioButtonNabuNetwork = new System.Windows.Forms.RadioButton();
            this.comboNabuNetwork = new System.Windows.Forms.ComboBox();
            this.panelHomeBrew = new System.Windows.Forms.Panel();
            this.radioButtonHomeBrew = new System.Windows.Forms.RadioButton();
            this.comboHomeBrew = new System.Windows.Forms.ComboBox();
            this.panelLocalDirectory = new System.Windows.Forms.Panel();
            this.radioButtonLocalDirectory = new System.Windows.Forms.RadioButton();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panelLog = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBoxMode.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panelHeadless.SuspendLayout();
            this.panelGameRoom.SuspendLayout();
            this.panelNabuNetwork.SuspendLayout();
            this.panelHomeBrew.SuspendLayout();
            this.panelLocalDirectory.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSegmentName,
            this.toolStripProgressBar,
            this.toolStripMode});
            this.statusStrip1.Location = new System.Drawing.Point(0, 699);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(766, 25);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripSegmentName
            // 
            this.toolStripSegmentName.Name = "toolStripSegmentName";
            this.toolStripSegmentName.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 19);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripMode
            // 
            this.toolStripMode.Name = "toolStripMode";
            this.toolStripMode.Size = new System.Drawing.Size(0, 20);
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnStart.Location = new System.Drawing.Point(0, 644);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(766, 55);
            this.btnStart.TabIndex = 17;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(766, 28);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(108, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.Controls.Add(this.radioTcpip);
            this.groupBoxMode.Controls.Add(this.radioSerial);
            this.groupBoxMode.Controls.Add(this.comboSerialPorts);
            this.groupBoxMode.Controls.Add(this.chkChannel);
            this.groupBoxMode.Controls.Add(this.txtTcpipPort);
            this.groupBoxMode.Controls.Add(this.label1);
            this.groupBoxMode.Location = new System.Drawing.Point(6, 259);
            this.groupBoxMode.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.Size = new System.Drawing.Size(753, 100);
            this.groupBoxMode.TabIndex = 10;
            this.groupBoxMode.TabStop = false;
            this.groupBoxMode.Text = "Mode";
            // 
            // radioTcpip
            // 
            this.radioTcpip.AutoSize = true;
            this.radioTcpip.Location = new System.Drawing.Point(37, 68);
            this.radioTcpip.Name = "radioTcpip";
            this.radioTcpip.Size = new System.Drawing.Size(72, 21);
            this.radioTcpip.TabIndex = 13;
            this.radioTcpip.TabStop = true;
            this.radioTcpip.Text = "TCP/IP";
            this.radioTcpip.UseVisualStyleBackColor = true;
            this.radioTcpip.CheckedChanged += new System.EventHandler(this.radioTcpip_CheckedChanged);
            // 
            // radioSerial
            // 
            this.radioSerial.AutoSize = true;
            this.radioSerial.Location = new System.Drawing.Point(37, 29);
            this.radioSerial.Name = "radioSerial";
            this.radioSerial.Size = new System.Drawing.Size(112, 21);
            this.radioSerial.TabIndex = 10;
            this.radioSerial.TabStop = true;
            this.radioSerial.Text = "Serial RS422";
            this.radioSerial.UseVisualStyleBackColor = true;
            this.radioSerial.CheckedChanged += new System.EventHandler(this.radioSerial_CheckedChanged);
            // 
            // comboSerialPorts
            // 
            this.comboSerialPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSerialPorts.FormattingEnabled = true;
            this.comboSerialPorts.ItemHeight = 16;
            this.comboSerialPorts.Location = new System.Drawing.Point(460, 28);
            this.comboSerialPorts.Name = "comboSerialPorts";
            this.comboSerialPorts.Size = new System.Drawing.Size(121, 24);
            this.comboSerialPorts.TabIndex = 16;
            this.comboSerialPorts.SelectedIndexChanged += new System.EventHandler(this.comboSerialPorts_SelectedIndexChanged);
            // 
            // chkChannel
            // 
            this.chkChannel.AutoSize = true;
            this.chkChannel.Location = new System.Drawing.Point(460, 69);
            this.chkChannel.Name = "chkChannel";
            this.chkChannel.Size = new System.Drawing.Size(134, 21);
            this.chkChannel.TabIndex = 14;
            this.chkChannel.Text = "Ask For Channel";
            this.chkChannel.UseVisualStyleBackColor = true;
            this.chkChannel.CheckedChanged += new System.EventHandler(this.chkChannel_CheckedChanged);
            // 
            // txtTcpipPort
            // 
            this.txtTcpipPort.Location = new System.Drawing.Point(460, 30);
            this.txtTcpipPort.Name = "txtTcpipPort";
            this.txtTcpipPort.Size = new System.Drawing.Size(121, 22);
            this.txtTcpipPort.TabIndex = 13;
            this.txtTcpipPort.Text = "5816";
            this.txtTcpipPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTcpipPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTcpipPort_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(364, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Port:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.panelHeadless);
            this.groupBox5.Controls.Add(this.panelGameRoom);
            this.groupBox5.Controls.Add(this.panelNabuNetwork);
            this.groupBox5.Controls.Add(this.panelHomeBrew);
            this.groupBox5.Controls.Add(this.panelLocalDirectory);
            this.groupBox5.Location = new System.Drawing.Point(6, 29);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(10);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(753, 225);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Source:";
            // 
            // panelHeadless
            // 
            this.panelHeadless.Controls.Add(this.radioButtonHeadless);
            this.panelHeadless.Location = new System.Drawing.Point(12, 183);
            this.panelHeadless.Name = "panelHeadless";
            this.panelHeadless.Size = new System.Drawing.Size(719, 34);
            this.panelHeadless.TabIndex = 19;
            // 
            // radioButtonHeadless
            // 
            this.radioButtonHeadless.AutoSize = true;
            this.radioButtonHeadless.Location = new System.Drawing.Point(13, 7);
            this.radioButtonHeadless.Name = "radioButtonHeadless";
            this.radioButtonHeadless.Size = new System.Drawing.Size(221, 21);
            this.radioButtonHeadless.TabIndex = 16;
            this.radioButtonHeadless.TabStop = true;
            this.radioButtonHeadless.Text = "Control from NABU (Headless)";
            this.radioButtonHeadless.UseVisualStyleBackColor = true;
            this.radioButtonHeadless.CheckedChanged += new System.EventHandler(this.radioButtonHeadless_CheckedChanged);
            // 
            // panelGameRoom
            // 
            this.panelGameRoom.Controls.Add(this.comboGameRoom);
            this.panelGameRoom.Controls.Add(this.radioButtonGameRoom);
            this.panelGameRoom.Location = new System.Drawing.Point(12, 103);
            this.panelGameRoom.Name = "panelGameRoom";
            this.panelGameRoom.Size = new System.Drawing.Size(719, 34);
            this.panelGameRoom.TabIndex = 5;
            // 
            // comboGameRoom
            // 
            this.comboGameRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboGameRoom.FormattingEnabled = true;
            this.comboGameRoom.Location = new System.Drawing.Point(355, 5);
            this.comboGameRoom.Name = "comboGameRoom";
            this.comboGameRoom.Size = new System.Drawing.Size(353, 24);
            this.comboGameRoom.TabIndex = 14;
            this.comboGameRoom.SelectedIndexChanged += new System.EventHandler(this.comboGameRoom_SelectedIndexChanged);
            // 
            // radioButtonGameRoom
            // 
            this.radioButtonGameRoom.AutoSize = true;
            this.radioButtonGameRoom.Location = new System.Drawing.Point(13, 5);
            this.radioButtonGameRoom.Name = "radioButtonGameRoom";
            this.radioButtonGameRoom.Size = new System.Drawing.Size(108, 21);
            this.radioButtonGameRoom.TabIndex = 0;
            this.radioButtonGameRoom.TabStop = true;
            this.radioButtonGameRoom.Text = "Game Room";
            this.radioButtonGameRoom.UseVisualStyleBackColor = true;
            this.radioButtonGameRoom.CheckedChanged += new System.EventHandler(this.radioButtonGameRoom_CheckedChanged);
            // 
            // panelNabuNetwork
            // 
            this.panelNabuNetwork.Controls.Add(this.radioButtonNabuNetwork);
            this.panelNabuNetwork.Controls.Add(this.comboNabuNetwork);
            this.panelNabuNetwork.Location = new System.Drawing.Point(12, 24);
            this.panelNabuNetwork.Name = "panelNabuNetwork";
            this.panelNabuNetwork.Size = new System.Drawing.Size(719, 33);
            this.panelNabuNetwork.TabIndex = 1;
            // 
            // radioButtonNabuNetwork
            // 
            this.radioButtonNabuNetwork.AutoSize = true;
            this.radioButtonNabuNetwork.Location = new System.Drawing.Point(13, 6);
            this.radioButtonNabuNetwork.Name = "radioButtonNabuNetwork";
            this.radioButtonNabuNetwork.Size = new System.Drawing.Size(144, 21);
            this.radioButtonNabuNetwork.TabIndex = 10;
            this.radioButtonNabuNetwork.TabStop = true;
            this.radioButtonNabuNetwork.Text = "NabuNetwork.com";
            this.radioButtonNabuNetwork.UseVisualStyleBackColor = true;
            this.radioButtonNabuNetwork.CheckedChanged += new System.EventHandler(this.radioButtonNabuNetwork_CheckedChanged);
            // 
            // comboNabuNetwork
            // 
            this.comboNabuNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboNabuNetwork.FormattingEnabled = true;
            this.comboNabuNetwork.Location = new System.Drawing.Point(355, 5);
            this.comboNabuNetwork.Name = "comboNabuNetwork";
            this.comboNabuNetwork.Size = new System.Drawing.Size(353, 24);
            this.comboNabuNetwork.TabIndex = 11;
            this.comboNabuNetwork.SelectedIndexChanged += new System.EventHandler(this.comboCycle_SelectedIndexChanged);
            // 
            // panelHomeBrew
            // 
            this.panelHomeBrew.Controls.Add(this.radioButtonHomeBrew);
            this.panelHomeBrew.Controls.Add(this.comboHomeBrew);
            this.panelHomeBrew.Location = new System.Drawing.Point(12, 63);
            this.panelHomeBrew.Name = "panelHomeBrew";
            this.panelHomeBrew.Size = new System.Drawing.Size(719, 34);
            this.panelHomeBrew.TabIndex = 2;
            // 
            // radioButtonHomeBrew
            // 
            this.radioButtonHomeBrew.AutoSize = true;
            this.radioButtonHomeBrew.Location = new System.Drawing.Point(13, 7);
            this.radioButtonHomeBrew.Name = "radioButtonHomeBrew";
            this.radioButtonHomeBrew.Size = new System.Drawing.Size(155, 21);
            this.radioButtonHomeBrew.TabIndex = 12;
            this.radioButtonHomeBrew.TabStop = true;
            this.radioButtonHomeBrew.Text = "Homebrew Software";
            this.radioButtonHomeBrew.UseVisualStyleBackColor = true;
            this.radioButtonHomeBrew.CheckedChanged += new System.EventHandler(this.radioButtonHomeBrew_CheckedChanged);
            // 
            // comboHomeBrew
            // 
            this.comboHomeBrew.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHomeBrew.FormattingEnabled = true;
            this.comboHomeBrew.Location = new System.Drawing.Point(355, 6);
            this.comboHomeBrew.Name = "comboHomeBrew";
            this.comboHomeBrew.Size = new System.Drawing.Size(353, 24);
            this.comboHomeBrew.TabIndex = 13;
            this.comboHomeBrew.SelectedIndexChanged += new System.EventHandler(this.comboHomeBrew_SelectedIndexChanged);
            // 
            // panelLocalDirectory
            // 
            this.panelLocalDirectory.Controls.Add(this.radioButtonLocalDirectory);
            this.panelLocalDirectory.Controls.Add(this.btnChooseFolder);
            this.panelLocalDirectory.Controls.Add(this.txtLocalPath);
            this.panelLocalDirectory.Location = new System.Drawing.Point(12, 143);
            this.panelLocalDirectory.Name = "panelLocalDirectory";
            this.panelLocalDirectory.Size = new System.Drawing.Size(719, 34);
            this.panelLocalDirectory.TabIndex = 4;
            // 
            // radioButtonLocalDirectory
            // 
            this.radioButtonLocalDirectory.AutoSize = true;
            this.radioButtonLocalDirectory.Location = new System.Drawing.Point(13, 7);
            this.radioButtonLocalDirectory.Name = "radioButtonLocalDirectory";
            this.radioButtonLocalDirectory.Size = new System.Drawing.Size(96, 21);
            this.radioButtonLocalDirectory.TabIndex = 16;
            this.radioButtonLocalDirectory.TabStop = true;
            this.radioButtonLocalDirectory.Text = "Local Path";
            this.radioButtonLocalDirectory.UseVisualStyleBackColor = true;
            this.radioButtonLocalDirectory.CheckedChanged += new System.EventHandler(this.radioButtonLocalDirectory_CheckedChanged);
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Location = new System.Drawing.Point(162, 2);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(146, 30);
            this.btnChooseFolder.TabIndex = 17;
            this.btnChooseFolder.Text = "Select Target";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Location = new System.Drawing.Point(355, 6);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.Size = new System.Drawing.Size(353, 22);
            this.txtLocalPath.TabIndex = 18;
            this.txtLocalPath.TextChanged += new System.EventHandler(this.txtFileLocation_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.AddExtension = false;
            this.openFileDialog1.CheckFileExists = false;
            this.openFileDialog1.FileName = "Folder";
            this.openFileDialog1.Filter = "NABU files (*.nabu)|*.nabu";
            this.openFileDialog1.ValidateNames = false;
            // 
            // panelLog
            // 
            this.panelLog.Controls.Add(this.txtLog);
            this.panelLog.Location = new System.Drawing.Point(0, 367);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(766, 272);
            this.panelLog.TabIndex = 21;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.ForeColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(6, 5);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(753, 268);
            this.txtLog.TabIndex = 16;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 724);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBoxMode);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "NabuNetwork.com Adapter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.panelHeadless.ResumeLayout(false);
            this.panelHeadless.PerformLayout();
            this.panelGameRoom.ResumeLayout(false);
            this.panelGameRoom.PerformLayout();
            this.panelNabuNetwork.ResumeLayout(false);
            this.panelNabuNetwork.PerformLayout();
            this.panelHomeBrew.ResumeLayout(false);
            this.panelHomeBrew.PerformLayout();
            this.panelLocalDirectory.ResumeLayout(false);
            this.panelLocalDirectory.PerformLayout();
            this.panelLog.ResumeLayout(false);
            this.panelLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripMode;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSegmentName;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxMode;
        private System.Windows.Forms.TextBox txtTcpipPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboSerialPorts;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButtonNabuNetwork;
        private System.Windows.Forms.TextBox txtLocalPath;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.RadioButton radioButtonLocalDirectory;
        private System.Windows.Forms.RadioButton radioTcpip;
        private System.Windows.Forms.RadioButton radioSerial;
        private System.Windows.Forms.ComboBox comboNabuNetwork;
        private System.Windows.Forms.ComboBox comboHomeBrew;
        private System.Windows.Forms.RadioButton radioButtonHomeBrew;
        private System.Windows.Forms.CheckBox chkChannel;
        private System.Windows.Forms.Panel panelLocalDirectory;
        private System.Windows.Forms.Panel panelNabuNetwork;
        private System.Windows.Forms.Panel panelHomeBrew;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panelGameRoom;
        private System.Windows.Forms.ComboBox comboGameRoom;
        private System.Windows.Forms.RadioButton radioButtonGameRoom;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panelHeadless;
        private System.Windows.Forms.RadioButton radioButtonHeadless;
    }
}

