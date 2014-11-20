﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;
using SwapStff.Core.Cache;

namespace SwapStff.Service
{
    public class ProfileService : IProfileService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string PROFILES_ALL_KEY = "SwapStff.Profiles.all";

        /// <summary>
        /// Time in seconds before cache expires
        /// </summary>
        private const int PROFILES_CACHE_TIME = 2000;

        #endregion

        private IRepository<Profile> _ProfileRepository;
        private readonly ICacheManager _cacheManager;

        public ProfileService(IRepository<Profile> ProfileRepository, ICacheManager cacheManager)
        {
            this._ProfileRepository = ProfileRepository;
            this._cacheManager = cacheManager;
        }

        public List<Profile> GetAll()
        {
            //return _cacheManager.Get(PROFILES_ALL_KEY, () =>
            //{
               
            //});
            return _ProfileRepository.GetAll().ToList();
        }

        public Profile GetById(string id)
        {

            return GetAll().Find(l => l.ProfileId.ToString() == id);
        }

        public void Insert(Profile model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Profile");
            }
            
            
            List<Profile> Profiles = GetAll();
            Profiles.Add(model);
            _cacheManager.Set(PROFILES_ALL_KEY, Profiles, PROFILES_CACHE_TIME);

            _ProfileRepository.Insert(model);
        }

        public void Update(Profile model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Profile");
            }

            Profile profile = GetById(model.ProfileId.ToString());

            _cacheManager.Set(PROFILES_ALL_KEY, profile, PROFILES_CACHE_TIME);

            if (profile != null)
                _ProfileRepository.Update(model);
        }

        public void Delete(Profile model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Profile");
            }

            List<Profile> Profiles = GetAll();
            Profiles.Remove(Profiles.Find(l => l.ProfileId == model.ProfileId));
            _cacheManager.Set(PROFILES_ALL_KEY, Profiles, PROFILES_CACHE_TIME);

            _ProfileRepository.Delete(model);
        }
    }
}