using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SwapStff.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapStff.Data.Mapping
{
    public class ErrorMap : EntityTypeConfiguration<LogError>
    {
        public ErrorMap()
        {
            ToTable("Log4Net_Error");
            HasKey(c => c.ID).Property(c => c.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(c => c.Exception).IsRequired().HasMaxLength(2000);
        }
    }
}
