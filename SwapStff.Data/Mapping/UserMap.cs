using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SwapStff.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Data.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        
        public UserMap()
        {
            ToTable("Users");
            HasKey(c => c.UserID).Property(c => c.UserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.UserID).IsRequired();
            Property(c => c.UserName).HasMaxLength(50);
            Property(c => c.EmailID).HasMaxLength(50);
            Property(c => c.Password).HasMaxLength(15);
            Property(c => c.ParentID);
            Property(c => c.CreatedOn);
            Property(c => c.LastUpdatedOn);
            Property(c => c.IsActive);
        }
    }
}
