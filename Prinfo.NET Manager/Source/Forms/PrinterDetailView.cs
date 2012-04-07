using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace com.monitoring.prinfo.manager
{
    public partial class PrinterDetailView : Form
    {
        private Printer printer;
        private PrinterManager printerManager = new PrinterManager();

        public PrinterDetailView(Printer printer)
        {
            InitializeComponent();
            this.printer = printer;

        }

        private void PrinterDetailView_Load(object sender, EventArgs e)
        {
            if (printer.Pingable)
                pingableLabel.Text = "Ja";
            else
                pingableLabel.Text = "Nein";

            textBox1.Text = printer.HostName;
            textBox2.Text = printer.Manufacturer;
            textBox3.Text = printer.Model;
            textBox4.Text = string.IsNullOrEmpty(printer.LastCheck)?"Nie":printer.LastCheck;
            textBox5.Text = printer.PageCount == -1?"Nicht vorhanden":printer.PageCount.ToString();
            textBox6.Text = printer.PageCountColor == -1 ? "Nicht vorhanden" : printer.PageCountColor.ToString();
            textBox7.Text = printer.Description;
            textBox8.Text = printer.SysLocation;
            textBox9.Text = printer.SysContact;

            if (printer.Supplies.Count == 0)
            {
                listView1.Items.Add("keine Verbrauchsteile");
                listView1.Enabled = false;

                button1.Enabled = false;
            }
            else
                foreach (var supply in printer.Supplies)
                {
                    var item = new ListViewItem(supply.Description);

                    if (supply.Value < 5)
                        item.BackColor = Color.MistyRose;
                    else if(supply.Value < 15)
                        item.BackColor = Color.LightYellow;
                   


                    item.SubItems.Add(supply.Value.ToString() + " %");

                    listView1.Items.Add(item);
                }

            var entries = printerManager.ArchivDatabase.GetEntriesById(printer.Id);

            if (entries.Count > 0)
            {
                InitChart(entries);
            }
            else
            {
                label13.Visible = false;
            }

        }

        public void InitChart(List<Printer> entries)
        {
            zedGraphControl1 = new ZedGraphControl();

            this.zedGraphControl1.Location = new System.Drawing.Point(12, 737);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(591, 265);
            this.zedGraphControl1.TabIndex = 7;

            this.panel1.Controls.Add(this.zedGraphControl1);


            GraphPane myPane = zedGraphControl1.GraphPane;

            // set the title and axis labels
            myPane.Title.Text = "Übersicht gedruckte Seiten";
            myPane.Title.FontSpec.Size = 24.0f;
            myPane.XAxis.Title.Text = "Zeitpunkt";
            myPane.XAxis.Title.FontSpec.Size = 16f;
            myPane.YAxis.Title.Text = "gedruckte Seiten";
            myPane.YAxis.Title.FontSpec.Size = 16f;

            PointPairList ppL = new PointPairList();
            PointPairList ppL2 = new PointPairList();
   

            // mono pointpairlist
            foreach (var entry in entries)
            {
                if(entry.PageCount > -1)
                    ppL.Add(new XDate(DateTime.Parse(entry.LastCheck)), entry.PageCount);
                if (entry.PageCountColor > -1)
                    ppL2.Add(new XDate(DateTime.Parse(entry.LastCheck)), entry.PageCountColor);
            }

            myPane.AddCurve("Mono", ppL, Color.Blue, SymbolType.Circle);
            myPane.AddCurve("Color", ppL2, Color.Red, SymbolType.Circle);

            myPane.XAxis.Type = AxisType.Date;
            myPane.YAxis.Type = AxisType.Linear;
            

            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0F);
            myPane.AxisChange();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new DetailNotificationSettings(this.printer).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // this.Close();
        }

    }
}
