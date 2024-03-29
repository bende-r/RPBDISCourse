﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VideoRentalWeb.DataModels;

#nullable disable

namespace VideoRentalWeb.Migrations
{
    [DbContext(typeof(VideoRentalContext))]
    partial class VideoRentalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("VideoRentalModels.Clientele", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ClientID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"), 1L, 1);

                    b.Property<string>("Addres")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Middlename")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Passport")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Phone")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("ClientId")
                        .HasName("PK__Clientel__E67E1A04315A897C");

                    b.ToTable("Clientele", (string)null);
                });

            modelBuilder.Entity("VideoRentalModels.Disk", b =>
                {
                    b.Property<int>("DiskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DiskID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiskId"), 1L, 1);

                    b.Property<string>("CreationYear")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("varchar(4)");

                    b.Property<int>("DiskType")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int")
                        .HasColumnName("GenreID");

                    b.Property<string>("MainActor")
                        .IsRequired()
                        .HasMaxLength(90)
                        .IsUnicode(false)
                        .HasColumnType("varchar(90)");

                    b.Property<int>("Producer")
                        .HasColumnType("int");

                    b.Property<DateTime>("Recording")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("DiskId");

                    b.HasIndex("DiskType");

                    b.HasIndex("GenreId");

                    b.HasIndex("Producer");

                    b.ToTable("Disks");
                });

            modelBuilder.Entity("VideoRentalModels.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("GenreID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("VideoRentalModels.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PositionID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PositionId"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("PositionId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("VideoRentalModels.Pricelist", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PriceID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PriceId"), 1L, 1);

                    b.Property<int>("DiskId")
                        .HasColumnType("int")
                        .HasColumnName("DiskID");

                    b.Property<decimal?>("Price")
                        .HasColumnType("money");

                    b.HasKey("PriceId")
                        .HasName("PK__Pricelis__4957584FC87A6E36");

                    b.HasIndex("DiskId");

                    b.ToTable("Pricelist", (string)null);
                });

            modelBuilder.Entity("VideoRentalModels.Producer", b =>
                {
                    b.Property<int>("ProduceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProduceID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProduceId"), 1L, 1);

                    b.Property<string>("Country")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("ProduceId")
                        .HasName("PK__Producer__2EEBECB32A180669");

                    b.ToTable("Producers");
                });

            modelBuilder.Entity("VideoRentalModels.Staff", b =>
                {
                    b.Property<int>("StaffId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("StaffID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StaffId"), 1L, 1);

                    b.Property<DateTime?>("DateOfEmployment")
                        .HasColumnType("date");

                    b.Property<string>("Middlename")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<int?>("PositionId")
                        .HasColumnType("int")
                        .HasColumnName("PositionID");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("StaffId");

                    b.HasIndex("PositionId");

                    b.ToTable("Staff", (string)null);
                });

            modelBuilder.Entity("VideoRentalModels.Taking", b =>
                {
                    b.Property<int>("TakeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TakeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TakeId"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int")
                        .HasColumnName("ClientID");

                    b.Property<DateTime>("DateOfCapture")
                        .HasColumnType("date");

                    b.Property<int>("DiskId")
                        .HasColumnType("int")
                        .HasColumnName("DiskID");

                    b.Property<bool>("PaymentMark")
                        .HasColumnType("bit");

                    b.Property<bool>("RefundMark")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("date");

                    b.Property<int>("StaffId")
                        .HasColumnType("int")
                        .HasColumnName("StaffID");

                    b.HasKey("TakeId")
                        .HasName("PK__Taking__AC0C2240A69151F3");

                    b.HasIndex("ClientId");

                    b.HasIndex("DiskId");

                    b.HasIndex("StaffId");

                    b.ToTable("Taking", (string)null);
                });

            modelBuilder.Entity("VideoRentalModels.Type", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TypeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TypeId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("TypeId");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("VideoRentalModels.ViewAllDisk", b =>
                {
                    b.Property<string>("Country")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("CreationYear")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("varchar(4)");

                    b.Property<int>("DiskId")
                        .HasColumnType("int")
                        .HasColumnName("DiskID");

                    b.Property<string>("DiskTitle")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("GenreTitle")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("MainActor")
                        .IsRequired()
                        .HasMaxLength(90)
                        .IsUnicode(false)
                        .HasColumnType("varchar(90)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime>("Recording")
                        .HasColumnType("date");

                    b.Property<string>("TypeTitle")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.ToView("View_AllDisks");
                });

            modelBuilder.Entity("VideoRentalWeb.DataModels.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("VideoRentalWeb.DataModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("VideoRentalWeb.DataModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VideoRentalWeb.DataModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("VideoRentalWeb.DataModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VideoRentalModels.Disk", b =>
                {
                    b.HasOne("VideoRentalModels.Type", "DiskTypeNavigation")
                        .WithMany("Disks")
                        .HasForeignKey("DiskType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Disks__DiskType__3F466844");

                    b.HasOne("VideoRentalModels.Genre", "Genre")
                        .WithMany("Disks")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Disks__GenreID__3E52440B");

                    b.HasOne("VideoRentalModels.Producer", "ProducerNavigation")
                        .WithMany("Disks")
                        .HasForeignKey("Producer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Disks__Producer__3D5E1FD2");

                    b.Navigation("DiskTypeNavigation");

                    b.Navigation("Genre");

                    b.Navigation("ProducerNavigation");
                });

            modelBuilder.Entity("VideoRentalModels.Pricelist", b =>
                {
                    b.HasOne("VideoRentalModels.Disk", "Disk")
                        .WithMany("Pricelists")
                        .HasForeignKey("DiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Pricelist__DiskI__4316F928");

                    b.Navigation("Disk");
                });

            modelBuilder.Entity("VideoRentalModels.Staff", b =>
                {
                    b.HasOne("VideoRentalModels.Position", "Position")
                        .WithMany("staff")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Staff__PositionI__47DBAE45");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("VideoRentalModels.Taking", b =>
                {
                    b.HasOne("VideoRentalModels.Clientele", "Client")
                        .WithMany("Takings")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Taking__ClientID__4D94879B");

                    b.HasOne("VideoRentalModels.Disk", "Disk")
                        .WithMany("Takings")
                        .HasForeignKey("DiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Taking__DiskID__4E88ABD4");

                    b.HasOne("VideoRentalModels.Staff", "Staff")
                        .WithMany("Takings")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Taking__StaffID__4F7CD00D");

                    b.Navigation("Client");

                    b.Navigation("Disk");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("VideoRentalModels.Clientele", b =>
                {
                    b.Navigation("Takings");
                });

            modelBuilder.Entity("VideoRentalModels.Disk", b =>
                {
                    b.Navigation("Pricelists");

                    b.Navigation("Takings");
                });

            modelBuilder.Entity("VideoRentalModels.Genre", b =>
                {
                    b.Navigation("Disks");
                });

            modelBuilder.Entity("VideoRentalModels.Position", b =>
                {
                    b.Navigation("staff");
                });

            modelBuilder.Entity("VideoRentalModels.Producer", b =>
                {
                    b.Navigation("Disks");
                });

            modelBuilder.Entity("VideoRentalModels.Staff", b =>
                {
                    b.Navigation("Takings");
                });

            modelBuilder.Entity("VideoRentalModels.Type", b =>
                {
                    b.Navigation("Disks");
                });
#pragma warning restore 612, 618
        }
    }
}
