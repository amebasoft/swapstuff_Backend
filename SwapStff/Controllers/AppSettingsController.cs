using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;

using SwapStff.Models;
using SwapStff.Service;
namespace SwapStff.Controllers
{
    [RoutePrefix("AppSettings")]
    public class AppSettingsController : ApiController
    {
        public IAppSettingService AppSettingService { get; set; }
        public AppSettingsController(IAppSettingService AppSettingService)
        {
            this.AppSettingService = AppSettingService;
        }
        // GET api/AppSettings
        //http://swapstff.com/AppSettings/GetAppSettings
        [Route("GetAppSettings")]
        [HttpGet]
        public IHttpActionResult GetAppSettings()
        {
            var appSettings = AppSettingService.GetAll();
            var models = new List<AppSettingModel>();
            Mapper.CreateMap<SwapStff.Entity.AppSetting, SwapStff.Models.AppSettingModel>();
            foreach (var appSetting in appSettings)
            {
                models.Add(Mapper.Map<SwapStff.Entity.AppSetting, SwapStff.Models.AppSettingModel>(appSetting));
            }

            return Json(models);
        }

    }
}
