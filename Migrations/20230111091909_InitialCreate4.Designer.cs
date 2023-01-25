﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebShop.Main.DBContext;

#nullable disable

namespace WebShop.Migrations
{
    [DbContext(typeof(ShopContext))]
    [Migration("20230111091909_InitialCreate6")]
    partial class InitialCreate6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebShop.Main.Conext.CartItems", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("cartItems");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Category", b =>
                {
                    b.Property<Guid?>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CatId");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Order", b =>
                {
                    b.Property<Guid?>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DeliveryOptions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TotalPrice")
                        .HasColumnType("int");

                    b.Property<string>("UsedPromocode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderId");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Product", b =>
                {
                    b.Property<Guid?>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Available")
                        .HasColumnType("int");

                    b.Property<Guid?>("CategorytId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("CategorytId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Promocode", b =>
                {
                    b.Property<Guid?>("PromocodetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.HasKey("PromocodetId");

                    b.ToTable("promocodes");
                });

            modelBuilder.Entity("WebShop.Main.Conext.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Online")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("WebShop.Main.Context.OrderList", b =>
                {
                    b.Property<Guid>("OrderListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderListId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("orderLists");
                });

            modelBuilder.Entity("WebShop.Main.Conext.CartItems", b =>
                {
                    b.HasOne("WebShop.Main.Conext.Product", "Product")
                        .WithOne()
                        .HasForeignKey("WebShop.Main.Conext.CartItems", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShop.Main.Conext.User", "User")
                        .WithMany("CartItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Product", b =>
                {
                    b.HasOne("WebShop.Main.Conext.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategorytId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("WebShop.Main.Context.OrderList", b =>
                {
                    b.HasOne("WebShop.Main.Conext.Order", "Order")
                        .WithMany("OrderLists")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShop.Main.Conext.Product", "Product")
                        .WithOne()
                        .HasForeignKey("WebShop.Main.Context.OrderList", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Order", b =>
                {
                    b.Navigation("OrderLists");
                });

            modelBuilder.Entity("WebShop.Main.Conext.User", b =>
                {
                    b.Navigation("CartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
