using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDal.ModelFluentApi
{
    public class ProductEntityType : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
          .Property(e => e.CategoryId).HasColumnName("CategoryID");

            builder
            .Property(e => e.ProductName).HasMaxLength(40);

            // ...
        }
    }
}
