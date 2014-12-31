using System;
using Autofac;
using FileExchange.Core.DTO;
using FileExchange.EmailSender;
using FileExchange.Helplers;
using FileExchange.Models;

namespace FileExchange.Infrastructure.Notifications.FileNotification
{
    public static class FileNotification
    {
        public static void SendChangeFileNotification(object fileNotification)
        {
            try
            {
                var fileNotificationModel = (fileNotification) as FileNotificationModel;

                if (fileNotificationModel != null && fileNotificationModel.FileUserNotifications != null)
                {

                    IMailer mailer = fileNotificationModel.Mailer;
                    var templateModel = new BaseFileTemplateViewModel();
                    templateModel.FileName = fileNotificationModel.OriginalFileName;
                    templateModel.FileId = fileNotificationModel.FileId;
                    templateModel.FileUrl = fileNotificationModel.FileUrl;
                    foreach (var notificationUser in fileNotificationModel.FileUserNotifications)
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
                                        MVC.DisplayEmailTemplates.Views._layout,
                                        templateModel);
                                    subject = "Access to file denied";
                                    break;
                                }
                            case NotificationType.descriptionChanged:
                                templateText = RenderViewHelper.RenderPartialToString(
                                        MVC.DisplayEmailTemplates.Views.FileChangedTemplate,
                                        MVC.DisplayEmailTemplates.Views._layout,
                                        templateModel);
                                subject = "file desctiption is changed";
                                break;
                            case NotificationType.fileDelited:
                                templateText = RenderViewHelper.RenderPartialToString(
                                    MVC.DisplayEmailTemplates.Views.FileDeletedTemplate,
                                    MVC.DisplayEmailTemplates.Views._layout,
                                    templateModel);
                                subject = "File is deleted";
                                break;
                        }
                        mailer.SendEmailTo(notificationUser.Email, subject, templateText);
                    }
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        } 
    }
}