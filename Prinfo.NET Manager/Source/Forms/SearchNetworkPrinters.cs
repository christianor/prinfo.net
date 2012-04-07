using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace com.monitoring.prinfo.manager
{

    public partial class SearchNetworkPrinters : Form
    {
        PrinterManager manager = new PrinterManager();

        public SearchNetworkPrinters()
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            object[] arr = e.UserState as object[];

            AddItemToListView(arr[0] as string, (bool)arr[1]);
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arr = e.Argument as object[];
            int[] startIpSegments = arr[0] as int[];
            int[] endIpSegments = arr[1] as int[];

            string ipString = IpIterator.IpSegsToString(startIpSegments);

            backgroundWorker1.ReportProgress(0, new object[] { ipString, manager.IsAPrinter(ipString) });

            while (!IpIterator.IpSegsEqual(startIpSegments, endIpSegments))
            {
                IpIterator.RaiseIpSegs(startIpSegments);
                ipString = IpIterator.IpSegsToString(startIpSegments);

                backgroundWorker1.ReportProgress(0, new object[] { ipString, manager.IsAPrinter(ipString) });
                
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            var startIp = textBox1.Text;
            var endIp = textBox2.Text;

            //
            // check for correct ips
            //
            try
            {
                IPAddress.Parse(startIp);
                IPAddress.Parse(endIp);
            }
            catch (FormatException)
            {
                MessageBox.Show("Sie haben eine ungültige IP angegeben.", "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //
            // retrieve the segments
            //
            int[] startIpSegments = IpIterator.StringToIpSeg(startIp);
            int[] endIpSegments = IpIterator.StringToIpSeg(endIp);
            
            // check if the endIp is realy bigger than the startIp

            if (IpIterator.ValidateStartAndEnd(startIpSegments, endIpSegments))
            {
                object[] arr = new object[] { startIpSegments, endIpSegments };
                backgroundWorker1.RunWorkerAsync(arr as object);
                
            }
            else
            {
                MessageBox.Show("Start- und End-Ip ergeben keinen Sinn. Bitte beachten sie das die End-Ip höher als ihre Start-Ip sein muss.", "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
        }

        public void AddItemToListView(string ip, bool isPrinter)
        {
            var item = new ListViewItem(ip);
            item.Group = isPrinter ? listViewGroup1 : listViewGroup2;
            item.SubItems.Add(isPrinter ? "Ja" : "Nein");
            listView1.Items.Add(item);
        }

        private void SearchNetworkPrinters_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Printer> list = new List<Printer>();

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                list.Add( new Printer { HostName = item.Text } );
            }

            foreach (var printer in list)
            {
                manager.PrinterDatabase.CreatePrinter(printer.HostName);
            }
        }

    }
}
