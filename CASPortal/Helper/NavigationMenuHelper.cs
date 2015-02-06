using CASPortal.CASWCFService;
using CASPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CASPortal.Helper
{
    public class NavigationMenuHelper
    {
        public string GetNavigationMenuString(List<NavigationMenu> navMenus, string rootMenu)
        {
            string[] menuDesc = new string[2];
            StringBuilder sb = new StringBuilder();

            foreach (NavigationMenu navMenu in navMenus)
            {
                if (navMenu.MenuType.Equals(true) && navMenu.MenuName.ToLower().Equals(rootMenu.ToLower())){

                    menuDesc = navMenu.MenuDescription.Split(',');
                    try
                    {
                        sb.Append("<li><a id=" + menuDesc[0] + " href='" + BaseHelper.GetSiteUrl() + navMenu.MenuCalls + "'>");
                        sb.Append("<i class='" + menuDesc[1] + "'></i>");
                    }
                    catch (Exception ex){ }
                    
                    sb.Append(" " + navMenu.MenuTitle + "</a></li>");
                }
                else
                {
                    if (navMenu.MenuType.Equals(false)){
                        menuDesc = navMenu.MenuDescription.Split(',');

                        try
                        {
                            sb.Append("<li><a id=" + menuDesc[0] + " href='#'>");
                            sb.Append("<i class='" + menuDesc[1] + "'></i>");
                        }
                        catch (Exception ex) { }

                        sb.Append(" " + navMenu.MenuTitle);
                        sb.Append("<span class='fa arrow'></span></a>");
                        sb.Append(GetNavigationMenuChild(navMenus, navMenu.MenuCalls));
                    }
                }
            }

            return sb.ToString();
        }

        public string GetNavigationMenuChild(List<NavigationMenu> navMenus, string parentmenu)
        {
            StringBuilder sb = new StringBuilder("<ul class='ulSubMenu nav nav-second-level collapse'>");

            foreach (NavigationMenu navMenu in navMenus.Where(m => m.MenuName.Equals(parentmenu)))
                sb.Append("<li><a id=" + navMenu.MenuDescription + " href='" + BaseHelper.GetSiteUrl() + navMenu.MenuCalls + "'>" + navMenu.MenuTitle + "</a></li>");

            sb.Append("</ul>");

            return sb.ToString();
        }

        public bool CheckMenuPermission(string URL)
        {
            try
            {
                List<NavigationMenu> navMenus = new List<NavigationMenu>();
                NavigationMenuRepository repo = new NavigationMenuRepository();

                if (HttpContext.Current.Session["NavigationMenu"] == null)
                {
                    HttpContext.Current.Session["NavigationMenu"] = repo.GetNavigationMenu("WebAccess");
                    navMenus = (List<NavigationMenu>)HttpContext.Current.Session["NavigationMenu"];
                }
                else
                    navMenus = (List<NavigationMenu>)HttpContext.Current.Session["NavigationMenu"];

                var menu = navMenus.Where(m => m.MenuCalls.Equals(URL)).SingleOrDefault();

                if (menu == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}