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

            var Items = Itemservice.GetItems();
            var models = new List<ItemModel>();
            Mapper.CreateMap<SwapStff.Entity.Item, SwapStff.Models.ItemModel>();
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
        public ActionResult Delete(int ItemID)
        {
            if (ItemID <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = Itemservice.GetItems().Where(x => x.ItemID==ItemID).FirstOrDefault();
            Mapper.CreateMap<SwapStff.Entity.Item, SwapStff.Models.ItemModel>();
            SwapStff.Models.ItemModel itemModel = Mapper.Map<SwapStff.Entity.Item, SwapStff.Models.ItemModel>(item);

            if (itemModel == null)
            {
                return HttpNotFound();
            }
            return View(itemModel);
        }

        //
        // POST: /UserItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ItemId)
        {
            try
            {
                //Delete Image from Blob
                DeleteImageFromBlob(ItemId.ToString());

                var Item = Itemservice.GetAll().Where(x => x.ItemID == ItemId).FirstOrDefault();
                Itemservice.Delete(Item);
                
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ErrorLogging.LogError(ex);
                return View();
            }
        }
        public Boolean DeleteImageFromBlob(string ItemID)
        {
            Boolean Status = false;
            try
            {
                //Delete the Existing Item
                var ItemDel = Itemservice.GetItems().Where(x => x.ItemID == Convert.ToInt32(ItemID)).FirstOrDefault();

                if (ItemDel.ItemImage != "")
                {
                    //Delete Image from Blob
                    BlobCloudService objBlob = new BlobCloudService();
                    objBlob.DeleteImageFromBlob(ItemDel.ItemImage);
                }

                Status = true;
            }
            catch(Exception ex)
            {
                ErrorLogging.LogError(ex);
                Status = false;
            }
            return Status;
        }
    }
}
