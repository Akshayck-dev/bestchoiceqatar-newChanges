using BestChoiceQatar.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    [AuthorizeAccess]
    public class EmailNotificationsController : BackendBaseController
    {
        // GET: Backend/EmailNotifications
        public ActionResult Index()
        {
            return View();
        }
    }
}