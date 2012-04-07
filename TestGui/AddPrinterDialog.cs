using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestGui
{
    public partial class AddPrinterDialog : Form
    {
        public AddPrinterDialog()
        {
            InitializeComponent();
        }

        private void AddPrinterDialog_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        public string Hostname
        {
            get
            {
                return textBox1.Text;
            }
        }
    }
}
