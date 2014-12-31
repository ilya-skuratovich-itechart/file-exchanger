using System.Web;

namespace FileExchange.Infrastructure.FileHelpers
{
    public class FileProvider:IFileProvider
    {
        public void SaveAs(HttpPostedFileBase file,string path)
        {
           file.SaveAs(path);
        }

        public void Delete(string path)
        {
            System.IO.File.Delete(path);
        }
    }
}