
using System.Web.Mvc;

namespace BestChoiceQatar.Areas.Backend
{
    public class BackendAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Backend";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Backend_default",
                "backend/{controller}/{action}/{id}",
                new { controller = "default", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}