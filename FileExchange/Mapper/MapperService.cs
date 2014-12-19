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
                .ForMember(m => m.ImagePath, e => e.ResolveUsing(n =>
                    string.Format("/{0}", Path.Combine(ConfigHelper.FilesFolder, n.UniqImageName).Replace(@"\", "/"))));

            AutoMapper.Mapper.CreateMap<FileCategories, FileCategoryModel>();

            AutoMapper.Mapper.CreateMap<FileCategories, System.Web.Mvc.SelectListItem>()
                .ForMember(s => s.Value, m => m.MapFrom(f => f.CategoryId))
                .ForMember(s => s.Text, m => m.MapFrom(f => f.CategoryName));

            AutoMapper.Mapper.CreateMap<ExchangeFile, EditExchangeFileModel>()
               .ForMember(s => s.SelectedFileCategoryId, m => m.MapFrom(f => f.FileCategoryId));

            AutoMapper.Mapper.CreateMap<News, ViewNewsViewModel>()
                .ForMember(m => m.ImagePath, e => e.ResolveUsing(n =>
                    string.Format("/{0}", Path.Combine(ConfigHelper.FilesFolder, n.UniqImageName).Replace(@"\", "/")))); ;
        }

    }
}