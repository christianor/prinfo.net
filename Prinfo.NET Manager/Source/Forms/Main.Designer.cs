
namespace com.monitoring.prinfo.manager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private PrinterManager printerManager = new PrinterManager();

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.druckerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hinzufügenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.druckerAuswahlÜberprüfenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alleDruckerÜberprüfenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.druckerAuswahlLöschenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alleDruckerLöschenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.nachDruckernImNetzwerkSuchenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dienstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startStoppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.serviceInstallierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dienstDeinstallierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.addPrinterToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.deletePrinterToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshListViewToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.searchTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.searchButton = new System.Windows.Forms.ToolStripButton();
            this.printerListView = new System.Windows.Forms.ListView();
            this.Hostname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Erreichbar = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Letzte_Prüfung = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Hersteller = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Modell = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Beschreibung = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.printerRightClickContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.druckerAktualisierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weboberflächeÖffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.löschenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.versionText = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkPrintersBgWorker = new System.ComponentModel.BackgroundWorker();
            this.loadPrinterBgWorker = new System.ComponentModel.BackgroundWorker();
            this.searchBgWorker = new System.ComponentModel.BackgroundWorker();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.infoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.beendenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.printerRightClickContext.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.notifyIconContextStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.druckerToolStripMenuItem,
            this.dienstToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(742, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.einstellungenToolStripMenuItem1,
            this.toolStripSeparator1,
            this.beendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.dateiToolStripMenuItem.Text = "Manager";
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("infoToolStripMenuItem.Image")));
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // einstellungenToolStripMenuItem1
            // 
            this.einstellungenToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("einstellungenToolStripMenuItem1.Image")));
            this.einstellungenToolStripMenuItem1.Name = "einstellungenToolStripMenuItem1";
            this.einstellungenToolStripMenuItem1.Size = new System.Drawing.Size(145, 22);
            this.einstellungenToolStripMenuItem1.Text = "Einstellungen";
            this.einstellungenToolStripMenuItem1.Click += new System.EventHandler(this.einstellungenToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(142, 6);
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.beendenToolStripMenuItem.Text = "Beenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click_1);
            // 
            // druckerToolStripMenuItem
            // 
            this.druckerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hinzufügenToolStripMenuItem,
            this.toolStripSeparator3,
            this.druckerAuswahlÜberprüfenToolStripMenuItem,
            this.alleDruckerÜberprüfenToolStripMenuItem,
            this.toolStripSeparator4,
            this.druckerAuswahlLöschenToolStripMenuItem,
            this.alleDruckerLöschenToolStripMenuItem,
            this.toolStripSeparator5,
            this.nachDruckernImNetzwerkSuchenToolStripMenuItem});
            this.druckerToolStripMenuItem.Name = "druckerToolStripMenuItem";
            this.druckerToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.druckerToolStripMenuItem.Text = "Drucker";
            // 
            // hinzufügenToolStripMenuItem
            // 
            this.hinzufügenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("hinzufügenToolStripMenuItem.Image")));
            this.hinzufügenToolStripMenuItem.Name = "hinzufügenToolStripMenuItem";
            this.hinzufügenToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.hinzufügenToolStripMenuItem.Text = "Hinzufügen";
            this.hinzufügenToolStripMenuItem.Click += new System.EventHandler(this.hinzufügenToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(260, 6);
            // 
            // druckerAuswahlÜberprüfenToolStripMenuItem
            // 
            this.druckerAuswahlÜberprüfenToolStripMenuItem.Name = "druckerAuswahlÜberprüfenToolStripMenuItem";
            this.druckerAuswahlÜberprüfenToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.druckerAuswahlÜberprüfenToolStripMenuItem.Text = "Drucker Auswahl überprüfen";
            this.druckerAuswahlÜberprüfenToolStripMenuItem.Click += new System.EventHandler(this.druckerAuswahlÜberprüfenToolStripMenuItem_Click);
            // 
            // alleDruckerÜberprüfenToolStripMenuItem
            // 
            this.alleDruckerÜberprüfenToolStripMenuItem.Name = "alleDruckerÜberprüfenToolStripMenuItem";
            this.alleDruckerÜberprüfenToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.alleDruckerÜberprüfenToolStripMenuItem.Text = "Alle Drucker überprüfen";
            this.alleDruckerÜberprüfenToolStripMenuItem.Click += new System.EventHandler(this.alleDruckerÜberprüfenToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(260, 6);
            // 
            // druckerAuswahlLöschenToolStripMenuItem
            // 
            this.druckerAuswahlLöschenToolStripMenuItem.Name = "druckerAuswahlLöschenToolStripMenuItem";
            this.druckerAuswahlLöschenToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.druckerAuswahlLöschenToolStripMenuItem.Text = "Drucker Auswahl löschen";
            this.druckerAuswahlLöschenToolStripMenuItem.Click += new System.EventHandler(this.druckerAuswahlLöschenToolStripMenuItem_Click);
            // 
            // alleDruckerLöschenToolStripMenuItem
            // 
            this.alleDruckerLöschenToolStripMenuItem.Name = "alleDruckerLöschenToolStripMenuItem";
            this.alleDruckerLöschenToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.alleDruckerLöschenToolStripMenuItem.Text = "Alle Drucker löschen";
            this.alleDruckerLöschenToolStripMenuItem.Click += new System.EventHandler(this.alleDruckerLöschenToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(260, 6);
            // 
            // nachDruckernImNetzwerkSuchenToolStripMenuItem
            // 
            this.nachDruckernImNetzwerkSuchenToolStripMenuItem.Name = "nachDruckernImNetzwerkSuchenToolStripMenuItem";
            this.nachDruckernImNetzwerkSuchenToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.nachDruckernImNetzwerkSuchenToolStripMenuItem.Text = "Nach Druckern im Netzwerk suchen";
            this.nachDruckernImNetzwerkSuchenToolStripMenuItem.Click += new System.EventHandler(this.nachDruckernImNetzwerkSuchenToolStripMenuItem_Click);
            // 
            // dienstToolStripMenuItem
            // 
            this.dienstToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startStoppToolStripMenuItem,
            this.toolStripSeparator6,
            this.serviceInstallierenToolStripMenuItem,
            this.dienstDeinstallierenToolStripMenuItem});
            this.dienstToolStripMenuItem.Name = "dienstToolStripMenuItem";
            this.dienstToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.dienstToolStripMenuItem.Text = "Dienst";
            this.dienstToolStripMenuItem.DropDownOpening += new System.EventHandler(this.dienstToolStripMenuItem_DropDownOpening);
            // 
            // startStoppToolStripMenuItem
            // 
            this.startStoppToolStripMenuItem.Name = "startStoppToolStripMenuItem";
            this.startStoppToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startStoppToolStripMenuItem.Text = "Start / Stopp";
            this.startStoppToolStripMenuItem.Click += new System.EventHandler(this.startStoppToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(177, 6);
            // 
            // serviceInstallierenToolStripMenuItem
            // 
            this.serviceInstallierenToolStripMenuItem.Name = "serviceInstallierenToolStripMenuItem";
            this.serviceInstallierenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.serviceInstallierenToolStripMenuItem.Text = "Dienst installieren";
            this.serviceInstallierenToolStripMenuItem.Click += new System.EventHandler(this.serviceInstallierenToolStripMenuItem_Click);
            // 
            // dienstDeinstallierenToolStripMenuItem
            // 
            this.dienstDeinstallierenToolStripMenuItem.Name = "dienstDeinstallierenToolStripMenuItem";
            this.dienstDeinstallierenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dienstDeinstallierenToolStripMenuItem.Text = "Dienst deinstallieren";
            this.dienstDeinstallierenToolStripMenuItem.Click += new System.EventHandler(this.dienstDeinstallierenToolStripMenuItem_Click);
            // 
            // toolBar
            // 
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPrinterToolbarButton,
            this.deletePrinterToolbarButton,
            this.toolStripSeparator2,
            this.refreshListViewToolbarButton,
            this.toolStripSeparator8,
            this.toolStripLabel1,
            this.searchTextBox,
            this.searchButton});
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(742, 25);
            this.toolBar.TabIndex = 1;
            this.toolBar.Text = "toolStrip1";
            // 
            // addPrinterToolbarButton
            // 
            this.addPrinterToolbarButton.Image = ((System.Drawing.Image)(resources.GetObject("addPrinterToolbarButton.Image")));
            this.addPrinterToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addPrinterToolbarButton.Name = "addPrinterToolbarButton";
            this.addPrinterToolbarButton.Size = new System.Drawing.Size(131, 22);
            this.addPrinterToolbarButton.Text = "Drucker hinzufügen";
            this.addPrinterToolbarButton.Click += new System.EventHandler(this.addPrinterToolBarButton_Click);
            // 
            // deletePrinterToolbarButton
            // 
            this.deletePrinterToolbarButton.Image = ((System.Drawing.Image)(resources.GetObject("deletePrinterToolbarButton.Image")));
            this.deletePrinterToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deletePrinterToolbarButton.Name = "deletePrinterToolbarButton";
            this.deletePrinterToolbarButton.Size = new System.Drawing.Size(112, 22);
            this.deletePrinterToolbarButton.Text = "Drucker löschen";
            this.deletePrinterToolbarButton.Click += new System.EventHandler(this.deletePrinterSelection_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // refreshListViewToolbarButton
            // 
            this.refreshListViewToolbarButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshListViewToolbarButton.Image")));
            this.refreshListViewToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshListViewToolbarButton.Name = "refreshListViewToolbarButton";
            this.refreshListViewToolbarButton.Size = new System.Drawing.Size(218, 22);
            this.refreshListViewToolbarButton.Text = "Liste mit Datenbank synchronisieren";
            this.refreshListViewToolbarButton.ToolTipText = "Aktualisiert die Liste mit den aktuellen Druckern und Werten aus der Datenbank";
            this.refreshListViewToolbarButton.Click += new System.EventHandler(this.refreshListViewButton_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLabel1.Image")));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(26, 22);
            this.toolStripLabel1.Text = " ";
            // 
            // searchTextBox
            // 
            this.searchTextBox.BackColor = System.Drawing.Color.White;
            this.searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.searchTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(100, 25);
            this.searchTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyUp);
            // 
            // searchButton
            // 
            this.searchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.searchButton.Image = ((System.Drawing.Image)(resources.GetObject("searchButton.Image")));
            this.searchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(50, 22);
            this.searchButton.Text = "Suchen";
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // printerListView
            // 
            this.printerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Hostname,
            this.Erreichbar,
            this.Letzte_Prüfung,
            this.Hersteller,
            this.Modell,
            this.Beschreibung});
            this.printerListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printerListView.FullRowSelect = true;
            this.printerListView.HideSelection = false;
            this.printerListView.Location = new System.Drawing.Point(0, 49);
            this.printerListView.Name = "printerListView";
            this.printerListView.Size = new System.Drawing.Size(742, 495);
            this.printerListView.TabIndex = 2;
            this.printerListView.UseCompatibleStateImageBehavior = false;
            this.printerListView.View = System.Windows.Forms.View.Details;
            this.printerListView.DoubleClick += new System.EventHandler(this.printerListView_DoubleClick);
            this.printerListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.printerListView_MouseClick);
            // 
            // Hostname
            // 
            this.Hostname.Text = "Hostname";
            this.Hostname.Width = 93;
            // 
            // Erreichbar
            // 
            this.Erreichbar.Text = "Erreichbar";
            this.Erreichbar.Width = 71;
            // 
            // Letzte_Prüfung
            // 
            this.Letzte_Prüfung.Text = "Letzte Prüfung";
            this.Letzte_Prüfung.Width = 114;
            // 
            // Hersteller
            // 
            this.Hersteller.Text = "Hersteller";
            this.Hersteller.Width = 106;
            // 
            // Modell
            // 
            this.Modell.Text = "Modell";
            this.Modell.Width = 100;
            // 
            // Beschreibung
            // 
            this.Beschreibung.Text = "Beschreibung";
            this.Beschreibung.Width = 234;
            // 
            // printerRightClickContext
            // 
            this.printerRightClickContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailsToolStripMenuItem,
            this.druckerAktualisierenToolStripMenuItem,
            this.weboberflächeÖffnenToolStripMenuItem,
            this.toolStripSeparator7,
            this.löschenToolStripMenuItem});
            this.printerRightClickContext.Name = "printerRightClickContext";
            this.printerRightClickContext.Size = new System.Drawing.Size(194, 120);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("detailsToolStripMenuItem.Image")));
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // druckerAktualisierenToolStripMenuItem
            // 
            this.druckerAktualisierenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("druckerAktualisierenToolStripMenuItem.Image")));
            this.druckerAktualisierenToolStripMenuItem.Name = "druckerAktualisierenToolStripMenuItem";
            this.druckerAktualisierenToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.druckerAktualisierenToolStripMenuItem.Text = "Drucker aktualisieren";
            this.druckerAktualisierenToolStripMenuItem.Click += new System.EventHandler(this.druckerAktualisierenToolStripMenuItem_Click);
            // 
            // weboberflächeÖffnenToolStripMenuItem
            // 
            this.weboberflächeÖffnenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("weboberflächeÖffnenToolStripMenuItem.Image")));
            this.weboberflächeÖffnenToolStripMenuItem.Name = "weboberflächeÖffnenToolStripMenuItem";
            this.weboberflächeÖffnenToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.weboberflächeÖffnenToolStripMenuItem.Text = "Weboberfläche öffnen";
            this.weboberflächeÖffnenToolStripMenuItem.Click += new System.EventHandler(this.weboberflächeÖffnenToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(190, 6);
            // 
            // löschenToolStripMenuItem
            // 
            this.löschenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("löschenToolStripMenuItem.Image")));
            this.löschenToolStripMenuItem.Name = "löschenToolStripMenuItem";
            this.löschenToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.löschenToolStripMenuItem.Text = "Löschen";
            this.löschenToolStripMenuItem.Click += new System.EventHandler(this.löschenToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText,
            this.toolStripProgressBar1,
            this.versionText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(742, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // versionText
            // 
            this.versionText.Name = "versionText";
            this.versionText.Size = new System.Drawing.Size(0, 17);
            // 
            // checkPrintersBgWorker
            // 
            this.checkPrintersBgWorker.WorkerReportsProgress = true;
            this.checkPrintersBgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkPrintersBgWorker_DoWork);
            this.checkPrintersBgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.checkPrintersBgWorker_ProgressChanged);
            this.checkPrintersBgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.checkPrintersBgWorker_RunWorkerCompleted);
            // 
            // loadPrinterBgWorker
            // 
            this.loadPrinterBgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.loadPrinterBgWorker_DoWork);
            this.loadPrinterBgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.loadPrinterBgWorker_RunWorkerCompleted);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.notifyIconContextStrip;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Prinfo.NET Manager";
            this.notifyIcon1.Visible = true;
            // 
            // notifyIconContextStrip
            // 
            this.notifyIconContextStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem1,
            this.toolStripSeparator9,
            this.beendenToolStripMenuItem1});
            this.notifyIconContextStrip.Name = "notifyIconContextStrip";
            this.notifyIconContextStrip.Size = new System.Drawing.Size(121, 54);
            // 
            // infoToolStripMenuItem1
            // 
            this.infoToolStripMenuItem1.Name = "infoToolStripMenuItem1";
            this.infoToolStripMenuItem1.Size = new System.Drawing.Size(120, 22);
            this.infoToolStripMenuItem1.Text = "Info";
            this.infoToolStripMenuItem1.Click += new System.EventHandler(this.infoToolStripMenuItem1_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(117, 6);
            // 
            // beendenToolStripMenuItem1
            // 
            this.beendenToolStripMenuItem1.Name = "beendenToolStripMenuItem1";
            this.beendenToolStripMenuItem1.Size = new System.Drawing.Size(120, 22);
            this.beendenToolStripMenuItem1.Text = "Beenden";
            this.beendenToolStripMenuItem1.Click += new System.EventHandler(this.beendenToolStripMenuItem1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(742, 566);
            this.Controls.Add(this.printerListView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prinfo.NET Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.printerRightClickContext.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.notifyIconContextStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem druckerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hinzufügenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alleDruckerÜberprüfenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem druckerAuswahlÜberprüfenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem druckerAuswahlLöschenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alleDruckerLöschenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dienstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startStoppToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripMenuItem serviceInstallierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dienstDeinstallierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton addPrinterToolbarButton;
        private System.Windows.Forms.ToolStripButton deletePrinterToolbarButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ListView printerListView;
        private System.Windows.Forms.ColumnHeader Hostname;
        private System.Windows.Forms.ColumnHeader Erreichbar;
        private System.Windows.Forms.ColumnHeader Letzte_Prüfung;
        private System.Windows.Forms.ColumnHeader Hersteller;
        private System.Windows.Forms.ColumnHeader Modell;
        private System.Windows.Forms.ColumnHeader Beschreibung;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ContextMenuStrip printerRightClickContext;
        private System.Windows.Forms.ToolStripMenuItem weboberflächeÖffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem löschenToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel versionText;
        private System.ComponentModel.BackgroundWorker checkPrintersBgWorker;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripMenuItem druckerAktualisierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton refreshListViewToolbarButton;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker loadPrinterBgWorker;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox searchTextBox;
        private System.ComponentModel.BackgroundWorker searchBgWorker;
        private System.Windows.Forms.ToolStripButton searchButton;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextStrip;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem nachDruckernImNetzwerkSuchenToolStripMenuItem;



    }
}

