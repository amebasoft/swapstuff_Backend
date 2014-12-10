using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

using SwapStff.Models;
using SwapStff.Service;
namespace SwapStff.Controllers
{
    public class UserAppSettingsController : Controller
    {
        
        public IAppSettingService AppSettingService { get; set; }
        public UserAppSettingsController(IAppSettingService AppSettingService)
        {
            this.AppSettingService = AppSettingService;
        }
        // GET: /UserAppSettings/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /UserAppSettings/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserAppSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserAppSettings/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserAppSettings/Edit
        public ActionResult Edit()
        {
            var appSettings = AppSettingService.GetAll().FirstOrDefault();
         
            Mapper.CreateMap<SwapStff.Entity.AppSetting, SwapStff.Models.AppSettingModel>();
            SwapStff.Models.AppSettingModel appSettingModel = Mapper.Map<SwapStff.Entity.AppSetting, SwapStff.Models.AppSettingModel>(appSettings);

            if (appSettings == null)
            {
                return HttpNotFound();
            }
            return View(appSettingModel);
        }

        //
        // POST: /UserAppSettings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "SettingID,MaxDistance")] AppSettingModel appSettingModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<SwapStff.Models.AppSettingModel, SwapStff.Entity.AppSetting>();
                    SwapStff.Entity.AppSetting appSetting = Mapper.Map<SwapStff.Models.AppSettingModel, SwapStff.Entity.AppSetting>(appSettingModel);
                    AppSettingService.Update(appSetting); //Update Operation
                }
                appSettingModel.Message="Success";
                return View(appSettingModel);
            }
            catch
            {
                appSettingModel.Message = "Error";
                return View(appSettingModel);
            }
        }

        //
        // GET: /UserAppSettings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UserAppSettings/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
