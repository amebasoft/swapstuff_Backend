using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SwapStff.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Data.Mapping
{
    public class ProfileMap : EntityTypeConfiguration<Profile>
    {
        public ProfileMap()
        {
            ToTable("Profiles");
            HasKey(c => c.ProfileId).Property(c => c.ProfileId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.ProfileId).IsRequired();
            Property(c => c.Username).HasMaxLength(255);
            Property(c => c.Latitude).HasPrecision(18,10);
            Property(c => c.Longitude).HasPrecision(18, 10);
            Property(c => c.Distance);
            Property(c => c.DateTimeCreated);
            Property(c => c.GCM_RegistrationID);
            Property(c => c.ItemMatchNotification);
            Property(c => c.ChatNotification);
        }
    }
}
