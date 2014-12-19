using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Helplers;
using FileExchange.ModelBinders;
using FileExchange.Models;
using FileExchange.Models.DataTable;
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
        [HttpGet]
        public virtual ActionResult AddUserFile()
        {
            IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(_fileCategoriesService.GetAll());
            var fileModel = new CreateExchangeFileModel(fileCategoriesListItems);
            return View(fileModel);
        }

        [Authorize]
        public virtual ActionResult AddUserFile(CreateExchangeFileModel userFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                        AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(
                            _fileCategoriesService.GetAll());
                    userFile.FileCategories = fileCategoriesListItems;
                    return View(userFile);
                }

                using (_unitOfWork.BeginTransaction())
                {
                    int userId = (int)WebSecurity.CurrentUserId;
                    string uniqFileName = Guid.NewGuid().ToString() + Path.GetExtension(userFile.File.FileName);
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        ExchangeFile exchangeFile = _exchangeFileService.Add(userId, userFile.SelectedFileCategoryId,
                            userFile.Description, uniqFileName,
                            userFile.File.FileName, userFile.Tags, userFile.DenyAll,
                            userFile.AllowViewAnonymousUsers);

                        var path =
                            System.Web.HttpContext.Current.Server.MapPath(string.Format("~/{0}",
                                Path.Combine(ConfigHelper.FilesFolder, uniqFileName)));
                        userFile.File.SaveAs(path);
                        _unitOfWork.SaveChanges();
                        transaction.Complete();
                    }
                }
                return RedirectToAction(MVC.File.ActionNames.UserFiles);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        public virtual ActionResult EditUserFile(int fileId)
        {
            int userId = WebSecurity.CurrentUserId;

            IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(_fileCategoriesService.GetAll());
            EditExchangeFileModel userFile =
                AutoMapper.Mapper.Map<EditExchangeFileModel>(_exchangeFileService.GetUserFile(fileId, userId));
            userFile.FileCategories = fileCategoriesListItems;
            return View(userFile);
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult EditUserFile(EditExchangeFileModel userFile)
        {
            int userId = WebSecurity.CurrentUserId;
            if (ModelState.IsValid)
            {
                _exchangeFileService.Update(userId, userFile.FileId, userFile.SelectedFileCategoryId,
                    userFile.Description, userFile.Tags, userFile.DenyAll, userFile.AllowViewAnonymousUsers);
                _unitOfWork.SaveChanges();
                return RedirectToAction(MVC.File.ActionNames.UserFiles);
            }

            IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(_fileCategoriesService.GetAll());
            userFile.FileCategories = fileCategoriesListItems;
            return View(userFile);

        }

        [Authorize]
        public virtual ActionResult DeleteUserFile(int fileId)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    int userId = (int)WebSecurity.CurrentUserId;
                    ExchangeFile userFile = _exchangeFileService.GetUserFile(fileId, userId);
                    if (userFile == null)
                        throw new ArgumentException("");
                    _exchangeFileService.RemoveUserFile(userId, fileId);
                    var filePath =
                        System.Web.HttpContext.Current.Server.MapPath(string.Format("~/{0}",
                            Path.Combine(ConfigHelper.FilesFolder, userFile.UniqFileName)));
                    System.IO.File.Delete(filePath);
                    _unitOfWork.SaveChanges();
                    transaction.Complete();
                }
                return View(MVC.File.ActionNames.UserFiles);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }


        public virtual ActionResult ViewCategoryFiles(int categoryId)
        {

            return View(categoryId);
        }

        public  ActionResult ViewFile(int fileId)
        {
            

        }

        [Authorize]
        [HttpPost]
        public virtual JsonResult ViewCategoryFilesTableFilter([ModelBinder(typeof(DataTablesBinder))] DefaultDataTablesRequest param)
        {
            try
            { 
                int totalRecords = 0;
                IEnumerable<ExchangeFile> categoryFiles = _exchangeFileService.GetFilteredCategoryFilesPaged(param.Id,
                    WebSecurity.IsAuthenticated, param.Start, param.Length, out totalRecords);

                var resultUserFiles = from val in categoryFiles
                    select new[]
                    {
                        Convert.ToString(val.FileId),
                        string.Empty,
                        val.OrigFileName,
                        val.Description,
                        string.Empty,
                        Convert.ToString(val.CreateDate),
                        Convert.ToString(val.ModifyDate)
                    };
                var result = new
                {
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

        [Authorize]
        public virtual JsonResult UserFilesTableFilter(JQueryDataTablesModel param)
        {
            try
            {
                int totalRecords = 0;
                int userId = (int)WebSecurity.CurrentUserId;

                List<ExchangeFile> userFiles = _exchangeFileService.GetUserFilesPaged(userId, param.iDisplayStart, param.iDisplayLength, out totalRecords);
                var resultUserFiles = from val in userFiles
                                      select new[]
                    {
                        Convert.ToString(val.FileId),
                        string.Empty,
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
