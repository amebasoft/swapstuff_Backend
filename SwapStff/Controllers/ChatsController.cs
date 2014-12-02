using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using System.Web;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;


using SwapStff.Models;
using SwapStff.Service;
using SwapStff.Infrastructure;

namespace SwapStff.Controllers
{
    [RoutePrefix("Chats")]
    public class ChatsController : ApiController
    {
        public IProfileService ProfileService { get; set; }
        public IChatService ChatService { get; set; }
        public IItemService Itemservice { get; set; }
        public ChatsController(IProfileService ProfileService, IChatService ChatService, IItemService Itemservice)
        {
            this.ProfileService = ProfileService;
            this.ChatService = ChatService;
            this.Itemservice = Itemservice;
        }

        // GET api/Chats
        //http://swapstff.com/Chats/GetAllChats
        [Route("GetAllChats")]
        [HttpGet]
        public IHttpActionResult GetAllChats()
        {
            var Chats = ChatService.GetAll();
            var models = new List<ChatModel>();
            Mapper.CreateMap<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>();
            foreach (var Chat in Chats)
            {
                //SwapStff.Models.ChatModel ChatModel = Mapper.Map<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>(Chat);
                models.Add(Mapper.Map<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>(Chat));
            }
            return Json(models);
        }

        // GET api/Chats
        //http://swapstff.com/Chats/GetChat/1
        [Route("GetChat/{ChatId}")]
        [HttpGet]
        public IHttpActionResult GetChat(int ChatId)
        {
            var Chat = ChatService.GetById(ChatId.ToString());
            Mapper.CreateMap<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>();
            SwapStff.Models.ChatModel ChatModel = Mapper.Map<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>(Chat);
            return Json(ChatModel);
        }


        // GET api/Chats
        //http://swapstff.com/Chats/GetChatList/{Json}
        [Route("GetChatList")]
        [HttpPost]
        public IHttpActionResult GetChatList([FromBody]ChatModel ChatModel)
        {
            List<int?> ListProfileIDs =new List<int?>(); 
            ListProfileIDs.Add(ChatModel.ProfileIdBy);
            ListProfileIDs.Add(ChatModel.ProfileIdTo);

            var Chats = ChatService.GetAll().Where( x=> ListProfileIDs.Contains(x.ProfileIdBy) && ListProfileIDs.Contains(x.ProfileIdTo)).ToList();
            
            var ItemIDTo = Itemservice.GetItemsWOImage().Where(m => m.ProfileID == ChatModel.ProfileIdTo && m.IsActive == true).Select(m => m.ItemID).FirstOrDefault();   //(from m in Items where m.ProfileID == ChatModel.ProfileIdTo && m.IsActive == true select m.ItemID).FirstOrDefault();

            var Results = (from m in Chats where m.ItemID==ChatModel.ItemID && m.ProfileIdBy==ChatModel.ProfileIdBy && m.ProfileIdTo==ChatModel.ProfileIdTo select m)
                          .Concat(from m in Chats where m.ItemID == ItemIDTo && m.ProfileIdBy == ChatModel.ProfileIdTo && m.ProfileIdTo==ChatModel.ProfileIdBy select m)
                          .OrderBy(x => x.DateTimeCreated);

            var models = new List<ChatModel>();

            Mapper.CreateMap<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>();
            foreach (var Chat in Results)
            {
                //When Our Profile ID (By) will match with Others ProfileID (To) then Update Read Message status
                if ((Chat.ProfileIdTo == ChatModel.ProfileIdBy) && (Chat.ItemID == ItemIDTo))
                {
                    //Update the Chat For IsRead Field true, It will Update by Trigger in SQL Table
                    ChatService.Update(new SwapStff.Entity.Chat {ChatId=Chat.ChatId,ItemID=Chat.ItemID,ProfileIdBy=Chat.ProfileIdBy,
                        ProfileIdTo=Chat.ProfileIdTo, ChatContent=Chat.ChatContent,DateTimeCreated=Chat.DateTimeCreated,IsRead=true });
                    //End : Update the Chat For IsRead Field true

                    //Set the Read Message Status for List
                    Chat.IsRead = true;
                }

                models.Add(Mapper.Map<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>(Chat));
            }
            return Json(models);
        }

        // GET api/Chats
        //http://swapstff.com/Chats/GetChatList/{Json}
        [Route("GetChatListUnread")]
        [HttpPost]
        public IHttpActionResult GetChatListUnread([FromBody]ChatModel ChatModel)
        {
            List<int?> ListProfileIDs = new List<int?>();
            ListProfileIDs.Add(ChatModel.ProfileIdBy);
            ListProfileIDs.Add(ChatModel.ProfileIdTo);

            var Chats = ChatService.GetAll().Where(x => ListProfileIDs.Contains(x.ProfileIdBy) && ListProfileIDs.Contains(x.ProfileIdTo)).ToList();

            var ItemIDTo = Itemservice.GetItemsWOImage().Where(m => m.ProfileID == ChatModel.ProfileIdTo && m.IsActive == true).Select(m => m.ItemID).FirstOrDefault();   //(from m in Items where m.ProfileID == ChatModel.ProfileIdTo && m.IsActive == true select m.ItemID).FirstOrDefault();

            var Results = (from m in Chats where m.ItemID == ChatModel.ItemID && m.ProfileIdBy == ChatModel.ProfileIdBy && m.ProfileIdTo == ChatModel.ProfileIdTo && m.IsRead==false select m)
                          .Concat(from m in Chats where m.ItemID == ItemIDTo && m.ProfileIdBy == ChatModel.ProfileIdTo && m.ProfileIdTo == ChatModel.ProfileIdBy && m.IsRead == false select m)
                          .OrderBy(x => x.DateTimeCreated);

            var models = new List<ChatModel>();

            Mapper.CreateMap<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>();
            foreach (var Chat in Results)
            {
                //When Our Profile ID (By) will match with Others ProfileID (To) then Update Read Message status
                if ((Chat.ProfileIdTo == ChatModel.ProfileIdBy) && (Chat.ItemID == ItemIDTo) && (Chat.IsRead==false))
                {
                    //Update the Chat For IsRead Field true, It will Update by Trigger in SQL Table
                    ChatService.Update(new SwapStff.Entity.Chat
                    {
                        ChatId = Chat.ChatId,
                        ItemID = Chat.ItemID,
                        ProfileIdBy = Chat.ProfileIdBy,
                        ProfileIdTo = Chat.ProfileIdTo,
                        ChatContent = Chat.ChatContent,
                        DateTimeCreated = Chat.DateTimeCreated,
                        IsRead = true
                    });
                    //End : Update the Chat For IsRead Field true

                    //Set the Read Message Status for List
                    Chat.IsRead = true;

                    models.Add(Mapper.Map<SwapStff.Entity.Chat, SwapStff.Models.ChatModel>(Chat));
                }
                
            }
            return Json(models);
        }

        // GET api/Chats
        //http://swapstff.com/Chats/GetChatNotificationList/{Json}
        [Route("GetChatNotificationList")]
        [HttpPost]
        public IHttpActionResult GetChatNotificationList([FromBody]ChatModel ChatModel)
        {
            var Chats = ChatService.GetAll().ToList();
            
            //Sample Query
            //from m in Chats
            //where (from c in Chats where c.ItemID==60 && c.ProfileIdBy==91 select c.ProfileIdTo).Distinct().Contains(m.ProfileIdBy)
            //&& m.ProfileIdTo==91 && m.IsRead==false
            //group m by new{m.ItemID,m.ProfileIdBy} into g
            //select new {ItemID=g.Key.ItemID, ProfileIdBy=g.Key.ProfileIdBy,Count=g.Count()}


            var Results = (from m in Chats
                           where (from c in Chats where c.ItemID == ChatModel.ItemID && c.ProfileIdBy == ChatModel.ProfileIdBy select c.ProfileIdTo).Distinct().Contains(m.ProfileIdBy)
                           && m.ProfileIdTo == ChatModel.ProfileIdBy && m.IsRead == false
                           group m by new { m.ItemID, m.ProfileIdBy } into g
                           select new { ItemID = g.Key.ItemID, ProfileIdBy = g.Key.ProfileIdBy, Count = g.Count() });

            var models = new List<ChatModel>();
            foreach (var Chat in Results)
            {

                models.Add(new ChatModel { ChatId = 0, ItemID = Chat.ItemID, ProfileIdBy = Chat.ProfileIdBy, ProfileIdTo = ChatModel.ProfileIdBy,
                                           ChatContent = "",CountMessage=Chat.Count});
            }
            return Json(models);
        }
        // POST api/Chats
        //http://swapstff.com/Chats/SaveChat/{Json}
        [Route("SaveChat")]
        [HttpPost]
        public HttpResponseMessage SaveChat([FromBody]ChatModel ChatModel)
        {
            string ChatID = "-1";
            try
            {
                if (ChatModel.ChatContent.Trim() != "")
                {
                    Mapper.CreateMap<SwapStff.Models.ChatModel, SwapStff.Entity.Chat>();
                    SwapStff.Entity.Chat Chat = Mapper.Map<SwapStff.Models.ChatModel, SwapStff.Entity.Chat>(ChatModel);
                    if (ChatModel.ChatId <= 0)
                    {
                        ChatService.Insert(Chat); //Save Operation
                    }
                    else
                    {
                        ChatService.Update(Chat); //Update Operation
                    }
                    ChatID = Chat.ChatId.ToString();
                }

                //Code For GCM
                var profile = ProfileService.GetById(ChatModel.ProfileIdTo.ToString());

                if (profile != null)
                {
                    if (profile.ChatNotification==1)
                    {
                        var Chats = ChatService.GetAll().ToList();
                        var ItemIDTo = Itemservice.GetItemsWOImage().Where(m => m.ProfileID == ChatModel.ProfileIdTo && m.IsActive == true).Select(m => m.ItemID).FirstOrDefault();

                        var Results = (from m in Chats
                                       where m.ItemID == ChatModel.ItemID && m.ProfileIdTo == ChatModel.ProfileIdTo && m.IsRead == false
                                       group m by new { m.ItemID, m.ProfileIdTo } into g
                                       select new
                                       {
                                           ItemID = g.Key.ItemID,
                                           ProfileIdBy = g.Key.ProfileIdTo,
                                           ChatContent = (from c in Chats
                                                          where c.ProfileIdTo == ChatModel.ProfileIdTo && c.IsRead == false
                                                          orderby c.DateTimeCreated descending
                                                          select c.ChatContent).FirstOrDefault(),
                                           Count = g.Count()
                                       }).ToList();

                        var models = new List<ChatModel>();
                        var ChatMessage = "";
                        if (Results.Count() > 0)
                        {
                            //Message Send as Json Style

                            //[{ItemIDTo:101,ProfileIDTo:201,ChatContent:"This is message",CountMessage:10},
                            //{ItemIDTo:102,ProfileIDTo:203,ChatContent:"Message",CountMessage:5},
                            //{ItemIDTo:103,ProfileIDTo:204,ChatContent:"JJJJ Message",CountMessage:15}]
                            int MessageCount = Convert.ToInt32(Results.Sum(x => x.Count).ToString());
                            //ChatMessage = "[";

                            //foreach (var Chat in Results)
                            //{
                            //    //ChatMessage = ChatMessage + "{ItemIDTo:" + Chat.ItemID + ",ProfileIDTo:" + Chat.ProfileIdBy + ",ChatContent:" + Chat.ChatContent + ",CountMessage:" + Chat.Count + "},";
                            //}
                            //ChatMessage = ChatMessage.Substring(0, ChatMessage.Length - 1);
                            //ChatMessage = ChatMessage + "]";

                            ChatMessage = "You have " + MessageCount + " new message(s).";

                            string GCM_RegistrationID = profile.GCM_RegistrationID;
                            string Result = SendGCM_Notifications(GCM_RegistrationID, ChatMessage);
                        }
                    }
                }
                //End : Code For GCM

                //string GCM_RegistrationID = ProfileService.GetById(ChatModel.ProfileIdTo.ToString()).GCM_RegistrationID;
                //string Result = SendGCM_Notifications(GCM_RegistrationID, ChatModel.ChatContent);

                //string GCM_RegistrationID = "APA91bGxDhXZefRa1Z6oUesCshZWBI2PvktYCZV2dJ8MhxXRhoV6H0hqGM9fcmqcqsWTlvGouSJbmZO-AogAZ9yFO0bS8Tb_PONWkVpdclt1nxwA2hqMjbQmCS5nOHFUtFbQJf1p3qHWAw6mYtFUpbM_zN_bYEftHA";
                //string Result = SendGCM_Notifications(GCM_RegistrationID, ChatModel.ChatContent);

                return Request.CreateResponse(HttpStatusCode.OK, ChatID, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.NotImplemented, ChatID.ToString(), Configuration.Formatters.JsonFormatter);
            }
        }

        // DELETE api/Chats/5
        //http://swapstff.com/Chats/DeleteChat/1
        [Route("DeleteChat/{ChatId}")]
        [HttpGet]
        public HttpResponseMessage DeleteChat(int ChatId)
        {
            try
            {
                var Chat = ChatService.GetById(ChatId.ToString());
                ChatService.Delete(Chat);
                return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS", Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging.LogError(ex);
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
                    string postData = "{ \"collapse_key\": \"swapstffnew \",  \"time_to_live\": 0,  \"delay_while_idle\": false,  \"data\": {    \"message\": \"" + value + "\",    \"time\": \"" + System.DateTime.Now.ToString() + "\" },  \"registration_ids\":[\"" + regId.Replace("'", "\"") + "\"]}";

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
                    // SaveLog.LogError("Server reponse for Push Notifications : - " + sResponseFromServer.ToString());
                    // SaveLog.LogError("---------------Push Notifications Finished----------------");
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
                //SaveLog.LogError("Server Error=" + e.Response.ToString());
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

        //public string SendNotification(string deviceId, string message)
        //{
        //    string GoogleAppID = "AIzaSyAQ3vYXZ892sbP5R1l0SPsg4APBpBwtpUQ";
        //    var SENDER_ID = "549193984962";
        //    var value = message;
        //    WebRequest tRequest;
        //    tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
        //    tRequest.Method = "post";
        //    tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
        //    tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

        //    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

        //    string postData = "collapse_key=swapstff&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "registration_id=" + deviceId + "";
        //    Console.WriteLine(postData);
        //    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    tRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = tRequest.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse tResponse = tRequest.GetResponse();

        //    dataStream = tResponse.GetResponseStream();

        //    StreamReader tReader = new StreamReader(dataStream);

        //    String sResponseFromServer = tReader.ReadToEnd();

        //    tReader.Close();
        //    dataStream.Close();
        //    tResponse.Close();
        //    return sResponseFromServer;
        //}
    }
}
