
using EFTask.Models;
using EFTask.RelationMapingConfiguration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EFTask.Data
{
    public class ApplicationDbContext : DbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Unit> Units { get; set; }

        public  DbSet<Item> Items { get; set; }
       public DbSet<UnitItem> UnitItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedItem> OrderedItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // the above line same like the below 3 lines
            //modelBuilder.ApplyConfiguration(new ItemConfiguration());
            //modelBuilder.ApplyConfiguration(new UnitConfiguration());
            //modelBuilder.ApplyConfiguration(new UnitItemMapping());



            //        modelBuilder.Entity<Grade>()
            //.HasMany<Student>(g => g.Students)
            //.WithOne(s => s.Grade)
            //.HasForeignKey(s => s.CurrentGradeId);

            //        modelBuilder.Entity<Grade>()
            //            .HasMany<Student>(g => g.Students)
            //            .WithOne(s => s.Grade)
            //            .HasForeignKey(s => s.CurrentGradeId)
            //            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
