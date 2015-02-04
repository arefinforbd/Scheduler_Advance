using CASPortal.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CASPortal.Controllers
{
    public class RedirectingActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NavigationMenuHelper navHelper = new NavigationMenuHelper();
            bool menuStatus = navHelper.CheckMenuPermission(HttpContext.Current.Request.Url.AbsolutePath);

            base.OnActionExecuting(filterContext);

            if (menuStatus == false)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "CustomerInformation",
                    action = "WelcomeMessage"
                }));
            }
        }
    }
}