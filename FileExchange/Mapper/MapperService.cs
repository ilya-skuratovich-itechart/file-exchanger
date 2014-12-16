using FileExchange.Core.BusinessObjects;
using FileExchange.Models;

namespace FileExchange.Mapper
{
    public class MapperService
    {
        public static void RegisterMapping()
        {
            AutoMapper.Mapper.CreateMap<News, NewsModel>();
        }
    }
}