
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;

	private global::Gtk.Action FileAction;

	private global::Gtk.Action SettingsAction;

	private global::Gtk.Action AboutAction;

	private global::Gtk.Action FileAction1;

	private global::Gtk.Action SettingsAction1;

	private global::Gtk.Action AboutAction1;

	private global::Gtk.Action HelpAction;

	private global::Gtk.Action AboutAction2;

	private global::Gtk.Action ExitAction;

	private global::Gtk.Action SettingsAction2;

	private global::Gtk.VBox vbox1;

	private global::Gtk.Fixed fixed18;

	private global::Gtk.MenuBar menubar1;

	private global::Gtk.Frame frame1;

	private global::Gtk.Alignment GtkAlignment;

	private global::Gtk.VBox vbox2;

	private global::Gtk.HBox hbox9;

	private global::Gtk.Fixed fixed2;

	private global::Gtk.RadioButton radioButtonNabuNetwork;

	private global::Gtk.Fixed fixed3;

	private global::Gtk.ComboBox comboNabuNetwork;

	private global::Gtk.HBox hbox10;

	private global::Gtk.Fixed fixed4;

	private global::Gtk.RadioButton radiobuttonHomebrew;

	private global::Gtk.Fixed fixed5;

	private global::Gtk.ComboBox comboHomeBrew;

	private global::Gtk.HBox hbox11;

	private global::Gtk.Fixed fixed6;

	private global::Gtk.RadioButton radiobuttonGameRoom;

	private global::Gtk.Fixed fixed7;

	private global::Gtk.ComboBox comboGameRoom;

	private global::Gtk.HBox hbox12;

	private global::Gtk.Fixed fixed8;

	private global::Gtk.RadioButton radioButtonLocalDirectory;

	private global::Gtk.Fixed fixed9;

	private global::Gtk.Entry txtLocalPath;

	private global::Gtk.HBox hbox13;

	private global::Gtk.Fixed fixed10;

	private global::Gtk.RadioButton radioButtonHeadless;

	private global::Gtk.Fixed fixed11;

	private global::Gtk.Label GtkLabel10;

	private global::Gtk.VBox vbox4;

	private global::Gtk.Frame frame3;

	private global::Gtk.Alignment GtkAlignment2;

	private global::Gtk.VBox vbox3;

	private global::Gtk.HBox hbox1;

	private global::Gtk.Fixed fixed1;

	private global::Gtk.RadioButton radioSerial;

	private global::Gtk.Fixed fixed12;

	private global::Gtk.Label lblPort;

	private global::Gtk.Fixed fixed13;

	private global::Gtk.ComboBox comboSerialPorts;

	private global::Gtk.Entry txtTcpipPort;

	private global::Gtk.HBox hbox2;

	private global::Gtk.Fixed fixed14;

	private global::Gtk.RadioButton radioTcpip;

	private global::Gtk.Fixed fixed15;

	private global::Gtk.CheckButton chkChannel;

	private global::Gtk.Label frameMode;

	private global::Gtk.ScrolledWindow GtkScrolledWindow;

	private global::Gtk.TextView txtLog;

	private global::Gtk.Button btnStart;

	private global::Gtk.HBox hbox3;

	private global::Gtk.ProgressBar progressbar1;

	private global::Gtk.Fixed fixed16;

	private global::Gtk.Fixed fixed17;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup("Default");
		this.FileAction = new global::Gtk.Action("FileAction", global::Mono.Unix.Catalog.GetString("File"), null, null);
		this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString("File");
		w1.Add(this.FileAction, null);
		this.SettingsAction = new global::Gtk.Action("SettingsAction", global::Mono.Unix.Catalog.GetString("Settings"), null, null);
		this.SettingsAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Settings");
		w1.Add(this.SettingsAction, null);
		this.AboutAction = new global::Gtk.Action("AboutAction", global::Mono.Unix.Catalog.GetString("About"), null, null);
		this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString("About");
		w1.Add(this.AboutAction, null);
		this.FileAction1 = new global::Gtk.Action("FileAction1", global::Mono.Unix.Catalog.GetString("File"), null, null);
		this.FileAction1.ShortLabel = global::Mono.Unix.Catalog.GetString("File");
		w1.Add(this.FileAction1, null);
		this.SettingsAction1 = new global::Gtk.Action("SettingsAction1", global::Mono.Unix.Catalog.GetString("Settings"), null, null);
		this.SettingsAction1.ShortLabel = global::Mono.Unix.Catalog.GetString("Settings");
		w1.Add(this.SettingsAction1, null);
		this.AboutAction1 = new global::Gtk.Action("AboutAction1", global::Mono.Unix.Catalog.GetString("About"), null, null);
		this.AboutAction1.ShortLabel = global::Mono.Unix.Catalog.GetString("About");
		w1.Add(this.AboutAction1, null);
		this.HelpAction = new global::Gtk.Action("HelpAction", global::Mono.Unix.Catalog.GetString("Help"), null, null);
		this.HelpAction.ShortLabel = global::Mono.Unix.Catalog.GetString("About");
		w1.Add(this.HelpAction, null);
		this.AboutAction2 = new global::Gtk.Action("AboutAction2", global::Mono.Unix.Catalog.GetString("About"), null, null);
		this.AboutAction2.ShortLabel = global::Mono.Unix.Catalog.GetString("About");
		w1.Add(this.AboutAction2, null);
		this.ExitAction = new global::Gtk.Action("ExitAction", global::Mono.Unix.Catalog.GetString("Exit"), null, null);
		this.ExitAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Exit");
		w1.Add(this.ExitAction, null);
		this.SettingsAction2 = new global::Gtk.Action("SettingsAction2", global::Mono.Unix.Catalog.GetString("Settings"), null, null);
		this.SettingsAction2.ShortLabel = global::Mono.Unix.Catalog.GetString("Settings");
		w1.Add(this.SettingsAction2, null);
		this.UIManager.InsertActionGroup(w1, 0);
		this.AddAccelGroup(this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("MainWindow");
		this.Icon = global::Gdk.Pixbuf.LoadFromResource("NabuAdaptorLinux.nabu.ico");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.AllowGrow = false;
		this.DefaultWidth = 700;
		this.DefaultHeight = 600;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.fixed18 = new global::Gtk.Fixed();
		this.fixed18.HeightRequest = 30;
		this.fixed18.Name = "fixed18";
		this.fixed18.HasWindow = false;
		// Container child fixed18.Gtk.Fixed+FixedChild
		this.UIManager.AddUiFromString(@"<ui><menubar name='menubar1'><menu name='FileAction1' action='FileAction1'><menuitem name='ExitAction' action='ExitAction'/></menu><menu name='SettingsAction1' action='SettingsAction1'><menuitem name='SettingsAction2' action='SettingsAction2'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='AboutAction2' action='AboutAction2'/></menu></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.fixed18.Add(this.menubar1);
		this.vbox1.Add(this.fixed18);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.fixed18]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.frame1 = new global::Gtk.Frame();
		this.frame1.Name = "frame1";
		this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame1.Gtk.Container+ContainerChild
		this.GtkAlignment = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment.Name = "GtkAlignment";
		this.GtkAlignment.LeftPadding = ((uint)(12));
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		this.vbox2 = new global::Gtk.VBox();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox9 = new global::Gtk.HBox();
		this.hbox9.Name = "hbox9";
		this.hbox9.Spacing = 6;
		// Container child hbox9.Gtk.Box+BoxChild
		this.fixed2 = new global::Gtk.Fixed();
		this.fixed2.WidthRequest = 320;
		this.fixed2.HeightRequest = 35;
		this.fixed2.Name = "fixed2";
		this.fixed2.HasWindow = false;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.radioButtonNabuNetwork = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("NabuNetwork.com"));
		this.radioButtonNabuNetwork.CanFocus = true;
		this.radioButtonNabuNetwork.Name = "radioButtonNabuNetwork";
		this.radioButtonNabuNetwork.DrawIndicator = true;
		this.radioButtonNabuNetwork.UseUnderline = true;
		this.radioButtonNabuNetwork.Group = new global::GLib.SList(global::System.IntPtr.Zero);
		this.fixed2.Add(this.radioButtonNabuNetwork);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.radioButtonNabuNetwork]));
		w4.X = 10;
		w4.Y = 9;
		this.hbox9.Add(this.fixed2);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.fixed2]));
		w5.Position = 0;
		// Container child hbox9.Gtk.Box+BoxChild
		this.fixed3 = new global::Gtk.Fixed();
		this.fixed3.WidthRequest = 430;
		this.fixed3.HeightRequest = 35;
		this.fixed3.Name = "fixed3";
		this.fixed3.HasWindow = false;
		// Container child fixed3.Gtk.Fixed+FixedChild
		this.comboNabuNetwork = global::Gtk.ComboBox.NewText();
		this.comboNabuNetwork.WidthRequest = 430;
		this.comboNabuNetwork.Name = "comboNabuNetwork";
		this.fixed3.Add(this.comboNabuNetwork);
		this.hbox9.Add(this.fixed3);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.fixed3]));
		w7.Position = 1;
		w7.Expand = false;
		w7.Fill = false;
		this.vbox2.Add(this.hbox9);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox9]));
		w8.Position = 0;
		w8.Expand = false;
		w8.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox10 = new global::Gtk.HBox();
		this.hbox10.Name = "hbox10";
		this.hbox10.Spacing = 6;
		// Container child hbox10.Gtk.Box+BoxChild
		this.fixed4 = new global::Gtk.Fixed();
		this.fixed4.WidthRequest = 320;
		this.fixed4.HeightRequest = 35;
		this.fixed4.Name = "fixed4";
		this.fixed4.HasWindow = false;
		// Container child fixed4.Gtk.Fixed+FixedChild
		this.radiobuttonHomebrew = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Homebrew Software"));
		this.radiobuttonHomebrew.CanFocus = true;
		this.radiobuttonHomebrew.Name = "radiobuttonHomebrew";
		this.radiobuttonHomebrew.DrawIndicator = true;
		this.radiobuttonHomebrew.UseUnderline = true;
		this.radiobuttonHomebrew.Group = this.radioButtonNabuNetwork.Group;
		this.fixed4.Add(this.radiobuttonHomebrew);
		global::Gtk.Fixed.FixedChild w9 = ((global::Gtk.Fixed.FixedChild)(this.fixed4[this.radiobuttonHomebrew]));
		w9.X = 10;
		w9.Y = 9;
		this.hbox10.Add(this.fixed4);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.fixed4]));
		w10.Position = 0;
		// Container child hbox10.Gtk.Box+BoxChild
		this.fixed5 = new global::Gtk.Fixed();
		this.fixed5.WidthRequest = 430;
		this.fixed5.HeightRequest = 35;
		this.fixed5.Name = "fixed5";
		this.fixed5.HasWindow = false;
		// Container child fixed5.Gtk.Fixed+FixedChild
		this.comboHomeBrew = global::Gtk.ComboBox.NewText();
		this.comboHomeBrew.WidthRequest = 430;
		this.comboHomeBrew.Name = "comboHomeBrew";
		this.fixed5.Add(this.comboHomeBrew);
		this.hbox10.Add(this.fixed5);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.fixed5]));
		w12.Position = 1;
		w12.Expand = false;
		w12.Fill = false;
		this.vbox2.Add(this.hbox10);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox10]));
		w13.Position = 1;
		w13.Expand = false;
		w13.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox11 = new global::Gtk.HBox();
		this.hbox11.Name = "hbox11";
		this.hbox11.Spacing = 6;
		// Container child hbox11.Gtk.Box+BoxChild
		this.fixed6 = new global::Gtk.Fixed();
		this.fixed6.WidthRequest = 320;
		this.fixed6.HeightRequest = 35;
		this.fixed6.Name = "fixed6";
		this.fixed6.HasWindow = false;
		// Container child fixed6.Gtk.Fixed+FixedChild
		this.radiobuttonGameRoom = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Game Room"));
		this.radiobuttonGameRoom.CanFocus = true;
		this.radiobuttonGameRoom.Name = "radiobuttonGameRoom";
		this.radiobuttonGameRoom.DrawIndicator = true;
		this.radiobuttonGameRoom.UseUnderline = true;
		this.radiobuttonGameRoom.Group = this.radioButtonNabuNetwork.Group;
		this.fixed6.Add(this.radiobuttonGameRoom);
		global::Gtk.Fixed.FixedChild w14 = ((global::Gtk.Fixed.FixedChild)(this.fixed6[this.radiobuttonGameRoom]));
		w14.X = 10;
		w14.Y = 9;
		this.hbox11.Add(this.fixed6);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox11[this.fixed6]));
		w15.Position = 0;
		// Container child hbox11.Gtk.Box+BoxChild
		this.fixed7 = new global::Gtk.Fixed();
		this.fixed7.WidthRequest = 430;
		this.fixed7.HeightRequest = 35;
		this.fixed7.Name = "fixed7";
		this.fixed7.HasWindow = false;
		// Container child fixed7.Gtk.Fixed+FixedChild
		this.comboGameRoom = global::Gtk.ComboBox.NewText();
		this.comboGameRoom.WidthRequest = 430;
		this.comboGameRoom.Name = "comboGameRoom";
		this.fixed7.Add(this.comboGameRoom);
		this.hbox11.Add(this.fixed7);
		global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.hbox11[this.fixed7]));
		w17.Position = 1;
		w17.Expand = false;
		w17.Fill = false;
		this.vbox2.Add(this.hbox11);
		global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox11]));
		w18.Position = 2;
		w18.Expand = false;
		w18.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox12 = new global::Gtk.HBox();
		this.hbox12.Name = "hbox12";
		this.hbox12.Spacing = 6;
		// Container child hbox12.Gtk.Box+BoxChild
		this.fixed8 = new global::Gtk.Fixed();
		this.fixed8.WidthRequest = 250;
		this.fixed8.HeightRequest = 35;
		this.fixed8.Name = "fixed8";
		this.fixed8.HasWindow = false;
		// Container child fixed8.Gtk.Fixed+FixedChild
		this.radioButtonLocalDirectory = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Local Path"));
		this.radioButtonLocalDirectory.CanFocus = true;
		this.radioButtonLocalDirectory.Name = "radioButtonLocalDirectory";
		this.radioButtonLocalDirectory.DrawIndicator = true;
		this.radioButtonLocalDirectory.UseUnderline = true;
		this.radioButtonLocalDirectory.Group = this.radioButtonNabuNetwork.Group;
		this.fixed8.Add(this.radioButtonLocalDirectory);
		global::Gtk.Fixed.FixedChild w19 = ((global::Gtk.Fixed.FixedChild)(this.fixed8[this.radioButtonLocalDirectory]));
		w19.X = 10;
		w19.Y = 9;
		this.hbox12.Add(this.fixed8);
		global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.fixed8]));
		w20.Position = 0;
		// Container child hbox12.Gtk.Box+BoxChild
		this.fixed9 = new global::Gtk.Fixed();
		this.fixed9.WidthRequest = 500;
		this.fixed9.HeightRequest = 35;
		this.fixed9.Name = "fixed9";
		this.fixed9.HasWindow = false;
		// Container child fixed9.Gtk.Fixed+FixedChild
		this.txtLocalPath = new global::Gtk.Entry();
		global::Gtk.Tooltips w21 = new Gtk.Tooltips();
		w21.SetTip(this.txtLocalPath, "Example:  https://www.nabunetwork.com/cycle2022", "Example:  https://www.nabunetwork.com/cycle2022");
		this.txtLocalPath.WidthRequest = 500;
		this.txtLocalPath.CanFocus = true;
		this.txtLocalPath.Name = "txtLocalPath";
		this.txtLocalPath.IsEditable = true;
		this.txtLocalPath.InvisibleChar = '•';
		this.fixed9.Add(this.txtLocalPath);
		this.hbox12.Add(this.fixed9);
		global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.fixed9]));
		w23.Position = 1;
		this.vbox2.Add(this.hbox12);
		global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox12]));
		w24.Position = 3;
		w24.Expand = false;
		w24.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox13 = new global::Gtk.HBox();
		this.hbox13.Name = "hbox13";
		this.hbox13.Spacing = 6;
		// Container child hbox13.Gtk.Box+BoxChild
		this.fixed10 = new global::Gtk.Fixed();
		this.fixed10.WidthRequest = 250;
		this.fixed10.HeightRequest = 35;
		this.fixed10.Name = "fixed10";
		this.fixed10.HasWindow = false;
		// Container child fixed10.Gtk.Fixed+FixedChild
		this.radioButtonHeadless = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Control from NABU (Headless)"));
		this.radioButtonHeadless.CanFocus = true;
		this.radioButtonHeadless.Name = "radioButtonHeadless";
		this.radioButtonHeadless.DrawIndicator = true;
		this.radioButtonHeadless.UseUnderline = true;
		this.radioButtonHeadless.Group = this.radioButtonNabuNetwork.Group;
		this.fixed10.Add(this.radioButtonHeadless);
		global::Gtk.Fixed.FixedChild w25 = ((global::Gtk.Fixed.FixedChild)(this.fixed10[this.radioButtonHeadless]));
		w25.X = 10;
		w25.Y = 9;
		this.hbox13.Add(this.fixed10);
		global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.hbox13[this.fixed10]));
		w26.Position = 0;
		// Container child hbox13.Gtk.Box+BoxChild
		this.fixed11 = new global::Gtk.Fixed();
		this.fixed11.WidthRequest = 500;
		this.fixed11.HeightRequest = 35;
		this.fixed11.Name = "fixed11";
		this.fixed11.HasWindow = false;
		this.hbox13.Add(this.fixed11);
		global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.hbox13[this.fixed11]));
		w27.Position = 1;
		this.vbox2.Add(this.hbox13);
		global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox13]));
		w28.Position = 4;
		w28.Expand = false;
		w28.Fill = false;
		this.GtkAlignment.Add(this.vbox2);
		this.frame1.Add(this.GtkAlignment);
		this.GtkLabel10 = new global::Gtk.Label();
		this.GtkLabel10.Name = "GtkLabel10";
		this.GtkLabel10.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Source</b>");
		this.GtkLabel10.UseMarkup = true;
		this.frame1.LabelWidget = this.GtkLabel10;
		this.vbox1.Add(this.frame1);
		global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.frame1]));
		w31.Position = 1;
		w31.Expand = false;
		w31.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.vbox4 = new global::Gtk.VBox();
		this.vbox4.Name = "vbox4";
		this.vbox4.Spacing = 6;
		// Container child vbox4.Gtk.Box+BoxChild
		this.frame3 = new global::Gtk.Frame();
		this.frame3.Name = "frame3";
		this.frame3.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame3.Gtk.Container+ContainerChild
		this.GtkAlignment2 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment2.Name = "GtkAlignment2";
		this.GtkAlignment2.LeftPadding = ((uint)(12));
		// Container child GtkAlignment2.Gtk.Container+ContainerChild
		this.vbox3 = new global::Gtk.VBox();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		// Container child vbox3.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.fixed1 = new global::Gtk.Fixed();
		this.fixed1.WidthRequest = 400;
		this.fixed1.HeightRequest = 35;
		this.fixed1.Name = "fixed1";
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.radioSerial = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Serial RS422"));
		this.radioSerial.CanFocus = true;
		this.radioSerial.Name = "radioSerial";
		this.radioSerial.DrawIndicator = true;
		this.radioSerial.UseUnderline = true;
		this.radioSerial.Group = new global::GLib.SList(global::System.IntPtr.Zero);
		this.fixed1.Add(this.radioSerial);
		global::Gtk.Fixed.FixedChild w32 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.radioSerial]));
		w32.X = 10;
		w32.Y = 9;
		this.hbox1.Add(this.fixed1);
		global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.fixed1]));
		w33.Position = 0;
		w33.Expand = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.fixed12 = new global::Gtk.Fixed();
		this.fixed12.HeightRequest = 35;
		this.fixed12.Name = "fixed12";
		this.fixed12.HasWindow = false;
		// Container child fixed12.Gtk.Fixed+FixedChild
		this.lblPort = new global::Gtk.Label();
		this.lblPort.Name = "lblPort";
		this.lblPort.LabelProp = global::Mono.Unix.Catalog.GetString("Port:");
		this.fixed12.Add(this.lblPort);
		global::Gtk.Fixed.FixedChild w34 = ((global::Gtk.Fixed.FixedChild)(this.fixed12[this.lblPort]));
		w34.Y = 10;
		this.hbox1.Add(this.fixed12);
		global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.fixed12]));
		w35.Position = 1;
		w35.Expand = false;
		w35.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.fixed13 = new global::Gtk.Fixed();
		this.fixed13.HeightRequest = 35;
		this.fixed13.Name = "fixed13";
		this.fixed13.HasWindow = false;
		// Container child fixed13.Gtk.Fixed+FixedChild
		this.comboSerialPorts = global::Gtk.ComboBox.NewText();
		this.comboSerialPorts.WidthRequest = 200;
		this.comboSerialPorts.Name = "comboSerialPorts";
		this.fixed13.Add(this.comboSerialPorts);
		// Container child fixed13.Gtk.Fixed+FixedChild
		this.txtTcpipPort = new global::Gtk.Entry();
		this.txtTcpipPort.WidthRequest = 200;
		this.txtTcpipPort.CanFocus = true;
		this.txtTcpipPort.Name = "txtTcpipPort";
		this.txtTcpipPort.IsEditable = true;
		this.txtTcpipPort.InvisibleChar = '•';
		this.fixed13.Add(this.txtTcpipPort);
		this.hbox1.Add(this.fixed13);
		global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.fixed13]));
		w38.Position = 2;
		w38.Expand = false;
		w38.Fill = false;
		this.vbox3.Add(this.hbox1);
		global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox1]));
		w39.Position = 0;
		w39.Expand = false;
		w39.Fill = false;
		// Container child vbox3.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		// Container child hbox2.Gtk.Box+BoxChild
		this.fixed14 = new global::Gtk.Fixed();
		this.fixed14.WidthRequest = 250;
		this.fixed14.HeightRequest = 35;
		this.fixed14.Name = "fixed14";
		this.fixed14.HasWindow = false;
		// Container child fixed14.Gtk.Fixed+FixedChild
		this.radioTcpip = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("TCP/IP"));
		this.radioTcpip.CanFocus = true;
		this.radioTcpip.Name = "radioTcpip";
		this.radioTcpip.DrawIndicator = true;
		this.radioTcpip.UseUnderline = true;
		this.radioTcpip.Group = this.radioSerial.Group;
		this.fixed14.Add(this.radioTcpip);
		global::Gtk.Fixed.FixedChild w40 = ((global::Gtk.Fixed.FixedChild)(this.fixed14[this.radioTcpip]));
		w40.X = 10;
		w40.Y = 9;
		this.hbox2.Add(this.fixed14);
		global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.fixed14]));
		w41.Position = 0;
		// Container child hbox2.Gtk.Box+BoxChild
		this.fixed15 = new global::Gtk.Fixed();
		this.fixed15.HeightRequest = 35;
		this.fixed15.Name = "fixed15";
		this.fixed15.HasWindow = false;
		// Container child fixed15.Gtk.Fixed+FixedChild
		this.chkChannel = new global::Gtk.CheckButton();
		this.chkChannel.CanFocus = true;
		this.chkChannel.Name = "chkChannel";
		this.chkChannel.Label = global::Mono.Unix.Catalog.GetString("Ask For Channel");
		this.chkChannel.DrawIndicator = true;
		this.chkChannel.UseUnderline = true;
		this.fixed15.Add(this.chkChannel);
		global::Gtk.Fixed.FixedChild w42 = ((global::Gtk.Fixed.FixedChild)(this.fixed15[this.chkChannel]));
		w42.Y = 10;
		this.hbox2.Add(this.fixed15);
		global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.fixed15]));
		w43.Position = 1;
		this.vbox3.Add(this.hbox2);
		global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox2]));
		w44.Position = 1;
		w44.Expand = false;
		w44.Fill = false;
		this.GtkAlignment2.Add(this.vbox3);
		this.frame3.Add(this.GtkAlignment2);
		this.frameMode = new global::Gtk.Label();
		this.frameMode.Name = "frameMode";
		this.frameMode.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Mode</b>");
		this.frameMode.UseMarkup = true;
		this.frame3.LabelWidget = this.frameMode;
		this.vbox4.Add(this.frame3);
		global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.frame3]));
		w47.Position = 0;
		w47.Expand = false;
		w47.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.txtLog = new global::Gtk.TextView();
		this.txtLog.HeightRequest = 300;
		this.txtLog.CanFocus = true;
		this.txtLog.Name = "txtLog";
		this.txtLog.Editable = false;
		this.txtLog.CursorVisible = false;
		this.txtLog.AcceptsTab = false;
		this.GtkScrolledWindow.Add(this.txtLog);
		this.vbox4.Add(this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w49 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.GtkScrolledWindow]));
		w49.Position = 1;
		// Container child vbox4.Gtk.Box+BoxChild
		this.btnStart = new global::Gtk.Button();
		this.btnStart.HeightRequest = 70;
		this.btnStart.CanFocus = true;
		this.btnStart.Name = "btnStart";
		this.btnStart.UseUnderline = true;
		this.btnStart.Label = global::Mono.Unix.Catalog.GetString("Start Server");
		this.vbox4.Add(this.btnStart);
		global::Gtk.Box.BoxChild w50 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.btnStart]));
		w50.Position = 2;
		w50.Expand = false;
		w50.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.hbox3 = new global::Gtk.HBox();
		this.hbox3.Name = "hbox3";
		this.hbox3.Spacing = 6;
		// Container child hbox3.Gtk.Box+BoxChild
		this.progressbar1 = new global::Gtk.ProgressBar();
		this.progressbar1.HeightRequest = 22;
		this.progressbar1.Name = "progressbar1";
		this.progressbar1.Fraction = 0.01D;
		this.hbox3.Add(this.progressbar1);
		global::Gtk.Box.BoxChild w51 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.progressbar1]));
		w51.Position = 0;
		// Container child hbox3.Gtk.Box+BoxChild
		this.fixed16 = new global::Gtk.Fixed();
		this.fixed16.WidthRequest = 198;
		this.fixed16.Name = "fixed16";
		this.fixed16.HasWindow = false;
		this.hbox3.Add(this.fixed16);
		global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.fixed16]));
		w52.Position = 1;
		// Container child hbox3.Gtk.Box+BoxChild
		this.fixed17 = new global::Gtk.Fixed();
		this.fixed17.WidthRequest = 220;
		this.fixed17.Name = "fixed17";
		this.fixed17.HasWindow = false;
		this.hbox3.Add(this.fixed17);
		global::Gtk.Box.BoxChild w53 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.fixed17]));
		w53.Position = 2;
		this.vbox4.Add(this.hbox3);
		global::Gtk.Box.BoxChild w54 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.hbox3]));
		w54.Position = 3;
		w54.Expand = false;
		w54.Fill = false;
		this.vbox1.Add(this.vbox4);
		global::Gtk.Box.BoxChild w55 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.vbox4]));
		w55.Position = 2;
		this.Add(this.vbox1);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.AboutAction2.Activated += new global::System.EventHandler(this.OnAboutActionActivated);
		this.ExitAction.Activated += new global::System.EventHandler(this.OnExitActionActivated);
		this.SettingsAction2.Activated += new global::System.EventHandler(this.OnSettingsAction2Activated);
		this.radioButtonNabuNetwork.Clicked += new global::System.EventHandler(this.OnRadiobuttonNabuNetworkClicked);
		this.comboNabuNetwork.Changed += new global::System.EventHandler(this.OnComboNabuNetworkChanged);
		this.radiobuttonHomebrew.Clicked += new global::System.EventHandler(this.OnRadiobuttonHomeBrewClicked);
		this.comboHomeBrew.Changed += new global::System.EventHandler(this.OnComboHomeBrewChanged);
		this.radiobuttonGameRoom.Clicked += new global::System.EventHandler(this.OnRadiobuttonGameRoomClicked);
		this.comboGameRoom.Changed += new global::System.EventHandler(this.OnComboGameRoomChanged);
		this.radioButtonLocalDirectory.Clicked += new global::System.EventHandler(this.OnRadiobuttonLocalDirectoryClicked);
		this.txtLocalPath.Changed += new global::System.EventHandler(this.OnTxtLocalPathChanged);
		this.radioButtonHeadless.Clicked += new global::System.EventHandler(this.OnRadiobuttonHeadlessClicked);
		this.radioSerial.Clicked += new global::System.EventHandler(this.OnRadioSerialClicked);
		this.comboSerialPorts.Changed += new global::System.EventHandler(this.OnComboSerialPortsChanged);
		this.txtTcpipPort.Changed += new global::System.EventHandler(this.OnTxtTcpipPortChanged);
		this.txtTcpipPort.KeyPressEvent += new global::Gtk.KeyPressEventHandler(this.OnTxtTcpipPortKeyPressEvent);
		this.radioTcpip.Clicked += new global::System.EventHandler(this.OnRadioTcpipClicked);
		this.chkChannel.Clicked += new global::System.EventHandler(this.OnChkChannelClicked);
		this.btnStart.Clicked += new global::System.EventHandler(this.OnBtnStartClicked);
	}
}
