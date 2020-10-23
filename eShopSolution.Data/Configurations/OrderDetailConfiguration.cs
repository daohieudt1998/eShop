using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class OrderDetailConfiguration: IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Order)
                .WithMany(y => y.OrderDetails)
                .HasForeignKey(z => z.OrderId);

            builder.HasOne(x => x.Product)
                .WithMany(y => y.OrderDetails)
                .HasForeignKey(z => z.ProductId);
        }
    }
}
