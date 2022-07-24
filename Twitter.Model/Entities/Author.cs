using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Model.Entities
{
    [Table("Author", Schema = "JH.TwitterStreamingApp")]
    public class Author: Base
    {
       public string TwwitterAuthorId { get; set; }

        public string TwitterName { get; set; }

        public string TwitterHandle { get; set; }

        public string TwitterImageUrl { get; set; }

        public static void OnModelCreating(EntityTypeBuilder<Author> builder)
        {
            OnModelCreating<Author>(builder);
            builder.Property(author => author.TwwitterAuthorId).HasMaxLength(50);
            builder.Property(author => author.TwitterName).HasMaxLength(200);
            builder.Property(author => author.TwitterHandle).HasMaxLength(200);
            builder.Property(author => author.TwitterImageUrl).HasMaxLength(500);
           
        }
    }
}
