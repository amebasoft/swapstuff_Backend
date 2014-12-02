using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SwapStff.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Data.Mapping
{
    public class AppSettingMap : EntityTypeConfiguration<AppSetting>
    {
        public AppSettingMap()
        {
            ToTable("AppSettings");
            HasKey(c => c.SettingID).Property(c => c.SettingID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.MaxDistance).IsRequired();
        }
    }
}
