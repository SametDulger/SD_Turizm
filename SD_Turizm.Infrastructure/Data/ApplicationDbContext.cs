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

        // Audit
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Lookup Tables
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<BoardType> BoardTypes { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<SaleStatus> SaleStatuses { get; set; }
        public DbSet<PersonType> PersonTypes { get; set; }
        public DbSet<RoomLocation> RoomLocations { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            SeedData(modelBuilder);

            // Configure relationships and constraints
            // Hotel entity'si Vendor'dan türetildiği için AddressId yok
            // Address bilgisi Vendor.Address property'sinde tutuluyor

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

            // Configure Hotel Price property
            modelBuilder.Entity<Hotel>()
                .Property(h => h.Price)
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

            // Configure PackageItem relationships
            modelBuilder.Entity<PackageItem>()
                .HasOne(pi => pi.Package)
                .WithMany(p => p.PackageItems)
                .HasForeignKey(pi => pi.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PackageItem>()
                .HasOne(pi => pi.Vendor)
                .WithMany()
                .HasForeignKey(pi => pi.VendorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Sale relationships
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Address)
                .WithMany()
                .HasForeignKey(s => s.AddressId)
                .OnDelete(DeleteBehavior.SetNull);

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

            // Configure performance indexes
            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.CreatedDate);
                
            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.CustomerName);
                
            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.Status);
                
            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.CreatedDate);
                
            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.Username);
                
            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.TableName);
                
            modelBuilder.Entity<CariTransaction>()
                .HasIndex(ct => ct.TransactionDate);
                
            modelBuilder.Entity<CariTransaction>()
                .HasIndex(ct => ct.Amount);

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
                new Role { Id = 100, Name = "Admin", Description = "Sistem yöneticisi", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Role { Id = 101, Name = "Manager", Description = "Yönetici", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Role { Id = 102, Name = "User", Description = "Kullanıcı", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Admin User (Username: admin, Password: Admin123!)
        
            // Admin123! için AuthService tarafından üretilen doğru hash
            var adminPasswordHash = "$2a$12$6YE13Vek.PNfeHBg0jPQ8eCOKMFEKuzN7dO3/dnSrGxVmIS9CzV/u";
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 100, 
                    Username = "admin", 
                    Email = "admin@sdturizm.com", 
                    PasswordHash = adminPasswordHash,
                    FirstName = "System",
                    LastName = "Administrator",
                    IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastLoginDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed Admin User Role
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 100, UserId = 100, RoleId = 100, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 100, Name = "Hotels.Read", Description = "Otel okuma izni", Resource = "Hotels", Action = "Read", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 101, Name = "Hotels.Write", Description = "Otel yazma izni", Resource = "Hotels", Action = "Write", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 102, Name = "Sales.Read", Description = "Satış okuma izni", Resource = "Sales", Action = "Read", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 103, Name = "Sales.Write", Description = "Satış yazma izni", Resource = "Sales", Action = "Write", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Permission { Id = 104, Name = "Reports.Read", Description = "Rapor okuma izni", Resource = "Reports", Action = "Read", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );



            // Seed Role Permissions
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { Id = 100, RoleId = 100, PermissionId = 100, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 101, RoleId = 100, PermissionId = 101, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 102, RoleId = 100, PermissionId = 102, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 103, RoleId = 100, PermissionId = 103, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RolePermission { Id = 104, RoleId = 100, PermissionId = 104, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Addresses
            modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, Street = "Atatürk Caddesi No:123", City = "İstanbul", Country = "Türkiye", PostalCode = "34000", FullAddress = "Atatürk Caddesi No:123, İstanbul, Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Address { Id = 2, Street = "Kızılay Meydanı No:45", City = "Ankara", Country = "Türkiye", PostalCode = "06000", FullAddress = "Kızılay Meydanı No:45, Ankara, Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Address { Id = 3, Street = "Alsancak Mahallesi No:67", City = "İzmir", Country = "Türkiye", PostalCode = "35000", FullAddress = "Alsancak Mahallesi No:67, İzmir, Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Hotels
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Code = "HTL001", Name = "Grand Hotel İstanbul", Description = "5 yıldızlı lüks otel", Phone = "+90 212 555 0101", Email = "info@grandhotel.com", Address = "İstanbul, Türkiye", Country = "Türkiye", RoomLocations = "Deniz,Göl,Şehir", RoomTypes = "SGL,DBL,TRP,DBL+1,TRP+1", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Hotel { Id = 2, Code = "HTL002", Name = "Ankara Plaza Hotel", Description = "4 yıldızlı iş oteli", Phone = "+90 312 555 0202", Email = "info@ankaraplaza.com", Address = "Ankara, Türkiye", Country = "Türkiye", RoomLocations = "Şehir,Park", RoomTypes = "SGL,DBL,TRP", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Hotel { Id = 3, Code = "HTL003", Name = "İzmir Resort Hotel", Description = "Sahil kenarında resort", Phone = "+90 232 555 0303", Email = "info@izmirresort.com", Address = "İzmir, Türkiye", Country = "Türkiye", RoomLocations = "Deniz,Sahil", RoomTypes = "SGL,DBL,TRP,DBL+1", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Hotel { Id = 4, Code = "HTL004", Name = "Antalya Beach Resort", Description = "Deniz kenarında all-inclusive resort", Phone = "+90 242 555 0404", Email = "info@antalyabeach.com", Address = "Antalya, Türkiye", Country = "Türkiye", RoomLocations = "Deniz,Havuz,Bahçe", RoomTypes = "SGL,DBL,TRP,DBL+1,TRP+1,Suite", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Hotel { Id = 5, Code = "HTL005", Name = "Bodrum Marina Hotel", Description = "Marina manzaralı butik otel", Phone = "+90 252 555 0505", Email = "info@bodrummarina.com", Address = "Bodrum, Türkiye", Country = "Türkiye", RoomLocations = "Marina,Şehir", RoomTypes = "SGL,DBL,Suite", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Hotel { Id = 6, Code = "HTL006", Name = "Kapadokya Cave Hotel", Description = "Mağara otel deneyimi", Phone = "+90 384 555 0606", Email = "info@capadociacave.com", Address = "Nevşehir, Türkiye", Country = "Türkiye", RoomLocations = "Vadi,Kayalar", RoomTypes = "Cave Room,Deluxe Cave,Suite", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Tour Operators (ID range: 10-19)
            modelBuilder.Entity<TourOperator>().HasData(
                new TourOperator { Id = 10, Code = "TOUR001", Name = "Anadolu Turizm", Description = "Güvenilir tur operatörü", Phone = "+90 212 555 0404", Email = "info@anadoluturizm.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TourOperator { Id = 11, Code = "TOUR002", Name = "Kapadokya Tours", Description = "Kapadokya uzmanı", Phone = "+90 384 555 0505", Email = "info@kapadokyatours.com", Address = "Nevşehir, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TourOperator { Id = 12, Code = "TOUR003", Name = "Akdeniz Turizm", Description = "Akdeniz bölgesi uzmanı", Phone = "+90 242 555 0606", Email = "info@akdenizturizm.com", Address = "Antalya, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TourOperator { Id = 13, Code = "TOUR004", Name = "Ege Turizm", Description = "Ege bölgesi rehberi", Phone = "+90 232 555 0707", Email = "info@egeturizm.com", Address = "İzmir, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Tours
            modelBuilder.Entity<Tour>().HasData(
                new Tour { Id = 1, Code = "TUR001", Name = "İstanbul Tarihi Yarımada Turu", VehicleType = "Otobüs", Region = "İstanbul", TourOperatorId = 10, Description = "Ayasofya, Topkapı Sarayı, Sultanahmet Camii", Duration = "1 Gün", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Tour { Id = 2, Code = "TUR002", Name = "Kapadokya Balon Turu", VehicleType = "Minibüs", Region = "Kapadokya", TourOperatorId = 11, Description = "Balon turu ve vadiler", Duration = "2 Gün", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Tour { Id = 3, Code = "TUR003", Name = "Antalya Şehir Turu", VehicleType = "Minibüs", Region = "Antalya", TourOperatorId = 12, Description = "Kaleiçi, Hadrian Kapısı, Düden Şelalesi", Duration = "1 Gün", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Tour { Id = 4, Code = "TUR004", Name = "Pamukkale Hierapolis Turu", VehicleType = "Otobüs", Region = "Denizli", TourOperatorId = 12, Description = "Beyaz travertenler ve antik şehir", Duration = "1 Gün", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Tour { Id = 5, Code = "TUR005", Name = "Efes Antik Kenti Turu", VehicleType = "Otobüs", Region = "İzmir", TourOperatorId = 13, Description = "Efes antik kenti ve Meryem Ana Evi", Duration = "1 Gün", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Tour { Id = 6, Code = "TUR006", Name = "Bodrum Tekne Turu", VehicleType = "Tekne", Region = "Bodrum", TourOperatorId = 13, Description = "Kara Ada, Karaincir koyu ve yüzme molası", Duration = "1 Gün", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Sales - Son 1 ay içinde dağıtılmış tarihler (Temmuz-Ağustos 2025)
            var currentDate = new DateTime(2025, 8, 9, 0, 0, 0, DateTimeKind.Utc);
            var lastMonth = new DateTime(2025, 7, 15, 0, 0, 0, DateTimeKind.Utc);
            var lastWeek = new DateTime(2025, 8, 2, 0, 0, 0, DateTimeKind.Utc);
            var yesterday = new DateTime(2025, 8, 8, 0, 0, 0, DateTimeKind.Utc);
            
            modelBuilder.Entity<Sale>().HasData(
                new Sale { Id = 1, PNRNumber = "PNR001", CariCode = "CARI001", SaleDate = lastMonth, CustomerName = "Mehmet Demir", Status = "Confirmed", AgencyCode = "AGY001", PackageCode = "PKG001", IsPackageSale = true, TotalAmount = 2500.00m, Currency = "EUR", SellerType = "Otel", FileCode = "FILE001", SalePrice = 2500.00m, PurchasePrice = 2000.00m, ProductName = "İstanbul Kapadokya Paketi", TotalAmountTL = 88750.00m, AddressId = 1, CreatedDate = lastMonth, IsActive = true },
                new Sale { Id = 2, PNRNumber = "PNR002", CariCode = "CARI002", SaleDate = lastWeek, CustomerName = "Ayşe Yılmaz", Status = "Pending", AgencyCode = "AGY002", PackageCode = "PKG002", IsPackageSale = true, TotalAmount = 1800.00m, Currency = "EUR", SellerType = "Tur", FileCode = "FILE002", SalePrice = 1800.00m, PurchasePrice = 1500.00m, ProductName = "Ankara Turu", TotalAmountTL = 63900.00m, AddressId = 2, CreatedDate = lastWeek, IsActive = true },
                new Sale { Id = 3, PNRNumber = "PNR003", CariCode = "CARI003", SaleDate = yesterday, CustomerName = "Can Özkan", Status = "Confirmed", AgencyCode = "AGY003", PackageCode = "PKG003", IsPackageSale = false, TotalAmount = 3200.00m, Currency = "EUR", SellerType = "Uçak", FileCode = "FILE003", SalePrice = 3200.00m, PurchasePrice = 2800.00m, ProductName = "Antalya Uçak Bileti", TotalAmountTL = 113600.00m, AddressId = 3, CreatedDate = yesterday, IsActive = true },
                new Sale { Id = 4, PNRNumber = "PNR004", CariCode = "CARI004", SaleDate = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc), CustomerName = "Zeynep Kaya", Status = "Processing", AgencyCode = "AGY004", PackageCode = "PKG004", IsPackageSale = true, TotalAmount = 2800.00m, Currency = "EUR", SellerType = "Kruvaziyer", FileCode = "FILE004", SalePrice = 2800.00m, PurchasePrice = 2400.00m, ProductName = "Akdeniz Kruvaziyer Turu", TotalAmountTL = 99400.00m, AddressId = 1, CreatedDate = new DateTime(2025, 8, 6, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Sale { Id = 5, PNRNumber = "PNR005", CariCode = "CARI005", SaleDate = new DateTime(2025, 7, 25, 0, 0, 0, DateTimeKind.Utc), CustomerName = "Ali Şen", Status = "Confirmed", AgencyCode = "AGY005", PackageCode = "PKG005", IsPackageSale = false, TotalAmount = 1200.00m, Currency = "EUR", SellerType = "Tur", FileCode = "FILE005", SalePrice = 1200.00m, PurchasePrice = 1000.00m, ProductName = "Efes Antik Kenti Turu", TotalAmountTL = 42600.00m, AddressId = 2, CreatedDate = new DateTime(2025, 7, 25, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Exchange Rates
            modelBuilder.Entity<ExchangeRate>().HasData(
                new ExchangeRate { Id = 1, Date = new DateTime(2025, 8, 9, 0, 0, 0, DateTimeKind.Utc), FromCurrency = "EUR", ToCurrency = "TRY", Currency = "EUR", Rate = 35.50m, CreatedDate = new DateTime(2025, 8, 9, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new ExchangeRate { Id = 2, Date = new DateTime(2025, 8, 9, 0, 0, 0, DateTimeKind.Utc), FromCurrency = "USD", ToCurrency = "TRY", Currency = "USD", Rate = 32.80m, CreatedDate = new DateTime(2025, 8, 9, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Hotel Prices
            modelBuilder.Entity<HotelPrice>().HasData(
                new HotelPrice { Id = 1, HotelId = 1, RoomType = "DBL", RoomLocation = "Deniz", BoardType = "BB", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 150.00m, ChildPrice = 75.00m, InfantPrice = 0.00m, SinglePrice = 120.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new HotelPrice { Id = 2, HotelId = 2, RoomType = "SGL", RoomLocation = "Şehir", BoardType = "HB", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 100.00m, ChildPrice = 50.00m, InfantPrice = 0.00m, SinglePrice = 100.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Tour Prices
            modelBuilder.Entity<TourPrice>().HasData(
                new TourPrice { Id = 1, TourId = 1, TourOperatorId = 10, StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 80.00m, ChildPrice = 40.00m, InfantPrice = 0.00m, SinglePrice = 80.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TourPrice { Id = 2, TourId = 2, TourOperatorId = 11, StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 200.00m, ChildPrice = 100.00m, InfantPrice = 0.00m, SinglePrice = 200.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Cari Transactions
            modelBuilder.Entity<CariTransaction>().HasData(
                new CariTransaction { Id = 1, CariCode = "CARI001", CustomerName = "Mehmet Demir", TransactionDate = lastMonth, TransactionType = "Credit", Amount = 2500.00m, Currency = "EUR", AmountTL = 88750.00m, Description = "İstanbul Kapadokya Paketi satışı", PNRNumber = "PNR001", SaleId = 1, CreatedDate = lastMonth, IsActive = true },
                new CariTransaction { Id = 2, CariCode = "CARI002", CustomerName = "Ayşe Yılmaz", TransactionDate = lastWeek, TransactionType = "Credit", Amount = 1800.00m, Currency = "EUR", AmountTL = 63900.00m, Description = "Ankara Turu satışı", PNRNumber = "PNR002", SaleId = 2, CreatedDate = lastWeek, IsActive = true }
            );

            // Seed Guides (ID range: 30-39)
            modelBuilder.Entity<Guide>().HasData(
                new Guide { Id = 30, Code = "GUIDE001", Name = "Ahmet Yılmaz", Languages = "Türkçe,İngilizce,Almanca", Description = "Deneyimli rehber", Phone = "+90 212 555 0606", Email = "ahmet@guide.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Guide { Id = 31, Code = "GUIDE002", Name = "Fatma Demir", Languages = "Türkçe,İngilizce,İspanyolca", Description = "Kapadokya uzmanı", Phone = "+90 384 555 0707", Email = "fatma@guide.com", Address = "Nevşehir, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Transfer Companies (ID range: 40-49)
            modelBuilder.Entity<TransferCompany>().HasData(
                new TransferCompany { Id = 40, Code = "TRANS001", Name = "İstanbul Transfer", Routes = "Havalimanı-Şehir,Şehir-Havalimanı", VehicleType = "Minibüs", Region = "İstanbul", Description = "Güvenilir transfer hizmeti", Phone = "+90 212 555 0808", Email = "info@istanbultransfer.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TransferCompany { Id = 41, Code = "TRANS002", Name = "Ankara Transfer", Routes = "Havalimanı-Şehir,Şehir-Havalimanı", VehicleType = "Otobüs", Region = "Ankara", Description = "Konforlu transfer", Phone = "+90 312 555 0909", Email = "info@ankaratransfer.com", Address = "Ankara, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Airlines (ID range: 50-59)
            modelBuilder.Entity<Airline>().HasData(
                new Airline { Id = 50, Code = "AIR001", Name = "Türk Hava Yolları", AircraftTypes = "A320,A330,B777", FlightRegions = "İç Hat,Dış Hat", Description = "Ulusal havayolu", Phone = "+90 212 555 1010", Email = "info@thy.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Airline { Id = 51, Code = "AIR002", Name = "Pegasus Airlines", AircraftTypes = "A320,A321", FlightRegions = "İç Hat,Dış Hat", Description = "Düşük maliyetli havayolu", Phone = "+90 212 555 1111", Email = "info@flypgs.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Cruises (ID range: 20-29)
            modelBuilder.Entity<Cruise>().HasData(
                new Cruise { Id = 20, Code = "CRUISE001", Name = "MSC Cruises", RoomLocations = "İç Kabin,Dış Kabin,Balkon", RoomTypes = "SGL,DBL,TRP,DBL+1", Description = "Lüks kruvaziyer", Phone = "+90 212 555 1212", Email = "info@msccruises.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Cruise { Id = 21, Code = "CRUISE002", Name = "Royal Caribbean", RoomLocations = "İç Kabin,Dış Kabin,Balkon,Suit", RoomTypes = "SGL,DBL,TRP,DBL+1,TRP+1", Description = "Aile dostu kruvaziyer", Phone = "+90 212 555 1313", Email = "info@royalcaribbean.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Rent A Car (ID range: 60-69)
            modelBuilder.Entity<RentACar>().HasData(
                new RentACar { Id = 60, Code = "RENT001", Name = "Avis Rent A Car", AvailableCars = "Ekonomik,Orta Sınıf,Lüks", Description = "Güvenilir araç kiralama", Phone = "+90 212 555 1414", Email = "info@avis.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RentACar { Id = 61, Code = "RENT002", Name = "Hertz Rent A Car", AvailableCars = "Ekonomik,Orta Sınıf,Lüks,Spor", Description = "Kaliteli araç kiralama", Phone = "+90 212 555 1515", Email = "info@hertz.com", Address = "İstanbul, Türkiye", Country = "Türkiye", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Packages (ID range: 70-79)
            modelBuilder.Entity<Package>().HasData(
                new Package { Id = 70, Code = "PKG001", Name = "İstanbul Kapadokya Paketi", Description = "3 gün İstanbul + 2 gün Kapadokya", PackagePrice = 2500.00m, Currency = "EUR", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Package { Id = 71, Code = "PKG002", Name = "Ankara Turu", Description = "2 gün Ankara şehir turu", PackagePrice = 1800.00m, Currency = "EUR", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Package Items (ID range: 80-89)
            modelBuilder.Entity<PackageItem>().HasData(
                new PackageItem { Id = 80, PackageId = 70, Date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), ServiceType = "HTL", Description = "Grand Hotel İstanbul", VendorType = "Hotel", ItemType = "Hotel", ItemId = 1, Quantity = 1, UnitPrice = 150.00m, TotalPrice = 150.00m, Currency = "EUR", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new PackageItem { Id = 81, PackageId = 70, Date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), ServiceType = "TUR", Description = "İstanbul Tarihi Yarımada Turu", VendorType = "Tour", ItemType = "Tour", ItemId = 1, Quantity = 1, UnitPrice = 80.00m, TotalPrice = 80.00m, Currency = "EUR", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new PackageItem { Id = 82, PackageId = 71, Date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), ServiceType = "HTL", Description = "Ankara Plaza Hotel", VendorType = "Hotel", ItemType = "Hotel", ItemId = 2, Quantity = 1, UnitPrice = 100.00m, TotalPrice = 100.00m, Currency = "EUR", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Sale Items (ID range: 90-99)
            modelBuilder.Entity<SaleItem>().HasData(
                new SaleItem { Id = 90, SaleId = 1, ItemType = "Package", VendorType = "Internal", ItemId = 70, Quantity = 1, UnitPrice = 2500.00m, TotalPrice = 2500.00m, Price = 2500.00m, Currency = "EUR", CreatedDate = new DateTime(2025, 7, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new SaleItem { Id = 91, SaleId = 2, ItemType = "Package", VendorType = "Internal", ItemId = 71, Quantity = 1, UnitPrice = 1800.00m, TotalPrice = 1800.00m, Price = 1800.00m, Currency = "EUR", CreatedDate = new DateTime(2025, 8, 2, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Sale Persons (ID range: 200-209)
            modelBuilder.Entity<SalePerson>().HasData(
                new SalePerson { Id = 200, SaleId = 1, FirstName = "Mehmet", LastName = "Demir", Nationality = "Türk", PassportNumber = "A12345678", CreatedDate = new DateTime(2025, 7, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new SalePerson { Id = 201, SaleId = 2, FirstName = "Ayşe", LastName = "Yılmaz", Nationality = "Türk", PassportNumber = "B87654321", CreatedDate = new DateTime(2025, 8, 2, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Airline Prices (ID range: 300-309)
            modelBuilder.Entity<AirlinePrice>().HasData(
                new AirlinePrice { Id = 300, AirlineId = 50, Route = "İstanbul-Ankara", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 120.00m, ChildPrice = 60.00m, InfantPrice = 0.00m, SinglePrice = 120.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new AirlinePrice { Id = 301, AirlineId = 51, Route = "İstanbul-İzmir", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 100.00m, ChildPrice = 50.00m, InfantPrice = 0.00m, SinglePrice = 100.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Cruise Prices (ID range: 310-319)
            modelBuilder.Entity<CruisePrice>().HasData(
                new CruisePrice { Id = 310, CruiseId = 20, RoomType = "DBL", RoomLocation = "Dış Kabin", BoardType = "FB", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 800.00m, ChildPrice = 400.00m, InfantPrice = 0.00m, SinglePrice = 800.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new CruisePrice { Id = 311, CruiseId = 21, RoomType = "TRP", RoomLocation = "Balkon", BoardType = "AI", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 1200.00m, ChildPrice = 600.00m, InfantPrice = 0.00m, SinglePrice = 1200.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Transfer Prices (ID range: 320-329)
            modelBuilder.Entity<TransferPrice>().HasData(
                new TransferPrice { Id = 320, TransferCompanyId = 40, Route = "Havalimanı-Şehir", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 25.00m, ChildPrice = 12.50m, InfantPrice = 0.00m, SinglePrice = 25.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new TransferPrice { Id = 321, TransferCompanyId = 41, Route = "Havalimanı-Şehir", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 30.00m, ChildPrice = 15.00m, InfantPrice = 0.00m, SinglePrice = 30.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Rent A Car Prices (ID range: 330-339)
            modelBuilder.Entity<RentACarPrice>().HasData(
                new RentACarPrice { Id = 330, RentACarId = 60, CarType = "Ekonomik", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 50.00m, ChildPrice = 0.00m, InfantPrice = 0.00m, SinglePrice = 50.00m, DailyPrice = 50.00m, WeeklyPrice = 300.00m, MonthlyPrice = 1200.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new RentACarPrice { Id = 331, RentACarId = 61, CarType = "Lüks", StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 100.00m, ChildPrice = 0.00m, InfantPrice = 0.00m, SinglePrice = 100.00m, DailyPrice = 100.00m, WeeklyPrice = 600.00m, MonthlyPrice = 2400.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Guide Prices (ID range: 340-349)
            modelBuilder.Entity<GuidePrice>().HasData(
                new GuidePrice { Id = 340, GuideId = 30, StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 60.00m, ChildPrice = 30.00m, InfantPrice = 0.00m, SinglePrice = 60.00m, DailyPrice = 200.00m, HalfDayPrice = 120.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new GuidePrice { Id = 341, GuideId = 31, StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), Currency = "EUR", AdultPrice = 80.00m, ChildPrice = 40.00m, InfantPrice = 0.00m, SinglePrice = 80.00m, DailyPrice = 250.00m, HalfDayPrice = 150.00m, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Vendors (sample vendor data)
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor { Id = 1001, Code = "VND001", Name = "ABC Turizm", Description = "Güvenilir iş ortağı", Phone = "+90 212 555 0101", Email = "info@abcturizm.com", Address = "İstanbul, Türkiye", Country = "Türkiye", Type = "Agency", CariCode = "ABC001", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Vendor { Id = 1002, Code = "VND002", Name = "XYZ Travel", Description = "Deneyimli tur operatörü", Phone = "+90 312 555 0202", Email = "info@xyztravel.com", Address = "Ankara, Türkiye", Country = "Türkiye", Type = "TourOperator", CariCode = "XYZ002", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Vendor { Id = 1003, Code = "VND003", Name = "Başarı Tour", Description = "Kaliteli hizmet", Phone = "+90 232 555 0303", Email = "info@basaritour.com", Address = "İzmir, Türkiye", Country = "Türkiye", Type = "Agency", CariCode = "BSR003", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Vendor { Id = 1004, Code = "VND004", Name = "Deniz Turizm", Description = "Kruvaziyer uzmanı", Phone = "+90 242 555 0404", Email = "info@denizturizm.com", Address = "Antalya, Türkiye", Country = "Türkiye", Type = "Cruise", CariCode = "DNZ004", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true },
                new Vendor { Id = 1005, Code = "VND005", Name = "Güneş Travel", Description = "Aile dostu hizmet", Phone = "+90 252 555 0505", Email = "info@gunestravel.com", Address = "Bodrum, Türkiye", Country = "Türkiye", Type = "Agency", CariCode = "GNS005", CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true }
            );

            // Seed Lookup Tables
            SeedLookupData(modelBuilder);
        }

        private void SeedLookupData(ModelBuilder modelBuilder)
        {
            // Seed Currencies
            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Code = "EUR", Name = "Euro", Symbol = "€", Flag = "🇪🇺", IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Currency { Id = 2, Code = "USD", Name = "Amerikan Doları", Symbol = "$", Flag = "🇺🇸", IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Currency { Id = 3, Code = "TRY", Name = "Türk Lirası", Symbol = "₺", Flag = "🇹🇷", IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Room Types
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { Id = 1, Code = "SGL", Name = "Tek Kişilik", Description = "Tek yataklı oda", Capacity = 1, IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new RoomType { Id = 2, Code = "DBL", Name = "Çift Kişilik", Description = "Çift yataklı oda", Capacity = 2, IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new RoomType { Id = 3, Code = "TRP", Name = "Üç Kişilik", Description = "Üç yataklı oda", Capacity = 3, IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new RoomType { Id = 4, Code = "DBL+1", Name = "Çift Kişilik + 1", Description = "İki kişilik + bir ek yatak", Capacity = 3, IsActive = true, DisplayOrder = 4, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new RoomType { Id = 5, Code = "TRP+1", Name = "Üç Kişilik + 1", Description = "Üç kişilik + bir ek yatak", Capacity = 4, IsActive = true, DisplayOrder = 5, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Board Types
            modelBuilder.Entity<BoardType>().HasData(
                new BoardType { Id = 1, Code = "OB", Name = "Oda Kahvaltı", Description = "Sadece kahvaltı dahil", IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new BoardType { Id = 2, Code = "BB", Name = "Yarım Pansiyon", Description = "Kahvaltı ve akşam yemeği", IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new BoardType { Id = 3, Code = "HB", Name = "Tam Pansiyon", Description = "Üç öğün yemek", IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new BoardType { Id = 4, Code = "FB", Name = "Her Şey Dahil", Description = "Yemek ve içecekler dahil", IsActive = true, DisplayOrder = 4, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new BoardType { Id = 5, Code = "AI", Name = "Her Şey Dahil", Description = "Alkollü ve alkolsüz içecekler", IsActive = true, DisplayOrder = 5, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new BoardType { Id = 6, Code = "UAI", Name = "Ultra Her Şey Dahil", Description = "Premium alkollü içecekler dahil", IsActive = true, DisplayOrder = 6, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Vendor Types
            modelBuilder.Entity<VendorType>().HasData(
                new VendorType { Id = 1, Code = "Airline", Name = "Havayolu", Description = "Havayolu şirketleri", IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VendorType { Id = 2, Code = "Hotel", Name = "Otel", Description = "Konaklama tesisleri", IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VendorType { Id = 3, Code = "Cruise", Name = "Kruvaziyer", Description = "Kruvaziyer şirketleri", IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VendorType { Id = 4, Code = "Guide", Name = "Rehber", Description = "Turist rehberleri", IsActive = true, DisplayOrder = 4, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VendorType { Id = 5, Code = "RentACar", Name = "Araç Kiralama", Description = "Araç kiralama şirketleri", IsActive = true, DisplayOrder = 5, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VendorType { Id = 6, Code = "TransferCompany", Name = "Transfer Şirketi", Description = "Transfer hizmeti sağlayıcıları", IsActive = true, DisplayOrder = 6, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Sale Statuses
            modelBuilder.Entity<SaleStatus>().HasData(
                new SaleStatus { Id = 1, Code = "Pending", Name = "Beklemede", Description = "İşlem beklemede", Color = "warning", IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SaleStatus { Id = 2, Code = "Confirmed", Name = "Onaylandı", Description = "İşlem onaylandı", Color = "success", IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SaleStatus { Id = 3, Code = "Cancelled", Name = "İptal Edildi", Description = "İşlem iptal edildi", Color = "danger", IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SaleStatus { Id = 4, Code = "Completed", Name = "Tamamlandı", Description = "İşlem tamamlandı", Color = "primary", IsActive = true, DisplayOrder = 4, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Person Types
            modelBuilder.Entity<PersonType>().HasData(
                new PersonType { Id = 1, Code = "PAX", Name = "Yolcu", Description = "Yetişkin yolcu", MinAge = 12, MaxAge = 99, IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new PersonType { Id = 2, Code = "CHD", Name = "Çocuk", Description = "Çocuk yolcu", MinAge = 2, MaxAge = 11, IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new PersonType { Id = 3, Code = "INF", Name = "Bebek", Description = "Bebek yolcu", MinAge = 0, MaxAge = 1, IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Room Locations
            modelBuilder.Entity<RoomLocation>().HasData(
                new RoomLocation { Id = 1, Code = "DNZ", Name = "Deniz Manzaralı", Description = "Deniz manzaralı oda", IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new RoomLocation { Id = 2, Code = "GNL", Name = "Genel", Description = "Standart konum", IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new RoomLocation { Id = 3, Code = "PRK", Name = "Park Manzaralı", Description = "Park manzaralı oda", IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed Product Types
            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { Id = 1, Code = "Otel", Name = "Otel", Description = "Konaklama hizmeti", Icon = "fa-bed", IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new ProductType { Id = 2, Code = "Tur", Name = "Tur", Description = "Tur hizmeti", Icon = "fa-map", IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new ProductType { Id = 3, Code = "Transfer", Name = "Transfer", Description = "Transfer hizmeti", Icon = "fa-bus", IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new ProductType { Id = 4, Code = "RentACar", Name = "Rent A Car", Description = "Araç kiralama", Icon = "fa-car", IsActive = true, DisplayOrder = 4, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new ProductType { Id = 5, Code = "Rehber", Name = "Rehber", Description = "Rehberlik hizmeti", Icon = "fa-user-tie", IsActive = true, DisplayOrder = 5, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new ProductType { Id = 6, Code = "Uçak", Name = "Uçak", Description = "Havayolu hizmeti", Icon = "fa-plane", IsActive = true, DisplayOrder = 6, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new ProductType { Id = 7, Code = "Gemi", Name = "Gemi", Description = "Kruvaziyer hizmeti", Icon = "fa-ship", IsActive = true, DisplayOrder = 7, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // TransactionType seed data
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType { Id = 1, Code = "Income", Name = "Gelir", Description = "Gelir işlemi", IsActive = true, DisplayOrder = 1, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new TransactionType { Id = 2, Code = "Expense", Name = "Gider", Description = "Gider işlemi", IsActive = true, DisplayOrder = 2, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new TransactionType { Id = 3, Code = "BORC", Name = "Borç", Description = "Borç işlemi", IsActive = true, DisplayOrder = 3, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new TransactionType { Id = 4, Code = "ALACAK", Name = "Alacak", Description = "Alacak işlemi", IsActive = true, DisplayOrder = 4, CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
} 