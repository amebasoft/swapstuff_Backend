using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SwapStff.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Data.Mapping
{
    public class ItemMatchMap : EntityTypeConfiguration<ItemMatch>
    {
        public ItemMatchMap()
        {
            ToTable("ItemMatchs");
            HasKey(c => c.ItemMatchID).Property(c => c.ItemMatchID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.ItemID).IsRequired();
            //ItemID as foreign key
            HasRequired(p => p.Item)
                .WithMany(c => c.ItemMatches)
                .HasForeignKey(p => p.ItemID);

            Property(c => c.ProfileIdBy).IsRequired();
            //ProfileID as foreign key
            HasRequired(p => p.Profile)
                .WithMany(c => c.ItemMatches)
                .HasForeignKey(p => p.ProfileIdBy);

            Property(c => c.Distance);
            Property(c => c.IsLikeDislikeAbuseBy);
            Property(c => c.DateTimeCreated);
            Property(c => c.AbuseMessage);

        }
    }
}
