using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

using AutoMapper;
using SwapStff.Models;
using SwapStff.Service;
namespace SwapStff.Controllers
{
    public class UserProfilesController : Controller
    {
        
        public IProfileService ProfileService { get; set; }
        public IItemService Itemservice { get; set; }
        public IItemMatchService ItemMatchService { get; set; }

        public UserProfilesController(IProfileService ProfileService, IItemService Itemservice, IItemMatchService ItemMatchService)
        {
            this.ProfileService = ProfileService;
            this.Itemservice = Itemservice;
            this.ItemMatchService = ItemMatchService;
        }
        //
        // GET: /UserProfiles/
        public ActionResult Index()
        {
            var profiles = ProfileService.GetAll();
            var models = new List<ProfileModel>();
            Mapper.CreateMap<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>();
            foreach (var profile in profiles)
            {
                models.Add(Mapper.Map<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>(profile));
            }

            return View(models);
        }

        //
        // GET: /UserProfiles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserProfiles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserProfiles/Create
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
        // GET: /UserProfiles/Edit/5
        public ActionResult Edit(int ProfileId)
        {
            if (ProfileId <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var profile = ProfileService.GetById(ProfileId.ToString());
            Mapper.CreateMap<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>();
            SwapStff.Models.ProfileModel profileModel = Mapper.Map<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>(profile);

            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profileModel);
        }

        //
        // POST: /UserProfiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ProfileId,Username,Latitude,Longitude,DateTimeCreated,Distance")] ProfileModel profileModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<SwapStff.Models.ProfileModel, SwapStff.Entity.Profile>();
                SwapStff.Entity.Profile profile = Mapper.Map<SwapStff.Models.ProfileModel, SwapStff.Entity.Profile>(profileModel);
                ProfileService.Update(profile); //Update Operation
                return RedirectToAction("Index");
            }
            return View(profileModel);
        }

        //
        // GET: /UserProfiles/Delete/5
        public ActionResult Delete(int ProfileId)
        {
            if (ProfileId <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var profile = ProfileService.GetById(ProfileId.ToString());
            Mapper.CreateMap<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>();
            SwapStff.Models.ProfileModel profileModel = Mapper.Map<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>(profile);

            if (profileModel == null)
            {
                return HttpNotFound();
            }
            return View(profileModel);
        }

        //
        // POST: /UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ProfileId)
        {
            try
            {
                var ItemMatch = ItemMatchService.GetAll().Where(x => x.ProfileIdBy == ProfileId).FirstOrDefault();
                if (ItemMatch != null)
                {
                    ItemMatchService.Delete(ItemMatch);
                }
                else
                {
                    return HttpNotFound();
                }

                //Delete from profile, It will delete from Items, Chat & Profiles
                var profile = ProfileService.GetById(ProfileId.ToString());
                ProfileService.Delete(profile);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
    }
}
