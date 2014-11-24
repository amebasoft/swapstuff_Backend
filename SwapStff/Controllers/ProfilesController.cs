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
    [RoutePrefix("Profiles")]
    public class ProfilesController : ApiController
    {
        public IProfileService ProfileService { get; set; }
        public IItemService Itemservice { get; set; }
        public IItemMatchService ItemMatchService { get; set; }
        public IChatService ChatService { get; set; }
       
        public ProfilesController(IProfileService ProfileService, IItemService Itemservice, IItemMatchService ItemMatchService, IChatService ChatService)
        {
            this.ProfileService = ProfileService;
            this.Itemservice = Itemservice;
            this.ItemMatchService = ItemMatchService;
            this.ChatService = ChatService;
        }

        // GET api/profiles
        //http://swapstff.com/profiles/GetAllProfiles
        [Route("GetAllProfiles")]
        [HttpGet]
        public IHttpActionResult GetAllProfiles()
        {
            var profiles = ProfileService.GetAll();
            var models = new List<ProfileModel>();
            Mapper.CreateMap<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>();
            foreach (var profile in profiles)
            {
                models.Add(Mapper.Map<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>(profile));
            }
                        
            return Json(models);
        }

        // GET api/profiles
        //http://swapstff.com/profiles/GetProfile/1
        [Route("GetProfile/{ProfileId}")]
        [HttpGet]
        public IHttpActionResult GetProfile(int ProfileId)
        {
            var profile = ProfileService.GetById(ProfileId.ToString());
            Mapper.CreateMap<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>();
            SwapStff.Models.ProfileModel profileModel = Mapper.Map<SwapStff.Entity.Profile, SwapStff.Models.ProfileModel>(profile);
            return Json(profileModel);
        }

        // POST api/profiles
        //http://swapstff.com/profiles/SaveProfile/{"ProfileId":1,"Username":"Test","Latitude":23.3000000000,"Longitude":34.3000000000,"DateTimeCreated":"2014-09-22T00:19:54.673","Distance":10.00}
        [Route("SaveProfile")]
        [HttpPost]
        public HttpResponseMessage SaveProfile([FromBody]ProfileModel profileModel)
        {
            string ProfileID = "-1";
            try
            {
                Mapper.CreateMap<SwapStff.Models.ProfileModel, SwapStff.Entity.Profile>();
                SwapStff.Entity.Profile profile = Mapper.Map<SwapStff.Models.ProfileModel, SwapStff.Entity.Profile>(profileModel);
                if (profileModel.ProfileId <= 0)
                {
                    ////Delete the Old One According to GCM Registation
                    //var profileObj= ProfileService.GetAll().Where(x => x.GCM_RegistrationID == profileModel.GCM_RegistrationID).FirstOrDefault();
                    //if (profileObj != null)
                    //{
                    //    //Delete from Item Matches
                    //    var ItemMatch = ItemMatchService.GetAll().Where(x => x.ProfileIdBy == profileModel.ProfileId).FirstOrDefault();
                    //    if (ItemMatch != null)
                    //    {
                    //        ItemMatchService.Delete(ItemMatch);
                    //    }
                    //    //End : Delete from Item Matches
                    //    SwapStff.Entity.Profile profileDel = new SwapStff.Entity.Profile 
                    //    { ProfileId=profileObj.ProfileId, Username=profileObj.Username,Latitude=profileObj.Latitude,Longitude=profileObj.Longitude,
                    //        DateTimeCreated=profileObj.DateTimeCreated, Distance=profileObj.Distance, GCM_RegistrationID=profileObj.GCM_RegistrationID };
                    //    ProfileService.Delete(profileDel);
                    //}
                    ////End : Delete the Old One According to GCM Registation

                    ProfileService.Insert(profile); //Save Operation
                }
                else
                {
                    //Change the Fields Becuase, NO need to change it from App
                    var profileObj = ProfileService.GetById(profileModel.ProfileId.ToString());
                    profile.Username = profileObj.Username;
                    profile.GCM_RegistrationID = profileObj.GCM_RegistrationID;
                    //End : Change the Fields Becuase, NO need to change it from App

                    ProfileService.Update(profile); //Update Operation
                }
                ProfileID = profile.ProfileId.ToString();

                return Request.CreateResponse(HttpStatusCode.OK, ProfileID, Configuration.Formatters.JsonFormatter);
            }
            catch(Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                return Request.CreateResponse(HttpStatusCode.NotImplemented, ProfileID.ToString(), Configuration.Formatters.JsonFormatter);
            }
        }

        // DELETE api/profiles/5
        //http://swapstff.com/profiles/DeleteProfile/1
        [Route("DeleteProfile/{ProfileId}")]
        [HttpGet]
        public HttpResponseMessage DeleteProfile(int ProfileId)
        {
            try
            {
                var ChatBy = ChatService.GetAll().Where(x => x.ProfileIdBy == ProfileId).FirstOrDefault();
                if (ChatBy != null)
                {
                    ChatService.Delete(ChatBy);
                }

                var ChatTo = ChatService.GetAll().Where(x => x.ProfileIdTo == ProfileId).FirstOrDefault();
                if (ChatTo != null)
                {
                    ChatService.Delete(ChatTo);
                }

                //Delete from Item Matches
                var ItemMatch = ItemMatchService.GetAll().Where(x => x.ProfileIdBy == ProfileId).FirstOrDefault();
                if (ItemMatch != null)
                {
                    ItemMatchService.Delete(ItemMatch);
                }

                var Item = Itemservice.GetAll().Where(x => x.ProfileID == ProfileId).FirstOrDefault();
                if (Item != null)
                {
                    Itemservice.Delete(Item);
                    //Delete Image from Blob
                    DeleteImageFromBlob(Item.ItemID.ToString());
                }

                //Delete from profile, It will delete from Items, Chat & Profiles
                var profile = ProfileService.GetById(ProfileId.ToString());
                ProfileService.Delete(profile);
                return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS", Configuration.Formatters.JsonFormatter);
            }
            catch(Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                return Request.CreateResponse(HttpStatusCode.NotImplemented, "ERROR", Configuration.Formatters.JsonFormatter);
            }
        }
        // DELETE api/profiles/5
        //http://swapstff.com/profiles/DeleteProfile/1
        [Route("GetProfileByUserName")]
        [HttpGet]
        public IHttpActionResult GetProfileByUserName([FromUri] string UserName)
        {
            var ProfileID = ProfileService.GetAll().Where(x=> x.Username==UserName).Select(x => x.ProfileId).FirstOrDefault().ToString();
            var Items = Itemservice.GetAll();
            var Item =(from m in Items where m.ProfileID==Convert.ToInt32(ProfileID) && m.IsActive==true select m).FirstOrDefault();

            Mapper.CreateMap<SwapStff.Entity.Item, SwapStff.Models.ItemModel>();
            SwapStff.Models.ItemModel ItemModel = Mapper.Map<SwapStff.Entity.Item, SwapStff.Models.ItemModel>(Item);
            return Json(ItemModel);
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
    }
}
