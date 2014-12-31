using System.Web;

namespace FileExchange.Infrastructure.FileHelpers
{
    public interface IFileProvider
    {
        void SaveAs(HttpPostedFileBase file, string path);
        void Delete(string path);
    }
}