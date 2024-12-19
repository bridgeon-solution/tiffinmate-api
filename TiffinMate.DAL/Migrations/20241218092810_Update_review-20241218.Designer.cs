﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TiffinMate.DAL.DbContexts;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241218092810_Update_review-20241218")]
    partial class Update_review20241218
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TiffinMate.DAL.Entities.Admin", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ApiLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("HttpMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LoggedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("QueryString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RequestBody")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RequestPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResponseBody")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ResponseStatusCode")
                        .HasColumnType("integer");

                    b.Property<long>("TimeTaken")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ApiLogs");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Categories", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("categoryname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.FoodItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("categoryid")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("day")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("foodname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("is_veg")
                        .HasColumnType("boolean");

                    b.Property<decimal>("price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("providerid")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("categoryid");

                    b.HasIndex("providerid");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Provider", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("certificate")
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<bool>("is_blocked")
                        .HasColumnType("boolean");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<string>("refresh_token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("refreshtoken_expiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("provider");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("username")
                        .HasColumnType("text");

                    b.Property<string>("verification_status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("False");

                    b.HasKey("id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.ProviderDetails", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<string>("about")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("account_no")
                        .HasColumnType("integer");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("logo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("phone_no")
                        .HasColumnType("integer");

                    b.Property<string>("resturent_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.HasIndex("ProviderId")
                        .IsUnique();

                    b.ToTable("ProvidersDetails");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.Review", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<string>("review")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.HasIndex("ProviderId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("image")
                        .HasColumnType("text");

                    b.Property<bool>("is_blocked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("subscription_status")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.FoodItem", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Categories", "category")
                        .WithMany("foodItems")
                        .HasForeignKey("categoryid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "provider")
                        .WithMany("FoodItems")
                        .HasForeignKey("providerid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");

                    b.Navigation("provider");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.ProviderDetails", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "Provider")
                        .WithOne("ProviderDetails")
                        .HasForeignKey("TiffinMate.DAL.Entities.ProviderEntity.ProviderDetails", "ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.Review", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "Provider")
                        .WithMany("Review")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.User", "User")
                        .WithMany("Review")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Categories", b =>
                {
                    b.Navigation("foodItems");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Provider", b =>
                {
                    b.Navigation("FoodItems");

                    b.Navigation("ProviderDetails")
                        .IsRequired();

                    b.Navigation("Review");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.User", b =>
                {
                    b.Navigation("Review");
                });
#pragma warning restore 612, 618
        }
    }
}
