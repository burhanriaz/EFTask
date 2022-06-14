using EFTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFTask.RelationMapingConfiguration
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Unit").HasIndex(e=>e.UnitType).IsUnique();
            builder.Property(e => e.UnitType)
                .IsRequired(true)
                 .HasMaxLength(50);

           
        }
    }
}
