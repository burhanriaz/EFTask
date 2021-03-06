// <auto-generated />
using System;
using EFTask.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFTask.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220613132702_Newone")]
    partial class Newone
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("EFTask.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,0)");

                    b.HasKey("ItemId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("EFTask.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("EFTask.Models.OrderedItem", b =>
                {
                    b.Property<int>("OrderId_FK")
                        .HasColumnType("int");

                    b.Property<int>("ItemId_Fk")
                        .HasColumnType("int");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<decimal>("Sub_Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UnitID_Fk")
                        .HasColumnType("int");

                    b.Property<int?>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("OrderId_FK", "ItemId_Fk");

                    b.HasIndex("ItemId");

                    b.HasIndex("UnitId");

                    b.ToTable("OrderedItems");
                });

            modelBuilder.Entity("EFTask.Models.Unit", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("UnitType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UnitId");

                    b.HasIndex("UnitType")
                        .IsUnique();

                    b.ToTable("Unit");
                });

            modelBuilder.Entity("EFTask.Models.UnitItem", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("ItemId", "UnitId");

                    b.HasIndex("UnitId");

                    b.ToTable("UnitItem");
                });

            modelBuilder.Entity("EFTask.Models.OrderedItem", b =>
                {
                    b.HasOne("EFTask.Models.Item", "Item")
                        .WithMany("OrderedItems")
                        .HasForeignKey("ItemId");

                    b.HasOne("EFTask.Models.Order", "Order")
                        .WithMany("OrderItem")
                        .HasForeignKey("OrderId_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFTask.Models.Unit", "unit")
                        .WithMany("OrderedItems")
                        .HasForeignKey("UnitId");

                    b.Navigation("Item");

                    b.Navigation("Order");

                    b.Navigation("unit");
                });

            modelBuilder.Entity("EFTask.Models.UnitItem", b =>
                {
                    b.HasOne("EFTask.Models.Item", "Item")
                        .WithMany("UnitItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFTask.Models.Unit", "Unit")
                        .WithMany("UnitItems")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("EFTask.Models.Item", b =>
                {
                    b.Navigation("OrderedItems");

                    b.Navigation("UnitItems");
                });

            modelBuilder.Entity("EFTask.Models.Order", b =>
                {
                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("EFTask.Models.Unit", b =>
                {
                    b.Navigation("OrderedItems");

                    b.Navigation("UnitItems");
                });
#pragma warning restore 612, 618
        }
    }
}
