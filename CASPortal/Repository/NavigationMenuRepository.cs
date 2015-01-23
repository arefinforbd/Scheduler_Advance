using CASPortal.CASWCFService;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Repository
{
    public class NavigationMenuRepository
    {
        public List<NavigationMenu> GetNavigationMenu(string rootMenu)
        {
            NavigationMenuParser parser = new NavigationMenuParser();
            List<NavigationMenu> navMenus = new List<NavigationMenu>();

            navMenus = parser.GetNavigationMenu(rootMenu);

            return navMenus;
        }
    }
}