using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;


using SwapStff.Models;
using SwapStff.Service;
using SwapStff.Core.UtilityManager;

namespace SwapStff.Controllers
{
    [RoutePrefix("Items")]
    public class ItemsController : ApiController
    {
        public IProfileService ProfileService { get; set; }
        public IItemService Itemservice { get; set; }
        public IItemMatchService ItemMatchService { get; set; }

        public ItemsController(IProfileService ProfileService, IItemService Itemservice, IItemMatchService ItemMatchService)
        {
            this.ProfileService = ProfileService;
            this.Itemservice = Itemservice;
            this.ItemMatchService = ItemMatchService;
        }

        // GET api/Items
        //http://swapstff.com/Items/GetAllItems
        [Route("GetAllItems")]
        [HttpGet]
        public IHttpActionResult GetAllItems()
        {
            var Items = Itemservice.GetAll();
            var models = new List<ItemModel>();
            Mapper.CreateMap<SwapStff.Entity.Item, SwapStff.Models.ItemModel>();
            foreach (var Item in Items)
            {
                models.Add(Mapper.Map<SwapStff.Entity.Item, SwapStff.Models.ItemModel>(Item));
            }
            return Json(models);
        }

        // GET api/Items
        //http://swapstff.com/Items/GetItemsNearBy
        [Route("GetItemsNearBy")]
        [HttpPost]
        public IHttpActionResult GetItemsNearBy([FromBody]ItemModel ItemModel)
        {
            var Items = Itemservice.GetAll();
            var Profiles = ProfileService.GetAll();
            var ItemMatchs = ItemMatchService.GetAll();

            Int32 DistanceTo =Convert.ToInt32(ProfileService.GetById(ItemModel.ProfileID.ToString()).Distance);
            double LatitudeProfileTo = Convert.ToDouble(ProfileService.GetById(ItemModel.ProfileID.ToString()).Latitude);
            double LongitudeProfileTo = Convert.ToDouble(ProfileService.GetById(ItemModel.ProfileID.ToString()).Longitude);


            var ItemsList = (from i in Items join p in Profiles on i.ProfileID equals p.ProfileId
                             where (i.IsActive == true) && (i.ItemID != ItemModel.ItemID) && (!(from im in ItemMatchs where im.ProfileIdBy==ItemModel.ProfileID select im.ItemID).Contains(i.ItemID))
                             select new
                             {   
                                 i.ItemID, i.ProfileID, i.ItemTitle, i.ItemDescription, i.ItemImage,
                                 i.ItemDateTimeCreated, p.Latitude, p.Longitude
                             }).OrderByDescending(x => x.ItemDateTimeCreated);

            var models = new List<ItemModel>();
            foreach (var Item in ItemsList)
            {
                double DistanceBy = 0;
                double LatitudeProfileBy = Convert.ToDouble(Item.Latitude);
                double LongitudeProfileBy = Convert.ToDouble(Item.Longitude);

                DistanceBy = GoogleDistance.Calc(LatitudeProfileTo, LongitudeProfileTo, LatitudeProfileBy, LongitudeProfileBy);
                if (DistanceTo >= DistanceBy)
                {
                    models.Add(new ItemModel
                    {
                        ItemID = Item.ItemID,
                        ProfileID = Item.ProfileID,
                        ItemDescription = Item.ItemDescription,
                        ItemTitle = Item.ItemTitle,
                        ItemImage = Item.ItemImage,
                        ItemDateTimeCreated = Item.ItemDateTimeCreated,
                        Distance = DistanceBy
                    });
                }
            }
            return Json(models);
        }

        // GET api/Items
        //http://swapstff.com/Items/GetItem/1
        [Route("GetItem/{ItemId}")]
        [HttpGet]
        public IHttpActionResult GetItem(int ItemId)
        {
            var Item = Itemservice.GetById(ItemId.ToString());
            Mapper.CreateMap<SwapStff.Entity.Item, SwapStff.Models.ItemModel>();
            SwapStff.Models.ItemModel ItemModel = Mapper.Map<SwapStff.Entity.Item, SwapStff.Models.ItemModel>(Item);
            return Json(ItemModel);
        }

        // POST api/Items
        //http://swapstff.com/Items/SaveItem/{json}
        [Route("SaveItem")]
        [HttpPost]
        public HttpResponseMessage SaveItem([FromBody]ItemModel ItemModel)
        {
            string ItemID = "-1";
            try
            {
                //Generate URL for Image
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(ItemModel.ItemImage);
                string uniqueBlobName = string.Format("image_{0}{1}", Guid.NewGuid().ToString(), ".jpg");
                //End : Generate URL for Image

                //Set URL for Image to Blank
                //I will update later after Successfully updation DB
                ItemModel.ItemImage = "";
                //End : Set URL for Image to Blank
               

                Mapper.CreateMap<SwapStff.Models.ItemModel, SwapStff.Entity.Item>();
                SwapStff.Entity.Item Item = Mapper.Map<SwapStff.Models.ItemModel, SwapStff.Entity.Item>(ItemModel);
                if (ItemModel.ItemID <= 0)
                {
                    Itemservice.Insert(Item); //Save Operation
                }
                else
                {
                    if (ItemModel.DeleteFirst == 1) //1 for true
                    {
                        //Delete the Existing Item
                        var ItemDel = Itemservice.GetById(ItemModel.ItemID.ToString());
                        //Delete Record from DB
                        Itemservice.Delete(ItemDel);
                        //Delete Image from Blob
                        DeleteImageFromBlob(ItemModel.ItemID.ToString());
                        //End : Delete the Existing Item

                        //Insert the New item
                        Itemservice.Insert(Item); //Save Operation
                    }
                    else
                    {
                        Itemservice.Update(Item); //Update Operation
                    }
                }
                ItemID = Item.ItemID.ToString();

                if (Convert.ToInt32(ItemID) > 0) //Operation Performed on it
                {
                    //Delete Image from Blob
                    DeleteImageFromBlob(ItemID.ToString());
                    BlobCloudService objBlob = new BlobCloudService();
                    string URL = objBlob.UploadBlobImage(uniqueBlobName, imageBytes);
                    
                    //Update the Existing Item URL
                    var ItemUpdate = Itemservice.GetById(ItemID.ToString());
                    ItemUpdate.ItemImage = URL;

                    Itemservice.Update(ItemUpdate);
                }
                

                return Request.CreateResponse(HttpStatusCode.OK, ItemID, Configuration.Formatters.JsonFormatter);
            }
            catch(Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                return Request.CreateResponse(HttpStatusCode.NotImplemented, ItemID.ToString(), Configuration.Formatters.JsonFormatter);
            }
        }
        public Boolean DeleteImageFromBlob(string ItemID)
        {
            Boolean Status = false;
            try
            {
                //Delete the Existing Item
                var ItemDel = Itemservice.GetById(ItemID.ToString());

                if (ItemDel.ItemImage != "")
                {
                    //Delete Image from Blob
                    BlobCloudService objBlob = new BlobCloudService();
                    objBlob.DeleteImageFromBlob(ItemDel.ItemImage);
                }
              
                Status = true;
            }
            catch
            {
                Status = false;
            }
            return Status;
        }
       
       
        // DELETE api/Items/5
        //http://swapstff.com/Items/DeleteItem/1
        [Route("DeleteItem/{ItemId}")]
        [HttpGet]
        public HttpResponseMessage DeleteItem(int ItemId)
        {
            try
            {
                var Item = Itemservice.GetById(ItemId.ToString());
                Itemservice.Delete(Item);
                //Delete Image from Blob
                DeleteImageFromBlob(ItemId.ToString());

                return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS", Configuration.Formatters.JsonFormatter);
            }
            catch(Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                return Request.CreateResponse(HttpStatusCode.NotImplemented, "ERROR", Configuration.Formatters.JsonFormatter);
            }
        }
    }
}
