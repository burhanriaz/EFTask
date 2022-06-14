using EFTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EFTask.RelationMapingConfiguration
{
   
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
           builder.ToTable("Item");
            builder.Property(e => e.Name)
                           .IsRequired(true)
                           .HasMaxLength(50);
           

            builder.Property(e => e.Price)
                           .IsRequired(true)
                           .HasColumnType("decimal(18, 0)");
        }

    }
}
