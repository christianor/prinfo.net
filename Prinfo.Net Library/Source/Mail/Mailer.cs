using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using System.Threading;


namespace com.monitoring.prinfo
{
    /// <summary>
    /// Einfache Mailfunktionalität
    /// <remarks>
    /// Die Konfigurationsdaten müssen geladen werden bevor eine Mail versendet wird,
    /// da von ihnen innerhalb der Klasse gebrauch gemacht wird
    /// </remarks>
    /// </summary>
    public static class Mailer
    {
        /// <summary>
        /// Versendet eine Email über SMTP, verwendet dabei die Daten aus der Konfigurationsdatei, 
        /// sie muss daher vorher durch <code>Config.Load()</code> geladen werden
        /// </summary>
        /// <param name="to">Empfänger</param>
        /// <param name="subject">Betreff</param>
        /// <param name="body">Der Rumpf der Nachricht</param>
        /// <param name="attachment">Der Anhang der Nachricht</param>
        public static void SendMailOverSmtp(string to, string subject, string body, Attachment attachment = null)
        {

            to = to.Replace(';', ',');

            using (MailMessage mailMsg = new MailMessage(Config.Mail.From, to))
            {
                mailMsg.Body = body;
                mailMsg.Subject = subject;
                if (attachment != null) mailMsg.Attachments.Add(attachment);
                

                //SMTP client
                using (SmtpClient smtpClient = new SmtpClient(Config.Mail.SmtpServer))
                {
                    // set smtp-client with basicAuthentication
                    smtpClient.UseDefaultCredentials = false;

                    smtpClient.Credentials = new NetworkCredential(Config.Mail.Username, Config.Mail.Password);
                    smtpClient.EnableSsl = Config.Mail.UseSsl;
                    smtpClient.Port = Config.Mail.Port;

                    smtpClient.Send(mailMsg);
                }
            }
        }
    }
}