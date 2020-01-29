using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;



namespace ChemiFriend.Utility
{
    public class MessageServices
    {
        //public static bool SendBookingEmail(Booking Booking, string bookingTable)
        //{

        //    if (Booking != null)
        //    {
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/BookingTemplate.html")))
        //        {
        //            body = reader.ReadToEnd();
        //        }
        //        body = body.Replace("{Name}", Booking.Name);
        //        body = body.Replace("{BookingTemplate}", bookingTable);

        //        string AdminEmail = System.Configuration.ConfigurationSettings.AppSettings["Email"].ToString();
        //        EmailSender email = new EmailSender();
        //        return email.SendEmail(Booking.Email, AdminEmail, "Cottage Booking by-" + Booking.Name, body);

        //    }
        //    return false;





        //}

      

    }

}