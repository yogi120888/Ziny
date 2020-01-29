using System;
using System.IO;
using System.Net.Mail;


namespace ChemiFriend.Utility
{
    public class EmailSender
    {
       
        //public bool SendEmailWithAcctment(string Fromemail, string FromName, string Toemail, string bcc, string Subject, String body, HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.ReplyTo = new MailAddress(Fromemail);
        //        //string To = ConfigurationManager.AppSettings["EnquiryEmail"];
        //        //mail.From = new MailAddress(To, FromName);
        //        mail.To.Add(Toemail);
        //        if (!string.IsNullOrEmpty(bcc))
        //            mail.Bcc.Add(bcc);
        //        mail.Subject = Subject;
        //        mail.Body = body;
        //        mail.IsBodyHtml = true;

        //        if (file != null)
        //        {
        //            mail.Attachments.Add(new Attachment(file.InputStream, file.FileName));
        //        }
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}




     
        public bool SendEmail(string Fromemail, string FromName, string Toemail, string bcc, string Subject, String body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.ReplyTo = new MailAddress(Fromemail);

                //string To = ConfigurationManager.AppSettings["EnquiryEmail"];
                //mail.From = new MailAddress(To, FromName);
             //   mail.From = new MailAddress(Fromemail, FromName);
                mail.To.Add(Toemail);
                if (!string.IsNullOrEmpty(bcc))
                    mail.Bcc.Add(bcc);
                mail.Subject = Subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                //var message = new MailMessage();
                //message.From = new MailAddress(Fromemail);
                //message.To.Add(Toemail);

                //message.Subject = Subject;
                //message.Body = body;
                //message.IsBodyHtml = true;

                //using (var smtpClient = new SmtpClient())
                //{
                //    smtpClient.UseDefaultCredentials = false;
                //    smtpClient.Host = "mail.crystalhues.com";
                //    smtpClient.Port = 587;
                //    smtpClient.EnableSsl =false;
                //    smtpClient.Credentials = new System.Net.NetworkCredential("om@crystalhues.com", "oCHL@2232s");
                //    smtpClient.Send(message);
                //}
                //SmtpClient smtp = new SmtpClient
                //{
                //    Host = "mail.crystalhues.com", // smtp server address here…
                //    Port = 587,

                //    EnableSsl = false,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Credentials = new System.Net.NetworkCredential("om@crystalhues.com", "oCHL@2232s"),
                //    Timeout = 30000,
                //};
                //MailMessage message = new MailMessage(Fromemail, Toemail, Subject, body);
                //smtp.Send(message);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
      
        public bool SendEmail(string Toemail, string bcc, string Subject, String body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(Toemail);
                if (!string.IsNullOrEmpty(bcc))
                    mail.Bcc.Add(bcc);
                mail.Subject = Subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}