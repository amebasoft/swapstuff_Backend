using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SwapStff.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Data.Mapping
{
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            ToTable("Items");
            HasKey(c => c.ItemID).Property(c => c.ItemID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            Property(c => c.ProfileID).IsRequired();

            //ProfileID as foreign key
            HasRequired(p => p.Profiles)
                .WithMany(c => c.Items)
                .HasForeignKey(p => p.ProfileID);

            Property(c => c.ItemTitle).IsRequired().HasMaxLength(100);
            Property(c => c.ItemDescription).IsRequired().HasMaxLength(1024);
            Property(c => c.ItemImage).IsRequired();
            Property(c => c.ItemDateTimeCreated);
            Property(c => c.IsActive);

        }
    }
}
