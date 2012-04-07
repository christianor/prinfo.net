using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.monitoring.prinfo;
using System.IO;

// Achtung! nach dem erstellen eines neuen Builds müssen die entstandenen DLLs im im Build/web verzeichnis in das bin verzeichnis
// verschoben werden
namespace Prinfo.NET_Web_Manager
{
    public partial class _Default : System.Web.UI.Page
    {
        private PrinterManager printerManager;

        public _Default()
        {
            printerManager = new PrinterManager(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName);
            
        }

        protected void TestMethod(object sender, EventArgs e)
        {
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            printerList.HeaderStyle.BackColor = System.Drawing.Color.Gray;

            printerList.DataSource = printerManager.LoadPrinterList();
            
            printerList.DataBind();
        }
    }
}