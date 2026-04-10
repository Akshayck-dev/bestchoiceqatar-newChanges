using BestChoiceQatar.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestChoiceQatar.Core.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AuthorizeAccessAttribute : AuthorizeAttribute
    {
        public bool RequiresSupreAdmin { get; set; }

        private bool IsNoPermission { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            AppUser _user = httpContext.Session["Geri_User"] as AppUser;

            if (_user == null)
            {
                return false;
            }

            if (RequiresSupreAdmin)
            {
                IsNoPermission = true;
                return _user.IsSuperAdmin;
            }
            else
            {
                return true;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = this.IsNoPermission ? (new LoginCheckController()).RedirectToDashBoard() : (new LoginCheckController()).RedirectToLogin();
        }
    }

    public sealed class LoginCheckController : Controller
    {
        public ActionResult RedirectToLogin()
        {
            return Redirect("/backend/default/index");
        }

        public ActionResult RedirectToDashBoard()
        {
            return Redirect("/backend/default/dashboard");
        }
    }

}