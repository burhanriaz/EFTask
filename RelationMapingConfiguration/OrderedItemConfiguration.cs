using EFTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFTask.RelationMapingConfiguration
{
    public class OrderedItemConfiguration : IEntityTypeConfiguration<OrderedItem>
    {

        public void Configure(EntityTypeBuilder<OrderedItem> builder)
        {


            builder.Property(e => e.Quantity)
                    .IsRequired(true)
                    .HasMaxLength(50);


            builder.HasOne(d => d.UnitItem)
                .WithMany(p => p.OrderItem)
                .HasForeignKey(d =>d.UnitItemIdFK);



            builder.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId_FK);


        }
    }
}
