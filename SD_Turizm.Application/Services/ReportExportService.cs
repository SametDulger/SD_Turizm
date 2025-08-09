using System.Text;
using System.Text.Json;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class ReportExportService : IReportExportService
    {
        public async Task<byte[]> ExportToExcelAsync(object data, string sheetName = "Rapor")
        {
            // Export to Excel using CSV format as fallback
            // In production, implement with EPPlus or similar library
            var csvData = await ExportToCsvAsync(data, sheetName);
            return csvData;
        }

        public async Task<byte[]> ExportToPdfAsync(object data, string title = "Rapor")
        {
            // Export to PDF using HTML format as fallback
            // In production, implement with iTextSharp or similar library
            var jsonData = await ExportToJsonAsync(data);
            var pdfContent = $@"
                <html>
                <head>
                    <title>{title}</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 20px; }}
                        h1 {{ color: #333; }}
                        table {{ border-collapse: collapse; width: 100%; }}
                        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                        th {{ background-color: #f2f2f2; }}
                    </style>
                </head>
                <body>
                    <h1>{title}</h1>
                    <p>Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                    <pre>{jsonData}</pre>
                </body>
                </html>";

            return Encoding.UTF8.GetBytes(pdfContent);
        }

        public async Task<byte[]> ExportToCsvAsync(object data, string fileName = "rapor")
        {
            var csvBuilder = new StringBuilder();
            
            // Add header
            csvBuilder.AppendLine("Rapor Adı,Değer,Tarih");
            
            // Add data
            if (data is IEnumerable<dynamic> enumerable)
            {
                foreach (var item in enumerable)
                {
                    csvBuilder.AppendLine($"{item.Name},{item.Value},{DateTime.Now:dd.MM.yyyy}");
                }
            }
            else
            {
                csvBuilder.AppendLine($"Genel Rapor,{data},{DateTime.Now:dd.MM.yyyy}");
            }

            return await Task.FromResult(Encoding.UTF8.GetBytes(csvBuilder.ToString()));
        }

        public async Task<string> ExportToJsonAsync(object data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return await Task.FromResult(JsonSerializer.Serialize(data, options));
        }

        public async Task<byte[]> ExportSalesReportAsync(DateTime startDate, DateTime endDate, string format = "excel")
        {
            var salesData = new List<object>
            {
                new { Date = startDate.ToString("dd.MM.yyyy"), Sales = 150, Revenue = 25000, Customers = 25 },
                new { Date = startDate.AddDays(1).ToString("dd.MM.yyyy"), Sales = 180, Revenue = 30000, Customers = 30 },
                new { Date = startDate.AddDays(2).ToString("dd.MM.yyyy"), Sales = 200, Revenue = 35000, Customers = 35 }
            };

            return format.ToLower() switch
            {
                "excel" => await ExportToExcelAsync(salesData, "Satış Raporu"),
                "pdf" => await ExportToPdfAsync(salesData, "Satış Raporu"),
                "csv" => await ExportToCsvAsync(salesData, "satis_raporu"),
                _ => await ExportToExcelAsync(salesData, "Satış Raporu")
            };
        }

        public async Task<byte[]> ExportCustomerReportAsync(DateTime startDate, DateTime endDate, string format = "excel")
        {
            var customerData = new List<object>
            {
                new { Customer = "Ahmet Yılmaz", TotalSpent = 15000, Visits = 8, LastVisit = DateTime.Now.AddDays(-5) },
                new { Customer = "Fatma Demir", TotalSpent = 12000, Visits = 6, LastVisit = DateTime.Now.AddDays(-10) },
                new { Customer = "Mehmet Kaya", TotalSpent = 9800, Visits = 5, LastVisit = DateTime.Now.AddDays(-15) }
            };

            return format.ToLower() switch
            {
                "excel" => await ExportToExcelAsync(customerData, "Müşteri Raporu"),
                "pdf" => await ExportToPdfAsync(customerData, "Müşteri Raporu"),
                "csv" => await ExportToCsvAsync(customerData, "musteri_raporu"),
                _ => await ExportToExcelAsync(customerData, "Müşteri Raporu")
            };
        }

        public async Task<byte[]> ExportFinancialReportAsync(DateTime startDate, DateTime endDate, string format = "excel")
        {
            var financialData = new List<object>
            {
                new { Category = "Gelir", Amount = 150000, Percentage = 100 },
                new { Category = "Gider", Amount = 80000, Percentage = 53.33 },
                new { Category = "Kar", Amount = 70000, Percentage = 46.67 }
            };

            return format.ToLower() switch
            {
                "excel" => await ExportToExcelAsync(financialData, "Finansal Rapor"),
                "pdf" => await ExportToPdfAsync(financialData, "Finansal Rapor"),
                "csv" => await ExportToCsvAsync(financialData, "finansal_rapor"),
                _ => await ExportToExcelAsync(financialData, "Finansal Rapor")
            };
        }

        public async Task<byte[]> ExportProductReportAsync(DateTime startDate, DateTime endDate, string format = "excel")
        {
            var productData = new List<object>
            {
                new { Product = "İstanbul Turu", Sales = 150, Revenue = 375000, Profit = 75000 },
                new { Product = "Kapadokya Turu", Sales = 120, Revenue = 240000, Profit = 60000 },
                new { Product = "Antalya Turu", Sales = 100, Revenue = 200000, Profit = 50000 }
            };

            return format.ToLower() switch
            {
                "excel" => await ExportToExcelAsync(productData, "Ürün Raporu"),
                "pdf" => await ExportToPdfAsync(productData, "Ürün Raporu"),
                "csv" => await ExportToCsvAsync(productData, "urun_raporu"),
                _ => await ExportToExcelAsync(productData, "Ürün Raporu")
            };
        }

        public async Task<byte[]> ExportAuditReportAsync(DateTime startDate, DateTime endDate, string format = "excel")
        {
            var auditData = new List<object>
            {
                new { User = "admin", Action = "CREATE", Table = "Users", RecordId = 123, Timestamp = DateTime.Now.AddHours(-1) },
                new { User = "manager", Action = "UPDATE", Table = "Sales", RecordId = 456, Timestamp = DateTime.Now.AddHours(-2) },
                new { User = "user1", Action = "DELETE", Table = "Products", RecordId = 789, Timestamp = DateTime.Now.AddHours(-3) }
            };

            return format.ToLower() switch
            {
                "excel" => await ExportToExcelAsync(auditData, "Denetim Raporu"),
                "pdf" => await ExportToPdfAsync(auditData, "Denetim Raporu"),
                "csv" => await ExportToCsvAsync(auditData, "denetim_raporu"),
                _ => await ExportToExcelAsync(auditData, "Denetim Raporu")
            };
        }

        public async Task<byte[]> ExportCustomReportAsync(object data, string reportType, string format = "excel")
        {
            return format.ToLower() switch
            {
                "excel" => await ExportToExcelAsync(data, $"{reportType} Raporu"),
                "pdf" => await ExportToPdfAsync(data, $"{reportType} Raporu"),
                "csv" => await ExportToCsvAsync(data, $"{reportType.ToLower()}_raporu"),
                _ => await ExportToExcelAsync(data, $"{reportType} Raporu")
            };
        }
    }
}
