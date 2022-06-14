using EFTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFTask.RelationMapingConfiguration
{
    public class OrderedItemConfiguration : IEntityTypeConfiguration<OrderedItem>
    {

        public void Configure(EntityTypeBuilder<OrderedItem> builder)
        {
            builder.HasKey(s => new { s.OrderId_FK, s.ItemId_Fk });

            builder.Property(e => e.Quantity)
                    .IsRequired(true)
                    .HasMaxLength(50);

            builder.HasOne(d => d.Order)
                .WithMany(p => p.OrderItem)
                .HasForeignKey(d => d.OrderId_FK)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Item)
                    .WithMany(p => p.OrderedItems)
                    .HasForeignKey(d => d.ItemId_Fk)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Unit)
                .WithMany(s => s.OrderedItems)
                .HasForeignKey(d => d.UnitId_Fk)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
