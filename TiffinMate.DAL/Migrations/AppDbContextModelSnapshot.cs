﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TiffinMate.DAL.DbContexts;

#nullable disable

namespace TiffinMate.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("refresh_token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("refreshtoken_expiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("user_name")
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

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.Order", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("Menuid")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<Guid>("menu_id")
                        .HasColumnType("uuid");

                    b.Property<int>("order_status")
                        .HasColumnType("integer");

                    b.Property<string>("order_string")
                        .HasColumnType("text");

                    b.Property<Guid>("provider_id")
                        .HasColumnType("uuid");

                    b.Property<string>("start_date")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("total_price")
                        .HasColumnType("numeric");

                    b.Property<string>("transaction_id")
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("Menuid");

                    b.HasIndex("provider_id");

                    b.HasIndex("user_id");

                    b.ToTable("order");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.OrderDetails", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("category_id")
                        .HasColumnType("uuid");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("fooditem_image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("fooditem_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("fooditem_price")
                        .HasColumnType("numeric");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<Guid>("order_id")
                        .HasColumnType("uuid");

                    b.Property<string>("ph_no")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("category_id");

                    b.HasIndex("order_id");

                    b.ToTable("orderDetails");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.PaymentHistory", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<bool>("is_paid")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("payment_date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("subscription_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("subscription_id");

                    b.HasIndex("user_id");

                    b.ToTable("paymentHistory");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.Subscription", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("cancelled_at")
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<Guid>("menu_id")
                        .HasColumnType("uuid");

                    b.Property<int>("order_status")
                        .HasColumnType("integer");

                    b.Property<string>("order_string")
                        .HasColumnType("text");

                    b.Property<Guid>("provider_id")
                        .HasColumnType("uuid");

                    b.Property<string>("start_date")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("total_price")
                        .HasColumnType("numeric");

                    b.Property<string>("transaction_id")
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("menu_id");

                    b.HasIndex("provider_id");

                    b.HasIndex("user_id");

                    b.ToTable("subscriptions");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.SubscriptionDetails", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("category_id")
                        .HasColumnType("uuid");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<string>("ph_no")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("subscription_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("category_id");

                    b.HasIndex("subscription_id");

                    b.ToTable("subscriptionDetails");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Categories", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("category_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.FoodItem", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("category_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("day")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("food_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<Guid>("menu_id")
                        .HasColumnType("uuid");

                    b.Property<decimal>("price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("provider_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.HasIndex("category_id");

                    b.HasIndex("menu_id");

                    b.HasIndex("provider_id");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Menu", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("is_available")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<decimal>("monthly_plan_amount")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasDefaultValue(0m);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("provider_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.ToTable("menus");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Notification", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<bool>("isread")
                        .HasColumnType("boolean");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("notification_type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("recipient_id")
                        .HasColumnType("text");

                    b.Property<string>("recipient_type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("notifications");
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

                    b.Property<string>("user_name")
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

                    b.Property<Guid>("provider_id")
                        .HasColumnType("uuid");

                    b.Property<string>("resturent_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id")
                        .IsUnique();

                    b.ToTable("ProvidersDetails");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.Rating", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<Guid>("provider_id")
                        .HasColumnType("uuid");

                    b.Property<int>("rating")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.HasIndex("user_id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.Review", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("is_delete")
                        .HasColumnType("boolean");

                    b.Property<Guid>("provider_id")
                        .HasColumnType("uuid");

                    b.Property<string>("review")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.HasIndex("user_id");

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

                    b.Property<string>("refresh_token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("refreshtoken_expiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("user");

                    b.Property<bool>("subscription_status")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.Order", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Menu", null)
                        .WithMany("order")
                        .HasForeignKey("Menuid");

                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "provider")
                        .WithMany("order")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.User", "user")
                        .WithMany("order")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("provider");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.OrderDetails", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Categories", "Category")
                        .WithMany("order_details")
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.OrderEntity.Order", "order")
                        .WithMany("details")
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("order");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.PaymentHistory", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.OrderEntity.Subscription", "subscription")
                        .WithMany("payment_history")
                        .HasForeignKey("subscription_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.User", "user")
                        .WithMany("payment_history")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("subscription");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.Subscription", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Menu", "menu")
                        .WithMany("subscription")
                        .HasForeignKey("menu_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "provider")
                        .WithMany("subscription")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.User", "user")
                        .WithMany("subscription")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("menu");

                    b.Navigation("provider");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.SubscriptionDetails", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Categories", "Category")
                        .WithMany("subscription_details")
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.OrderEntity.Subscription", "subscription")
                        .WithMany("details")
                        .HasForeignKey("subscription_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("subscription");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.FoodItem", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Categories", "category")
                        .WithMany("food_items")
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Menu", "menu")
                        .WithMany("food_items")
                        .HasForeignKey("menu_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "provider")
                        .WithMany("food_items")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");

                    b.Navigation("menu");

                    b.Navigation("provider");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Menu", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "provider")
                        .WithMany("menus")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("provider");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.ProviderDetails", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "Provider")
                        .WithOne("provider_details")
                        .HasForeignKey("TiffinMate.DAL.Entities.ProviderEntity.ProviderDetails", "provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.Rating", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "provider")
                        .WithMany("rating")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.User", "user")
                        .WithMany("rating")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("provider");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.Review", b =>
                {
                    b.HasOne("TiffinMate.DAL.Entities.ProviderEntity.Provider", "provider")
                        .WithMany("review")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TiffinMate.DAL.Entities.User", "user")
                        .WithMany("review")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("provider");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.Order", b =>
                {
                    b.Navigation("details");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.OrderEntity.Subscription", b =>
                {
                    b.Navigation("details");

                    b.Navigation("payment_history");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Categories", b =>
                {
                    b.Navigation("food_items");

                    b.Navigation("order_details");

                    b.Navigation("subscription_details");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Menu", b =>
                {
                    b.Navigation("food_items");

                    b.Navigation("order");

                    b.Navigation("subscription");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.ProviderEntity.Provider", b =>
                {
                    b.Navigation("food_items");

                    b.Navigation("menus");

                    b.Navigation("order");

                    b.Navigation("provider_details")
                        .IsRequired();

                    b.Navigation("rating");

                    b.Navigation("review");

                    b.Navigation("subscription");
                });

            modelBuilder.Entity("TiffinMate.DAL.Entities.User", b =>
                {
                    b.Navigation("order");

                    b.Navigation("payment_history");

                    b.Navigation("rating");

                    b.Navigation("review");

                    b.Navigation("subscription");
                });
#pragma warning restore 612, 618
        }
    }
}
