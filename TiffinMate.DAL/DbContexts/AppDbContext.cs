using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;
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

                entity.HasOne(p => p.ProviderDetails)
                      .WithOne(pd => pd.Provider)
                      .HasForeignKey<ProviderDetails>(pd => pd.ProviderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.role).HasDefaultValue("provider");
               
            });




            //modelBuilder.Entity<ProviderDetails>(entity =>
            //{
            //    entity.HasKey(pd => pd.id);
            //    entity.Property(pd => pd.id)
            //          .HasColumnType("uuid")
            //          .IsRequired()
            //          .HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(pd => pd.ProviderId)
            //          .IsRequired();
            //});

            modelBuilder.Entity<ProviderDetails>(entity =>
            {
                entity.HasKey(pd => pd.id);
                entity.Property(pd => pd.id)
                      .HasColumnType("uuid")
                      .IsRequired()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(pd => pd.ProviderId)
                      .IsRequired();

                entity.HasOne(pd => pd.Provider)
                      .WithOne(p => p.ProviderDetails)
                      .HasForeignKey<ProviderDetails>(pd => pd.ProviderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasOne(e => e.category)
                .WithMany(c => c.foodItems)
                .HasForeignKey(d => d.categoryid);

            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasOne(f => f.provider)
                .WithMany(c => c.FoodItems)
                .HasForeignKey(d => d.providerid)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => new { r.ProviderId, r.UserId });

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Review)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(r => r.Provider)
                      .WithMany(p => p.Review)
                      .HasForeignKey(r => r.ProviderId)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.Property(r => r.review)
                      .IsRequired();
            });



        }


    }
}
