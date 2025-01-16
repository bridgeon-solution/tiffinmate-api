using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Entities.ProviderEntity;
using User = TiffinMate.DAL.Entities.User;

namespace TiffinMate.DAL.DbContexts
{
    public class AppDbContext:DbContext
    {
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderDetails> ProvidersDetails { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Menu> menus { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }
        public DbSet<Subscription> subscriptions { get; set; }
        public DbSet<SubscriptionDetails> subscriptionDetails { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.id);
                entity.Property(a => a.id)
                .HasColumnType("uuid")
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.id)
                .HasColumnType("uuid")
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.is_blocked)
                    .HasDefaultValue(false);
                entity.Property(e => e.role).HasDefaultValue("user");
            });
            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(a => a.id);
                entity.Property(a => a.id)
                      .HasColumnType("uuid")
                      .IsRequired()
                      .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(p => p.verification_status)
                      .HasDefaultValue(false);

                entity.HasOne(p => p.provider_details)
                      .WithOne(pd => pd.Provider)
                      .HasForeignKey<ProviderDetails>(pd => pd.provider_id)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.role).HasDefaultValue("provider");
               
            });

            modelBuilder.Entity<ProviderDetails>(entity =>
            {
                entity.HasKey(pd => pd.id);
                entity.Property(pd => pd.id)
                      .HasColumnType("uuid")
                      .IsRequired()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(pd => pd.provider_id)
                      .IsRequired();

                entity.HasOne(pd => pd.Provider)
                      .WithOne(p => p.provider_details)
                      .HasForeignKey<ProviderDetails>(pd => pd.provider_id)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasOne(e => e.category)
                .WithMany(c => c.food_items)
                .HasForeignKey(d => d.category_id);

            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasOne(f => f.provider)
                .WithMany(c => c.food_items)
                .HasForeignKey(d => d.provider_id)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => new { r.id });


                entity.HasOne(r => r.user)
                      .WithMany(u => u.review)
                      .HasForeignKey(r => r.user_id)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(r => r.provider)
                      .WithMany(p => p.review)
                      .HasForeignKey(r => r.provider_id)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.Property(r => r.review)
                      .IsRequired();
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasMany(m => m.food_items)
                .WithOne(f => f.menu)
                .HasForeignKey(f => f.menu_id);
                

            });
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasOne(m => m.provider)
                .WithMany(p => p.menus)
                .HasForeignKey(f => f.provider_id);

                entity.Property(m=>m.is_available)
                .HasDefaultValue(true);

                entity.Property(m => m.monthly_plan_amount)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(m => m.user)
                .WithMany(m => m.order)
                .HasForeignKey(f => f.user_id)
                .OnDelete(DeleteBehavior.Cascade);


                entity.HasMany(m => m.details)
                .WithOne(c => c.order)
                .HasForeignKey(o => o.order_id)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.provider)
                .WithMany(p => p.order)
                .HasForeignKey(p => p.provider_id)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.details)
                .WithOne(od => od.order)
                .HasForeignKey(od => od.order_id)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasOne(m => m.user)
                .WithMany(m => m.subscription)
                .HasForeignKey(f => f.user_id)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(m => m.details)
                .WithOne(c => c.subscription)
                .HasForeignKey(o => o.subscription_id)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.provider)
                .WithMany(p => p.subscription)
                .HasForeignKey(p => p.provider_id)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.details)
                .WithOne(od => od.subscription)
                .HasForeignKey(od => od.subscription_id)
                .OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasOne(o => o.Category)
                .WithMany(c => c.order_details)
                .HasForeignKey(o => o.category_id)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SubscriptionDetails>(entity =>
            {
                entity.HasOne(o => o.Category)
                .WithMany(c => c.subscription_details)
                .HasForeignKey(o => o.category_id);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasOne(r => r.provider)
                .WithMany(p => p.rating)
                .HasForeignKey(r => r.provider_id);

                entity.HasOne(r => r.user)
                .WithMany(u => u.rating)
                .HasForeignKey(r => r.user_id);
            });

             
        }


    }
}
