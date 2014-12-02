using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;
using SwapStff.Core.Cache;

namespace SwapStff.Service
{
    public class AppSettingService : IAppSettingService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string AppSettings_ALL_KEY = "SwapStff.AppSettingss.all";

        /// <summary>
        /// Time in seconds before cache expires
        /// </summary>
        private const int AppSettings_CACHE_TIME = 2000;

        #endregion

        private IRepository<AppSetting> _AppSettingsRepository;
        private readonly ICacheManager _cacheManager;

        public AppSettingService(IRepository<AppSetting> AppSettingsRepository, ICacheManager cacheManager)
        {
            this._AppSettingsRepository = AppSettingsRepository;
            this._cacheManager = cacheManager;
        }

        public List<AppSetting> GetAll()
        {
            //return _cacheManager.Get(AppSettingsS_ALL_KEY, () =>
            //{
               
            //});
            return _AppSettingsRepository.GetAll().ToList();
        }

        public AppSetting GetById(string id)
        {

            return GetAll().Find(l => l.SettingID.ToString() == id);
        }
        public List<AppSetting> GetAppSettings()
        {
            var AppSettings = _AppSettingsRepository.GetBy(x => new
            {
                x.SettingID,
                x.MaxDistance
            }, x => x.SettingID != -1);

            var appSettingList = new List<AppSetting>();
            foreach (var item in AppSettings)
            {
                appSettingList.Add(new AppSetting
                {
                    SettingID = item.SettingID,
                    MaxDistance = item.MaxDistance
                });
            }
            return appSettingList;
        }
        public void Insert(AppSetting model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("AppSettings");
            }
            
            
            List<AppSetting> AppSettings = GetAll();
            AppSettings.Add(model);
            _cacheManager.Set(AppSettings_ALL_KEY, AppSettings, AppSettings_CACHE_TIME);

            _AppSettingsRepository.Insert(model);
        }

        public void Update(AppSetting model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("AppSettings");
            }

            AppSetting AppSettings = GetById(model.SettingID.ToString());

            _cacheManager.Set(AppSettings_ALL_KEY, AppSettings, AppSettings_CACHE_TIME);

            if (AppSettings != null)
                _AppSettingsRepository.Update(model);
        }

        public void Delete(AppSetting model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("AppSettings");
            }

            List<AppSetting> AppSettings = GetAll();
            AppSettings.Remove(AppSettings.Find(l => l.SettingID == model.SettingID));
            _cacheManager.Set(AppSettings_ALL_KEY, AppSettings, AppSettings_CACHE_TIME);

            _AppSettingsRepository.Delete(model);
        }
    }
}
