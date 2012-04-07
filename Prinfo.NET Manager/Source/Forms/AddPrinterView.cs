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
    public partial class AddPrinterView : Form
    {
        public Printer Printer { set; get; }

        public AddPrinterView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            PrinterDatabase db = new PrinterDatabase();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Der Hostname darf nicht leer sein. Bitte geben Sie mindestens ein Zeichen ein.", "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Printer = db.CreatePrinter(textBox1.Text);
                    this.DialogResult = DialogResult.OK;

                    this.Close();
                }
                catch (Exception ae)
                {
                    MessageBox.Show("Unerwarteter Fehler. Details: " + ae.Message, "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Log(ae.Message, LogType.Error);
                }
            }
        }

        private void AddPrinterView_Activated(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
