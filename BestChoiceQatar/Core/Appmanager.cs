using BestChoiceQatar.Core.Entities;
using BestChoiceQatar.Core.Entities.Charts;
using BestChoiceQatar.Core.Entities.Common;
using BestChoiceQatar.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace BestChoiceQatar.Core
{
    public class Appmanager
    {

        public static BestChoiceQatarEntities db = new BestChoiceQatarEntities();

        public static AppUser User
        {
            get
            {
                return HttpContext.Current.Session["Geri_User"] as AppUser;
            }
        }

        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        //send Application emails
        public static void SendEmail(string subject, string content)
        {
            try
            {
                MailMessage _msg = new MailMessage();
                //_msg.To.Add("princetomy12@gmail.com");
                 _msg.To.Add("info@bestchoiceqatar.net");
                _msg.From = new MailAddress("info@bestchoiceqatar.net", "BestchoiceQatar Team");
                _msg.CC.Add("joby@bestchoiceqatar.net");
                _msg.CC.Add("bestchoice.limo@gmail.com");
                _msg.Bcc.Add("princetomy12@gmail.com");
                _msg.Bcc.Add("okb.basil@gmail.com");
                _msg.Bcc.Add("praveentomy1234@gmail.com");
                _msg.Subject = subject;

                string _template = File.ReadAllText(HostingEnvironment.MapPath("/Views/Home/Template.html"));
                content = _template.Replace("{mail_body}", content);

                _msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html));

                //  _msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html));

                //if (attachments != null)
                //{
                //    foreach (string _attachment in attachments)
                //    {
                //        if (File.Exists(_attachment))
                //        {
                //            _msg.Attachments.Add(new Attachment(_attachment));
                //        }
                //    }
                //}
                using (SmtpClient _client = new SmtpClient())
                {
                    // _client.Credentials = new NetworkCredential("princetomy12@gmail.com", "rlfknifghykzhkzp");
                    //  _client.Credentials = new NetworkCredential("ginternationalnoreply@gmail.com", "rwxmrcehslhjebiy");

                    //_client.Host = "smtp.gmail.com";
                    //_client.Port = 587;
                    // _client.Host = "relay-hosting.secureserver.net";
                    //  _client.Port = 25;
                    // _client.EnableSsl = false;         
                    _client.Send(_msg);
                }
            }
            catch (Exception ex)
            {
                //Log logger = new Log();
                //logger.ExceptionMessage = ex.Message.ToString();
                //logger.ExceptionType = ex.GetType().Name.ToString();
                //logger.ExceptionSource = ex.StackTrace.ToString();
                //// logger.ExceptionUrl = context.Current.Request.Url.ToString();
                //logger.Logdate = DateTime.Now.Date;
                //db.Logs.Add(logger);
               // db.SaveChangesAsync();

              //  SendExceptionLogEmail(ex);
                ex.Message.ToString();
            }
        }




        /// <summary>
        /// For trace error log from email
        /// </summary>
        /// <param name="log"></param>

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
              //  Appmanager.SendExceptionLogEmail(exception);
                return null;
            }
        }
        public enum userLogin
        {
            Admin = 1,
            HR =2,
            Employee = 3,
            ProjectManager = 4,
            TeamLead = 5,
           
        }

        public static void SendExceptionLogEmail(Exception log)
        {
            try
            {
                if (log == null)
                {
                    return;
                }

                HttpException _httpEx = log as HttpException;

                if (_httpEx != null && _httpEx.GetHttpCode() == 404)
                {
                    return;
                }

                string _body = JsonConvert.SerializeObject(log, Formatting.Indented);

                _body = _body.Replace("\r\n", "<br />");

                if (User != null)
                {
                    _body = "User: " + JsonConvert.SerializeObject(User, Formatting.Indented) + "<br />" + _body;
                }

                if (HttpContext.Current != null)
                {
                    _body = "URL: " + HttpContext.Current.Request.Url + "<br />" + "Prev URL: " + HttpContext.Current.Request.UrlReferrer + "<br />" + _body;

                    if (HttpContext.Current.Request.Form.Count > 0)
                    {
                        _body += $"<br />------------- POST DATA -------------<br />";

                        for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
                        {
                            _body += $"{HttpContext.Current.Request.Form.Keys[i]} ==> {string.Join("|", HttpContext.Current.Request.Form.GetValues(HttpContext.Current.Request.Form.Keys[i]))} <br />";
                            i++;
                        }
                    }

                    if (HttpContext.Current.Request.QueryString.Count > 0)
                    {
                        _body += $"<br />------------- GET DATA -------------<br />";

                        for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
                        {
                            _body += $"{HttpContext.Current.Request.QueryString.Keys[i]} ==> {string.Join("|", HttpContext.Current.Request.QueryString.GetValues(HttpContext.Current.Request.QueryString.Keys[i]))} <br />";
                            i++;
                        }
                    }

                    if (HttpContext.Current.Request.Cookies.Count > 0)
                    {
                        _body += $"<br />------------- COOKIES -------------<br />";

                        for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
                        {
                            _body += $"{HttpContext.Current.Request.Cookies.Keys[i]} ==> {HttpContext.Current.Request.Cookies[HttpContext.Current.Request.Cookies.Keys[i]].Value} <br />";
                            i++;
                        }
                    }
                }
                MailMessage _msg = new MailMessage();

                _msg.To.Add("princetomy12@gmail.com");
                _msg.From = new MailAddress("no-reply@NavaloorDevelopers.com", "GERI International Consultancy");
                _msg.CC.Add("praveentomy1234@gmail.com");
                _msg.Subject = _body;
                using (SmtpClient _client = new SmtpClient())
                {
                    _client.Send(_msg);
                }
            }
            catch
            {

            }
        }

        public static async Task<DashboardData> GetDashboardData()
        {
            DashboardData _result = new DashboardData();

            try
            {
                var db = new BestChoiceQatarEntities();
                DateTime _lastMonth = DateTime.Now.Date.AddDays(-30);

                _result.Lastmonth = _lastMonth.ToString("dd/MM/yyyy");
                DateTime end = DateTime.Now.Date.AddDays(1);
                DateTime start = DateTime.Now.Date.AddDays(-30);

                var _chartItems = Enumerable.Range(0, end.Subtract(start).Days)
                 .Select(o => new ChartItem { x = start.AddDays(o).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds })
                 .ToList();

                var _query = from t in db.Enquiries
                             where t.IsDataActive/* && t.CreatedOn >= start && t.CreatedOn < end*/
                             select new
                             {
                                 t.CreatedOn,
                                 t.Message,
                                 t.ID,
                                 t.FullName,
                                 t.Email,
                                 t.ContactNumber,
                                 t.Subject
                             };

                var _lineChartData = (await _query.ToListAsync()).GroupBy(i => i.CreatedOn.Date).Select(g => new ChartItem
                {
                    x = g.Key.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                    y = g.Count(),
                    z = g.Count()
                }).ToList();

                _chartItems = _chartItems.Select(i => new ChartItem
                {
                    x = i.x,
                    y = _lineChartData.Any(a => a.x == i.x) ? _lineChartData.FirstOrDefault(a => a.x == i.x).y : 0
                }).ToList();
                //_result.TopHospitals = (await db.Hospitals.Where(h => h.IsDataActive).Select(i => new DonutChartItem
                //{
                //    label = i.Name,
                //    value = db.Registrations.Where(r => r.HospitalLookedFor.Contains(i.ID.ToString()) && r.IsDataActive).Count()
                //}).ToListAsync());

                //_result.TopHospitals = _result.TopHospitals.OrderByDescending(o => o.value).Take(5).ToList();
                _result.OrderSummary = _chartItems;
                _result.RecentAppointments = (await _query.ToListAsync()).OrderByDescending(r => r.ID).Take(6).Select(r => new RecentAppointments
                {
                    Id = r.ID,
                    Name = r.FullName,
                    Subject = r.Subject,
                    Email = r.Email
                }).ToList();

                _result.count = db.Enquiries.Where(x => x.IsDataActive).Count();
            }
            catch (Exception ex)
            {
                SendExceptionLogEmail(ex);
            }

            return _result;
        }

    }
}