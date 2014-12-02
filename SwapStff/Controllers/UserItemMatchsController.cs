using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using SwapStff.Models;
using SwapStff.Service;
using SwapStff.Core.UtilityManager;
using SwapStff.Infrastructure;

namespace SwapStff.Controllers
{
    public class UserItemMatchsController : Controller
    {
        public IProfileService ProfileService { get; set; }
        public IItemService Itemservice { get; set; }
        public IItemMatchService ItemMatchService { get; set; }
        public IChatService ChatService { get; set; }

        public UserItemMatchsController(IProfileService ProfileService, IItemService Itemservice, IItemMatchService ItemMatchService, IChatService ChatService)
        {
            this.ProfileService = ProfileService;
            this.Itemservice = Itemservice;
            this.ItemMatchService = ItemMatchService;
            this.ChatService = ChatService;
        }

        // GET: /UserItemMatchs/
        public ActionResult Index()
        {
            var models = GetItemMatchList();

            return View(models);
        }

        public List<UserItemMatchModel> GetItemMatchList()
        {
            var models = new List<UserItemMatchModel>();

            var Profiles = ProfileService.GetProfiles();
            var Items = Itemservice.GetItems();
            var Items1 = Itemservice.GetItems();
            var ItemMatch = ItemMatchService.GetItemMatchs();

            var ItemMatchList = (from im in ItemMatch
                                 join i in Items on im.ProfileIdBy equals i.ProfileID
                                 join i1 in Items1 on im.ItemID equals i1.ItemID
                                 where im.IsLikeDislikeAbuseBy == 1
                                 orderby im.DateTimeCreated
                                 select new {
                                     ProfileID1 = im.ProfileIdBy,
                                     ItemID1 = i.ItemID,
                                     ItemTitle1 = i.ItemTitle,
                                     ItemImage1 = i.ItemImage,
                                     ProfileID2 = i1.ProfileID,
                                     ItemID2 = im.ItemID,
                                     ItemTitle2 = i1.ItemTitle,
                                     ItemImage2 = i1.ItemImage,
                                     im.IsLikeDislikeAbuseBy
                                 });

            foreach (var item in ItemMatchList)
            {
                var IsAlreadyAdded = models.Where(x => x.ProfileID1 == item.ProfileID2 && x.ItemID1 == item.ItemID2 && x.ProfileID2 == item.ProfileID1 && x.ItemID2 == item.ItemID1).FirstOrDefault();

                if (IsAlreadyAdded == null)
                {
                    double Distance = 0;
                    double LatitudeProfileBy = Convert.ToDouble(Profiles.Where(x => x.ProfileId == item.ProfileID1).Select(x=>x.Latitude).FirstOrDefault());
                    double LongitudeProfileBy = Convert.ToDouble(Profiles.Where(x => x.ProfileId == item.ProfileID1).Select(x => x.Longitude).FirstOrDefault());
                    double LatitudeProfileTo = Convert.ToDouble(Profiles.Where(x => x.ProfileId == item.ProfileID2).Select(x => x.Latitude).FirstOrDefault());
                    double LongitudeProfileTo = Convert.ToDouble(Profiles.Where(x => x.ProfileId == item.ProfileID2).Select(x => x.Longitude).FirstOrDefault());

                    Distance = GoogleDistance.Calc(LatitudeProfileTo, LongitudeProfileTo, LatitudeProfileBy, LongitudeProfileBy);

                    models.Add(new UserItemMatchModel
                    {
                        ProfileID1 = item.ProfileID1,
                        ItemID1 = item.ItemID1,
                        ItemTitle1 = item.ItemTitle1,
                        ItemImage1 = item.ItemImage1,
                        ProfileID2 = item.ProfileID2,
                        ItemID2 = item.ItemID2,
                        ItemTitle2 = item.ItemTitle2,
                        ItemImage2 = item.ItemImage2,
                        Distance =  Distance
                    });
                }
         
            }


            return models;
        }
        // GET: /UserItemMatchs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserItemMatchs/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserItemMatchs/Create
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
        // GET: /UserItemMatchs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /UserItemMatchs/Edit/5
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
        // GET: /UserItemMatchs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UserItemMatchs/Delete/5
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
