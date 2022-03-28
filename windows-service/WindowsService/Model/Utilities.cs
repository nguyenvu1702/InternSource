using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsService.Model
{
    public class Utilities
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Utilities));
        public static DateTime? GetDateTime(DateTime? dateTime)
        {
            return dateTime != null ? dateTime.Value.ToLocalTime() : dateTime;
        }

        public static string RegexFormat(string input)
        {
            if (input != null)
            {
                return Regex.Replace(input, @"\s+", " ").Trim();
            }
            else
                return input;
        }

        public static string GetConfigValue(string key)
        {
            return RegexFormat(ConfigurationManager.AppSettings[key].ToString());
        }

        public static bool CompareDateFromFormat(DateTime dateFrom, DateTime dateTo, string format)
        {
            return GetDateStringFormat(dateFrom, format) == GetDateStringFormat(dateTo, format);
        }

        public static string GetDateStringFormat(DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        public static SmtpClient GetSmtpClient()
        {
            string userMailServer = GetConfigValue("userMailServer");
            string passwordMailServer = GetConfigValue("passwordMailServer");
            int portMailServer = int.Parse(GetConfigValue("portMailServer"));
            string hostMailServer = GetConfigValue("hostMailServer");
            SmtpClient client = new SmtpClient(hostMailServer, portMailServer);
            NetworkCredential basicCredential = new NetworkCredential(userMailServer, passwordMailServer);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = basicCredential;
            return client;
        }

        public static void SendMail(SmtpClient client, string from, string to, string subject, string body, string cc, string bcc)
        {
            try
            {
                MailMessage message = new MailMessage(from, to, subject, body)
                {
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                if (!string.IsNullOrEmpty(cc))
                {
                    message.CC.Add(cc);
                }

                if (!string.IsNullOrEmpty(bcc))
                {
                    message.CC.Add(bcc);
                }
                client.Send(message);
            }
            catch (Exception ii)
            {
                log.Error("SendMail Error: " + ii.StackTrace);
            }
        }
        
    }
}
