using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using SwapStff.Models;
using SwapStff.Service;
using SwapStff.Infrastructure;
namespace SwapStff.Controllers
{
    public class UserChatsController : Controller
    {
        
        public IProfileService ProfileService { get; set; }
        public IChatService ChatService { get; set; }
        public IItemService Itemservice { get; set; }
        public UserChatsController(IProfileService ProfileService, IChatService ChatService, IItemService Itemservice)
        {
            this.ProfileService = ProfileService;
            this.ChatService = ChatService;
            this.Itemservice = Itemservice;
        }

        // GET: /UserChats/
        public ActionResult Index()
        {
            var models = GetChatList();
            
            return View(models);
        }

        public List<UserChatsModel> GetChatList()
        {
            var Chats = ChatService.GetChats();
            var Items = Itemservice.GetItems();

            var models = new List<UserChatsModel>();

            var Results = (from c in Chats
                           join i in Items on c.ItemID equals i.ItemID
                           orderby i.ItemTitle
                           select new
                           {
                               ChatId = c.ChatId,
                               ItemIDBy = i.ItemID,
                               ItemTitleBy = i.ItemTitle,
                               ItemImageBy = i.ItemImage,
                               ProfileIdBy = c.ProfileIdBy,
                               ProfileIdTo = c.ProfileIdTo,
                               ChatContent = c.ChatContent,
                               DateTimeCreated = c.DateTimeCreated,
                               IsRead = c.IsRead
                           }).ToList();

            foreach (var item in Results)
            {
                var ItemToResult = Items.Where(m => m.ProfileID == item.ProfileIdTo && m.IsActive == true).Select(m => new { m.ItemID, m.ItemTitle, m.ItemImage }).FirstOrDefault();

                models.Add(new UserChatsModel
                {
                    ChatId = item.ChatId,
                    ItemIDBy = item.ItemIDBy,
                    ItemTitleBy = item.ItemTitleBy,
                    ItemImageBy = item.ItemImageBy,
                    ProfileIdBy = item.ProfileIdBy,
                    ItemIDTo = ItemToResult.ItemID,
                    ItemTitleTo = ItemToResult.ItemTitle,
                    ItemImageTo = ItemToResult.ItemImage,
                    ProfileIdTo = item.ProfileIdTo,
                    ChatContent = item.ChatContent,
                    DateTimeCreated = item.DateTimeCreated,
                    IsRead = item.IsRead
                });
            }

            return models;
        }
        //
        // GET: /UserChats/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

       
        // GET: /UserChats/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /UserChats/Edit/5
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
        // GET: /UserItems/Delete/5
        public ActionResult Delete(int ChatId)
        {
            if (ChatId <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var chat = GetChatList().Where(m => m.ChatId==ChatId).FirstOrDefault();

            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        //
        // POST: /UserItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ChatId)
        {
            try
            {
                var chat = ChatService.GetAll().Where(x => x.ChatId == ChatId).FirstOrDefault();
                ChatService.Delete(chat);

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
