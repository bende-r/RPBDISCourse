using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace VideoRentalModels
{
    public partial class VideoRentalContext : DbContext
    {
        public VideoRentalContext()
        {
        }

        public VideoRentalContext(DbContextOptions<VideoRentalContext> options) : base(options)
        {
        }

        public virtual DbSet<Clientele> Clienteles { get; set; } = null!;
        public virtual DbSet<Disk> Disks { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Pricelist> Pricelists { get; set; } = null!;
        public virtual DbSet<Producer> Producers { get; set; } = null!;
        public virtual DbSet<Taking> Takings { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<ViewAllDisk> ViewAllDisks { get; set; } = null!;
        public virtual DbSet<Staff> Staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("D:\\Labs\\3 course\\5 sem\\РПБДИС\\Video_rental\\Video_rental\\VideoRentalWebApplication\\appsettings.json");

            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("SQLConnection");
            _ = optionsBuilder
                .UseSqlServer(connectionString)

                .Options;
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientele>(entity =>
            {
                entity.HasKey(e => e.ClientId)
                    .HasName("PK__Clientel__E67E1A04315A897C");

                entity.ToTable("Clientele");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.Addres)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Passport)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Disk>(entity =>
            {
                entity.Property(e => e.DiskId).HasColumnName("DiskID");

                entity.Property(e => e.CreationYear)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.MainActor)
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.Recording).HasColumnType("date");

                entity.Property(e => e.Title)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.DiskTypeNavigation)
                    .WithMany(p => p.Disks)
                    .HasForeignKey(d => d.DiskType)
                    .HasConstraintName("FK__Disks__DiskType__3F466844");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Disks)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK__Disks__GenreID__3E52440B");

                entity.HasOne(d => d.ProducerNavigation)
                    .WithMany(p => p.Disks)
                    .HasForeignKey(d => d.Producer)
                    .HasConstraintName("FK__Disks__Producer__3D5E1FD2");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.PositionId).HasColumnName("PositionID");

                entity.Property(e => e.Title)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pricelist>(entity =>
            {
                entity.HasKey(e => e.PriceId)
                    .HasName("PK__Pricelis__4957584FC87A6E36");

                entity.ToTable("Pricelist");

                entity.Property(e => e.PriceId).HasColumnName("PriceID");

                entity.Property(e => e.DiskId).HasColumnName("DiskID");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Disk)
                    .WithMany(p => p.Pricelists)
                    .HasForeignKey(d => d.DiskId)
                    .HasConstraintName("FK__Pricelist__DiskI__4316F928");
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.HasKey(e => e.ProduceId)
                    .HasName("PK__Producer__2EEBECB32A180669");

                entity.Property(e => e.ProduceId).HasColumnName("ProduceID");

                entity.Property(e => e.Country)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Taking>(entity =>
            {
                entity.HasKey(e => e.TakeId)
                    .HasName("PK__Taking__AC0C2240A69151F3");

                entity.ToTable("Taking");

                entity.Property(e => e.TakeId).HasColumnName("TakeID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.DateOfCapture).HasColumnType("date");

                entity.Property(e => e.DiskId).HasColumnName("DiskID");

                entity.Property(e => e.ReturnDate).HasColumnType("date");

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Takings)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__Taking__ClientID__4D94879B");

                entity.HasOne(d => d.Disk)
                    .WithMany(p => p.Takings)
                    .HasForeignKey(d => d.DiskId)
                    .HasConstraintName("FK__Taking__DiskID__4E88ABD4");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Takings)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__Taking__StaffID__4F7CD00D");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewAllDisk>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_AllDisks");

                entity.Property(e => e.Country)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreationYear)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.DiskId).HasColumnName("DiskID");

                entity.Property(e => e.DiskTitle)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.GenreTitle)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MainActor)
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Recording).HasColumnType("date");

                entity.Property(e => e.TypeTitle)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.Property(e => e.DateOfEmployment).HasColumnType("date");

                entity.Property(e => e.Middlename)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PositionId).HasColumnName("PositionID");

                entity.Property(e => e.Surname)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Staff__PositionI__47DBAE45");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
