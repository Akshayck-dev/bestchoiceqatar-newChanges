using BestChoiceQatar.Controllers;
using BestChoiceQatar.Core;
using BestChoiceQatar.Core.Security;
using BestChoiceQatar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    [AuthorizeAccess]
    public class BackendBaseController : Controller
    {
        protected BestChoiceQatarEntities db = new BestChoiceQatarEntities();
        public static byte[] GetAuthstring(string email, string password)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(email.ToLower() + password);
                byte[] _hash;
                SHA256 shaM = new SHA256Managed();
                _hash = shaM.ComputeHash(data);
                return _hash;
            }
            catch (Exception exception)
            {
                Appmanager.SendExceptionLogEmail(exception);
                return null;
            }
        }

        public static DateTime Now
        {
            get
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
                return localTime;
            }
        }

        public static string AppURL
        {
            get
            {
                try
                {
                    return $"http://{ System.Web.HttpContext.Current.Request.Url.Authority}/";
                }
                catch (Exception exception)
                {
                    Appmanager.SendExceptionLogEmail(exception);

                    return null;
                }
            }
        }

        public static void SendEmail(string subject, string to, string content)
        {
            try
            {

                MailMessage _msg = new MailMessage();

                _msg.To.Add(to);
                _msg.From = new MailAddress("no-reply@bcmconsultancy.com", "BCM Consultancy");
                _msg.Subject = subject;

                string _template = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Views/Default/Template.html"));

                content = _template.Replace("{mail_body}", content).Replace("{url}", AppURL + "/areas/backend/assets/images/logo.png");

                _msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html));

                using (SmtpClient _client = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587)))
                {
                    _client.Credentials = new NetworkCredential("memail", "1 New Credential");
                    _client.Send(_msg);
                }
            }
            catch (Exception exception)
            {
               Appmanager.SendExceptionLogEmail(exception);
                exception.ToString();
            }
        }
    }
}