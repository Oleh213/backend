using System;
using System.Collections.Generic;
using WebShop.Main.Conext;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Context;

namespace WebShop.Main.DBContext
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<Product> products { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<DeliveryOptions> deliveryOptions { get; set; }

        public DbSet<Promocode> promocodes { get; set;}

        public DbSet<CartItems> cartItems { get; set; }

        public DbSet<OrderList> orderLists { get; set; }

        public DbSet<Cards> cards { get; set; }

        public DbSet<Info> info { get; set; }

        public DbSet<Characteristics> characteristics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItems>()
                .HasOne(x => x.User)
                .WithMany(p => p.CartItems)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Info>()
                .HasOne(x => x.Order)
                .WithOne(p => p.Info)
                .HasForeignKey<Info>(p => p.OrderId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.DeliveryOptions)
                .WithOne(x=> x.User)
                .HasForeignKey(key => key.UserId);
                

            modelBuilder.Entity<CartItems>()
                .HasOne(x => x.Product)
                .WithMany(x=> x.CartItems)
                .HasForeignKey(p => p.ProductId);


            modelBuilder.Entity<Category>()
                .HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .IsRequired()
                .HasForeignKey(x => x.CategorytId);          

            modelBuilder.Entity<OrderList>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderLists)
                .IsRequired()
                .HasForeignKey(x => x.OrderId);


            modelBuilder.Entity<OrderList>()
                .HasOne(x => x.Product)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(x => x.Characteristics)
                .WithMany(x => x.Product);

            modelBuilder.Entity<User>().HasKey(s => new { s.UserId });

            modelBuilder.Entity<Order>().HasKey(s => new { s.OrderId });

            modelBuilder.Entity<DeliveryOptions>().HasKey(s => new { s.DeliveryOptionsId });

            modelBuilder.Entity<OrderList>().HasKey(s => new { s.OrderListId });

            modelBuilder.Entity<Cards>().HasKey(s => new { s.CardId });

            modelBuilder.Entity<Product>().HasKey(s => new { s.ProductId });
            
            modelBuilder.Entity<CartItems>().HasKey(s => new { s.CartItemsId });

            modelBuilder.Entity<Category>().HasKey(s => new { s.CatId });

            modelBuilder.Entity<Info>().HasKey(s => new { s.InfoId });

            modelBuilder.Entity<Characteristics>().HasKey(s => new { s.CharacteristicsId });
        }
    }
}