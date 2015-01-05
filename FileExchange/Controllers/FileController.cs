using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Autofac;
using FileExchange.Areas.Admin.Models;
using FileExchange.Core.BandwidthThrottling;
using FileExchange.Infrastructure.ActionResults;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.DTO;
using FileExchange.Core.FileNotification;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using FileExchange.Helplers;
using FileExchange.Infrastructure.Configuration;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Infrastructure.ModelBinders;
using FileExchange.Infrastructure.NetHelperExtension;
using FileExchange.Models;
using FileExchange.Models.DataTable;
using FileExchange.Infrastructure.Notifications.FileNotification;
using FileExchange.Infrastructure.UserSecurity;
using WebMatrix.WebData;

namespace FileExchange.Controllers
{
    public partial class FileController : Controller
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private IFileNotificationSubscriberService _fileFileNotificationSubscriberService { get; set; }
        private IFileCategoriesService _fileCategoriesService { get; set; }
        private IExchangeFileService _fileExchangeService { get; set; }
        private IFileCommentService _fileCommentService { get; set; }
        private IBandwidthThrottlingSettings _bandwidthThrottlingSettings { get; set; }
        private IUserProfileService _userProfileService { get; set; }
        private IMailer _mailer { get; set; }
        private IWebSecurity _webSecurity { get; set; }

        private IFileProvider _fileProvider { get; set; }

        public FileController(IUnitOfWork unitOfWork, IFileCategoriesService fileCategoriesService,
            IFileNotificationSubscriberService fileFileNotificationSubscriberService,
            IExchangeFileService fileExchangeService,
            IFileCommentService fileCommentService,
            IBandwidthThrottlingSettings bandwidthThrottlingSettings,
            IUserProfileService userProfileService,
             IMailer mailer,
            IFileProvider fileProvider,
            IWebSecurity webSecurity)
        {
            _unitOfWork = unitOfWork;
            _fileCategoriesService = fileCategoriesService;
            _fileExchangeService = fileExchangeService;
            _fileCommentService = fileCommentService;
            _fileFileNotificationSubscriberService = fileFileNotificationSubscriberService;
            _bandwidthThrottlingSettings = bandwidthThrottlingSettings;
            _userProfileService = userProfileService;
            _mailer = mailer;
            _fileProvider = fileProvider;
            _webSecurity = webSecurity;

        }
        public virtual ActionResult FileSections()
        {
            List<FileCategoryModel> fileCategories =
                AutoMapper.Mapper.Map<List<FileCategoryModel>>(_fileCategoriesService.GetAll());
            return PartialView(MVC.File.Views._FileSections,fileCategories);
        }

        [Authorize]
        public virtual ActionResult UserFiles()
        {
            return View(MVC.File.Views.ViewNames.UserFiles);
        }

        [Authorize]
        [HttpGet]
        public virtual ActionResult AddUserFile()
        {
            IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(_fileCategoriesService.GetAll());
            var fileModel = new CreateExchangeFileModel(fileCategoriesListItems);
            return View(MVC.File.Views.ViewNames.AddUserFile,fileModel);
        }

        [Authorize]
        public virtual ActionResult AddUserFile(CreateExchangeFileModel userFile)
        {
            try
            {
              
                UserProfile userProfile = _userProfileService.GetUserById(_webSecurity.GetCurrentUserId());
                if (userProfile.FileMaxSizeKbps != 0)
                {
                    if (userFile.File.ContentLength/1024 >= userProfile.FileMaxSizeKbps)
                        ModelState.AddModelError(string.Empty,
                            string.Format("You can't upload this file. Max file size is {0} kbps.",
                                userProfile.FileMaxSizeKbps));
                }
                if (userFile.File == null)
                    ModelState.AddModelError(NameOf<CreateExchangeFileModel>.Property(s => s.File),
                        "Need to add a file.");
                if (!ModelState.IsValid)
                {
                    IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                        AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(
                            _fileCategoriesService.GetAll());
                    userFile.FileCategories = fileCategoriesListItems;
                    return View(MVC.File.Views.ViewNames.AddUserFile, userFile);
                }
                string uniqFileName = Guid.NewGuid().ToString() + Path.GetExtension(userFile.File.FileName);
                _unitOfWork.BeginTransaction();
                    _fileExchangeService.Add(_webSecurity.GetCurrentUserId(), userFile.SelectedFileCategoryId,
                        userFile.Description, uniqFileName,
                        userFile.File.FileName, userFile.Tags, userFile.DenyAll,
                        userFile.AllowViewAnonymousUsers);
                    var path =
                        this.HttpContext.Server.MapPath(string.Format("~/{0}",
                            Path.Combine(ConfigHelper.FilesFolder, uniqFileName)));
                    _unitOfWork.SaveChanges();
                    _fileProvider.SaveAs(userFile.File, path);
                _unitOfWork.CommitTransaction();
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
            int userId = _webSecurity.GetCurrentUserId();
            IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(_fileCategoriesService.GetAll());
            EditExchangeFileModel userFile =
                AutoMapper.Mapper.Map<EditExchangeFileModel>(_fileExchangeService.GetUserFile(fileId, userId));
            userFile.FileCategories = fileCategoriesListItems;
            return View(MVC.File.Views.ViewNames.EditUserFile,userFile);
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult EditUserFile(EditExchangeFileModel userFile)
        {
            int userId = _webSecurity.GetCurrentUserId();
            if (ModelState.IsValid)
            {
               _unitOfWork.BeginTransaction();
                    var oldExchangeFile =
                   AutoMapper.Mapper.Map<ExchangeFile>(_fileExchangeService.GetUserFile(userFile.FileId, userId));

                    ExchangeFile newExchangeFile = _fileExchangeService.Update(userId, userFile.FileId,
                        userFile.SelectedFileCategoryId,
                        userFile.Description, userFile.Tags, userFile.DenyAll, userFile.AllowViewAnonymousUsers);

                    var fileNotificationTracker = new FileNotificationTracker(_fileFileNotificationSubscriberService);
                    var notificationUsers = fileNotificationTracker.GetNotoficationUsersByChanges(oldExchangeFile,
                        newExchangeFile);
                    string fileUrl = Url.Action(MVC.File.ActionNames.ViewFile, MVC.File.Name, new { fileId = oldExchangeFile.FileId }, Request.Url.Scheme);
                    FileNotificationModel fileNotificationModel = new FileNotificationModel()
                    {
                        FileId = oldExchangeFile.FileId,
                        FileUrl = fileUrl,
                        OriginalFileName = oldExchangeFile.OrigFileName,
                        FileUserNotifications = notificationUsers,
                        Mailer = _mailer
                    };
                    _unitOfWork.SaveChanges();
                    _unitOfWork.CommitTransaction();
                    Task.Factory.StartNew(FileNotification.SendChangeFileNotification, fileNotificationModel);
                return RedirectToAction(MVC.File.ActionNames.UserFiles);
            }

            IEnumerable<System.Web.Mvc.SelectListItem> fileCategoriesListItems =
                AutoMapper.Mapper.Map<IEnumerable<System.Web.Mvc.SelectListItem>>(_fileCategoriesService.GetAll());
            userFile.FileCategories = fileCategoriesListItems;
            return View(MVC.File.Views.ViewNames.EditUserFile,userFile);
        }

      

        [Authorize]
        public virtual ActionResult DeleteUserFile(int fileId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                    int userId = (int)_webSecurity.GetCurrentUserId();
                    string fileUrl = Url.Action(MVC.File.ActionNames.ViewFile, MVC.File.Name, new { fileId = fileId }, Request.Url.Scheme);
                    ExchangeFile userFile = _fileExchangeService.GetUserFile(fileId, userId);
                    if (userFile == null)
                        throw new ArgumentException("file not exists. FileId:" +fileId.ToString());
                    var oldExchangeFile =
                       AutoMapper.Mapper.Map<ExchangeFile>(userFile);
                    var fileNotificationTracker = new FileNotificationTracker(_fileFileNotificationSubscriberService);
                    var notificationUsers = fileNotificationTracker.GetNotoficationUsersByChanges(oldExchangeFile,
                        null);
                  
                    FileNotificationModel fileNotificationModel = new FileNotificationModel()
                    {
                        FileId = oldExchangeFile.FileId,
                        FileUrl = fileUrl,
                        OriginalFileName = oldExchangeFile.OrigFileName,
                        FileUserNotifications = notificationUsers,
                        Mailer = _mailer
                    };
                    _fileCommentService.RemoveAll(fileId);
                    _fileFileNotificationSubscriberService.RemoveAll(fileId);
                    _fileExchangeService.RemoveUserFile(userId, fileId);
                    var filePath =
                        this.HttpContext.Server.MapPath(string.Format("~/{0}",
                            Path.Combine(ConfigHelper.FilesFolder, userFile.UniqFileName)));
                    _unitOfWork.SaveChanges();
                    _fileProvider.Delete(filePath);
                   _unitOfWork.CommitTransaction();
                    Task.Factory.StartNew(FileNotification.SendChangeFileNotification, fileNotificationModel);
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
            return View(MVC.File.Views.ViewNames.ViewCategoryFiles,categoryId);
        }

        public virtual ActionResult ViewFile(int fileId)
        {
            ExchangeFile ExchangeFile= _fileExchangeService.GetFilteredFile(fileId, _webSecurity.IsAuthenticated());
            if (ExchangeFile == null)
                throw new Exception(string.Format("File not exists or access denied. FileId = {0}", fileId));
            ViewExchangeFileViewModel exchangeFileViewModel = AutoMapper.Mapper.Map<ViewExchangeFileViewModel>(ExchangeFile);
            exchangeFileViewModel.Owner = ExchangeFile.User.UserName;
            if (_webSecurity.IsAuthenticated())
                exchangeFileViewModel.HasSubscription =
                    _fileFileNotificationSubscriberService.UserIsSubscibed(_webSecurity.GetCurrentUserId(), fileId);
            return View(exchangeFileViewModel);

        }

        public virtual ActionResult ViewFileComments(int fileId)
        {
            IEnumerable<FileCommentsViewModel> fileComments= AutoMapper.Mapper.Map<IEnumerable<FileCommentsViewModel>>(_fileCommentService.GetFileComments(fileId));
            return PartialView(MVC.File.Views._ViewFileComments,fileComments);
        }

        [HttpGet]
        public virtual ActionResult AddComment(int fileId)
        {
            return PartialView(MVC.File.Views._CreateComment, new CreateCommentViewModel() {FileId = fileId});
        }

        [HttpPost]
        public virtual ActionResult AddComment(CreateCommentViewModel commentModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Incorrect input data", MediaTypeNames.Text.Plain);
            }
            _fileCommentService.Add(commentModel.FileId, commentModel.Comment);
            _unitOfWork.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                IEnumerable<FileCommentsViewModel> fileComments = AutoMapper.Mapper.Map<IEnumerable<FileCommentsViewModel>>(_fileCommentService.GetFileComments(commentModel.FileId));
                return PartialView(MVC.File.Views._ViewFileComments, fileComments);
            }
            return RedirectToAction(MVC.File.ActionNames.ViewFile, new {fileId = commentModel.FileId});
        }

        public virtual BandwidthThrottlingFileResult DownloadFile(int fileId)
        {
            int? userId = null;
            ExchangeFile file = _fileExchangeService.GetFilteredFile(fileId, _webSecurity.IsAuthenticated());
            if (file == null)
                throw new Exception(string.Format("File not exists or access denied. FileId = {0}", fileId));
            if (_webSecurity.IsAuthenticated())
                userId = _webSecurity.GetCurrentUserId();
            int maxDownloadSpeed = _bandwidthThrottlingSettings.GetMaxDownloadSpeedKbps(userId);
            string filePath = FileHelper.GetFullFileFolderPath(file.UniqFileName);
            return new BandwidthThrottlingFileResult(filePath, file.OrigFileName,
                FileHelper.GetMimeTypeByFileName(file.UniqFileName), maxDownloadSpeed);
        }

        [Authorize]
        public virtual JsonResult SubscribeFileNotification(int fileId)
        {
            try
            {
                object result = null;
                _unitOfWork.BeginTransaction();
                    if (_fileFileNotificationSubscriberService.UserIsSubscibed(_webSecurity.GetCurrentUserId(), fileId))
                    {
                        result = new {Error = "Current user has a subscription", Success = false};
                    }
                    else
                    {
                        _fileFileNotificationSubscriberService.Add(_webSecurity.GetCurrentUserId(), fileId);
                        _unitOfWork.SaveChanges();
                     _unitOfWork.CommitTransaction();
                        result = new
                        {
                            Html = this.RenderViewToString(MVC.File.Views._fileSubscribe, true),
                            Success = true
                        };
                    }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException exc)
            {

                throw exc;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        [Authorize]
        public virtual JsonResult UnscribeFileNotification(int fileId)
        {
            object result = null;
            _unitOfWork.BeginTransaction();
                if (!_fileFileNotificationSubscriberService.UserIsSubscibed(_webSecurity.GetCurrentUserId(), fileId))
                {
                    result = new { Error = "Current user has not a subscription", Success = false };
                }
                else
                {
                    _fileFileNotificationSubscriberService.RemoveFromUser(fileId, _webSecurity.GetCurrentUserId());
                    _unitOfWork.SaveChanges();
                 _unitOfWork.CommitTransaction();
                    result = new
                    {
                        Html = this.RenderViewToString(MVC.File.Views._fileSubscribe, false),
                        Success = true
                    };
                }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult ViewCategoryFilesTableFilter([ModelBinder(typeof(DataTablesBinder))] DefaultDataTablesRequest param)
        {
            try
            { 
                int totalRecords = 0;
                IEnumerable<ExchangeFile> categoryFiles = _fileExchangeService.GetFilteredCategoryFilesPaged(param.Id,
                    _webSecurity.IsAuthenticated(), param.Start, param.Length, out totalRecords);

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
                int userId = (int)_webSecurity.GetCurrentUserId();

                List<ExchangeFile> userFiles = _fileExchangeService.GetUserFilesPaged(userId, param.iDisplayStart, param.iDisplayLength, out totalRecords);
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
