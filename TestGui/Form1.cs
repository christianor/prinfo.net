using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.monitoring.prinfo;
using System.Threading;

namespace TestGui
{
    public partial class Form1 : Form
    {

        PrinterManager prm = new PrinterManager();

        public Form1()
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;

            backgroundWorker2.WorkerReportsProgress = true;


        }

        private void ConnectAndCheck_button_Click(object sender, EventArgs e)
        {
            connectAndCheck_button.Enabled = false;
            backgroundWorker1.RunWorkerAsync();

        }

        private void CheckAndUpdatePrinterList(object sender, EventArgs e)
        {
            prm.LoadPrinterList();
            

            int count = 1;

            foreach (Printer printer in prm)
            {
                prm.PollPrinter(printer);
                prm.PrinterDatabase.UpdatePrinter(printer);
                count++;
            }
        }


        private void AddPrinter_button_Click(object sender, EventArgs e)
        {
            AddPrinterDialog dialog = new AddPrinterDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Printer newPrinter = prm.PrinterDatabase.CreatePrinter(dialog.Hostname);
                    backgroundWorker2.RunWorkerAsync(newPrinter);
                  
                }
                catch(ApplicationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddPrinterToListView1(Printer printer)
        {
            ListViewItem row = new ListViewItem(printer.Id);

            row.SubItems.Add(printer.HostName);
            row.SubItems.Add(printer.Pingable.ToString());
            row.SubItems.Add(printer.Manufacturer);
            row.SubItems.Add(printer.Model);
            row.SubItems.Add(printer.Description);
            row.SubItems.Add(printer.LastCheck);

            listView1.Items.Add(row);

        }

        private void WipePrinterDb_button_Click(object sender, EventArgs e)
        {
            prm.PrinterDatabase.Initialize();
        }

        private void RefreshData_button_Click(object sender, EventArgs e)
        {
            if(listView1.Items.Count > 0)
                listView1.Items.Clear();

            prm.LoadPrinterList();

            foreach (Printer printer in prm)
                AddPrinterToListView1(printer);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            prm.LoadPrinterList();

            int count = 1;
            foreach (Printer printer in prm)
            {
                prm.PollPrinter(printer);
                prm.PrinterDatabase.UpdatePrinter(printer);

                int prozValue = (int)((double)count / prm.PrinterList.Count * 100);

                if(prozValue <= 100)
                    backgroundWorker1.ReportProgress(prozValue);

                count++;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = e.ProgressPercentage + " %";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listView1.Items.Clear();

            prm.LoadPrinterList();

            foreach (Printer printer in prm)
                AddPrinterToListView1(printer);

            connectAndCheck_button.Enabled = true;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems[0] != null)
                MessageBox.Show(prm.PrinterDatabase.GetPrinterById(listView1.SelectedItems[0].Text).ToString());
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView1.SelectedItems)
                prm.PrinterDatabase.DeletePrinterByID(item.Text);

            listView1.Items.Clear();
            RefreshData_button_Click(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshData_button_Click(null, null);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker2.ReportProgress(0);
            prm.PollPrinter((Printer)e.Argument);
            backgroundWorker2.ReportProgress(50);
            prm.PrinterDatabase.UpdatePrinter((Printer)e.Argument);
            backgroundWorker2.ReportProgress(100);
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = e.ProgressPercentage + " %";
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshData_button_Click(null, null);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        } 

    }
}
