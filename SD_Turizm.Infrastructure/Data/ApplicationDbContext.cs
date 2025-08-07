using Microsoft.EntityFrameworkCore;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Entities.Prices;

namespace SD_Turizm.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Core Entities
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<TourOperator> TourOperators { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TransferCompany> TransferCompanies { get; set; }
        public DbSet<RentACar> RentACars { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Cruise> Cruises { get; set; }

        // Price Entities
        public DbSet<HotelPrice> HotelPrices { get; set; }
        public DbSet<TourPrice> TourPrices { get; set; }
        public DbSet<TransferPrice> TransferPrices { get; set; }
        public DbSet<RentACarPrice> RentACarPrices { get; set; }
        public DbSet<GuidePrice> GuidePrices { get; set; }
        public DbSet<AirlinePrice> AirlinePrices { get; set; }
        public DbSet<CruisePrice> CruisePrices { get; set; }

        // Package Entities
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageItem> PackageItems { get; set; }

        // Sale Entities
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<SalePerson> SalePersons { get; set; }

        // Financial
        public DbSet<CariTransaction> CariTransactions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        
        // Authentication
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            SeedData(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Address)
                .WithMany()
                .HasForeignKey(h => h.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tour>()
                .HasOne(t => t.TourOperator)
                .WithMany(to => to.Tours)
                .HasForeignKey(t => t.TourOperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TourPrice>()
                .HasOne(tp => tp.Tour)
                .WithMany(t => t.TourPrices)
                .HasForeignKey(tp => tp.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TourPrice>()
                .HasOne(tp => tp.TourOperator)
                .WithMany(to => to.TourPrices)
                .HasForeignKey(tp => tp.TourOperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HotelPrice>()
                .HasOne(hp => hp.Hotel)
                .WithMany(h => h.HotelPrices)
                .HasForeignKey(hp => hp.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Address)
                .WithMany()
                .HasForeignKey(s => s.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.SaleItems)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalePerson>()
                .HasOne(sp => sp.Sale)
                .WithMany(s => s.SalePersons)
                .HasForeignKey(sp => sp.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PackageItem>()
                .HasOne(pi => pi.Package)
                .WithMany(p => p.PackageItems)
                .HasForeignKey(pi => pi.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CariTransaction>()
                .HasOne(ct => ct.Sale)
                .WithMany()
                .HasForeignKey(ct => ct.SaleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure unique indexes
            modelBuilder.Entity<Hotel>()
                .HasIndex(h => h.Code)
                .IsUnique();

            modelBuilder.Entity<TourOperator>()
                .HasIndex(to => to.Code)
                .IsUnique();

            modelBuilder.Entity<Tour>()
                .HasIndex(t => t.Code)
                .IsUnique();

            modelBuilder.Entity<Airline>()
                .HasIndex(a => a.Code)
                .IsUnique();

            modelBuilder.Entity<Cruise>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Guide>()
                .HasIndex(g => g.Code)
                .IsUnique();

            modelBuilder.Entity<RentACar>()
                .HasIndex(rc => rc.Code)
                .IsUnique();

            modelBuilder.Entity<TransferCompany>()
                .HasIndex(tc => tc.Code)
                .IsUnique();

            modelBuilder.Entity<Package>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.PNRNumber)
                .IsUnique();

            modelBuilder.Entity<ExchangeRate>()
                .HasIndex(er => new { er.FromCurrency, er.ToCurrency, er.Date })
                .IsUnique();

            // Configure composite unique indexes for prices
            modelBuilder.Entity<HotelPrice>()
                .HasIndex(hp => new { hp.HotelId, hp.StartDate, hp.EndDate, hp.RoomType, hp.RoomLocation, hp.BoardType })
                .IsUnique();

            modelBuilder.Entity<TourPrice>()
                .HasIndex(tp => new { tp.TourId, tp.StartDate, tp.EndDate })
                .IsUnique();

            modelBuilder.Entity<AirlinePrice>()
                .HasIndex(ap => new { ap.AirlineId, ap.StartDate, ap.EndDate, ap.Route })
                .IsUnique();

            modelBuilder.Entity<CruisePrice>()
                .HasIndex(cp => new { cp.CruiseId, cp.StartDate, cp.EndDate, cp.RoomType, cp.RoomLocation, cp.BoardType })
                .IsUnique();

            modelBuilder.Entity<TransferPrice>()
                .HasIndex(tp => new { tp.TransferCompanyId, tp.StartDate, tp.EndDate, tp.Route })
                .IsUnique();

            modelBuilder.Entity<RentACarPrice>()
                .HasIndex(rp => new { rp.RentACarId, rp.StartDate, rp.EndDate, rp.CarType })
                .IsUnique();

            modelBuilder.Entity<GuidePrice>()
                .HasIndex(gp => new { gp.GuideId, gp.StartDate, gp.EndDate })
                .IsUnique();

            // Configure decimal precision
            modelBuilder.Entity<CariTransaction>()
                .Property(ct => ct.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CariTransaction>()
                .Property(ct => ct.AmountTL)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ExchangeRate>()
                .Property(er => er.Rate)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Package>()
                .Property(p => p.PackagePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.SalePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.PurchasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalAmountTL)
                .HasPrecision(18, 2);

            // Configure BasePrice decimal precision
            modelBuilder.Entity<AirlinePrice>()
                .Property(ap => ap.AdultPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AirlinePrice>()
                .Property(ap => ap.ChildPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AirlinePrice>()
                .Property(ap => ap.InfantPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AirlinePrice>()
                .Property(ap => ap.SinglePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CruisePrice>()
                .Property(cp => cp.AdultPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CruisePrice>()
                .Property(cp => cp.ChildPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CruisePrice>()
                .Property(cp => cp.InfantPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CruisePrice>()
                .Property(cp => cp.SinglePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransferPrice>()
                .Property(tp => tp.AdultPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransferPrice>()
                .Property(tp => tp.ChildPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransferPrice>()
                .Property(tp => tp.InfantPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransferPrice>()
                .Property(tp => tp.SinglePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HotelPrice>()
                .Property(hp => hp.AdultPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HotelPrice>()
                .Property(hp => hp.ChildPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HotelPrice>()
                .Property(hp => hp.InfantPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HotelPrice>()
                .Property(hp => hp.SinglePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TourPrice>()
                .Property(tp => tp.AdultPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TourPrice>()
                .Property(tp => tp.ChildPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TourPrice>()
                .Property(tp => tp.InfantPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TourPrice>()
                .Property(tp => tp.SinglePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentACarPrice>()
                .Property(rcp => rcp.AdultPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentACarPrice>()
                .Property(rcp => rcp.ChildPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentACarPrice>()
                .Property(rcp => rcp.InfantPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentACarPrice>()
                .Property(rcp => rcp.SinglePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentACarPrice>()
                .Property(rcp => rcp.DailyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentACarPrice>()
                .Property(rcp => rcp.WeeklyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentACarPrice>()
                .Property(rcp => rcp.MonthlyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GuidePrice>()
                .Property(gp => gp.AdultPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GuidePrice>()
                .Property(gp => gp.ChildPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GuidePrice>()
                .Property(gp => gp.InfantPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GuidePrice>()
                .Property(gp => gp.SinglePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GuidePrice>()
                .Property(gp => gp.DailyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GuidePrice>()
                .Property(gp => gp.HalfDayPrice)
                .HasPrecision(18, 2);

            // Configure authentication relationships
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Authentication Data
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 100, Name = "Admin", Description = "Sistem yöneticisi", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Role { Id = 101, Name = "Manager", Description = "Yönetici", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Role { Id = 102, Name = "User", Description = "Kullanıcı", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 100, Name = "Hotels.Read", Description = "Otel okuma izni", Resource = "Hotels", Action = "Read", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 101, Name = "Hotels.Write", Description = "Otel yazma izni", Resource = "Hotels", Action = "Write", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 102, Name = "Sales.Read", Description = "Satış okuma izni", Resource = "Sales", Action = "Read", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 103, Name = "Sales.Write", Description = "Satış yazma izni", Resource = "Sales", Action = "Write", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 104, Name = "Reports.Read", Description = "Rapor okuma izni", Resource = "Reports", Action = "Read", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Admin User (Password: Admin123!)
            var adminPasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg="; // SHA256 hash of "Admin123!"
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 100, 
                    Username = "admin", 
                    Email = "admin@sdturizm.com", 
                    PasswordHash = adminPasswordHash,
                    FirstName = "Admin",
                    LastName = "User",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed User Roles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 100, UserId = 100, RoleId = 100, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Role Permissions
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { Id = 100, RoleId = 100, PermissionId = 100, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 101, RoleId = 100, PermissionId = 101, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 102, RoleId = 100, PermissionId = 102, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 103, RoleId = 100, PermissionId = 103, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 104, RoleId = 100, PermissionId = 104, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Addresses
            modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, Street = "Atatürk Caddesi No:123", City = "İstanbul", Country = "Türkiye", PostalCode = "34000", FullAddress = "Atatürk Caddesi No:123, İstanbul, Türkiye", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Address { Id = 2, Street = "Kızılay Meydanı No:45", City = "Ankara", Country = "Türkiye", PostalCode = "06000", FullAddress = "Kızılay Meydanı No:45, Ankara, Türkiye", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Address { Id = 3, Street = "Alsancak Mahallesi No:67", City = "İzmir", Country = "Türkiye", PostalCode = "35000", FullAddress = "Alsancak Mahallesi No:67, İzmir, Türkiye", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Hotels
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Code = "HTL001", Name = "Grand Hotel İstanbul", Notes = "5 yıldızlı lüks otel", CariCode = "CARI001", Phone = "+90 212 555 0101", Email = "info@grandhotel.com", AddressId = 1, RoomLocations = "Deniz,Göl,Şehir", RoomTypes = "SGL,DBL,TRP,DBL+1,TRP+1", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Hotel { Id = 2, Code = "HTL002", Name = "Ankara Plaza Hotel", Notes = "4 yıldızlı iş oteli", CariCode = "CARI002", Phone = "+90 312 555 0202", Email = "info@ankaraplaza.com", AddressId = 2, RoomLocations = "Şehir,Park", RoomTypes = "SGL,DBL,TRP", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Hotel { Id = 3, Code = "HTL003", Name = "İzmir Resort Hotel", Notes = "Sahil kenarında resort", CariCode = "CARI003", Phone = "+90 232 555 0303", Email = "info@izmirresort.com", AddressId = 3, RoomLocations = "Deniz,Sahil", RoomTypes = "SGL,DBL,TRP,DBL+1", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Tour Operators
            modelBuilder.Entity<TourOperator>().HasData(
                new TourOperator { Id = 1, Code = "TOUR001", Name = "Anadolu Turizm", Notes = "Güvenilir tur operatörü", CariCode = "CARI004", Phone = "+90 212 555 0404", Email = "info@anadoluturizm.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TourOperator { Id = 2, Code = "TOUR002", Name = "Kapadokya Tours", Notes = "Kapadokya uzmanı", CariCode = "CARI005", Phone = "+90 384 555 0505", Email = "info@kapadokyatours.com", AddressId = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Tours
            modelBuilder.Entity<Tour>().HasData(
                new Tour { Id = 1, Code = "TUR001", Name = "İstanbul Tarihi Yarımada Turu", VehicleType = "Otobüs", Region = "İstanbul", TourOperatorId = 1, Description = "Ayasofya, Topkapı Sarayı, Sultanahmet Camii", Duration = "1 Gün", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Tour { Id = 2, Code = "TUR002", Name = "Kapadokya Balon Turu", VehicleType = "Minibüs", Region = "Kapadokya", TourOperatorId = 2, Description = "Balon turu ve vadiler", Duration = "2 Gün", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Sales
            modelBuilder.Entity<Sale>().HasData(
                new Sale { Id = 1, PNRNumber = "PNR001", CariCode = "CARI001", SaleDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), CustomerName = "Mehmet Demir", Status = "Confirmed", AgencyCode = "AGY001", PackageCode = "PKG001", IsPackageSale = true, TotalAmount = 2500.00m, Currency = "EUR", SellerType = "Package", FileCode = "FILE001", SalePrice = 2500.00m, PurchasePrice = 2000.00m, ProductName = "İstanbul Kapadokya Paketi", TotalAmountTL = 88750.00m, AddressId = 1, CreatedDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Sale { Id = 2, PNRNumber = "PNR002", CariCode = "CARI002", SaleDate = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), CustomerName = "Ayşe Yılmaz", Status = "Pending", AgencyCode = "AGY002", PackageCode = "PKG002", IsPackageSale = true, TotalAmount = 1800.00m, Currency = "EUR", SellerType = "Package", FileCode = "FILE002", SalePrice = 1800.00m, PurchasePrice = 1500.00m, ProductName = "Ankara Turu", TotalAmountTL = 63900.00m, AddressId = 2, CreatedDate = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Exchange Rates
            modelBuilder.Entity<ExchangeRate>().HasData(
                new ExchangeRate { Id = 1, Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), FromCurrency = "EUR", ToCurrency = "TRY", Currency = "EUR", Rate = 35.50m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new ExchangeRate { Id = 2, Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), FromCurrency = "USD", ToCurrency = "TRY", Currency = "USD", Rate = 32.80m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Hotel Prices
            modelBuilder.Entity<HotelPrice>().HasData(
                new HotelPrice { Id = 1, HotelId = 1, RoomType = "DBL", RoomLocation = "Deniz", BoardType = "BB", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 150.00m, ChildPrice = 75.00m, InfantPrice = 0.00m, SinglePrice = 120.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new HotelPrice { Id = 2, HotelId = 2, RoomType = "SGL", RoomLocation = "Şehir", BoardType = "HB", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 100.00m, ChildPrice = 50.00m, InfantPrice = 0.00m, SinglePrice = 100.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Tour Prices
            modelBuilder.Entity<TourPrice>().HasData(
                new TourPrice { Id = 1, TourId = 1, TourOperatorId = 1, StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 80.00m, ChildPrice = 40.00m, InfantPrice = 0.00m, SinglePrice = 80.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TourPrice { Id = 2, TourId = 2, TourOperatorId = 2, StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 200.00m, ChildPrice = 100.00m, InfantPrice = 0.00m, SinglePrice = 200.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Cari Transactions
            modelBuilder.Entity<CariTransaction>().HasData(
                new CariTransaction { Id = 1, CariCode = "CARI001", CustomerName = "Mehmet Demir", TransactionDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), TransactionType = "Credit", Amount = 2500.00m, Currency = "EUR", AmountTL = 88750.00m, Description = "İstanbul Kapadokya Paketi satışı", PNRNumber = "PNR001", SaleId = 1, CreatedDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new CariTransaction { Id = 2, CariCode = "CARI002", CustomerName = "Ayşe Yılmaz", TransactionDate = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), TransactionType = "Credit", Amount = 1800.00m, Currency = "EUR", AmountTL = 63900.00m, Description = "Ankara Turu satışı", PNRNumber = "PNR002", SaleId = 2, CreatedDate = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Guides
            modelBuilder.Entity<Guide>().HasData(
                new Guide { Id = 1, Code = "GUIDE001", Name = "Ahmet Yılmaz", Languages = "Türkçe,İngilizce,Almanca", Notes = "Deneyimli rehber", CariCode = "CARI006", Phone = "+90 212 555 0606", Email = "ahmet@guide.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Guide { Id = 2, Code = "GUIDE002", Name = "Fatma Demir", Languages = "Türkçe,İngilizce,İspanyolca", Notes = "Kapadokya uzmanı", CariCode = "CARI007", Phone = "+90 384 555 0707", Email = "fatma@guide.com", AddressId = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Transfer Companies
            modelBuilder.Entity<TransferCompany>().HasData(
                new TransferCompany { Id = 1, Code = "TRANS001", Name = "İstanbul Transfer", Routes = "Havalimanı-Şehir,Şehir-Havalimanı", VehicleType = "Minibüs", Region = "İstanbul", Notes = "Güvenilir transfer hizmeti", CariCode = "CARI008", Phone = "+90 212 555 0808", Email = "info@istanbultransfer.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TransferCompany { Id = 2, Code = "TRANS002", Name = "Ankara Transfer", Routes = "Havalimanı-Şehir,Şehir-Havalimanı", VehicleType = "Otobüs", Region = "Ankara", Notes = "Konforlu transfer", CariCode = "CARI009", Phone = "+90 312 555 0909", Email = "info@ankaratransfer.com", AddressId = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Airlines
            modelBuilder.Entity<Airline>().HasData(
                new Airline { Id = 1, Code = "AIR001", Name = "Türk Hava Yolları", AircraftTypes = "A320,A330,B777", FlightRegions = "İç Hat,Dış Hat", Notes = "Ulusal havayolu", CariCode = "CARI010", Phone = "+90 212 555 1010", Email = "info@thy.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Airline { Id = 2, Code = "AIR002", Name = "Pegasus Airlines", AircraftTypes = "A320,A321", FlightRegions = "İç Hat,Dış Hat", Notes = "Düşük maliyetli havayolu", CariCode = "CARI011", Phone = "+90 212 555 1111", Email = "info@flypgs.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Cruises
            modelBuilder.Entity<Cruise>().HasData(
                new Cruise { Id = 1, Code = "CRUISE001", Name = "MSC Cruises", RoomLocations = "İç Kabin,Dış Kabin,Balkon", RoomTypes = "SGL,DBL,TRP,DBL+1", Notes = "Lüks kruvaziyer", CariCode = "CARI012", Phone = "+90 212 555 1212", Email = "info@msccruises.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Cruise { Id = 2, Code = "CRUISE002", Name = "Royal Caribbean", RoomLocations = "İç Kabin,Dış Kabin,Balkon,Suit", RoomTypes = "SGL,DBL,TRP,DBL+1,TRP+1", Notes = "Aile dostu kruvaziyer", CariCode = "CARI013", Phone = "+90 212 555 1313", Email = "info@royalcaribbean.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Rent A Car
            modelBuilder.Entity<RentACar>().HasData(
                new RentACar { Id = 1, Code = "RENT001", Name = "Avis Rent A Car", AvailableCars = "Ekonomik,Orta Sınıf,Lüks", Notes = "Güvenilir araç kiralama", CariCode = "CARI014", Phone = "+90 212 555 1414", Email = "info@avis.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RentACar { Id = 2, Code = "RENT002", Name = "Hertz Rent A Car", AvailableCars = "Ekonomik,Orta Sınıf,Lüks,Spor", Notes = "Kaliteli araç kiralama", CariCode = "CARI015", Phone = "+90 212 555 1515", Email = "info@hertz.com", AddressId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Packages
            modelBuilder.Entity<Package>().HasData(
                new Package { Id = 1, Code = "PKG001", Name = "İstanbul Kapadokya Paketi", Description = "3 gün İstanbul + 2 gün Kapadokya", PackagePrice = 2500.00m, Currency = "EUR", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Package { Id = 2, Code = "PKG002", Name = "Ankara Turu", Description = "2 gün Ankara şehir turu", PackagePrice = 1800.00m, Currency = "EUR", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Package Items
            modelBuilder.Entity<PackageItem>().HasData(
                new PackageItem { Id = 1, PackageId = 1, Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), ServiceType = "HTL", Description = "Grand Hotel İstanbul", VendorType = "Hotel", ItemType = "Hotel", ItemId = 1, Quantity = 1, UnitPrice = 150.00m, TotalPrice = 150.00m, Currency = "EUR", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new PackageItem { Id = 2, PackageId = 1, Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), ServiceType = "TUR", Description = "İstanbul Tarihi Yarımada Turu", VendorType = "Tour", ItemType = "Tour", ItemId = 1, Quantity = 1, UnitPrice = 80.00m, TotalPrice = 80.00m, Currency = "EUR", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new PackageItem { Id = 3, PackageId = 2, Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), ServiceType = "HTL", Description = "Ankara Plaza Hotel", VendorType = "Hotel", ItemType = "Hotel", ItemId = 2, Quantity = 1, UnitPrice = 100.00m, TotalPrice = 100.00m, Currency = "EUR", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Sale Items
            modelBuilder.Entity<SaleItem>().HasData(
                new SaleItem { Id = 1, SaleId = 1, ItemType = "Package", VendorType = "Internal", ItemId = 1, Quantity = 1, UnitPrice = 2500.00m, TotalPrice = 2500.00m, Price = 2500.00m, Currency = "EUR", CreatedDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new SaleItem { Id = 2, SaleId = 2, ItemType = "Package", VendorType = "Internal", ItemId = 2, Quantity = 1, UnitPrice = 1800.00m, TotalPrice = 1800.00m, Price = 1800.00m, Currency = "EUR", CreatedDate = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Sale Persons
            modelBuilder.Entity<SalePerson>().HasData(
                new SalePerson { Id = 1, SaleId = 1, FirstName = "Mehmet", LastName = "Demir", Nationality = "Türk", PassportNumber = "A12345678", CreatedDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new SalePerson { Id = 2, SaleId = 2, FirstName = "Ayşe", LastName = "Yılmaz", Nationality = "Türk", PassportNumber = "B87654321", CreatedDate = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Airline Prices
            modelBuilder.Entity<AirlinePrice>().HasData(
                new AirlinePrice { Id = 1, AirlineId = 1, Route = "İstanbul-Ankara", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 120.00m, ChildPrice = 60.00m, InfantPrice = 0.00m, SinglePrice = 120.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new AirlinePrice { Id = 2, AirlineId = 2, Route = "İstanbul-İzmir", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 100.00m, ChildPrice = 50.00m, InfantPrice = 0.00m, SinglePrice = 100.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Cruise Prices
            modelBuilder.Entity<CruisePrice>().HasData(
                new CruisePrice { Id = 1, CruiseId = 1, RoomType = "DBL", RoomLocation = "Dış Kabin", BoardType = "FB", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 800.00m, ChildPrice = 400.00m, InfantPrice = 0.00m, SinglePrice = 800.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new CruisePrice { Id = 2, CruiseId = 2, RoomType = "TRP", RoomLocation = "Balkon", BoardType = "AI", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 1200.00m, ChildPrice = 600.00m, InfantPrice = 0.00m, SinglePrice = 1200.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Transfer Prices
            modelBuilder.Entity<TransferPrice>().HasData(
                new TransferPrice { Id = 1, TransferCompanyId = 1, Route = "Havalimanı-Şehir", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 25.00m, ChildPrice = 12.50m, InfantPrice = 0.00m, SinglePrice = 25.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TransferPrice { Id = 2, TransferCompanyId = 2, Route = "Havalimanı-Şehir", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 30.00m, ChildPrice = 15.00m, InfantPrice = 0.00m, SinglePrice = 30.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Rent A Car Prices
            modelBuilder.Entity<RentACarPrice>().HasData(
                new RentACarPrice { Id = 1, RentACarId = 1, CarType = "Ekonomik", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 50.00m, ChildPrice = 0.00m, InfantPrice = 0.00m, SinglePrice = 50.00m, DailyPrice = 50.00m, WeeklyPrice = 300.00m, MonthlyPrice = 1200.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RentACarPrice { Id = 2, RentACarId = 2, CarType = "Lüks", StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 100.00m, ChildPrice = 0.00m, InfantPrice = 0.00m, SinglePrice = 100.00m, DailyPrice = 100.00m, WeeklyPrice = 600.00m, MonthlyPrice = 2400.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Guide Prices
            modelBuilder.Entity<GuidePrice>().HasData(
                new GuidePrice { Id = 1, GuideId = 1, StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 60.00m, ChildPrice = 30.00m, InfantPrice = 0.00m, SinglePrice = 60.00m, DailyPrice = 200.00m, HalfDayPrice = 120.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new GuidePrice { Id = 2, GuideId = 2, StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 80.00m, ChildPrice = 40.00m, InfantPrice = 0.00m, SinglePrice = 80.00m, DailyPrice = 250.00m, HalfDayPrice = 150.00m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );
        }
    }
} 