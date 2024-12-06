using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;


namespace TiffinMate.DAL.DbContexts
{
    public class AppDbContext:DbContext
    {
       public DbSet<Provider> provider { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Provider entity
            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(p => p.id);
                entity.Property(p => p.id)
                .HasColumnType("uuid")
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");
               
            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
