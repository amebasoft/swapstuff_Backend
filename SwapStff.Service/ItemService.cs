using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;
using SwapStff.Core.Cache;

namespace SwapStff.Service
{
    public class ItemService : IItemService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ITEMS_ALL_KEY = "SwapStff.Items.all";

        /// <summary>
        /// Time in seconds before cache expires
        /// </summary>
        private const int ITEMS_CACHE_TIME = 2000;

        #endregion

        private IRepository<Item> _ItemRepository;
        private readonly ICacheManager _cacheManager;

        public ItemService(IRepository<Item> ItemRepository, ICacheManager cacheManager)
        {
            this._ItemRepository = ItemRepository;
            this._cacheManager = cacheManager;
        }

        public List<Item> GetAll()
        {
            //return _cacheManager.Get(ITEMS_ALL_KEY, () =>
            //{
                return _ItemRepository.GetAll().ToList();
            //});
        }

        public Item GetById(string id)
        {

            //return _ItemRepository.GetById(id);
            return GetAll().Find(l => l.ItemID.ToString() == id);
        }
        public List<Item> GetItems()
        {
            var Items = _ItemRepository.GetBy(x => new { x.ItemID, x.ProfileID, x.ItemTitle, x.ItemDescription, x.ItemImage ,x.ItemDateTimeCreated, x.IsActive }, x => x.ItemID != -1);

            var itemList = new List<Item>();
            foreach (var item in Items)
            {
                itemList.Add(new Item { ItemID = item.ItemID, ProfileID = item.ProfileID, ItemTitle = item.ItemTitle, ItemDescription = item.ItemDescription, ItemImage = item.ItemImage, ItemDateTimeCreated = item.ItemDateTimeCreated, IsActive = item.IsActive });
            }
            return itemList;
        }

        public List<Item> GetItemsWOImage()
        {
            var Items = _ItemRepository.GetBy(x => new { x.ItemID, x.ProfileID,  x.ItemTitle, x.ItemDescription, x.ItemDateTimeCreated, x.IsActive }, x => x.ItemID != -1);

            var itemList =new List<Item>();
            foreach (var item in Items)
            {
                itemList.Add(new Item { ItemID=item.ItemID, ProfileID= item.ProfileID, ItemTitle= item.ItemTitle, ItemDescription=item.ItemDescription, ItemImage="", ItemDateTimeCreated= item.ItemDateTimeCreated, IsActive=item.IsActive });
            }
            return itemList;
        }

        public void Insert(Item model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Item");
            }

            List<Item> Items = GetAll();
            Items.Add(model);
            _cacheManager.Set(ITEMS_ALL_KEY, Items, ITEMS_CACHE_TIME);

            _ItemRepository.Insert(model);
        }

        public void Update(Item model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Item");
            }

            //List<Item> Items = GetAll();
            //Items.Remove(Items.Find(l => l.ItemID == model.ItemID));
            //Items.Add(model);
            //_cacheManager.Set(ITEMS_ALL_KEY, Items, ITEMS_CACHE_TIME);

            Item item = GetById(model.ItemID.ToString());

            _cacheManager.Set(ITEMS_ALL_KEY, item, ITEMS_CACHE_TIME);

            if (item != null)
                _ItemRepository.Update(model);

            //_ItemRepository.Update(model);
        }

        public void Delete(Item model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Item");
            }

            List<Item> Items = GetAll();
            Items.Remove(Items.Find(l => l.ItemID == model.ItemID));
            _cacheManager.Set(ITEMS_ALL_KEY, Items, ITEMS_CACHE_TIME);

            _ItemRepository.Delete(model);
        }
    }
}
