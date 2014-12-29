﻿using System.IO;
using AutoMapper;
using FileExchange.Areas.Admin.Models;
using FileExchange.Core.BusinessObjects;
using FileExchange.EmailSender;
using FileExchange.Helplers;
using FileExchange.Models;

namespace FileExchange.Infrastructure.Mapper
{
    public class MapperService
    {
        public static void RegisterMapping()
        {
            AutoMapper.Mapper.CreateMap<News, CreateNewsModel>();

            AutoMapper.Mapper.CreateMap<News, EditNewsModel>()
                .ForMember(m => m.ImagePath, e => e.ResolveUsing(n =>
                    string.Format("/{0}", Path.Combine(ConfigHelper.FilesFolder, n.UniqImageName).Replace(@"\", "/"))));

            AutoMapper.Mapper.CreateMap<ExchangeFile, ExchangeFile>();

            AutoMapper.Mapper.CreateMap<FileCategories, FileCategoryModel>();
             
            AutoMapper.Mapper.CreateMap<FileCategories, System.Web.Mvc.SelectListItem>()
                .ForMember(s => s.Value, m => m.MapFrom(f => f.CategoryId))
                .ForMember(s => s.Text, m => m.MapFrom(f => f.CategoryName));

            AutoMapper.Mapper.CreateMap<ExchangeFile, EditExchangeFileModel>()
               .ForMember(s => s.SelectedFileCategoryId, m => m.MapFrom(f => f.FileCategoryId));

            AutoMapper.Mapper.CreateMap<News, ViewNewsViewModel>()
                .ForMember(m => m.ImagePath, e => e.ResolveUsing(n =>
                    string.Format("/{0}", Path.Combine(ConfigHelper.FilesFolder, n.UniqImageName).Replace(@"\", "/"))));

            AutoMapper.Mapper.CreateMap<ExchangeFile, ViewExchangeFileViewModel>();


            AutoMapper.Mapper.CreateMap<UserRoles, UserRolesModel>()
                .ForMember(r => r.RoleId, r => r.MapFrom(s => s.RoleId))
                .ForMember(r => r.RoleName, r => r.MapFrom(s => s.RoleName));

            AutoMapper.Mapper.CreateMap<UserInRoles, UserRolesModel>()
               .ForMember(r => r.RoleId, r => r.MapFrom(s => s.RoleId))
               .ForMember(r => r.RoleName, r => r.MapFrom(s => s.Role.RoleName));

            AutoMapper.Mapper.CreateMap<UserProfile, EditUserViewModel>()
                .ForMember(u => u.SelectedUserRoles, u => u.MapFrom(s => s.Roles));
           

            AutoMapper.Mapper.CreateMap<FileComments, FileCommentsViewModel>()
                .ForMember(s => s.UserName, m => m.MapFrom(c => c.File.User.UserName));

            AutoMapper.Mapper.CreateMap<GlobalSetting, GlobalSettingViewModel>();
        }

    }
}