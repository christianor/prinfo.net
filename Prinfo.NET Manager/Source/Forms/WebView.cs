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
    public partial class WebView : Form
    {
        public WebView(Printer printer)
        {
            InitializeComponent();
            this.Text = String.Format("{0} - {1}", printer.HostName, this.Text);

            webBrowser1.StatusTextChanged += new EventHandler( (object o, EventArgs e) => toolStripStatusLabel1.Text = webBrowser1.StatusText );

            webBrowser1.Url = new UriBuilder("http", printer.HostName, 80, "").Uri;
            webBrowser1.Refresh();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }
    }
}
