using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using SwapStff.Models;
using SwapStff.Service;
namespace SwapStff.Controllers
{
    public class ErrorExceptionLogsController : Controller
    {
        public IErrorExceptionLogService ErrorExceptionLogService { get; set; }

        public ErrorExceptionLogsController(IErrorExceptionLogService ErrorExceptionLogService)
        {
            this.ErrorExceptionLogService = ErrorExceptionLogService;
        }
        
        // GET: /ErrorExceptionLogs/
        public ActionResult Index()
        {
            var models = GetErrorExceptionLogsList();
            return View(models);
        }

        //
        // GET: /ErrorExceptionLogs/Details/5
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var error = GetErrorExceptionLogsList().Where(m => m.EventId == id).FirstOrDefault();

            if (error == null)
            {
                return HttpNotFound();
            }
            return View(error);
        }

        public List<ErrorExceptionLogsModel> GetErrorExceptionLogsList()
        {
            var errorExceptionLogs = ErrorExceptionLogService.GetErrorExceptionLogs();
            var models = new List<ErrorExceptionLogsModel>();
            Mapper.CreateMap<SwapStff.Entity.ErrorExceptionLogs, SwapStff.Models.ErrorExceptionLogsModel>();
            foreach (var errorExceptionLog in errorExceptionLogs)
            {
                models.Add(Mapper.Map<SwapStff.Entity.ErrorExceptionLogs, SwapStff.Models.ErrorExceptionLogsModel>(errorExceptionLog));
            }

            return models;
        }
        // GET: /ErrorExceptionLogs/Create
        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /ErrorExceptionLogs/Create
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
        // GET: /ErrorExceptionLogs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ErrorExceptionLogs/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ErrorExceptionLogs/Delete/5
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var error = GetErrorExceptionLogsList().Where(m => m.EventId == id).FirstOrDefault();

            if (error == null)
            {
                return HttpNotFound();
            }
            return View(error);
        }

        //
        // POST: /ErrorExceptionLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var error = ErrorExceptionLogService.GetErrorExceptionLogs().Where(x => x.EventId == id).FirstOrDefault();
                ErrorExceptionLogService.Delete(error);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ErrorLogging.LogError(ex);
                return View();
            }
        }
    }
}
