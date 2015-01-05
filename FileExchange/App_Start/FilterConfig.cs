using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FileExchange.Infrastructure.CustomAttirbutes;
using FileExchange.Infrastructure.Filtres;

namespace FileExchange
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalHandleErrorAttribute());
            Dictionary<string, List<string>> excludeBannedUserDic = new Dictionary<string, List<string>>()
            {
                {MVC.Account.Name, new List<string>() {MVC.Account.ActionNames.UserBanned}},
            };
            filters.Add(new BannedUserFilter(excludeBannedUserDic));
        }
    }
}