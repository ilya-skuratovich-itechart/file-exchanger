using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileExchange.Areas.Admin.Models;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;
using FileExchange.Infrastructure.ModelBinders;
using FileExchange.Models.DataTable;

namespace FileExchange.Areas.Admin.Controllers
{
    public partial class GlobalSettingsController : Controller
    {
        private IGlobalSettingService _globalSettingService { get; set; }

        public GlobalSettingsController(IGlobalSettingService globalSettingService)
        {
            _globalSettingService = globalSettingService;
        }

        public virtual ActionResult ViewSettings()
        {
            return View();
        }


        public ActionResult UpdateSetting(int settingId)
        {
            GlobalSettingViewModel settingModel =
                AutoMapper.Mapper.Map<GlobalSettingViewModel>(_globalSettingService.GetById(settingId));
            return View(settingModel);
        }

        [HttpPost]
        public ActionResult UpdateSetting(GlobalSettingViewModel settingModel)
        {
            return View();
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
