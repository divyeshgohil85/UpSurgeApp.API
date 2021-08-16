using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public class Mail
    {

        public IConfiguration _configuration { get; }

        public Mail(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        //void CallbackMethod(IAsyncResult ar)
        //{
        //    AsyncResult result = (AsyncResult)ar;
        //    AsyncMethodCaller caller = (AsyncMethodCaller)result.AsyncDelegate;

        //    string formatString = (string)ar.AsyncState;

        //    try
        //    {
        //        caller.EndInvoke(ar);
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = ex.Message;
        //        //throw new Exception(ex.Message);

        //    }
        //}

        private delegate StringMessageCL AsyncMethodCaller(string Subject, string Body, bool IsHtml, List<string> Emails, string LogoPath, string EmailPath);

        public async Task<StringMessageCL> SendEmail(string Subject, string Body, bool IsHtml, List<string> Emails, string LogoPath)
        {

            return await PrepareEmail(Subject, Body, IsHtml, Emails, LogoPath, "");



            //return new StringMessageCL("Email Sent", ResponseType.Success);
        }

        //public StringMessageCL SendErrorEmail(string Subject, string Body, bool IsHtml, List<string> Emails, string LogoPath)
        //{
        //    AsyncMethodCaller caller = new AsyncMethodCaller(PrepareEmail);
        //    IAsyncResult result = caller.BeginInvoke(Subject, Body, IsHtml, Emails, LogoPath, "",
        //        new AsyncCallback(CallbackMethod),
        //        "The call executed on thread {0}, with return value \"{1}\".");

        //    return new StringMessageCL("Email Sent", ResponseType.Success);
        //}



       
        private async Task<StringMessageCL> PrepareEmail(string mailSubject, string mailBody, bool IsHtml, List<string> SenderEmails, string LogoPath, string EmailPath)
        {
            string smtp_server = _configuration["MailSetting:smtp_server"];
            string smtp_port = _configuration["MailSetting:smtp_port"];
            string smtp_username = _configuration["MailSetting:mailClientUserName"];
            string smtp_password = _configuration["MailSetting:mailClientPassword"];
            string enableSSL = _configuration["MailSetting:EnableSSl"];
            string mailFrom = _configuration["MailSetting:mailFrom"];
            string displayName = _configuration["MailSetting:mailFromDisplayName"];
            string asynchEmail = _configuration["MailSetting:AsynchronousMail"];
            string sendCopyToSender = _configuration["MailSetting:sendCopyToSender"];


            if (SenderEmails.Count == 0)
            {
                return new StringMessageCL("No Recipent", ResponseType.Failed); ;
            }

            try
            {
                MailMessage Emailmsg = new MailMessage();
                Emailmsg.From = new MailAddress(mailFrom, displayName);


                //  string DebugEmail = System.Configuration.ConfigurationManager.AppSettings["DebugEmail"];


                foreach (string email in SenderEmails)
                {
                    if (!string.IsNullOrEmpty(email))
                        Emailmsg.To.Add(new MailAddress(email));
                }

                Emailmsg.Subject = mailSubject;

                Emailmsg.IsBodyHtml = IsHtml;
                Emailmsg.Priority = MailPriority.High;

                Emailmsg.Body = mailBody;

                int port_no = 587;

                int.TryParse(smtp_port.ToString(), out port_no);

                SmtpClient mailclient = new SmtpClient();
                mailclient.Host = smtp_server;
                mailclient.Port = port_no;
                mailclient.UseDefaultCredentials = false;

               // bool enable_ssl = true;

             //   bool.TryParse("false", out enable_ssl); // <add key="EnableSSl" value="true" />  Its should be false 

                mailclient.EnableSsl = true;

                if (smtp_username.Trim() != string.Empty && smtp_password.Trim() != string.Empty)
                {
                    mailclient.Credentials = new NetworkCredential(smtp_username, smtp_password);
                }


                mailclient.DeliveryMethod = SmtpDeliveryMethod.Network;
                string SendCopyToSender = sendCopyToSender;
                if (SendCopyToSender == "true")
                {
                    Emailmsg.To.Add(new MailAddress(mailFrom));
                }

                await mailclient.SendMailAsync(Emailmsg);
              


            }
            catch (Exception ex)
            {
                //  log.Error("Email Error", ex);
                if (!string.IsNullOrEmpty(EmailPath))
                {
                    if (!Directory.Exists(EmailPath))
                    {
                        Directory.CreateDirectory(EmailPath);
                    }

                    File.WriteAllText(Path.Combine(EmailPath, DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + DateTime.Now.Millisecond + "_" + Path.GetRandomFileName() + "_" + "file.html"), mailBody + "<br/>" + string.Join(", ", SenderEmails) + "<br/>" + ex.Message + "<br/>" + (ex.InnerException != null ? ex.InnerException.ToString() : ""));


                    string sender = string.Join(",", SenderEmails.ToArray());

                }
                return new StringMessageCL("Failed Exception", ResponseType.Exception); ;
            }

            return new StringMessageCL("Email Sent", ResponseType.Success);

        }

    }
}
