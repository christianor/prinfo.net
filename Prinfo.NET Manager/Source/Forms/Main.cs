using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Threading;

namespace com.monitoring.prinfo.manager
{
    public partial class MainForm : Form
    {
        private ListViewGroup offlineGroup;
        private ListViewGroup onlineGroup;
        private ListViewGroup emptyGroup;

        /// <summary>
        /// loading window
        /// </summary>
        private LoadingView loading = new LoadingView();

        /// <summary>
        /// construcotr initializes components and extended listview elements
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            statusText.Text = Config.Version.Description;

            offlineGroup = new ListViewGroup("Offline");
            onlineGroup = new ListViewGroup("Online");
            emptyGroup = new ListViewGroup("Keine Drucker");

            printerListView.Groups.Add(onlineGroup);
            printerListView.Groups.Add(offlineGroup);
            printerListView.Groups.Add(emptyGroup);

        }

        /// <summary>
        /// initialy loads the complete printer list and draws it to the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Config.Load();
            loadPrinterBgWorker.RunWorkerAsync();
            loading.ShowDialog();

            if (!ServiceState().Equals("Läuft"))
                notifyIcon1.ShowBalloonTip(30000, "Prinfo.NET Dienst", "Achtung der Prinfo.NET Dienst ist derzeit nicht gestartet", ToolTipIcon.Warning);
        }

        private void setListViewRow(Printer printer, int index)
        {
            ListViewItem row = printerListView.Items[index];
            row.SubItems[0].Text = printer.HostName;

            row.SubItems[1].Text = printer.Pingable ? "Ja" : "Nein";
            row.SubItems[2].Text = String.IsNullOrWhiteSpace(printer.LastCheck) ? "-" : printer.LastCheck;
            row.SubItems[3].Text = printer.Manufacturer;
            row.SubItems[4].Text = printer.Model;
            row.SubItems[5].Text = printer.Description;

            if (printer.Pingable)
                row.Group = onlineGroup;
            else
                row.Group = offlineGroup;
        }

        /// <summary>
        /// exit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beendenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// open the settingsView as dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void einstellungenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new SettingsView().ShowDialog();
        }

 
        /// <summary>
        /// paints a list of printers to the list view
        /// </summary>
        /// <param name="printers"></param>
        private void PaintPrinterListToListView(List<Printer> printers)
        {
            printerListView.Items.Clear();

            if (printers.Count > 0)
            {
                printerListView.Enabled = true;

                foreach (Printer printer in printers)
                {
                    ListViewItem row = new ListViewItem(printer.HostName);

                    row.SubItems.Add(printer.Pingable ? "Ja" : "Nein");
                    row.SubItems.Add(String.IsNullOrWhiteSpace(printer.LastCheck) ? "-" : printer.LastCheck);
                    row.SubItems.Add(printer.Manufacturer);
                    row.SubItems.Add(printer.Model);
                    row.SubItems.Add(printer.Description);

                    row.Tag = printer;

                    if (printer.Pingable)
                        row.Group = onlineGroup;
                    else
                        row.Group = offlineGroup;

                    printerListView.Items.Add(row);
                }
            }
            else
            {
                ListViewItem row = new ListViewItem("");
                row.Group = emptyGroup;

                printerListView.Items.Add(row);
                printerListView.Enabled = false;
            }
        }

        #region service controlls
        /// <summary>
        /// install the service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serviceInstallierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Der Prinfo.NET Dienst wird auf ihrem Computer installiert. Soll der Vorgang fortgesetzt werden?", "Prinfo.NET Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                new Thread(() =>
                {
                    try
                    {
                        ServiceManager.InstallPrinfoService();
                        MessageBox.Show("Dienst erfolgreich installiert", "Prinfo.NET Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dienst konnte nicht installiert werden. Ist der Dienst bereits installiert? Bitte prüfen Sie, ob Sie über administrative Berechtigungen verfügen und ob die Benutzerkonntensteuerung aktiv ist (dies könnte zu Problemen führen).\n\nDetails:\n" + ex.Message, "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Logger.Log("Couldn`t install Prinfo.NET Service. " + ex.Message, LogType.Error);
                    }
                }).Start();
        }

        /// <summary>
        /// starts or stops the service (depending on its state)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startStoppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    if (ServiceManager.PrinfoService.Status == ServiceControllerStatus.Running)
                    {
                        ServiceManager.PrinfoService.Stop();
                        MessageBox.Show("Dienst erfolgreich gestoppt", "Prinfo.NET Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        ServiceManager.PrinfoService.Start();
                        MessageBox.Show("Dienst erfolgreich gestartet", "Prinfo.NET Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dienst konnte nicht gestartet/gestoppt werden, ist der Dienst installiert?\n\nDetails:\n" + ex.Message + "\nPrüfen Sie ob die Anwendung als Administrator ausgeführt wird.", 
                        "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Log("Couldn`t stop Prinfo.NET Service. " + ex.Message, LogType.Error);
                }
            }).Start();
        }

        /// <summary>
        /// uninstall the service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dienstDeinstallierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
             DialogResult result = MessageBox.Show("Der Prinfo.NET Dienst wird deinstalliert. Soll der Vorgang fortgesetzt werden?", "Prinfo.NET Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
             if (result == DialogResult.Yes)
                new Thread(() =>
                {
                    try
                    {
                        ServiceManager.UninstallPrinfoService();
                        MessageBox.Show("Dienst erfolgreich deinstalliert", "Prinfo.NET Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dienst konnte nicht deinstalliert werden. Ist der Dienst installiert? Bitte prüfen Sie, ob Sie über administrative Berechtigungen verfügen und ob die Benutzerkonntensteuerung aktiv ist (dies könnte zu Problemen führen).\n\nDetails:\n" + ex.Message, "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }).Start();

        }
        #endregion service controlls

        #region remove printers
        /// <summary>
        /// remove a printer from the printerListView and the printerManager
        /// </summary>
        /// <param name="p"></param>
        private void RemovePrinter(Printer p)
        {
            foreach (ListViewItem item in printerListView.Items)
            {
                if ((item.Tag as Printer).Id == p.Id)
                {
                    printerListView.Items.RemoveAt(item.Index);
                    break;
                }
            }

            if (printerListView.Items.Count == 0)
            {
                ListViewItem row = new ListViewItem("");
                row.Group = emptyGroup;

                printerListView.Items.Add(row);
                printerListView.Enabled = false;
            }

            new Thread(
                () =>
                {
                    lock (printerManager.PrinterList)
                    {
                        printerManager.PrinterList.Remove(p);
                        printerManager.PrinterDatabase.DeletePrinterByID(p.Id);
                    }
                }).Start();
        }

        /// <summary>
        /// drops the printer database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alleDruckerLöschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Die Drucker Datenbank wird zurückgesetzt. Soll der Vorgang fortgesetzt werden?", "Prinfo.NET Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
           
            if (result == DialogResult.Yes)
            {
                printerManager.PrinterDatabase.Initialize();

                PaintPrinterListToListView(printerManager.LoadPrinterList());
                MessageBox.Show("Die Drucker Datenbank wurde auf den Ursprungszustand zurückgesetzt.", "Prinfo.NET Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
      
        }

        /// <summary>
        /// delete selected printers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deletePrinterSelection_Click(object sender, EventArgs e)
        {
            DeletePrinterSelection();
        }

        private void DeletePrinterSelection()
        {
            List<Printer> printers = new List<Printer>();

            foreach (ListViewItem item in printerListView.SelectedItems)
            {
                RemovePrinter(item.Tag as Printer);
            }
        }

        private void druckerAuswahlLöschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeletePrinterSelection();
        }

        private void löschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeletePrinterSelection();
        }

        private void deletePrintersBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (printerManager.PrinterList)
                foreach (var p in e.Argument as List<Printer>)
                {
                    printerManager.PrinterDatabase.DeletePrinterByID(p.Id);
                }
        }

        

        private void deletePrintersBgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PaintPrinterListToListView(printerManager.LoadPrinterList());
            löschenToolStripMenuItem.Enabled = true;
            alleDruckerLöschenToolStripMenuItem.Enabled = true;
            druckerAuswahlLöschenToolStripMenuItem.Enabled = true;
        }
        #endregion remove printers

        #region add printers

        /// <summary>
        /// add a printer to the printerListView and the printerManager
        /// </summary>
        /// <param name="printer"></param>
        private void AddPrinter(Printer printer)
        {
            if (printerListView.Enabled == false)
            {
                printerListView.Items.Clear();
                printerListView.Enabled = true;
            }

            ListViewItem row = new ListViewItem(printer.HostName);

            // print newly added printer green
            row.BackColor = Color.LightGreen;

            row.SubItems.Add(printer.Pingable ? "Ja" : "Nein");
            row.SubItems.Add(String.IsNullOrWhiteSpace(printer.LastCheck) ? "-" : printer.LastCheck);
            row.SubItems.Add(printer.Manufacturer);
            row.SubItems.Add(printer.Model);
            row.SubItems.Add(printer.Description);

            row.Tag = printer;

            if (printer.Pingable)
                row.Group = onlineGroup;
            else
                row.Group = offlineGroup;

            printerListView.Items.Add(row);

            new Thread(
                () =>
                {
                    lock (printerManager.PrinterList)
                    {
                        printerManager.PrinterList.Add(printer);
                    }
                }).Start();
        }

        /// <summary>
        /// add a printer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPrinterToolBarButton_Click(object sender, EventArgs e)
        {
            AddPrinterView view = new AddPrinterView();
            if (view.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddPrinter(view.Printer);
            }

        }

        /// <summary>
        /// add a printer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPrinterView view = new AddPrinterView();
            if (view.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddPrinter(view.Printer);
            }
        }
        #endregion add printers

        #region check printers

        private void alleDruckerÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            druckerAuswahlÜberprüfenToolStripMenuItem.Enabled = false;
            alleDruckerÜberprüfenToolStripMenuItem.Enabled = false;
            druckerAktualisierenToolStripMenuItem.Enabled = false;
            refreshListViewToolbarButton.Enabled = false;

            toolStripProgressBar1.Value = 0;
            checkPrintersBgWorker.RunWorkerAsync(printerManager.PrinterList);
        }

        private void checkPrintersBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (printerManager.PrinterList)
            {
                int count = 1;
                int printerNum = (e.Argument as List<Printer>).Count;

                foreach (Printer printer in e.Argument as List<Printer>)
                {
                    printerManager.PollPrinter(printer);
                    try
                    {
                        printerManager.PrinterDatabase.UpdatePrinter(printer);
                    }
                    catch (ApplicationException ae)
                    {
                        Logger.Log("Manager Exception, Details: " + ae.Message, LogType.Error);
                    }


                    int prozValue = (int)((double)count / printerNum * 100);

                    if (prozValue <= 100 && prozValue >= 0)
                    {
                        checkPrintersBgWorker.ReportProgress(prozValue, printer);
                    }


                    count++;
                }
            }
            printerManager.LoadPrinterList();
        }

        private void checkPrintersBgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            var printer = e.UserState as Printer;
            versionText.Text = printer.HostName + " geprüft";

            if(printer.Pingable == false)
                notifyIcon1.ShowBalloonTip(2000, "Drucker Offline", "Der Drucker " + printer.HostName + " ist offline.", ToolTipIcon.Warning);

            //
            // update the listViewItem after updating the printer - causes ugly flackering
            //
            /*
            int index = 0;
            foreach (ListViewItem item in printerListView.Items)
            {
                if (object.ReferenceEquals(printer, item.Tag))
                    setListViewRow(printer, index);

                index++;
            }*/
        }

        private void checkPrintersBgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            versionText.Text = "Aktualisiere Ansicht";
            // reload the printer list and paint it, may be better then a flackering list when the 
            // listview is updatet after a printer is checked
            PaintPrinterListToListView(printerManager.PrinterList);
            versionText.Text = "Fertig";

            alleDruckerÜberprüfenToolStripMenuItem.Enabled = true;
            druckerAuswahlÜberprüfenToolStripMenuItem.Enabled = true;
            druckerAktualisierenToolStripMenuItem.Enabled = true;
            refreshListViewToolbarButton.Enabled = true;
        }

        private void druckerAuswahlÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckPrinterSelectionWithBackgroundWorker();
        }

        private void druckerAktualisierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckPrinterSelectionWithBackgroundWorker();
        }

        private void CheckPrinterSelectionWithBackgroundWorker()
        {

            druckerAuswahlÜberprüfenToolStripMenuItem.Enabled = false;
            druckerAktualisierenToolStripMenuItem.Enabled = false;
            alleDruckerÜberprüfenToolStripMenuItem.Enabled = false;
            refreshListViewToolbarButton.Enabled = false;

            if (printerListView.SelectedItems.Count == 0)
                return;

            List<Printer> printerList = new List<Printer>();

            foreach (ListViewItem row in printerListView.SelectedItems)
                printerList.Add(row.Tag as Printer);

            toolStripProgressBar1.Value = 0;

            checkPrintersBgWorker.RunWorkerAsync(printerList);
        }

        #endregion check printers

        /// <summary>
        /// printerListView event handler launched by double click on element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printerListView_DoubleClick(object sender, EventArgs e)
        {
            if(printerListView.SelectedItems[0] != null)
                new PrinterDetailView(printerListView.SelectedItems[0].Tag as Printer).ShowDialog();
        }

        /// <summary>
        /// right click? if yes show context menu (printerRightClickContextMenu)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printerListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                printerRightClickContext.Show(MousePosition);
        }

        /// <summary>
        /// open web overview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weboberflächeÖffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printerListView.SelectedItems.Count > 0)
            {
                Printer p = printerListView.SelectedItems[0].Tag as Printer;

                if (p != null)
                    new WebView(p).Show();
            }
        }

        #region refresh list view
        private void refreshListViewBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            printerManager.LoadPrinterList();
        }

        private void refreshListViewBgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PaintPrinterListToListView(printerManager.PrinterList);
            refreshListViewToolbarButton.Enabled = true;
        }

        private void refreshListViewButton_Click(object sender, EventArgs e)
        {
            loading = new LoadingView();
            loadPrinterBgWorker.RunWorkerAsync();
            loading.ShowDialog();
        }
        #endregion

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowInfoBox();
       }

        private void loadPrinterBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            printerManager.LoadPrinterList();

        }

        private void loadPrinterBgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loading.Close();
            PaintPrinterListToListView(printerManager.PrinterList);

        }

        #region search implementation (poor)

        /// <summary>
        /// search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            
            if(Config.General.DirectSearch)
                DoSearch();
            else if (e.KeyCode == Keys.Enter)
                DoSearch();
        }

        private void DoSearch()
        {
            if (searchTextBox.Text.Length > 0)
            {
                var printers = from printer in printerManager.PrinterList where printer.HostName.Contains(searchTextBox.Text) select printer;
                PaintPrinterListToListView(printers.ToList<Printer>());
            }
            else
                PaintPrinterListToListView(printerManager.PrinterList);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            DoSearch();
        }
        #endregion

        private void dienstToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {


            dienstToolStripMenuItem.DropDownItems[0].Text = "Dienst starten/stoppen (" + ServiceState ()+ ")";
        }

        private void beendenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string ServiceState()
        {
            ServiceManager.PrinfoService.Refresh();
            string status = string.Empty;
            try
            {
                switch (ServiceManager.PrinfoService.Status)
                {
                    case ServiceControllerStatus.Paused:
                        return "Pause";
                    case ServiceControllerStatus.Running:
                        return "Läuft";
                    default:
                        return "Gestoppt";
                }
            }
            catch (Exception)
            {
                return "Nicht installiert";
            }
        }

        private void infoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowInfoBox();
        }

        private void ShowInfoBox()
        {
            PrinterManager mangi = new PrinterManager();
            mangi.LoadPrinterList();

            int supplyCount = 0;

            foreach (var printer in mangi)
            {
                supplyCount += printer.Supplies.Count;
            }

            MessageBox.Show("Status des Prinfo.NET Dienstes: " + ServiceState() + ".Derzeit sind " + mangi.PrinterList.Count + " Drucker in der Datenbank. Diese besitzen insgesamt " + supplyCount + " Verbrauchsteile. Prinfo.NET Bibliotheksversion: " + Config.Version.Description, "Prinfo.NET Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printerListView.SelectedItems[0] != null)
                new PrinterDetailView(printerListView.SelectedItems[0].Tag as Printer).ShowDialog();
        }

        private void nachDruckernImNetzwerkSuchenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SearchNetworkPrinters().ShowDialog();
            loadPrinterBgWorker.RunWorkerAsync();
            
        }

    }
}
