using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SwapStff.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Data.Mapping
{
    public class ChatMap : EntityTypeConfiguration<Chat>
    {
        public ChatMap()
        {
            ToTable("Chats");
            HasKey(c => c.ChatId).Property(c => c.ChatId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.ItemID).IsRequired();
            //ItemID as foreign key
            HasRequired(p => p.Item)
                .WithMany(c => c.Chats)
                .HasForeignKey(p => p.ItemID);

            Property(c => c.ProfileIdBy).IsRequired();
            //ProfileIDBy as foreign key
            HasRequired(p => p.Profile)
                .WithMany(c => c.Chats)
                .HasForeignKey(p => p.ProfileIdBy);

            Property(c => c.ProfileIdTo).IsRequired();
            //ProfileIDTo as foreign key
            HasRequired(p => p.Profile1)
                .WithMany(c => c.Chats1)
                .HasForeignKey(p => p.ProfileIdTo);

            Property(c => c.ChatContent);
            Property(c => c.DateTimeCreated);
            Property(c => c.IsRead);
        
        }

    }
}
