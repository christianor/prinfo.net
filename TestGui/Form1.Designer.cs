namespace TestGui
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView1 = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hostname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pingable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.manufacturer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.model = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.last_check = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.connectAndCheck_button = new System.Windows.Forms.Button();
            this.addPrinter_button = new System.Windows.Forms.Button();
            this.wipeDb_button = new System.Windows.Forms.Button();
            this.refreshData_button = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.hostname,
            this.pingable,
            this.manufacturer,
            this.model,
            this.description,
            this.last_check});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(325, 33);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1149, 472);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // id
            // 
            this.id.Text = "id";
            this.id.Width = 119;
            // 
            // hostname
            // 
            this.hostname.Text = "hostname";
            this.hostname.Width = 119;
            // 
            // pingable
            // 
            this.pingable.Text = "pingable";
            this.pingable.Width = 55;
            // 
            // manufacturer
            // 
            this.manufacturer.Text = "manufacturer";
            this.manufacturer.Width = 136;
            // 
            // model
            // 
            this.model.Text = "model";
            this.model.Width = 135;
            // 
            // description
            // 
            this.description.Text = "description";
            this.description.Width = 178;
            // 
            // last_check
            // 
            this.last_check.Text = "last check";
            this.last_check.Width = 118;
            // 
            // connectAndCheck_button
            // 
            this.connectAndCheck_button.Location = new System.Drawing.Point(16, 105);
            this.connectAndCheck_button.Margin = new System.Windows.Forms.Padding(4);
            this.connectAndCheck_button.Name = "connectAndCheck_button";
            this.connectAndCheck_button.Size = new System.Drawing.Size(289, 28);
            this.connectAndCheck_button.TabIndex = 1;
            this.connectAndCheck_button.Text = "connect and check printer list";
            this.connectAndCheck_button.UseVisualStyleBackColor = true;
            this.connectAndCheck_button.Click += new System.EventHandler(this.ConnectAndCheck_button_Click);
            // 
            // addPrinter_button
            // 
            this.addPrinter_button.Location = new System.Drawing.Point(16, 33);
            this.addPrinter_button.Margin = new System.Windows.Forms.Padding(4);
            this.addPrinter_button.Name = "addPrinter_button";
            this.addPrinter_button.Size = new System.Drawing.Size(100, 28);
            this.addPrinter_button.TabIndex = 2;
            this.addPrinter_button.Text = "Add PrinterList";
            this.addPrinter_button.UseVisualStyleBackColor = true;
            this.addPrinter_button.Click += new System.EventHandler(this.AddPrinter_button_Click);
            // 
            // wipeDb_button
            // 
            this.wipeDb_button.Location = new System.Drawing.Point(124, 33);
            this.wipeDb_button.Margin = new System.Windows.Forms.Padding(4);
            this.wipeDb_button.Name = "wipeDb_button";
            this.wipeDb_button.Size = new System.Drawing.Size(181, 28);
            this.wipeDb_button.TabIndex = 3;
            this.wipeDb_button.Text = "Wipe printer.db";
            this.wipeDb_button.UseVisualStyleBackColor = true;
            this.wipeDb_button.Click += new System.EventHandler(this.WipePrinterDb_button_Click);
            // 
            // refreshData_button
            // 
            this.refreshData_button.Location = new System.Drawing.Point(16, 69);
            this.refreshData_button.Margin = new System.Windows.Forms.Padding(4);
            this.refreshData_button.Name = "refreshData_button";
            this.refreshData_button.Size = new System.Drawing.Size(289, 28);
            this.refreshData_button.TabIndex = 5;
            this.refreshData_button.Text = "refresh data";
            this.refreshData_button.UseVisualStyleBackColor = true;
            this.refreshData_button.Click += new System.EventHandler(this.RefreshData_button_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 516);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1501, 26);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "Status";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 21);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(133, 20);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1501, 28);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem1});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(78, 24);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(102, 24);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1501, 542);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.refreshData_button);
            this.Controls.Add(this.wipeDb_button);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.connectAndCheck_button);
            this.Controls.Add(this.addPrinter_button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1519, 587);
            this.MinimumSize = new System.Drawing.Size(1519, 587);
            this.Name = "Form1";
            this.Text = "Prinfo.Net TestGUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader hostname;
        private System.Windows.Forms.ColumnHeader pingable;
        private System.Windows.Forms.ColumnHeader manufacturer;
        private System.Windows.Forms.ColumnHeader model;
        private System.Windows.Forms.Button connectAndCheck_button;
        private System.Windows.Forms.Button addPrinter_button;
        private System.Windows.Forms.Button wipeDb_button;
        private System.Windows.Forms.Button refreshData_button;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ColumnHeader description;
        private System.Windows.Forms.ColumnHeader last_check;


    }
}

