using System.Web;
using System.Web.Mvc;
using FileExchange.Infrastructure.CustomAttirbutes;

namespace FileExchange
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalHandleErrorAttribute());
        }
    }
}