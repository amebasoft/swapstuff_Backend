using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;
using SwapStff.Core.Cache;

namespace SwapStff.Service
{
    public class ChatService : IChatService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CHATS_ALL_KEY = "SwapStff.Chats.all";

        /// <summary>
        /// Time in seconds before cache expires
        /// </summary>
        private const int CHATS_CACHE_TIME = 2000;

        #endregion

        private IRepository<Chat> _ChatRepository;
        private readonly ICacheManager _cacheManager;

        public ChatService(IRepository<Chat> ChatRepository, ICacheManager cacheManager)
        {
            this._ChatRepository = ChatRepository;
            this._cacheManager = cacheManager;
        }

        public List<Chat> GetAll()
        {
            //return _cacheManager.Get(CHATS_ALL_KEY, () =>
            //{
                return _ChatRepository.GetAll().ToList();
            //});
        }

        public Chat GetById(string id)
        {
            return GetAll().Find(l => l.ChatId.ToString() == id);
        }

        public List<Chat> GetChats()
        {
            var Chats = _ChatRepository.GetBy(x => new
            {
                x.ChatId,
                x.ItemID,
                x.ProfileIdBy,
                x.ProfileIdTo,
                x.ChatContent,
                x.DateTimeCreated,
                x.IsRead
            }, x => x.ChatId != -1);

            var chatList = new List<Chat>();
            foreach (var item in Chats)
            {
                chatList.Add(new Chat
                {
                    ChatId=item.ChatId,
                    ItemID=item.ItemID,
                    ProfileIdBy=item.ProfileIdBy,
                    ProfileIdTo=item.ProfileIdTo,
                    ChatContent=item.ChatContent,
                    DateTimeCreated=item.DateTimeCreated,
                    IsRead=item.IsRead
                });
            }
            return chatList;
        }

        public void Insert(Chat model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Chat");
            }

            List<Chat> Chats = GetAll();
            Chats.Add(model);
            //_cacheManager.Set(CHATS_ALL_KEY, Chats, CHATS_CACHE_TIME);

            _ChatRepository.Insert(model);
        }

        public void Update(Chat model)
        {
            //if (model == null)
            //{
            //    throw new ArgumentNullException("Chat");
            //}

            //List<Chat> Chats = GetAll();
            //Chats.Remove(Chats.Find(l => l.ChatId == model.ChatId));
            //Chats.Add(model);
            //_cacheManager.Set(CHATS_ALL_KEY, Chats, CHATS_CACHE_TIME);

            //_ChatRepository.Update(model);


            if (model == null)
            {
                throw new ArgumentNullException("Chat");
            }

            Chat chat = GetById(model.ChatId.ToString());

            _cacheManager.Set(CHATS_ALL_KEY, chat, CHATS_CACHE_TIME);

            if (chat != null)
                _ChatRepository.Update(model);
        }

        public void Delete(Chat model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Chat");
            }

            List<Chat> Chats = GetAll();
            Chats.Remove(Chats.Find(l => l.ChatId == model.ChatId));
            _cacheManager.Set(CHATS_ALL_KEY, Chats, CHATS_CACHE_TIME);

            _ChatRepository.Delete(model);
        }
    }
}
