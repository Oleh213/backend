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
    [Migration("20230224091455_Migrations2")]
    partial class Migrations2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CharacteristicsProduct", b =>
                {
                    b.Property<Guid>("CharacteristicsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CharacteristicsId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CharacteristicsProduct");
                });

            modelBuilder.Entity("ProductProductImages", b =>
                {
                    b.Property<Guid>("ImagesImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductsProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ImagesImageId", "ProductsProductId");

                    b.HasIndex("ProductsProductId");

                    b.ToTable("ProductProductImages");
                });

            modelBuilder.Entity("WebShop.Main.Conext.CartItems", b =>
                {
                    b.Property<Guid>("CartItemsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CartItemsId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("cartItems");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Info", b =>
                {
                    b.Property<Guid>("InfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InfoId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("info");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.Property<string>("UsedPromocode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderId");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Available")
                        .HasColumnType("int");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Promocode", b =>
                {
                    b.Property<Guid>("PromocodetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
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

                    b.Property<int>("AccountBalance")
                        .HasColumnType("int");

                    b.Property<string>("Birthday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistData")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("WebShop.Main.Context.Cards", b =>
                {
                    b.Property<Guid>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Balance")
                        .HasColumnType("int");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cvv")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpiredDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardId");

                    b.ToTable("cards");
                });

            modelBuilder.Entity("WebShop.Main.Context.Characteristics", b =>
                {
                    b.Property<Guid>("CharacteristicsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CharacteristicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CharacteristicValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CharacteristicsId");

                    b.ToTable("characteristics");
                });

            modelBuilder.Entity("WebShop.Main.Context.Coments", b =>
                {
                    b.Property<Guid>("ComentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte?>("Rating")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ComentId");

                    b.HasIndex("UserId");

                    b.ToTable("coments");
                });

            modelBuilder.Entity("WebShop.Main.Context.DeliveryOptions", b =>
                {
                    b.Property<Guid>("DeliveryOptionsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeliveryOptionsId");

                    b.HasIndex("UserId");

                    b.ToTable("deliveryOptions");
                });

            modelBuilder.Entity("WebShop.Main.Context.OrderList", b =>
                {
                    b.Property<Guid>("OrderListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderListId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("orderLists");
                });

            modelBuilder.Entity("WebShop.Main.Context.ProductImages", b =>
                {
                    b.Property<Guid>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageId");

                    b.ToTable("productImages");
                });

            modelBuilder.Entity("CharacteristicsProduct", b =>
                {
                    b.HasOne("WebShop.Main.Context.Characteristics", null)
                        .WithMany()
                        .HasForeignKey("CharacteristicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShop.Main.Conext.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductProductImages", b =>
                {
                    b.HasOne("WebShop.Main.Context.ProductImages", null)
                        .WithMany()
                        .HasForeignKey("ImagesImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShop.Main.Conext.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebShop.Main.Conext.CartItems", b =>
                {
                    b.HasOne("WebShop.Main.Conext.Product", "Product")
                        .WithMany("CartItems")
                        .HasForeignKey("ProductId")
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

            modelBuilder.Entity("WebShop.Main.Conext.Info", b =>
                {
                    b.HasOne("WebShop.Main.Conext.Order", "Order")
                        .WithOne("Info")
                        .HasForeignKey("WebShop.Main.Conext.Info", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Product", b =>
                {
                    b.HasOne("WebShop.Main.Conext.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("WebShop.Main.Context.Coments", b =>
                {
                    b.HasOne("WebShop.Main.Conext.User", "User")
                        .WithMany("Coments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShop.Main.Context.DeliveryOptions", b =>
                {
                    b.HasOne("WebShop.Main.Conext.User", "User")
                        .WithMany("DeliveryOptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShop.Main.Context.OrderList", b =>
                {
                    b.HasOne("WebShop.Main.Conext.Order", "Order")
                        .WithMany("OrderLists")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShop.Main.Conext.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
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
                    b.Navigation("Info")
                        .IsRequired();

                    b.Navigation("OrderLists");
                });

            modelBuilder.Entity("WebShop.Main.Conext.Product", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("WebShop.Main.Conext.User", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("Coments");

                    b.Navigation("DeliveryOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
