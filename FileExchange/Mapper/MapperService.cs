using System.IO;
using FileExchange.Core.BusinessObjects;
using FileExchange.Helplers;
using FileExchange.Models;

namespace FileExchange.Mapper
{
    public class MapperService
    {
        public static void RegisterMapping()
        {
            AutoMapper.Mapper.CreateMap<News, CreateNewsModel>();
            AutoMapper.Mapper.CreateMap<News, EditNewsModel>()
                .ForMember(m=>m.ImagePath,e=>e.ResolveUsing(n=>
                  string.Format("/{0}",Path.Combine(ConfigHelper.FilesFolder,n.UniqImageName).Replace(@"\","/"))));
            AutoMapper.Mapper.CreateMap<FileCategories, FileCategoryModel>();
        }
    }
}