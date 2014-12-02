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
    public class AbuseItemsController : Controller
    {
        public IProfileService ProfileService { get; set; }
        public IItemService Itemservice { get; set; }
        public IItemMatchService ItemMatchService { get; set; }
        public AbuseItemsController(IProfileService ProfileService, IItemService Itemservice, IItemMatchService ItemMatchService)
        {
            this.ProfileService = ProfileService;
            this.Itemservice = Itemservice;
            this.ItemMatchService = ItemMatchService;
        }

        // GET: /AbuseItems/
        public ActionResult Index()
        {
            var Items = Itemservice.GetItems();
            var ItemMatchs = ItemMatchService.GetItemMatchs();

            var models =new List<AbuseItemsModel>();

            var Results = (from i in Items
                            join im in ItemMatchs on i.ItemID equals im.ItemID
                            where im.IsLikeDislikeAbuseBy == 3
                            orderby i.ItemTitle descending
                            select new
                            {
                                i.ItemID,
                                i.ProfileID,
                                i.ItemTitle,
                                i.ItemDescription,
                                i.ItemImage,
                                im.ProfileIdBy,
                                im.IsLikeDislikeAbuseBy,
                                im.AbuseMessage
                            }).ToList();


            foreach (var item in Results)
            {
                models.Add(new AbuseItemsModel
                {
                    ItemID = item.ItemID,
                    ProfileID = item.ProfileID,
                    ItemTitle = item.ItemTitle,
                    ItemDescription=item.ItemDescription, ItemImage=item.ItemImage, ProfileIdBy=item.ProfileIdBy,
                    IsLikeDislikeAbuseBy=item.IsLikeDislikeAbuseBy, AbuseMessage=item.AbuseMessage
                });
            }

            return View(models);
        }
	}
}