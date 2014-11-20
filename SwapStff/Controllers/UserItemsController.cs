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
    public class UserItemsController : Controller
    {
        
        public IProfileService ProfileService { get; set; }
        public IItemService Itemservice { get; set; }
        public IItemMatchService ItemMatchService { get; set; }

        public UserItemsController(IProfileService ProfileService, IItemService Itemservice, IItemMatchService ItemMatchService)
        {
            this.ProfileService = ProfileService;
            this.Itemservice = Itemservice;
            this.ItemMatchService = ItemMatchService;
        }
        //
        // GET: /UserItems/
        public ActionResult Index()
        {

            var Items = Itemservice.GetAll();
            var models = new List<ItemModel>();
            Mapper.CreateMap<SwapStff.Entity.Item, SwapStff.Models.ItemModel>();
            //models.Add(Mapper.Map<SwapStff.Entity.Item, SwapStff.Models.ItemModel>(Items));

            foreach (var Item in Items)
            {
                models.Add(Mapper.Map<SwapStff.Entity.Item, SwapStff.Models.ItemModel>(Item));
            }

            return View(models);
        }

        //
        // GET: /UserItems/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserItems/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserItems/Create
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
        // GET: /UserItems/Edit/5
        public ActionResult Edit(int ItemID)
        {
            return View();
        }

        //
        // POST: /UserItems/Edit/5
        [HttpPost]
        public ActionResult Edit(int ItemID, FormCollection collection)
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
        // GET: /UserItems/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UserItems/Delete/5
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
