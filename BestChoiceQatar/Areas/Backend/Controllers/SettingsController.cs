using BestChoiceQatar.Core;
using BestChoiceQatar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    public class SettingsController : BackendBaseController
    {        
        public async Task<ActionResult> Index()
        {
            var model = await db.Settings.FirstOrDefaultAsync();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(Setting model)
        {
            if (ModelState.IsValid)
            {
                Setting _settings = await db.Settings.FirstOrDefaultAsync();
                if (_settings != null)
                {
                    _settings.IntroText = model.IntroText;
                    _settings.Location = model.Location;
                    _settings.Email = model.Email;
                    _settings.Mobile1 = model.Mobile1;
                    _settings.Mobile2 = model.Mobile2;
                    _settings.Whatsup = model.Whatsup;
                    _settings.Instagram = model.Instagram;
                    _settings.Facebook = model.Facebook;
                    _settings.Twitter = model.Twitter;
                    _settings.LinkedIn = model.LinkedIn;
                    _settings.AboutText = model.AboutText;
                    _settings.HompageIntroText = model.HompageIntroText;
                    _settings.Twitter = model.Twitter;
                    _settings.Lastupdatedon = Appmanager.Now;

                    await db.SaveChangesAsync();
                }
                else
                {
                    _settings = new Setting();

                    _settings.IntroText = model.IntroText;
                    _settings.Email = model.Email;
                    _settings.Location = model.Location;
                    _settings.Mobile1 = model.Mobile1;
                    _settings.Mobile2 = model.Mobile2;
                    _settings.Whatsup = model.Whatsup;
                    _settings.Instagram = model.Instagram;
                    _settings.Facebook = model.Facebook;
                    _settings.Twitter = model.Twitter;
                    _settings.LinkedIn = model.LinkedIn;
                    _settings.AboutText = model.AboutText;
                    _settings.HompageIntroText = model.HompageIntroText;
                    _settings.Twitter = model.Twitter;
                    _settings.Lastupdatedon = Appmanager.Now;

                    db.Settings.Add(_settings);
                    await db.SaveChangesAsync();
                }
                TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Settings has been saved successfully.");
                return RedirectToAction("Index");
            }
            TempData["Notification"] = new Core.Entities.Common.Notification("Error", "One or more fields are missing or contains invalid value. Please try again later.");
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}