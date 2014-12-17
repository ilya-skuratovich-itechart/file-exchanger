using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Models;
using WebMatrix.WebData;

namespace FileExchange.Controllers
{
    public partial class FileController : Controller
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private IFileNotificationSubscriberService _fileFileNotificationSubscriberService { get; set; }
        private IFileCategoriesService _fileCategoriesService { get; set; }
        private IExchangeFileService _exchangeFileService { get; set; }
        public FileController(IUnitOfWork unitOfWork, IFileCategoriesService fileCategoriesService,
            IFileNotificationSubscriberService fileFileNotificationSubscriberService,
            IExchangeFileService exchangeFileService)
        {
            _unitOfWork = unitOfWork;
            _fileCategoriesService = fileCategoriesService;
            _exchangeFileService = exchangeFileService;
            _fileFileNotificationSubscriberService = fileFileNotificationSubscriberService;

        }
        public virtual ActionResult FileSections()
        {
            List<FileCategoryModel> fileCategories =
                AutoMapper.Mapper.Map<List<FileCategoryModel>>(_fileCategoriesService.GetAll());
            return View(fileCategories);
        }

        [Authorize]
        public virtual ActionResult UserFiles()
        {
            return View();
        }

        [Authorize]
        public virtual JsonResult UserFilesTableFilter(jQueryDataTableParamModel param)
        {

            try
            {
                int totalRecords;
                int userId = (int)WebSecurity.CurrentUserId;;
                string sortColumn = param.sColumns.Split(',')[param.SortingColumnNumber];
                List<ExchangeFile> userFiles = _exchangeFileService.GetUserFilesPaged(userId, param.iDisplayStart, param.iDisplayLength,
                    param.sSortDir_0 == "desc", sortColumn, out totalRecords);

                var resultUserFiles = from val in userFiles
                                    select new[]
                                    {
                                        string.Empty,
                                        Convert.ToString(val.FileId),
                                        Convert.ToString(val.OrigFileName),
                                        Convert.ToString(val.CreateDate),
                                        Convert.ToString(val.ModifyDate)
                                    };
                var result = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = resultUserFiles.Count(),
                    aaData = resultUserFiles

                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}
