using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace com.monitoring.prinfo.manager
{
    public partial class SettingsView : Form
    {
        public SettingsView()
        {
            InitializeComponent();
            LoadConfig();

            textBox1.Text = Config.Mail.From;
            textBox2.Text = Config.Mail.SmtpServer;
            textBox3.Text = Config.Mail.Username;
            textBox4.Text = Config.Mail.Port.ToString();
            textBox5.Text = Config.Mail.Password;

            if (Config.Mail.UseSsl == true)
                checkBox1.Checked = true;
        }

        private void LoadConfig()
        {
            try
            {
                Config.Load();
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error loading the configuration file Details: {0}", ex.Message), LogType.Error);
                MessageBox.Show(String.Format("Fehler beim Laden der Konfigurationsdatei. Details: {0}", ex.Message), "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            SaveEmailData();
        }

        private void SaveEmailData()
        {
            Config.Mail.From = textBox1.Text;
            Config.Mail.SmtpServer = textBox2.Text;
            Config.Mail.Username = textBox3.Text;

            Config.Mail.Password = textBox5.Text;

            Config.Mail.UseSsl = checkBox1.Checked;

            try
            {
                Config.Mail.Port = int.Parse(textBox4.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Der Wert von \"Port\" muss einem ganzzahligen Wert entsprechen, bitte überprüfen sie ihre Eingaben.", "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Config.Save();
        }



        private void tabPage2_Enter(object sender, EventArgs e)
        {
            checkBox3.Checked = Config.Notifications.AlertIfPrinterOffline;
            checkBox4.Checked = Config.Notifications.AlertIfSupplyLevelCritical;

            textBox7.Text = Config.Notifications.CriticalSupplyLevel.ToString();
            textBox8.Text = Config.Notifications.SendTo;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(textBox7.Text);
                if (value >= 100)
                    throw new Exception();

                Config.Notifications.CriticalSupplyLevel = value;
            }
            catch (Exception)
            {
                MessageBox.Show("Der kritische Schwellwert muss einem ganzzahligen Wert kleiner oder gleich 100 entsprechen, bitte überprüfen sie ihre Eingaben.", "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Config.Notifications.AlertIfPrinterOffline = checkBox3.Checked;
            Config.Notifications.AlertIfSupplyLevelCritical = checkBox4.Checked;
            Config.Notifications.SendTo = textBox8.Text;

            Config.Save();
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            checkBox5.Checked = Config.Reporting.SimpleMailReport;
            textBox9.Text = Config.Reporting.SendReportTo;
            textBox10.Text = Config.Reporting.TimeToStart.ToString("HH:mm:ss");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveReportingData();
        }

        private void SaveReportingData()
        {
            Config.Reporting.SimpleMailReport = checkBox5.Checked;
            Config.Reporting.SendReportTo = textBox9.Text;

            try
            {
                Config.Reporting.TimeToStart = DateTime.Parse(textBox10.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Bitte Tragen sie eine Uhrzeit für \"Report Uhrzeit\" ein.", "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Config.Save();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SaveReportingData();
                ServiceManager.RestartService();
                MessageBox.Show("Der Dienst wurde neu gestartet.", "Prinfo.NET Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der Dienst konnte nicht neu gestartet werden. Details: " + ex.Message, "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help.ShowHelp(this, "http://localhost");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new DetailNotificationSettings().ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveEmailData();
            new Thread(() =>
            {
                try
                {
                    Mailer.SendMailOverSmtp(string.IsNullOrEmpty(textBox11.Text) ? Config.Mail.From : textBox11.Text, "x+x Prinfo.NET Testmail x+x", "xx++x++xx++xx");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Beim Mailversand sind Fehler aufgetreten. Details: " + ex.Message, "Prinfo.NET Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }).Start();
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            checkBox6.Checked = Config.General.DirectSearch;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Config.General.DirectSearch = checkBox6.Checked;
            Config.Save();
        }

        private void SettingsView_Load(object sender, EventArgs e)
        {

        }
    }
}
