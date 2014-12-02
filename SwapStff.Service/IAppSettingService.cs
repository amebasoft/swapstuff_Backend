using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public interface IAppSettingService
    {
        List<AppSetting> GetAll();
        AppSetting GetById(string id);
        List<AppSetting> GetAppSettings();
        void Insert(AppSetting model);
        void Update(AppSetting model);
        void Delete(AppSetting model);
    }
}
