using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entity;

namespace DAL.Mappings
{
    public class UserMapping : EntityTypeConfiguration<tblUser>
    {
        public UserMapping()
        {
            ToTable("tblUser");
            HasKey(pk => pk.UserID);
            //Property(pr => pr.UserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            HasMany(e => e.tblAgencies);
            //WithOptional(e => e.tblUser);
            //HasForeignKey(e => e.CreatedBy);
        }
    }
}
