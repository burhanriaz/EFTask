using EFTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFTask.RelationMapingConfiguration
{
    public class UnitItemMapping : IEntityTypeConfiguration<UnitItem>
    {
        public void Configure(EntityTypeBuilder<UnitItem> builder)
        {
           builder.HasKey(sc => new { sc.ItemId, sc.UnitId });

            builder.ToTable("UnitItem");
           builder.HasOne<Unit>(u => u.Unit)
                .WithMany(i => i.UnitItems)
                .HasForeignKey(ui => ui.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Item>(u => u.Item)
                .WithMany(i => i.UnitItems)
                .HasForeignKey(ui => ui.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
          

        }
    }
}