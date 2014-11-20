using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;
using SwapStff.Core.Cache;

namespace SwapStff.Service
{
    public class ItemMatchService : IItemMatchService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ITEMMATCHS_ALL_KEY = "SwapStff.ItemMatchs.all";

        /// <summary>
        /// Time in seconds before cache expires
        /// </summary>
        private const int ITEMMATCHS_CACHE_TIME = 2000;

        #endregion

        private IRepository<ItemMatch> _ItemMatchRepository;
        private readonly ICacheManager _cacheManager;

        public ItemMatchService(IRepository<ItemMatch> ItemMatchRepository, ICacheManager cacheManager)
        {
            this._ItemMatchRepository = ItemMatchRepository;
            this._cacheManager = cacheManager;
        }

        public List<ItemMatch> GetAll()
        {
            //return _cacheManager.Get(ITEMMATCHS_ALL_KEY, () =>
            //{
                return _ItemMatchRepository.GetAll().ToList();
            //});
        }

        public ItemMatch GetById(string id)
        {
            return GetAll().Find(l => l.ItemMatchID.ToString() == id);
        }

        public void Insert(ItemMatch model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("ItemMatch");
            }

            List<ItemMatch> ItemMatchs = GetAll();
            ItemMatchs.Add(model);
            _cacheManager.Set(ITEMMATCHS_ALL_KEY, ItemMatchs, ITEMMATCHS_CACHE_TIME);

            _ItemMatchRepository.Insert(model);
        }

        public void Update(ItemMatch model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("ItemMatch");
            }

            List<ItemMatch> ItemMatchs = GetAll();
            ItemMatchs.Remove(ItemMatchs.Find(l => l.ItemMatchID == model.ItemMatchID));
            ItemMatchs.Add(model);
            _cacheManager.Set(ITEMMATCHS_ALL_KEY, ItemMatchs, ITEMMATCHS_CACHE_TIME);

            _ItemMatchRepository.Update(model);
        }

        public void Delete(ItemMatch model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("ItemMatch");
            }

            List<ItemMatch> ItemMatchs = GetAll();
            ItemMatchs.Remove(ItemMatchs.Find(l => l.ItemMatchID == model.ItemMatchID));
            _cacheManager.Set(ITEMMATCHS_ALL_KEY, ItemMatchs, ITEMMATCHS_CACHE_TIME);

            _ItemMatchRepository.Delete(model);
        }
    }
}
