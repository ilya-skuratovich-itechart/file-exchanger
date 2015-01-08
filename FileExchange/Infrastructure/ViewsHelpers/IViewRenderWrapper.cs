using System.Web.Mvc;

namespace FileExchange.Infrastructure.ViewsHelpers
{
    public interface IViewRenderWrapper
    {
        string RenderViewToString(Controller controller, string viewName, object model);
        string RenderViewToString(string viewPath, string layoutPath, object model);
    }
}