using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Model.Entities
{
    public abstract class Base : IBase
    {
        public int Id { get ; set ; }
        public DateTimeOffset Created { get ; set ; }
        public DateTimeOffset? Updated { get ; set; }

        public static void OnModelCreating<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity :class, IBase
        {
            builder.HasKey(entity => entity.Id);
        }
    }
}
