using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;
using AutoMapper;

using SwapStff.Models;
using SwapStff.Service;
using SwapStff.Core.UtilityManager;
using SwapStff.Infrastructure;

namespace SwapStff.Controllers
{
    [RoutePrefix("ItemMatchs")]
    public class ItemMatchsController : ApiController
    {
        public IProfileService ProfileService { get; set; }
        public IItemService Itemservice { get; set; }
        public IItemMatchService ItemMatchService { get; set; }
        public IChatService ChatService { get; set; }

        public ItemMatchsController(IProfileService ProfileService, IItemService Itemservice, IItemMatchService ItemMatchService, IChatService ChatService)
        {
            this.ProfileService = ProfileService;
            this.Itemservice = Itemservice;
            this.ItemMatchService = ItemMatchService;
             this.ChatService = ChatService;
        }

        // GET api/ItemMatchs
        //http://swapstff.com/ItemMatchs/GetAllItemMatchs
        [Route("GetAllItemMatchs")]
        [HttpGet]
        public IHttpActionResult GetAllItemMatchs()
        {
            var ItemMatchs = ItemMatchService.GetAll();
            var models = new List<ItemMatchModel>();
            Mapper.CreateMap<SwapStff.Entity.ItemMatch, SwapStff.Models.ItemMatchModel>();
            foreach (var ItemMatch in ItemMatchs)
            {
                //SwapStff.Models.ItemMatchModel ItemMatchModel = Mapper.Map<SwapStff.Entity.ItemMatch, SwapStff.Models.ItemMatchModel>(ItemMatch);
                models.Add(Mapper.Map<SwapStff.Entity.ItemMatch, SwapStff.Models.ItemMatchModel>(ItemMatch));
            }
            return Json(models);
        }

        // GET api/ItemMatchs
        //http://swapstff.com/ItemMatchs/GetItemMatch/1
        [Route("GetItemMatch/{ItemMatchId}")]
        [HttpGet]
        public IHttpActionResult GetItemMatch(int ItemMatchId)
        {
            var ItemMatch = ItemMatchService.GetById(ItemMatchId.ToString());
            Mapper.CreateMap<SwapStff.Entity.ItemMatch, SwapStff.Models.ItemMatchModel>();
            SwapStff.Models.ItemMatchModel ItemMatchModel = Mapper.Map<SwapStff.Entity.ItemMatch, SwapStff.Models.ItemMatchModel>(ItemMatch);
            return Json(ItemMatchModel);
        }


        // GET api/ItemMatchs
        //http://swapstff.com/ItemMatchs/GetItemMatchList/
        [Route("GetItemMatchList")]
        [HttpPost]
        public IHttpActionResult GetItemMatchList([FromBody]ItemMatchModel ItemMatchModel)
        {
            var Items = Itemservice.GetAll();
            var ItemMatchs = ItemMatchService.GetAll();
            var Profiles = ProfileService.GetAll();
            var Chats = ChatService.GetAll().ToList();

            var ItemMatchList = (from im in ItemMatchs
                                 join i in Items on im.ProfileIdBy equals i.ProfileID
                                 join p in Profiles on i.ProfileID equals p.ProfileId
                                 where (im.ItemID == ItemMatchModel.ItemID) && (im.IsLikeDislikeAbuseBy == 1)
                                 orderby im.DateTimeCreated descending
                                 select new
                                 {
                                     im.ItemMatchID,
                                     ItemID = i.ItemID,
                                     im.ProfileIdBy,
                                     im.Distance,
                                     IsLikeDislikeAbuseBy = (from imInner in ItemMatchs
                                                             where imInner.ItemID == i.ItemID && imInner.ProfileIdBy == ItemMatchModel.ProfileIdBy && imInner.IsLikeDislikeAbuseBy == 1
                                                             select imInner.IsLikeDislikeAbuseBy).FirstOrDefault(),
                                     im.DateTimeCreated,
                                     i.ItemTitle,
                                     i.ItemImage,
                                     p.Latitude,
                                     p.Longitude
                                 }).ToList();

            var Results = (from m in Chats
                           where m.ProfileIdTo == ItemMatchModel.ProfileIdBy && m.IsRead == false
                           group m by new { m.ItemID, m.ProfileIdBy } into g
                           select new { ItemID = g.Key.ItemID,
                                        ProfileIdBy = g.Key.ProfileIdBy,
                                        ChatContent = (from c in Chats
                                                       where c.ProfileIdTo == ItemMatchModel.ProfileIdBy && c.IsRead == false
                                                       orderby c.DateTimeCreated descending
                                                       select c.ChatContent).FirstOrDefault(),
                                        ChatDateTime = (from c in Chats
                                                        where c.ProfileIdTo == ItemMatchModel.ProfileIdBy && c.IsRead == false
                                                       orderby c.DateTimeCreated descending
                                                       select c.DateTimeCreated).FirstOrDefault(),
                                        Count = g.Count() });

            Int32 DistanceBy = Convert.ToInt32(ProfileService.GetById(ItemMatchModel.ProfileIdBy.ToString()).Distance);
            double LatitudeProfileBy = Convert.ToDouble(ProfileService.GetById(ItemMatchModel.ProfileIdBy.ToString()).Latitude);
            double LongitudeProfileBy = Convert.ToDouble(ProfileService.GetById(ItemMatchModel.ProfileIdBy.ToString()).Longitude);

            var models = new List<ItemMatchModel>();
            //Mapper.CreateMap<SwapStff.Entity.ItemMatch, SwapStff.Models.ItemMatchModel>();
            foreach (var ItemMatch in ItemMatchList)
            {
                if (ItemMatch.IsLikeDislikeAbuseBy == 1)
                {
                    double DistanceTo = 0;
                    double LatitudeProfileTo = Convert.ToDouble(ItemMatch.Latitude);
                    double LongitudeProfileTo = Convert.ToDouble(ItemMatch.Longitude);

                    DistanceTo = GoogleDistance.Calc(LatitudeProfileTo, LongitudeProfileTo, LatitudeProfileBy, LongitudeProfileBy);

                    Nullable<System.DateTime> _DateTimeCreated = ItemMatch.DateTimeCreated;
                    string _ChatMessage ="";
                    Nullable<int> _MessageCount = 0;

                    var ChatFound = Results.Where(x => x.ProfileIdBy == ItemMatch.ProfileIdBy);

                    var RecentChatDateTime = (from c in Chats
                                              where c.ProfileIdTo == ItemMatch.ProfileIdBy 
                                                       orderby c.DateTimeCreated descending
                                                       select c.DateTimeCreated).FirstOrDefault();

                    if (RecentChatDateTime != null)
                    {
                        _DateTimeCreated = RecentChatDateTime;
                    }
                    if (ChatFound.Count() > 0)
                    {
                        //_DateTimeCreated = Results.Where(x => x.ProfileIdBy == ItemMatch.ProfileIdBy).Select(x => x.ChatDateTime).FirstOrDefault();
                        _ChatMessage = Results.Where(x => x.ProfileIdBy == ItemMatch.ProfileIdBy).Select(x => x.ChatContent).FirstOrDefault();
                        _MessageCount = Results.Where(x => x.ProfileIdBy == ItemMatch.ProfileIdBy).Select(x => x.Count).FirstOrDefault();
                    }

                    //models.Add(Mapper.Map<SwapStff.Entity.ItemMatch, SwapStff.Models.ItemMatchModel>(ItemMatch));
                    models.Add(new ItemMatchModel
                    {
                        ItemMatchID = ItemMatch.ItemMatchID,
                        ItemID = ItemMatch.ItemID,
                        ProfileIdBy = ItemMatch.ProfileIdBy,
                        Distance =Convert.ToDecimal(DistanceTo),
                        IsLikeDislikeAbuseBy = ItemMatch.IsLikeDislikeAbuseBy,
                        DateTimeCreated = _DateTimeCreated,
                        ItemTitle = ItemMatch.ItemTitle,
                        ItemImage = ItemMatch.ItemImage,
                        ChatMessage = _ChatMessage,
                        MessageCount = _MessageCount
                    });
                }
            }
            
            return Json(models.OrderByDescending(x=> x.DateTimeCreated));
        }

        // POST api/ItemMatchs
        //http://swapstff.com/ItemMatchs/SaveItemMatch/{Json}
        [Route("SaveItemMatch")]
        [HttpPost]
        public HttpResponseMessage SaveItemMatch([FromBody]ItemMatchModel ItemMatchModel)
        {
            string ItemMatchID = "-1";
            try
            {
                Mapper.CreateMap<SwapStff.Models.ItemMatchModel, SwapStff.Entity.ItemMatch>();
                SwapStff.Entity.ItemMatch ItemMatch = Mapper.Map<SwapStff.Models.ItemMatchModel, SwapStff.Entity.ItemMatch>(ItemMatchModel);
                if (ItemMatchModel.ItemMatchID <= 0)
                {
                    //Roll Back the old one
                    var ItemMatchDel = ItemMatchService.GetAll().Where(x => x.ItemID==ItemMatchModel.ItemID && x.ProfileIdBy == ItemMatchModel.ProfileIdBy).FirstOrDefault();

                    if (ItemMatchDel != null)
                    {
                        ItemMatchService.Delete(ItemMatchDel);
                    }
                    //End : Roll Back the old one

                    ItemMatchService.Insert(ItemMatch); //Save Operation
                }
                else
                {
                    ItemMatchService.Update(ItemMatch); //Update Operation
                }
                ItemMatchID = ItemMatch.ItemMatchID.ToString();
                var items = Itemservice.GetItemsWOImage().ToList();

                var ItemIDTo = -1; 
                var ProfileIDTo = -1;
                ItemMatchID = "NotMatched";

                if (items.Count() > 0)
                {
                    ItemIDTo=Itemservice.GetItemsWOImage().Where(m => m.ProfileID == ItemMatchModel.ProfileIdBy && m.IsActive == true).Select(m => m.ItemID).FirstOrDefault();
                    ProfileIDTo=Itemservice.GetItemsWOImage().Where(m => m.ItemID == ItemMatchModel.ItemID).Select(m => m.ProfileID).FirstOrDefault();
                }
                var ItemMatchs = ItemMatchService.GetAll().Where(x=> x.ItemID==ItemIDTo && x.ProfileIdBy==ProfileIDTo && x.IsLikeDislikeAbuseBy==1).Select(x=> new{x.ItemMatchID,x.ItemID}).FirstOrDefault();
                if ((ItemMatchs != null) && (ItemMatchModel.IsLikeDislikeAbuseBy==1))
                {
                    string ChatMessage = "New Matches found.";
                    var profileTo = ProfileService.GetById(ProfileIDTo.ToString());

                    if (profileTo != null)
                    {
                        //ProfileID To
                        string GCM_RegistrationIDTo = profileTo.GCM_RegistrationID;
                        if (profileTo.ItemMatchNotification == 1)
                        {
                            string Result = SendGCM_Notifications(GCM_RegistrationIDTo, ChatMessage);
                        }

                        //ProfileID By
                        var profileBy = ProfileService.GetById(ItemMatchModel.ProfileIdBy.ToString());
                        if (profileBy != null)
                        {
                            string GCM_RegistrationIDBy = profileBy.GCM_RegistrationID;
                            if (profileBy.ItemMatchNotification == 1)
                            {
                                ItemMatchID = "Matched";
                                string Result = SendGCM_Notifications(GCM_RegistrationIDBy, ChatMessage);
                            }
                        }
                    }
                    
                }

                return Request.CreateResponse(HttpStatusCode.OK, ItemMatchID, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                return Request.CreateResponse(HttpStatusCode.NotImplemented, ItemMatchID.ToString(), Configuration.Formatters.JsonFormatter);
            }
        }

        // DELETE api/ItemMatchs/5
        //http://swapstff.com/ItemMatchs/DeleteItemMatch/1
        [Route("DeleteItemMatch/{ItemMatchId}")]
        [HttpGet]
        public HttpResponseMessage DeleteItemMatch(int ItemMatchId)
        {
            try
            {
                var ItemMatch = ItemMatchService.GetById(ItemMatchId.ToString());
                ItemMatchService.Delete(ItemMatch);
                return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS", Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                return Request.CreateResponse(HttpStatusCode.NotImplemented, "ERROR", Configuration.Formatters.JsonFormatter);
            }
        }

        private string SendGCM_Notifications(string regId, string value)
        {

            //SaveLog.serverpath = System.Web.HttpContext.Current.Server.MapPath(@"\MessageLog\MessageLog.log");
            //SaveLog.LogError("---------------Push Notifications Start----------------");
            //SaveLog.LogError("Push Notification start for RegId=" + regId.ToString());

            try
            {
                if (true)
                {
                    //regId = regId.TrimEnd(',').ToString();

                    //Bharat Testing
                    //regId = "APA91bGGmOXoREb_7Z8nHS2iFhdjIVvuCa888Xcebheh1opCLFFuWjiUkJl9tmKJ0mNRySd0Xri639Jc2kfRoGOF3FSaD_jiOB_dXHR-4nSnaLXVb6mW2foW0hZbUveYCjud2E3Y3vH61KyVLBco42SpMmR1dv6RsA";

                    var applicationID = ConfigurationManager.AppSettings["GCM_ApplicationID"].ToString();

                    var SENDER_ID = ConfigurationManager.AppSettings["GCM_SenderID"].ToString();
                    // var value = dtGCMRegistrationID.Rows[0].ToString();
                    //WebRequest tRequest;
                    var tRequest = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json"; //" application/x-www-form-urlencoded;charset=UTF-8";

                    tRequest.Headers.Add(HttpRequestHeader.Authorization, "key=" + applicationID + "");

                    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                    //Data_Post Format
                    string postData = "{ \"collapse_key\": \"swapstuffnew \",  \"time_to_live\": 0,  \"delay_while_idle\": false,  \"data\": {    \"message\": \"" + value + "\",    \"time\": \"" + System.DateTime.Now.ToString() + "\" },  \"registration_ids\":[\"" + regId.Replace("'", "\"") + "\"]}";

                    Console.WriteLine(postData);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    tRequest.ContentLength = byteArray.Length;

                    Stream dataStream = tRequest.GetRequestStream();

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    WebResponse tResponse = tRequest.GetResponse();

                    dataStream = tResponse.GetResponseStream();

                    StreamReader tReader = new StreamReader(dataStream);

                    String sResponseFromServer = tReader.ReadToEnd();

                    // Label3.Text = sResponseFromServer; //printing response from GCM server.
                    //SaveLog.LogError("Server reponse for Push Notifications : - " + sResponseFromServer.ToString());
                    //SaveLog.LogError("---------------Push Notifications Finished----------------");
                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                    return sResponseFromServer;
                }
                //else
                //{
                //    return "Nothing to send";
                //} 
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        return text;
                    }
                }
            }

        }
    }
}
