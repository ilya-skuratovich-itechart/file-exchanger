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
using FileExchange.ActionResults;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.DTO;
using FileExchange.Core.FileNotification;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
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
        private IExchangeFileService _fileExchangeService { get; set; }
        private IFileCommentService _fileCommentService { get; set; }

        public FileController(IUnitOfWork unitOfWork, IFileCategoriesService fileCategoriesService,
            IFileNotificationSubscriberService fileFileNotificationSubscriberService,
            IExchangeFileService fileExchangeService,
            IFileCommentService fileCommentService)
        {
            _unitOfWork = unitOfWork;
            _fileCategoriesService = fileCategoriesService;
            _fileExchangeService = fileExchangeService;
            _fileCommentService = fileCommentService;
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
                int userId = (int) WebSecurity.CurrentUserId;
                string uniqFileName = Guid.NewGuid().ToString() + Path.GetExtension(userFile.File.FileName);
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    ExchangeFile ExchangeFile = _fileExchangeService.Add(userId, userFile.SelectedFileCategoryId,
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
                AutoMapper.Mapper.Map<EditExchangeFileModel>(_fileExchangeService.GetUserFile(fileId, userId));
            userFile.FileCategories = fileCategoriesListItems;
            return View(userFile);
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<ActionResult> EditUserFile(EditExchangeFileModel userFile)
        {
            int userId = WebSecurity.CurrentUserId;
            if (ModelState.IsValid)
            {
                ExchangeFile exchangeFile = _fileExchangeService.GetUserFile(userFile.FileId, userId);
              var oldExchangeFile=  AutoMapper.Mapper.Map<ExchangeFile>(exchangeFile); 
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    ExchangeFile newExchangeFile = _fileExchangeService.Update(userId, userFile.FileId,
                        userFile.SelectedFileCategoryId,
                        userFile.Description, userFile.Tags, userFile.DenyAll, userFile.AllowViewAnonymousUsers);

                    var fileNotificationTracker = new FileNotificationTracker(_fileFileNotificationSubscriberService);
                    var notificationUsers = fileNotificationTracker.GetNotoficationUsersByChanges(oldExchangeFile,
                        newExchangeFile);

                    Action<object> sendFunc = (o) =>
                    {
                        try
                        {


                            List<FileUserNotification> notificUsers = (o) as List<FileUserNotification>;
                            if (notificUsers != null)
                            {
                                IMailer mailer = AutofacConfig.ApplicationContainer.Container.Resolve<IMailer>();
                                var templateModel = new BaseFileTemplateViewModel();
                                templateModel.FileName = userFile.OrigFileName;
                                templateModel.FileId = userFile.FileId;
                                foreach (var notificationUser in notificUsers)
                                {
                                    string templateText = string.Empty;
                                    templateModel.UserName = notificationUser.UserName;
                                    string subject = string.Empty;
                                    switch (notificationUser.NotificationType)
                                    {

                                        case NotificationType.accessDienied:
                                        {

                                            templateText = RenderViewHelper.RenderPartialToString(
                                                MVC.DisplayEmailTemplates.Views.FileAccessDeniedTemplate,
                                                templateModel);
                                            subject = "Access to file denied";
                                            break;
                                        }
                                        case NotificationType.descriptionChanged:
                                        templateText = RenderViewHelper.RenderPartialToString(
                                                MVC.DisplayEmailTemplates.Views.FileChangedTemplate,
                                                templateModel);
                                            subject = "file desctiption is changed";
                                            break;
                                        case NotificationType.fileDelited:
                                            templateText = RenderViewHelper.RenderPartialToString(
                                                MVC.DisplayEmailTemplates.Views.FileDeletedTemplate,
                                                templateModel);
                                            subject = "File is deleted";
                                            break;
                                        default:
                                            break;
                                    }
                                    mailer.SendEmailTo(notificationUser.Email, subject, templateText);
                                }
                            }
                        } 
                        catch (Exception exc )
                        {
                            
                            throw exc;
                        }

                    };

                    Task.Factory.StartNew(sendFunc, notificationUsers);
                    _unitOfWork.SaveChanges();
                    transaction.Complete();
                }
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
                    ExchangeFile userFile = _fileExchangeService.GetUserFile(fileId, userId);
                    if (userFile == null)
                        throw new ArgumentException("");
                    _fileExchangeService.RemoveUserFile(userId, fileId);
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

        public virtual ActionResult ViewFile(int fileId)
        {
            ExchangeFile ExchangeFile= _fileExchangeService.GetFilteredFile(fileId, WebSecurity.IsAuthenticated);
            if (ExchangeFile == null)
                throw new Exception(string.Format("File not exists or access denied. FileId = {0}", fileId));
            ViewExchangeFileViewModel exchangeFileViewModel = AutoMapper.Mapper.Map<ViewExchangeFileViewModel>(ExchangeFile);
            exchangeFileViewModel.Owner = ExchangeFile.User.UserName;
            if (WebSecurity.IsAuthenticated)
                exchangeFileViewModel.HasSubscription =
                    _fileFileNotificationSubscriberService.UserIsSubscibed(WebSecurity.CurrentUserId, fileId);
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
            ExchangeFile file = _fileExchangeService.GetFilteredFile(fileId, WebSecurity.IsAuthenticated);
            if (file == null)
                throw new Exception(string.Format("File not exists or access denied. FileId = {0}", fileId));
            string filePath = FileHelper.GetFullfilecommentsPath(file.UniqFileName);
            return new BandwidthThrottlingFileResult(filePath, file.OrigFileName,
                FileHelper.GetMimeTypeByFileName(file.UniqFileName), 20);
        }

        [Authorize]
        public virtual JsonResult SubscribeFileNotification(int fileId)
        {
            try
            {
                object result = null;
                using (var transaciton = _unitOfWork.BeginTransaction())
                {
                    if (_fileFileNotificationSubscriberService.UserIsSubscibed(WebSecurity.CurrentUserId, fileId))
                    {
                        result = new {Error = "Current user has a subscription", Success = false};
                    }
                    else
                    {
                        _fileFileNotificationSubscriberService.Add(WebSecurity.CurrentUserId, fileId);
                        _unitOfWork.SaveChanges();
                        transaciton.Complete();
                        result = new
                        {
                            Html = this.RenderViewToString(MVC.File.Views._fileSubscribe, true),
                            Success = true
                        };
                    }
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
            using (var transaciton = _unitOfWork.BeginTransaction())
            {
                if (!_fileFileNotificationSubscriberService.UserIsSubscibed(WebSecurity.CurrentUserId, fileId))
                {
                    result = new { Error = "Current user has not a subscription", Success = false };
                }
                else
                {
                    _fileFileNotificationSubscriberService.RemoveFromUser(fileId, WebSecurity.CurrentUserId);
                    _unitOfWork.SaveChanges();
                    transaciton.Complete();
                    result = new
                    {
                        Html = this.RenderViewToString(MVC.File.Views._fileSubscribe, false),
                        Success = true
                    };
                }
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
