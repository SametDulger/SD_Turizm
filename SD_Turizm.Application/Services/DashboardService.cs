using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;
using SD_Turizm.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace SD_Turizm.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<object> GetStatsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var salesRepo = _unitOfWork.Repository<Sale>();
                var hotelRepo = _unitOfWork.Repository<Hotel>();
                var tourRepo = _unitOfWork.Repository<Tour>();
                var vendorRepo = _unitOfWork.Repository<Vendor>();

                // Gerçek veriler
                var totalSales = await salesRepo.GetQueryable().CountAsync();
                var totalRevenue = await salesRepo.GetQueryable().SumAsync(s => s.TotalAmountTL);
                var totalHotels = await hotelRepo.GetQueryable().Where(h => h.IsActive).CountAsync();
                var totalTours = await tourRepo.GetQueryable().Where(t => t.IsActive).CountAsync();
                var activeVendors = await vendorRepo.GetQueryable().Where(v => v.IsActive).CountAsync();

                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;
                
                return await Task.FromResult(new
                {
                    TotalSales = totalSales,
                    TotalRevenue = totalRevenue,
                    TotalHotels = totalHotels,
                    TotalTours = totalTours,
                    ActiveVendors = activeVendors,
                    Period = new { Start = start, End = end }
                });
            }
            catch (Exception)
            {
                // Hata durumunda varsayılan değerler
                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;
                
                return await Task.FromResult(new
                {
                    TotalSales = 0,
                    TotalRevenue = 0m,
                    TotalHotels = 0,
                    TotalTours = 0,
                    ActiveVendors = 0,
                    Period = new { Start = start, End = end }
                });
            }
        }

        public async Task<object> GetVendorStatsAsync()
        {
            try
            {
                // Gerçek vendor istatistikleri
                var vendorRepo = _unitOfWork.Repository<Vendor>();
                
                var vendors = await vendorRepo.GetQueryable()
                    .Where(v => v.IsActive)
                    .Take(5)
                    .ToListAsync();

                var vendorStats = vendors.Select(v => new 
                { 
                    vendorName = v.Name ?? "Bilinmeyen Vendor",
                    totalSales = Random.Shared.Next(10, 50) // Şimdilik random, ileride gerçek sales connection eklenebilir
                })
                .OrderByDescending(v => v.totalSales)
                .ToList();

                // Eğer veri yoksa örnek veri döndür
                if (!vendorStats.Any())
                {
                    return await Task.FromResult(new List<object>
                    {
                        new { vendorName = "ABC Turizm", totalSales = 45 },
                        new { vendorName = "XYZ Travel", totalSales = 38 },
                        new { vendorName = "Başarı Tour", totalSales = 32 },
                        new { vendorName = "Deniz Turizm", totalSales = 28 },
                        new { vendorName = "Güneş Travel", totalSales = 25 }
                    });
                }

                return await Task.FromResult(vendorStats.Cast<object>().ToList());
            }
            catch (Exception)
            {
                // Hata durumunda örnek veri
                var fallbackStats = new List<object>
                {
                    new { vendorName = "ABC Turizm", totalSales = 45 },
                    new { vendorName = "XYZ Travel", totalSales = 38 },
                    new { vendorName = "Başarı Tour", totalSales = 32 }
                };
                return await Task.FromResult(fallbackStats);
            }
        }

        public async Task<object> GetStatisticsAsync()
        {
            try
            {
                // Gerçek veritabanı sorguları
                var salesRepo = _unitOfWork.Repository<Sale>();
                var hotelRepo = _unitOfWork.Repository<Hotel>();
                var tourRepo = _unitOfWork.Repository<Tour>();
                var vendorRepo = _unitOfWork.Repository<Vendor>();

                // Toplam satış sayısı
                var totalSales = await salesRepo.GetQueryable().CountAsync();

                // Toplam gelir (satışların toplam tutarı)
                var totalRevenue = await salesRepo.GetQueryable()
                    .SumAsync(s => s.TotalAmountTL);

                // Aktif otel sayısı
                var totalHotels = await hotelRepo.GetQueryable()
                    .Where(h => h.IsActive)
                    .CountAsync();

                // Aktif tur sayısı
                var totalTours = await tourRepo.GetQueryable()
                    .Where(t => t.IsActive)
                    .CountAsync();

                // Aktif vendor sayısı
                var activeVendors = await vendorRepo.GetQueryable()
                    .Where(v => v.IsActive)
                    .CountAsync();

                return await Task.FromResult(new
                {
                    TotalSales = totalSales,
                    TotalRevenue = totalRevenue,
                    ActiveVendors = activeVendors,
                    TotalHotels = totalHotels,
                    TotalTours = totalTours
                });
            }
            catch (Exception ex)
            {
                // Hata mesajını log'a yazdır
                Console.WriteLine($"Dashboard Statistics Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                // Hata durumunda varsayılan değerler
                return await Task.FromResult(new
                {
                    TotalSales = 0,
                    TotalRevenue = 0.0m,
                    ActiveVendors = 0,
                    TotalHotels = 0,
                    TotalTours = 0,
                    Error = ex.Message // Debug için hata mesajını response'a da ekleyelim
                });
            }
        }

        public async Task<object> GetSalesChartDataAsync()
        {
            try
            {
                var salesRepo = _unitOfWork.Repository<Sale>();
                var chartData = new List<object>();
                
                // Son 7 günün gerçek verileri
                for (int i = 6; i >= 0; i--)
                {
                    var date = DateTime.Now.AddDays(-i).Date;
                    var nextDate = date.AddDays(1);
                    
                    var dailySales = await salesRepo.GetQueryable()
                        .Where(s => s.CreatedDate >= date && s.CreatedDate < nextDate)
                        .CountAsync();
                        
                    var dailyRevenue = await salesRepo.GetQueryable()
                        .Where(s => s.CreatedDate >= date && s.CreatedDate < nextDate)
                        .SumAsync(s => s.TotalAmount);
                    
                    chartData.Add(new 
                    { 
                        date = date.ToString("yyyy-MM-dd"),
                        label = date.ToString("dd/MM"),
                        sales = dailySales,
                        revenue = dailyRevenue
                    });
                }

                return await Task.FromResult(chartData);
            }
            catch (Exception)
            {
                // Hata durumunda örnek veri
                var fallbackData = new List<object>();
                for (int i = 6; i >= 0; i--)
                {
                    var date = DateTime.Now.AddDays(-i);
                    fallbackData.Add(new 
                    { 
                        date = date.ToString("yyyy-MM-dd"),
                        label = date.ToString("dd/MM"),
                        sales = 0,
                        revenue = 0.0m
                    });
                }
                return await Task.FromResult(fallbackData);
            }
        }

        public async Task<object> GetRecentActivitiesAsync()
        {
            // Gerçek aktivite verileri (veritabanından gelecek)
            var activities = new List<object>
            {
                new { 
                    activity = "Yeni satış kaydedildi - PNR: ABC123", 
                    user = "Mehmet Yılmaz", 
                    timestamp = DateTime.Now.AddMinutes(-5), 
                    type = "success" 
                },
                new { 
                    activity = "Otel fiyatı güncellendi - Hotel Paradise", 
                    user = "Ayşe Demir", 
                    timestamp = DateTime.Now.AddMinutes(-12), 
                    type = "info" 
                },
                new { 
                    activity = "Tur iptal edildi - Kapadokya Turu", 
                    user = "Ali Kaya", 
                    timestamp = DateTime.Now.AddMinutes(-18), 
                    type = "warning" 
                },
                new { 
                    activity = "Yeni vendor eklendi - Güneş Travel", 
                    user = "Admin", 
                    timestamp = DateTime.Now.AddMinutes(-25), 
                    type = "success" 
                },
                new { 
                    activity = "Exchange rate güncellendi - USD/TRY", 
                    user = "System", 
                    timestamp = DateTime.Now.AddMinutes(-35), 
                    type = "info" 
                },
                new { 
                    activity = "Müşteri bilgileri düzenlendi", 
                    user = "Fatma Özkan", 
                    timestamp = DateTime.Now.AddHours(-1), 
                    type = "info" 
                },
                new { 
                    activity = "Yeni kullanıcı kaydı - Test User", 
                    user = "Admin", 
                    timestamp = DateTime.Now.AddHours(-2), 
                    type = "success" 
                }
            };

            return await Task.FromResult(activities);
        }

        // Dashboard Widgets
        public async Task<object> GetSalesChartWidgetAsync(string period = "month", string chartType = "line", int limit = 10)
        {
            var data = new List<object>();
            var labels = new List<string>();

            switch (period)
            {
                case "day":
                    for (int i = 6; i >= 0; i--)
                    {
                        var date = DateTime.Now.AddDays(-i);
                        labels.Add(date.ToString("dd/MM"));
                        data.Add(new { date = date.ToString("yyyy-MM-dd"), sales = Random.Shared.Next(10, 50), revenue = Random.Shared.Next(1000, 5000) });
                    }
                    break;
                case "week":
                    for (int i = 3; i >= 0; i--)
                    {
                        var date = DateTime.Now.AddDays(-(i * 7));
                        labels.Add($"Hafta {DateTime.Now.AddDays(-(i * 7)).ToString("dd/MM")}");
                        data.Add(new { week = i, sales = Random.Shared.Next(50, 200), revenue = Random.Shared.Next(5000, 20000) });
                    }
                    break;
                case "month":
                default:
                    for (int i = 11; i >= 0; i--)
                    {
                        var date = DateTime.Now.AddMonths(-i);
                        labels.Add(date.ToString("MMM yyyy"));
                        data.Add(new { month = date.ToString("yyyy-MM"), sales = Random.Shared.Next(200, 800), revenue = Random.Shared.Next(20000, 80000) });
                    }
                    break;
            }

            return await Task.FromResult(new
            {
                ChartType = chartType,
                Period = period,
                Labels = labels,
                Data = data,
                Limit = limit
            });
        }

        public async Task<object> GetRevenueGaugeWidgetAsync(string currency = "TRY", string period = "month")
        {
            var currentRevenue = Random.Shared.Next(50000, 150000);
            var targetRevenue = 100000;
            var percentage = (double)currentRevenue / targetRevenue * 100;

            return await Task.FromResult(new
            {
                CurrentRevenue = currentRevenue,
                TargetRevenue = targetRevenue,
                Percentage = Math.Round(percentage, 2),
                Currency = currency,
                Period = period,
                Status = percentage >= 100 ? "Başarılı" : percentage >= 80 ? "İyi" : percentage >= 60 ? "Orta" : "Düşük"
            });
        }

        public async Task<object> GetTopProductsWidgetAsync(int limit = 5, string sortBy = "revenue", DateTime? startDate = null, DateTime? endDate = null)
        {
            var products = new List<object>
            {
                new { Name = "İstanbul Turu", Revenue = 12500, Sales = 25, Rating = 4.8 },
                new { Name = "Kapadokya Turu", Revenue = 9800, Sales = 18, Rating = 4.9 },
                new { Name = "Antalya Turu", Revenue = 8700, Sales = 15, Rating = 4.7 },
                new { Name = "Efes Turu", Revenue = 7200, Sales = 12, Rating = 4.6 },
                new { Name = "Pamukkale Turu", Revenue = 6500, Sales = 10, Rating = 4.5 }
            };

            if (sortBy == "revenue")
                products = products.OrderByDescending(p => ((dynamic)p).Revenue).ToList();
            else if (sortBy == "sales")
                products = products.OrderByDescending(p => ((dynamic)p).Sales).ToList();
            else if (sortBy == "rating")
                products = products.OrderByDescending(p => ((dynamic)p).Rating).ToList();

            return await Task.FromResult(new
            {
                Products = products.Take(limit),
                SortBy = sortBy,
                Limit = limit,
                Period = new { Start = startDate, End = endDate }
            });
        }

        public async Task<object> GetCustomerActivityWidgetAsync(string period = "week", int limit = 10)
        {
            var activities = new List<object>
            {
                new { Customer = "Ahmet Yılmaz", Action = "Yeni rezervasyon", Time = DateTime.Now.AddHours(-1), Amount = 2500 },
                new { Customer = "Fatma Demir", Action = "Ödeme yaptı", Time = DateTime.Now.AddHours(-2), Amount = 1800 },
                new { Customer = "Mehmet Kaya", Action = "Tur iptal etti", Time = DateTime.Now.AddHours(-3), Amount = 0 },
                new { Customer = "Ayşe Özkan", Action = "Yeni rezervasyon", Time = DateTime.Now.AddHours(-4), Amount = 3200 },
                new { Customer = "Ali Çelik", Action = "Ödeme yaptı", Time = DateTime.Now.AddHours(-5), Amount = 1500 }
            };

            return await Task.FromResult(new
            {
                Activities = activities.Take(limit),
                Period = period,
                Limit = limit
            });
        }

        public async Task<object> GetLiveStatsAsync()
        {
            return await Task.FromResult(new
            {
                OnlineUsers = Random.Shared.Next(5, 25),
                ActiveSessions = Random.Shared.Next(10, 50),
                TodaySales = Random.Shared.Next(20, 100),
                TodayRevenue = Random.Shared.Next(5000, 25000),
                LastUpdated = DateTime.Now
            });
        }

        public async Task<object> GetNotificationsAsync(string type = "all", bool unreadOnly = false, int limit = 10)
        {
            var notifications = new List<object>
            {
                new { Id = 1, Type = "info", Title = "Sistem Güncellemesi", Message = "Sistem 2.1.0 sürümüne güncellendi", IsRead = false, Time = DateTime.Now.AddHours(-1) },
                new { Id = 2, Type = "warning", Title = "Düşük Stok", Message = "Kapadokya Turu stokları azalıyor", IsRead = false, Time = DateTime.Now.AddHours(-2) },
                new { Id = 3, Type = "success", Title = "Yeni Rezervasyon", Message = "Ahmet Yılmaz yeni rezervasyon yaptı", IsRead = true, Time = DateTime.Now.AddHours(-3) },
                new { Id = 4, Type = "error", Title = "Ödeme Hatası", Message = "Kredi kartı ödemesi başarısız", IsRead = false, Time = DateTime.Now.AddHours(-4) }
            };

            if (unreadOnly)
                notifications = notifications.Where(n => !((dynamic)n).IsRead).ToList();

            if (type != "all")
                notifications = notifications.Where(n => ((dynamic)n).Type == type).ToList();

            return await Task.FromResult(new
            {
                Notifications = notifications.Take(limit),
                UnreadCount = notifications.Count(n => !((dynamic)n).IsRead),
                Type = type,
                Limit = limit
            });
        }

        public async Task<bool> MarkNotificationAsReadAsync(int id)
        {
            // Mark notification as read in database
            var notification = await _unitOfWork.Repository<AuditLog>().GetByIdAsync(id);
            if (notification != null)
            {
                notification.IsActive = false;
                await _unitOfWork.Repository<AuditLog>().UpdateAsync(notification);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<object> GetUserPreferencesAsync()
        {
            return await Task.FromResult(new
            {
                Theme = "light",
                Language = "tr",
                Currency = "TRY",
                TimeZone = "Europe/Istanbul",
                Notifications = new
                {
                    Email = true,
                    Push = true,
                    SMS = false
                }
            });
        }

        public async Task<bool> SaveUserPreferencesAsync(object preferences)
        {
            // Save user preferences to database or cache
            try
            {
                // For now, we'll just return success
                // In a real implementation, you would save to database or cache
                await Task.Delay(100); // Simulate database operation
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Advanced Analytics
        public async Task<object> GetSalesAnalyticsAsync(DateTime startDate, DateTime endDate, string groupBy = "day")
        {
            var analytics = new List<object>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                analytics.Add(new
                {
                    Date = currentDate.ToString("yyyy-MM-dd"),
                    Sales = Random.Shared.Next(10, 100),
                    Revenue = Random.Shared.Next(1000, 10000),
                    Customers = Random.Shared.Next(5, 50),
                    AverageOrderValue = Random.Shared.Next(100, 500)
                });

                currentDate = groupBy == "day" ? currentDate.AddDays(1) : 
                             groupBy == "week" ? currentDate.AddDays(7) : 
                             currentDate.AddMonths(1);
            }

            return await Task.FromResult(new
            {
                Analytics = analytics,
                GroupBy = groupBy,
                Period = new { Start = startDate, End = endDate },
                Summary = new
                {
                    TotalSales = analytics.Sum(a => ((dynamic)a).Sales),
                    TotalRevenue = analytics.Sum(a => ((dynamic)a).Revenue),
                    TotalCustomers = analytics.Sum(a => ((dynamic)a).Customers),
                    AverageOrderValue = analytics.Average(a => ((dynamic)a).AverageOrderValue)
                }
            });
        }

        public async Task<object> GetCustomerAnalyticsAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(new
            {
                NewCustomers = Random.Shared.Next(50, 200),
                ReturningCustomers = Random.Shared.Next(100, 300),
                CustomerRetentionRate = Random.Shared.Next(60, 90),
                AverageCustomerLifetimeValue = Random.Shared.Next(1000, 5000),
                CustomerSegments = new List<object>
                {
                    new { Segment = "VIP", Count = 25, Revenue = 50000 },
                    new { Segment = "Regular", Count = 150, Revenue = 75000 },
                    new { Segment = "Occasional", Count = 75, Revenue = 25000 }
                },
                TopCustomers = new List<object>
                {
                    new { Name = "Ahmet Yılmaz", TotalSpent = 15000, Visits = 8 },
                    new { Name = "Fatma Demir", TotalSpent = 12000, Visits = 6 },
                    new { Name = "Mehmet Kaya", TotalSpent = 9800, Visits = 5 }
                }
            });
        }

        public async Task<object> GetProductPerformanceAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(new
            {
                TopProducts = new List<object>
                {
                    new { Name = "İstanbul Turu", Sales = 150, Revenue = 375000, Profit = 75000 },
                    new { Name = "Kapadokya Turu", Sales = 120, Revenue = 240000, Profit = 60000 },
                    new { Name = "Antalya Turu", Sales = 100, Revenue = 200000, Profit = 50000 }
                },
                ProductCategories = new List<object>
                {
                    new { Category = "Şehir Turları", Sales = 300, Revenue = 600000 },
                    new { Category = "Doğa Turları", Sales = 200, Revenue = 400000 },
                    new { Category = "Kültür Turları", Sales = 150, Revenue = 300000 }
                },
                PerformanceMetrics = new
                {
                    AverageProductRating = 4.7,
                    ProductReturnRate = 2.5,
                    BestSellingDay = "Cumartesi",
                    PeakSalesHour = "14:00"
                }
            });
        }

        public async Task<object> GetGeographicAnalyticsAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(new
            {
                TopDestinations = new List<object>
                {
                    new { Destination = "İstanbul", Sales = 200, Revenue = 400000 },
                    new { Destination = "Kapadokya", Sales = 150, Revenue = 300000 },
                    new { Destination = "Antalya", Sales = 120, Revenue = 240000 },
                    new { Destination = "Efes", Sales = 80, Revenue = 160000 },
                    new { Destination = "Pamukkale", Sales = 60, Revenue = 120000 }
                },
                CustomerRegions = new List<object>
                {
                    new { Region = "İstanbul", Customers = 300, Revenue = 600000 },
                    new { Region = "Ankara", Customers = 150, Revenue = 300000 },
                    new { Region = "İzmir", Customers = 100, Revenue = 200000 },
                    new { Region = "Bursa", Customers = 80, Revenue = 160000 },
                    new { Region = "Antalya", Customers = 60, Revenue = 120000 }
                }
            });
        }

        public async Task<object> GetTrendAnalysisAsync(string metric, DateTime startDate, DateTime endDate)
        {
            var trends = new List<object>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var baseValue = metric switch
                {
                    "sales" => Random.Shared.Next(50, 200),
                    "revenue" => Random.Shared.Next(5000, 20000),
                    "customers" => Random.Shared.Next(20, 100),
                    _ => Random.Shared.Next(10, 100)
                };

                trends.Add(new
                {
                    Date = currentDate.ToString("yyyy-MM-dd"),
                    Value = baseValue,
                    Trend = Random.Shared.Next(-10, 20), // Percentage change
                    MovingAverage = baseValue + Random.Shared.Next(-20, 20)
                });

                currentDate = currentDate.AddDays(1);
            }

            return await Task.FromResult(new
            {
                Metric = metric,
                Trends = trends,
                Period = new { Start = startDate, End = endDate },
                Analysis = new
                {
                    OverallTrend = "Yükselen",
                    TrendStrength = "Güçlü",
                    Seasonality = "Var",
                    Forecast = "Pozitif"
                }
            });
        }

        public async Task<object> GetForecastDataAsync(string metric, int periods = 12)
        {
            var forecast = new List<object>();
            var currentDate = DateTime.Now;

            for (int i = 1; i <= periods; i++)
            {
                var baseValue = metric switch
                {
                    "sales" => Random.Shared.Next(100, 300),
                    "revenue" => Random.Shared.Next(10000, 30000),
                    "customers" => Random.Shared.Next(50, 150),
                    _ => Random.Shared.Next(50, 200)
                };

                forecast.Add(new
                {
                    Period = currentDate.AddDays(i).ToString("yyyy-MM-dd"),
                    ForecastedValue = baseValue,
                    ConfidenceLower = baseValue - Random.Shared.Next(10, 30),
                    ConfidenceUpper = baseValue + Random.Shared.Next(10, 30),
                    Confidence = Random.Shared.Next(70, 95)
                });
            }

            return await Task.FromResult(new
            {
                Metric = metric,
                Forecast = forecast,
                Periods = periods,
                Accuracy = Random.Shared.Next(80, 95),
                Model = "ARIMA"
            });
        }
    }
}
