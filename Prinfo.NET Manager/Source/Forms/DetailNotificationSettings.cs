using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace com.monitoring.prinfo.manager
{
    public partial class DetailNotificationSettings : Form
    {
        private LoadingView loading = new LoadingView();
        private PrinterManager printerManager = new PrinterManager();
        private Printer printerToHighlight;

        public DetailNotificationSettings()
        {
            InitializeComponent();
        }

        public DetailNotificationSettings(Printer printerToHighlight) : this()
        {
            this.printerToHighlight = printerToHighlight;
        }

        private void DetailNotificationSettings_Load(object sender, EventArgs e)
        {
            loadPrintersBgWorker.RunWorkerAsync();
            loading.ShowDialog();
        }

        private void loadPrintersBgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loading.Close();
            treeView1.Nodes.Clear();
            var master = treeView1.Nodes.Add("Alle Drucker mit Verbrauchsteilen");

            foreach (var printer in printerManager.PrinterList.Where((p, b) => p.Supplies.Count > 0))
            {
                var node = master.Nodes.Add(String.Format("{0}  {1} Supplies", printer.HostName, printer.Supplies.Count));
                node.Tag = printer;

                foreach (var supply in printer)
                {
                    var supplyNode = node.Nodes.Add(String.Format("{0} Wert: {1} % > {2} Schwellwert: {3}", supply.Description, supply.Value, supply.NotifyWhenLow ? "Benachrichtigung aktiv" : "Benachrichtigung inaktiv", supply.NotificationValue));
                    supplyNode.Tag = supply;

                    if (supply.NotifyWhenLow)
                        supplyNode.BackColor = Color.LightGreen;
                }

                if (printerToHighlight != null)
                    if (printerToHighlight.Id == printer.Id)
                    {
                        node.Expand();
                        node.BackColor = Color.Yellow;
                    }
            }

            master.Expand();
            master.Text += " (" + master.Nodes.Count + ")";
        }

        private void loadPrintersBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            printerManager.LoadPrinterList();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
                if (e.Node.Checked == true)
                    node.Checked = true;
                else
                    node.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var value = int.Parse(textBox1.Text);

            // printers with changes
            var printers = new List<Printer>();


            foreach (TreeNode node in treeView1.Nodes[0].Nodes)
                foreach (TreeNode supplyNode in node.Nodes)
                    if (supplyNode.Checked)
                    {
                        var supply = supplyNode.Tag as Supply;

                        if (radioButton1.Checked)
                        {
                            supply.NotifyWhenLow = true;
                            supply.NotificationValue = value;
                        }
                        else if (radioButton2.Checked)
                        {
                            supply.NotifyWhenLow = false;
                            supply.NotificationValue = value;
                            supply.Notified = false;
                        }

                        printers.Add(node.Tag as Printer);
                    }

            // update printers with changes 
            printers.ForEach((p) => printerManager.PrinterDatabase.UpdatePrinter(p));

            this.Cursor = Cursors.Default;

            loadPrintersBgWorker.RunWorkerAsync();
            loading.ShowDialog();
        }
    }
}
