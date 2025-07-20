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

        // Base entities
        public DbSet<Address> Addresses { get; set; }
        
        // Vendors
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<TourOperator> TourOperators { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TransferCompany> TransferCompanies { get; set; }
        public DbSet<RentACar> RentACars { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Cruise> Cruises { get; set; }
        
        // Prices
        public DbSet<HotelPrice> HotelPrices { get; set; }
        public DbSet<TourPrice> TourPrices { get; set; }
        public DbSet<TransferPrice> TransferPrices { get; set; }
        public DbSet<RentACarPrice> RentACarPrices { get; set; }
        public DbSet<GuidePrice> GuidePrices { get; set; }
        public DbSet<AirlinePrice> AirlinePrices { get; set; }
        public DbSet<CruisePrice> CruisePrices { get; set; }
        
        // Packages
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageItem> PackageItems { get; set; }
        
        // Sales
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<SalePerson> SalePersons { get; set; }
        
        // Financial
        public DbSet<CariTransaction> CariTransactions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Prices)
                .WithOne(p => p.Hotel)
                .HasForeignKey(p => p.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TourOperator>()
                .HasMany(to => to.Tours)
                .WithOne(t => t.TourOperator)
                .HasForeignKey(t => t.TourOperatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Prices)
                .WithOne(p => p.Tour)
                .HasForeignKey(p => p.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransferCompany>()
                .HasMany(tc => tc.Prices)
                .WithOne(p => p.TransferCompany)
                .HasForeignKey(p => p.TransferCompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentACar>()
                .HasMany(rc => rc.Prices)
                .WithOne(p => p.RentACar)
                .HasForeignKey(p => p.RentACarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guide>()
                .HasMany(g => g.Prices)
                .WithOne(p => p.Guide)
                .HasForeignKey(p => p.GuideId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Airline>()
                .HasMany(a => a.Prices)
                .WithOne(p => p.Airline)
                .HasForeignKey(p => p.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cruise>()
                .HasMany(c => c.Prices)
                .WithOne(p => p.Cruise)
                .HasForeignKey(p => p.CruiseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Package>()
                .HasMany(p => p.Items)
                .WithOne(pi => pi.Package)
                .HasForeignKey(pi => pi.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Items)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Persons)
                .WithOne(sp => sp.Sale)
                .HasForeignKey(sp => sp.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Items)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CariTransaction>()
                .HasOne(ct => ct.Sale)
                .WithMany()
                .HasForeignKey(ct => ct.SaleId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure indexes
            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.PNRNumber)
                .IsUnique();

            modelBuilder.Entity<Hotel>()
                .HasIndex(h => h.Code)
                .IsUnique();

            modelBuilder.Entity<TourOperator>()
                .HasIndex(to => to.Code)
                .IsUnique();

            modelBuilder.Entity<TransferCompany>()
                .HasIndex(tc => tc.Code)
                .IsUnique();

            modelBuilder.Entity<RentACar>()
                .HasIndex(rc => rc.Code)
                .IsUnique();

            modelBuilder.Entity<Guide>()
                .HasIndex(g => g.Code)
                .IsUnique();

            modelBuilder.Entity<Airline>()
                .HasIndex(a => a.Code)
                .IsUnique();

            modelBuilder.Entity<Cruise>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Package>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Tour>()
                .HasIndex(t => t.Code)
                .IsUnique();

            // Configure composite unique constraints for prices
            modelBuilder.Entity<HotelPrice>()
                .HasIndex(hp => new { hp.HotelId, hp.StartDate, hp.EndDate, hp.RoomType, hp.RoomLocation, hp.BoardType })
                .IsUnique();

            modelBuilder.Entity<TourPrice>()
                .HasIndex(tp => new { tp.TourId, tp.StartDate, tp.EndDate })
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

            modelBuilder.Entity<AirlinePrice>()
                .HasIndex(ap => new { ap.AirlineId, ap.StartDate, ap.EndDate, ap.Route })
                .IsUnique();

            modelBuilder.Entity<CruisePrice>()
                .HasIndex(cp => new { cp.CruiseId, cp.StartDate, cp.EndDate, cp.RoomType, cp.RoomLocation, cp.BoardType })
                .IsUnique();

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Addresses
            var address1 = new Address
            {
                Id = 1,
                Street = "Atatürk Caddesi No:123",
                City = "İstanbul",
                Country = "Türkiye",
                PostalCode = "34000",
                FullAddress = "Atatürk Caddesi No:123, İstanbul, Türkiye",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            var address2 = new Address
            {
                Id = 2,
                Street = "Kızılay Meydanı No:45",
                City = "Ankara",
                Country = "Türkiye",
                PostalCode = "06000",
                FullAddress = "Kızılay Meydanı No:45, Ankara, Türkiye",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<Address>().HasData(address1, address2);

            // Hotels
            var hotel1 = new Hotel
            {
                Id = 1,
                Code = "HTL001",
                Name = "Grand Hotel İstanbul",
                Notes = "5 yıldızlı lüks otel",
                CariCode = "CARI001",
                Phone = "+90 212 555 0101",
                Email = "info@grandhotelistanbul.com",
                AddressId = 1,
                RoomTypes = "Standard,Deluxe,Suite",
                RoomLocations = "Deniz,Şehir",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            var hotel2 = new Hotel
            {
                Id = 2,
                Code = "HTL002",
                Name = "Ankara Plaza Hotel",
                Notes = "4 yıldızlı iş oteli",
                CariCode = "CARI002",
                Phone = "+90 312 555 0202",
                Email = "info@ankaraplaza.com",
                AddressId = 2,
                RoomTypes = "Standard,Business,Executive",
                RoomLocations = "Şehir",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<Hotel>().HasData(hotel1, hotel2);

            // Tour Operators
            var tourOp1 = new TourOperator
            {
                Id = 1,
                Code = "TOP001",
                Name = "Anadolu Turizm",
                Notes = "Güvenilir tur operatörü",
                CariCode = "CARI003",
                Phone = "+90 212 555 0303",
                Email = "info@anadoluturizm.com",
                AddressId = 1,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<TourOperator>().HasData(tourOp1);

            // Tours
            var tour1 = new Tour
            {
                Id = 1,
                Code = "TUR001",
                Name = "Kapadokya Turu",
                VehicleType = "Otobüs",
                Region = "Kapadokya",
                TourOperatorId = 1,
                Description = "2 günlük Kapadokya turu",
                Duration = "2 gün",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<Tour>().HasData(tour1);

            // Airlines
            var airline1 = new Airline
            {
                Id = 1,
                Code = "AIR001",
                Name = "Türk Hava Yolları",
                Notes = "Ulusal havayolu şirketi",
                CariCode = "CARI004",
                Phone = "+90 212 555 0404",
                Email = "info@thy.com",
                AddressId = 1,
                AircraftTypes = "A320,A330,B777",
                FlightRegions = "İç Hatlar,Dış Hatlar",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<Airline>().HasData(airline1);

            // Transfer Companies
            var transfer1 = new TransferCompany
            {
                Id = 1,
                Code = "TRA001",
                Name = "İstanbul Transfer",
                Notes = "Havalimanı transfer hizmeti",
                CariCode = "CARI005",
                Phone = "+90 212 555 0505",
                Email = "info@istanbultransfer.com",
                AddressId = 1,
                Routes = "Havalimanı-Şehir Merkezi",
                VehicleType = "Minibüs,VIP Araç",
                Region = "İstanbul",
                Description = "7/24 transfer hizmeti",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<TransferCompany>().HasData(transfer1);

            // Rent A Car
            var rentACar1 = new RentACar
            {
                Id = 1,
                Code = "RCR001",
                Name = "Anadolu Rent A Car",
                Notes = "Güvenilir araç kiralama",
                CariCode = "CARI006",
                Phone = "+90 212 555 0606",
                Email = "info@anadolurent.com",
                AddressId = 1,
                AvailableCars = "Ekonomik,Orta Sınıf,Lüks",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<RentACar>().HasData(rentACar1);

            // Guides
            var guide1 = new Guide
            {
                Id = 1,
                Code = "GEM001",
                Name = "Ahmet Yılmaz",
                Notes = "Deneyimli rehber",
                CariCode = "CARI007",
                Phone = "+90 532 555 0707",
                Email = "ahmet.yilmaz@guide.com",
                AddressId = 1,
                Languages = "Türkçe,İngilizce,Almanca",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<Guide>().HasData(guide1);

            // Cruises
            var cruise1 = new Cruise
            {
                Id = 1,
                Code = "CRU001",
                Name = "MSC Cruises",
                Notes = "Lüks kruvaziyer",
                CariCode = "CARI008",
                Phone = "+90 212 555 0808",
                Email = "info@msccruises.com",
                AddressId = 1,
                RoomTypes = "İç Kabin,Dış Kabin,Balkonlu,Suit",
                RoomLocations = "Ön,Orta,Arka",
                Description = "Akdeniz kruvaziyerleri",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<Cruise>().HasData(cruise1);

            // Packages
            var package1 = new Package
            {
                Id = 1,
                Code = "PKG001",
                Name = "İstanbul Kapadokya Paketi",
                Description = "3 günlük İstanbul + 2 günlük Kapadokya turu",
                PackagePrice = 2500.00m,
                Currency = "EUR",
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<Package>().HasData(package1);

            // Exchange Rates
            var exchangeRate1 = new ExchangeRate
            {
                Id = 1,
                Date = new DateTime(2024, 1, 1),
                FromCurrency = "EUR",
                ToCurrency = "TRY",
                Currency = "EUR",
                Rate = 35.50m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            var exchangeRate2 = new ExchangeRate
            {
                Id = 2,
                Date = new DateTime(2024, 1, 1),
                FromCurrency = "USD",
                ToCurrency = "TRY",
                Currency = "USD",
                Rate = 32.80m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<ExchangeRate>().HasData(exchangeRate1, exchangeRate2);

            // Sales
            var sale1 = new Sale
            {
                Id = 1,
                PNRNumber = "PNR001",
                CariCode = "CARI001",
                SaleDate = new DateTime(2023, 12, 27),
                CustomerName = "Mehmet Demir",
                Status = "Confirmed",
                AgencyCode = "AGY001",
                PackageCode = "PKG001",
                IsPackageSale = true,
                TotalAmount = 2500.00m,
                Currency = "EUR",
                SellerType = "Package",
                FileCode = "FILE001",
                SalePrice = 2500.00m,
                PurchasePrice = 2000.00m,
                ProductName = "İstanbul Kapadokya Paketi",
                TotalAmountTL = 88750.00m,
                CreatedDate = new DateTime(2023, 12, 27),
                IsActive = true
            };

            modelBuilder.Entity<Sale>().HasData(sale1);

            // Sale Items
            var saleItem1 = new SaleItem
            {
                Id = 1,
                SaleId = 1,
                Date = new DateTime(2023, 12, 27),
                ServiceType = "HTL",
                Description = "Grand Hotel - 2 gece",
                VendorId = 1,
                VendorType = "Hotel",
                UnitPrice = 150.00m,
                TotalPrice = 300.00m,
                Currency = "EUR",
                ItemType = "Hotel",
                ItemId = 1,
                Quantity = 2,
                Price = 150.00m,
                CreatedDate = new DateTime(2023, 12, 27),
                IsActive = true
            };

            modelBuilder.Entity<SaleItem>().HasData(saleItem1);

            // Sale Persons
            var salePerson1 = new SalePerson
            {
                Id = 1,
                SaleId = 1,
                FirstName = "Mehmet",
                LastName = "Demir",
                PersonType = "Adult",
                Age = 35,
                Nationality = "Türk",
                BirthDate = new DateTime(1989, 5, 15),
                PassportNumber = "A12345678",
                CreatedDate = new DateTime(2023, 12, 27),
                IsActive = true
            };

            modelBuilder.Entity<SalePerson>().HasData(salePerson1);

            // Cari Transactions
            var cariTransaction1 = new CariTransaction
            {
                Id = 1,
                CariCode = "CARI001",
                CustomerName = "Mehmet Demir",
                TransactionDate = new DateTime(2023, 12, 27),
                TransactionType = "Credit",
                Amount = 2500.00m,
                Currency = "EUR",
                AmountTL = 88750.00m,
                Description = "İstanbul Kapadokya Paketi satışı",
                PNRNumber = "PNR001",
                SaleId = 1,
                CreatedDate = new DateTime(2023, 12, 27),
                IsActive = true
            };

            modelBuilder.Entity<CariTransaction>().HasData(cariTransaction1);

            // Hotel Prices
            var hotelPrice1 = new HotelPrice
            {
                Id = 1,
                HotelId = 1,
                RoomType = "Standard",
                RoomLocation = "Deniz",
                BoardType = "BB",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 7, 1),
                Currency = "EUR",
                AdultPrice = 150.00m,
                ChildPrice = 75.00m,
                InfantPrice = 0.00m,
                SinglePrice = 120.00m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<HotelPrice>().HasData(hotelPrice1);

            // Tour Prices
            var tourPrice1 = new TourPrice
            {
                Id = 1,
                TourId = 1,
                TourOperatorId = 1,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 7, 1),
                Currency = "EUR",
                AdultPrice = 200.00m,
                ChildPrice = 100.00m,
                InfantPrice = 0.00m,
                SinglePrice = 180.00m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<TourPrice>().HasData(tourPrice1);

            // Airline Prices
            var airlinePrice1 = new AirlinePrice
            {
                Id = 1,
                AirlineId = 1,
                Route = "İstanbul-Ankara",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 7, 1),
                Currency = "EUR",
                AdultPrice = 80.00m,
                ChildPrice = 40.00m,
                InfantPrice = 10.00m,
                SinglePrice = 80.00m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<AirlinePrice>().HasData(airlinePrice1);

            // Transfer Prices
            var transferPrice1 = new TransferPrice
            {
                Id = 1,
                TransferCompanyId = 1,
                TransferId = 1,
                Route = "Havalimanı-Şehir Merkezi",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 7, 1),
                Currency = "EUR",
                AdultPrice = 25.00m,
                ChildPrice = 12.50m,
                InfantPrice = 0.00m,
                SinglePrice = 25.00m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<TransferPrice>().HasData(transferPrice1);

            // Rent A Car Prices
            var rentACarPrice1 = new RentACarPrice
            {
                Id = 1,
                RentACarId = 1,
                DailyPrice = 50.00m,
                WeeklyPrice = 300.00m,
                MonthlyPrice = 1200.00m,
                CarType = "Ekonomik",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 7, 1),
                Currency = "EUR",
                AdultPrice = 50.00m,
                ChildPrice = 0.00m,
                InfantPrice = 0.00m,
                SinglePrice = 50.00m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<RentACarPrice>().HasData(rentACarPrice1);

            // Guide Prices
            var guidePrice1 = new GuidePrice
            {
                Id = 1,
                GuideId = 1,
                DailyPrice = 100.00m,
                HalfDayPrice = 60.00m,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 7, 1),
                Currency = "EUR",
                AdultPrice = 100.00m,
                ChildPrice = 50.00m,
                InfantPrice = 0.00m,
                SinglePrice = 100.00m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<GuidePrice>().HasData(guidePrice1);

            // Cruise Prices
            var cruisePrice1 = new CruisePrice
            {
                Id = 1,
                CruiseId = 1,
                RoomType = "Standard",
                RoomLocation = "Orta",
                BoardType = "FB",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 7, 1),
                Currency = "EUR",
                AdultPrice = 500.00m,
                ChildPrice = 250.00m,
                InfantPrice = 50.00m,
                SinglePrice = 450.00m,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<CruisePrice>().HasData(cruisePrice1);

            // Package Items
            var packageItem1 = new PackageItem
            {
                Id = 1,
                PackageId = 1,
                Date = new DateTime(2024, 1, 2),
                ServiceType = "HTL",
                Description = "Grand Hotel - 2 gece",
                VendorId = 1,
                VendorType = "Hotel",
                ItemType = "Hotel",
                ItemId = 1,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            var packageItem2 = new PackageItem
            {
                Id = 2,
                PackageId = 1,
                Date = new DateTime(2024, 1, 4),
                ServiceType = "TUR",
                Description = "Kapadokya Turu - 2 gün",
                VendorId = 1,
                VendorType = "TourOperator",
                ItemType = "Tour",
                ItemId = 1,
                CreatedDate = new DateTime(2024, 1, 1),
                IsActive = true
            };

            modelBuilder.Entity<PackageItem>().HasData(packageItem1, packageItem2);
        }
    }
} 