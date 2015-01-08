using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FileExchange.Areas.Admin.Models;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.Infrastructure.ModelBinders;
using FileExchange.Infrastructure.NetHelperExtension;
using FileExchange.Models.DataTable;

namespace FileExchange.Areas.Admin.Controllers
{
    public partial class GlobalSettingsController : Controller
    {
        private IGlobalSettingService _globalSettingService { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }


        public GlobalSettingsController(IUnitOfWork unitOfWork, IGlobalSettingService globalSettingService)
        {
            _unitOfWork = unitOfWork;
            _globalSettingService = globalSettingService;
        }

        public virtual ActionResult ViewSettings()
        {
            return View();
        }


        public virtual ActionResult UpdateSetting(int settingId)
        {
            GlobalSettingViewModel settingModel =
                AutoMapper.Mapper.Map<GlobalSettingViewModel>(_globalSettingService.GetById(settingId));
            return View(settingModel);
        }

        [HttpPost]
        public virtual ActionResult UpdateSetting(GlobalSettingViewModel settingModel)
        {
            GlobalSetting globalSetting = _globalSettingService.GetById(settingModel.SettingId);
            if (globalSetting == null)
                throw new Exception(string.Format("global setting not exists. SettingId:{0}", settingModel.SettingId));
            var settingValueRegex = new Regex(globalSetting.VaidationRegexMask);
            if (!settingValueRegex.Match(settingModel.SettingValue).Success)
            {
                ModelState.AddModelError(NameOf<GlobalSettingViewModel>.Property(s=>s.SettingValue), "incorrect setting value.");
               
            }
            if (!ModelState.IsValid)
                return View(settingModel);
            _globalSettingService.Update(settingModel.SettingId,settingModel.SettingValue);
            _unitOfWork.SaveChanges();
            return RedirectToAction(MVC.Admin.GlobalSettings.ActionNames.ViewSettings);
        }

        public virtual JsonResult ViewSettingsTableFilter(
            [ModelBinder(typeof(DataTablesBinder))] DefaultDataTablesRequest param)
        {
            try
            {
                int totalRecords = 0;
                IEnumerable<GlobalSetting> globalSettings = _globalSettingService.GetPaged(param.Start, param.Length,
                    out totalRecords);
                var reusltGlobalSettings = from val in globalSettings
                                           select new[]
                    {
                        Convert.ToString(val.SettingId),
                        string.Empty,
                        val.SettingName,
                        val.SettingValue,
                        val.VaidationRegexMask,
                        val.Description
                    };
                var result = new
                {
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = reusltGlobalSettings.Count(),
                    aaData = reusltGlobalSettings

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
