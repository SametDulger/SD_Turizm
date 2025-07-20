# SD Turizm - Turizm Yönetim Sistemi

Modern ve kapsamlı bir turizm yönetim sistemi. 

> ⚠️ **ÖNEMLİ UYARI**: Bu proje şu anda **test aşamasında** bulunmaktadır. Geliştirme süreci devam etmekte olup, hata veya eksiklikler bulunabilir. Production ortamında kullanmadan önce kapsamlı test yapılması önerilir.

## 🏗️ Proje Yapısı

Bu proje **Clean Architecture** pattern'i kullanılarak 5 katmanlı bir yapıda tasarlanmıştır:

```
SD_Turizm/
├── SD_Turizm.Core/           # Domain Layer - Entities, Interfaces
├── SD_Turizm.Infrastructure/ # Data Layer - DbContext, Repositories
├── SD_Turizm.Application/    # Business Layer - Services
├── SD_Turizm.API/           # API Layer - REST API Controllers
└── SD_Turizm.Web/           # Presentation Layer - MVC Web App
```

### Katmanlar

#### 🎯 **SD_Turizm.Core** 
- **Entities**: Domain modelleri ve iş kuralları
- **Interfaces**: Repository ve Service kontratları
- **BaseEntity**: Tüm entity'ler için temel sınıf
- **Prices**: Fiyat modelleri 

#### 🗄️ **SD_Turizm.Infrastructure** 
- **ApplicationDbContext**: Entity Framework DbContext
- **Repositories**: Generic Repository ve Unit of Work pattern
- **Data**: Veritabanı konfigürasyonu ve seed data

#### ⚙️ **SD_Turizm.Application** 
- **Services**: İş mantığı ve domain servisleri
- **Interfaces**: Service kontratları

#### 🌐 **SD_Turizm.API** 
- **Controllers**: REST API endpoint'leri
- **Swagger**: API dokümantasyonu
- **CORS**: Cross-Origin Resource Sharing

#### 🖥️ **SD_Turizm.Web** 
- **Controllers**: MVC Controllers
- **Views**: Razor Views
- **Models**: DTOs ve View Models

## 🏢 İş Modülleri

### 1. **Vendor Yönetimi**
- **Hotels**: Otel yönetimi
- **TourOperators**: Tur operatörü yönetimi
- **TransferCompanies**: Transfer şirketi yönetimi
- **RentACars**: Araç kiralama şirketi yönetimi
- **Guides**: Rehber yönetimi
- **Airlines**: Havayolu şirketi yönetimi
- **Cruises**: Gemi turu yönetimi

### 2. **Fiyat Yönetimi**
- **HotelPrice**: Otel fiyatları
- **TourPrice**: Tur fiyatları
- **TransferPrice**: Transfer fiyatları
- **RentACarPrice**: Araç kiralama fiyatları
- **GuidePrice**: Rehber fiyatları
- **AirlinePrice**: Havayolu fiyatları
- **CruisePrice**: Gemi turu fiyatları

### 3. **Paket Yönetimi**
- **Package**: Tur paketleri
- **PackageItem**: Paket içerikleri
- **Tour**: Turlar

### 4. **Satış Yönetimi**
- **Sale**: Satış kayıtları
- **SaleItem**: Satış kalemleri
- **SalePerson**: Satış personeli

### 5. **Finansal Yönetimi**
- **CariTransaction**: Cari hesap işlemleri
- **ExchangeRate**: Döviz kurları

### 6. **Raporlama**
- **Dashboard**: Genel dashboard
- **Reports**: Detaylı raporlar

## 🛠️ Teknolojiler

### Backend
- **.NET 9**: En güncel .NET framework
- **Entity Framework Core 9.0.7**: ORM
- **SQL Server**: Veritabanı
- **Swagger/OpenAPI**: API dokümantasyonu
- **CORS**: Cross-origin istek desteği

### Frontend
- **ASP.NET Core MVC**: Web framework
- **Bootstrap 5**: CSS framework
- **jQuery**: JavaScript kütüphanesi
- **Razor Views**: Template engine
- **HTTP Client**: API iletişimi

### Mimari
- **Clean Architecture**: Katmanlı mimari
- **Repository Pattern**: Veri erişim pattern'i
- **Unit of Work**: Transaction yönetimi
- **SOLID Principles**: Yazılım tasarım prensipleri
- **Dependency Injection**: Bağımlılık enjeksiyonu

## 🚀 Kurulum

> 📝 **Not**: Bu proje test aşamasında olduğu için kurulum sırasında beklenmeyen sorunlarla karşılaşabilirsiniz. Herhangi bir hata durumunda issue açabilirsiniz.

### Gereksinimler
- .NET 9 SDK
- SQL Server 
- Visual Studio 2022 veya VS Code
- Entity Framework Tools 

### Adımlar

1. **Repository'yi klonlayın**
```bash
git clone https://github.com/SametDulger/SD_Turizm.git
cd SD_Turizm
```

2. **Veritabanı bağlantısını yapılandırın**
```json
// SD_Turizm.API/appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=MSI\\SQLEXPRESS;Database=SD_Turizm;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

3. **Veritabanını oluşturun**
```bash
# Entity Framework Tools yüklü değilse önce yükleyin
dotnet tool install --global dotnet-ef

# Migration oluşturun
cd SD_Turizm.API
dotnet ef migrations add InitialCreate

# Veritabanını güncelleyin
dotnet ef database update
```

4. **Projeyi çalıştırın**
```bash
# API projesini çalıştırın (https://localhost:7001, http://localhost:7000)
cd SD_Turizm.API
dotnet run

# Web projesini çalıştırın (yeni terminal, https://localhost:5001, http://localhost:5000)
cd SD_Turizm.Web
dotnet run
```


## 🔧 API Endpoints

### 📊 Dashboard Endpoints
- `GET /api/dashboard/statistics` - Dashboard istatistiklerini getir

### 📈 Reports Endpoints
- `GET /api/reports/sales` - Satış raporu (tarih aralığı, satıcı tipi, para birimi, PNR, dosya kodu, acente kodu, cari kod filtreleri ile)
- `GET /api/reports/sales/summary` - Satış özet raporu
- `GET /api/reports/financial` - Finansal rapor (tarih aralığı ve para birimi ile)
- `GET /api/reports/financial/summary` - Finansal özet raporu
- `GET /api/reports/customers` - Müşteri raporu (tarih aralığı ve cari kod ile)
- `GET /api/reports/customers/summary` - Müşteri özet raporu
- `GET /api/reports/products` - Ürün raporu (tarih aralığı ve ürün tipi ile)
- `GET /api/reports/products/summary` - Ürün özet raporu

### 🏨 Hotels Endpoints
- `GET /api/hotels` - Tüm otelleri getir
- `GET /api/hotels/{id}` - ID'ye göre otel getir
- `GET /api/hotels/code/{code}` - Koda göre otel getir
- `POST /api/hotels` - Yeni otel oluştur
- `PUT /api/hotels/{id}` - Otel güncelle
- `DELETE /api/hotels/{id}` - Otel sil

### 🎫 Sales Endpoints
- `GET /api/sales` - Tüm satışları getir
- `GET /api/sales/{id}` - ID'ye göre satış getir
- `GET /api/sales/pnr/{pnrNumber}` - PNR numarasına göre satış getir
- `GET /api/sales/date-range` - Tarih aralığına göre satışları getir
- `GET /api/sales/agency/{agencyCode}` - Acente koduna göre satışları getir
- `GET /api/sales/cari/{cariCode}` - Cari koduna göre satışları getir
- `POST /api/sales` - Yeni satış oluştur
- `PUT /api/sales/{id}` - Satış güncelle
- `DELETE /api/sales/{id}` - Satış sil

### 🏢 Tour Operators Endpoints
- `GET /api/touroperators` - Tüm tur operatörlerini getir
- `GET /api/touroperators/{id}` - ID'ye göre tur operatörü getir
- `GET /api/touroperators/code/{code}` - Koda göre tur operatörü getir
- `POST /api/touroperators` - Yeni tur operatörü oluştur
- `PUT /api/touroperators/{id}` - Tur operatörü güncelle
- `DELETE /api/touroperators/{id}` - Tur operatörü sil

### 🚌 Transfer Companies Endpoints
- `GET /api/transfercompany` - Tüm transfer şirketlerini getir
- `GET /api/transfercompany/{id}` - ID'ye göre transfer şirketi getir
- `POST /api/transfercompany` - Yeni transfer şirketi oluştur
- `PUT /api/transfercompany/{id}` - Transfer şirketi güncelle
- `DELETE /api/transfercompany/{id}` - Transfer şirketi sil

### 🚗 Rent A Car Endpoints
- `GET /api/rentacar` - Tüm araç kiralama şirketlerini getir
- `GET /api/rentacar/{id}` - ID'ye göre araç kiralama şirketi getir
- `POST /api/rentacar` - Yeni araç kiralama şirketi oluştur
- `PUT /api/rentacar/{id}` - Araç kiralama şirketi güncelle
- `DELETE /api/rentacar/{id}` - Araç kiralama şirketi sil

### 👥 Guide Endpoints
- `GET /api/guide` - Tüm rehberleri getir
- `GET /api/guide/{id}` - ID'ye göre rehber getir
- `POST /api/guide` - Yeni rehber oluştur
- `PUT /api/guide/{id}` - Rehber güncelle
- `DELETE /api/guide/{id}` - Rehber sil

### ✈️ Airline Endpoints
- `GET /api/airline` - Tüm havayolu şirketlerini getir
- `GET /api/airline/{id}` - ID'ye göre havayolu şirketi getir
- `POST /api/airline` - Yeni havayolu şirketi oluştur
- `PUT /api/airline/{id}` - Havayolu şirketi güncelle
- `DELETE /api/airline/{id}` - Havayolu şirketi sil

### 🚢 Cruise Endpoints
- `GET /api/cruise` - Tüm kruvaziyer şirketlerini getir
- `GET /api/cruise/{id}` - ID'ye göre kruvaziyer şirketi getir
- `POST /api/cruise` - Yeni kruvaziyer şirketi oluştur
- `PUT /api/cruise/{id}` - Kruvaziyer şirketi güncelle
- `DELETE /api/cruise/{id}` - Kruvaziyer şirketi sil

### 📦 Package Endpoints
- `GET /api/package` - Tüm paketleri getir
- `GET /api/package/{id}` - ID'ye göre paket getir
- `POST /api/package` - Yeni paket oluştur
- `PUT /api/package/{id}` - Paket güncelle
- `DELETE /api/package/{id}` - Paket sil

### 🎯 Tour Endpoints
- `GET /api/tour` - Tüm turları getir
- `GET /api/tour/{id}` - ID'ye göre tur getir
- `POST /api/tour` - Yeni tur oluştur
- `PUT /api/tour/{id}` - Tur güncelle
- `DELETE /api/tour/{id}` - Tur sil

### 📍 Address Endpoints
- `GET /api/address` - Tüm adresleri getir
- `GET /api/address/{id}` - ID'ye göre adres getir
- `POST /api/address` - Yeni adres oluştur
- `PUT /api/address/{id}` - Adres güncelle
- `DELETE /api/address/{id}` - Adres sil

### 💰 Cari Transaction Endpoints
- `GET /api/caritransaction` - Tüm cari işlemleri getir
- `GET /api/caritransaction/{id}` - ID'ye göre cari işlem getir
- `POST /api/caritransaction` - Yeni cari işlem oluştur
- `PUT /api/caritransaction/{id}` - Cari işlem güncelle
- `DELETE /api/caritransaction/{id}` - Cari işlem sil

### 💱 Exchange Rate Endpoints
- `GET /api/exchangerate` - Tüm döviz kurlarını getir
- `GET /api/exchangerate/{id}` - ID'ye göre döviz kuru getir
- `POST /api/exchangerate` - Yeni döviz kuru oluştur
- `PUT /api/exchangerate/{id}` - Döviz kuru güncelle
- `DELETE /api/exchangerate/{id}` - Döviz kuru sil

### 👤 Sale Person Endpoints
- `GET /api/saleperson` - Tüm satış personelini getir
- `GET /api/saleperson/{id}` - ID'ye göre satış personeli getir
- `POST /api/saleperson` - Yeni satış personeli oluştur
- `PUT /api/saleperson/{id}` - Satış personeli güncelle
- `DELETE /api/saleperson/{id}` - Satış personeli sil

### 🛍️ Sale Item Endpoints
- `GET /api/saleitem` - Tüm satış kalemlerini getir
- `GET /api/saleitem/{id}` - ID'ye göre satış kalemi getir
- `POST /api/saleitem` - Yeni satış kalemi oluştur
- `PUT /api/saleitem/{id}` - Satış kalemi güncelle
- `DELETE /api/saleitem/{id}` - Satış kalemi sil

### 📋 Package Item Endpoints
- `GET /api/packageitem` - Tüm paket kalemlerini getir
- `GET /api/packageitem/{id}` - ID'ye göre paket kalemi getir
- `POST /api/packageitem` - Yeni paket kalemi oluştur
- `PUT /api/packageitem/{id}` - Paket kalemi güncelle
- `DELETE /api/packageitem/{id}` - Paket kalemi sil

### 💵 Price Endpoints
**Hotel Prices:**
- `GET /api/hotelprice` - Tüm otel fiyatlarını getir
- `GET /api/hotelprice/{id}` - ID'ye göre otel fiyatı getir
- `POST /api/hotelprice` - Yeni otel fiyatı oluştur
- `PUT /api/hotelprice/{id}` - Otel fiyatı güncelle
- `DELETE /api/hotelprice/{id}` - Otel fiyatı sil

**Tour Prices:**
- `GET /api/tourprice` - Tüm tur fiyatlarını getir
- `GET /api/tourprice/{id}` - ID'ye göre tur fiyatı getir
- `POST /api/tourprice` - Yeni tur fiyatı oluştur
- `PUT /api/tourprice/{id}` - Tur fiyatı güncelle
- `DELETE /api/tourprice/{id}` - Tur fiyatı sil

**Airline Prices:**
- `GET /api/airlineprice` - Tüm havayolu fiyatlarını getir
- `GET /api/airlineprice/{id}` - ID'ye göre havayolu fiyatı getir
- `POST /api/airlineprice` - Yeni havayolu fiyatı oluştur
- `PUT /api/airlineprice/{id}` - Havayolu fiyatı güncelle
- `DELETE /api/airlineprice/{id}` - Havayolu fiyatı sil

**Cruise Prices:**
- `GET /api/cruiseprice` - Tüm kruvaziyer fiyatlarını getir
- `GET /api/cruiseprice/{id}` - ID'ye göre kruvaziyer fiyatı getir
- `POST /api/cruiseprice` - Yeni kruvaziyer fiyatı oluştur
- `PUT /api/cruiseprice/{id}` - Kruvaziyer fiyatı güncelle
- `DELETE /api/cruiseprice/{id}` - Kruvaziyer fiyatı sil

**Guide Prices:**
- `GET /api/guideprice` - Tüm rehber fiyatlarını getir
- `GET /api/guideprice/{id}` - ID'ye göre rehber fiyatı getir
- `POST /api/guideprice` - Yeni rehber fiyatı oluştur
- `PUT /api/guideprice/{id}` - Rehber fiyatı güncelle
- `DELETE /api/guideprice/{id}` - Rehber fiyatı sil

**Rent A Car Prices:**
- `GET /api/rentacarprice` - Tüm araç kiralama fiyatlarını getir
- `GET /api/rentacarprice/{id}` - ID'ye göre araç kiralama fiyatı getir
- `POST /api/rentacarprice` - Yeni araç kiralama fiyatı oluştur
- `PUT /api/rentacarprice/{id}` - Araç kiralama fiyatı güncelle
- `DELETE /api/rentacarprice/{id}` - Araç kiralama fiyatı sil

**Transfer Prices:**
- `GET /api/transferprice` - Tüm transfer fiyatlarını getir
- `GET /api/transferprice/{id}` - ID'ye göre transfer fiyatı getir
- `POST /api/transferprice` - Yeni transfer fiyatı oluştur
- `PUT /api/transferprice/{id}` - Transfer fiyatı güncelle
- `DELETE /api/transferprice/{id}` - Transfer fiyatı sil

## 🎨 Web Arayüzü

### Özellikler
- **Responsive Design**: Mobil uyumlu tasarım
- **Bootstrap 5**: Modern UI framework
- **CRUD Operations**: Tüm modüller için CRUD işlemleri
- **Search & Filter**: Arama ve filtreleme
- **Reports**: Detaylı raporlama
- **API Integration**: HTTP Client ile API iletişimi

### Sayfalar
- **Dashboard**: Genel istatistikler
- **Vendor Management**: Vendor yönetimi
- **Price Management**: Fiyat yönetimi
- **Sales Management**: Satış yönetimi
- **Reports**: Raporlama
- **Package Management**: Paket yönetimi
- **Financial Management**: Finansal yönetim

## 📈 Özellikler

### ✅ Tamamlanan Özellikler
- [x] Clean Architecture implementasyonu
- [x] Entity Framework Core entegrasyonu
- [x] Repository ve Unit of Work pattern'leri
- [x] REST API endpoints
- [x] MVC Web arayüzü
- [x] CRUD operasyonları (tüm modüller için)
- [x] Veritabanı ilişkileri ve seed data
- [x] Swagger dokümantasyonu
- [x] CORS yapılandırması
- [x] Dashboard ve raporlama sistemi
- [x] Bootstrap 5 responsive tasarım
- [x] DTO modelleri
- [x] Gelişmiş satış arama ve filtreleme
- [x] Unique index'ler ve composite constraints
- [x] Cascade delete ilişkileri
- [x] Dependency Injection ile service registration

### 🔄 Geliştirilebilecek Özellikler
- [ ] Authentication & Authorization
- [ ] Logging sistemi
- [ ] Caching mekanizması
- [ ] File upload/download
- [ ] Email notifications
- [ ] Real-time updates
- [ ] Mobile app
- [ ] Advanced reporting
- [ ] Unit tests
- [ ] Integration tests
- [ ] Error handling ve validation
- [ ] Performance optimizasyonu
- [ ] Security hardening
- [ ] Database migration scripts
- [ ] Deployment documentation

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.


## 🐛 Bilinen Sorunlar ve Sınırlamalar

Bu proje test aşamasında olduğu için aşağıdaki durumlar söz konusu olabilir:

- **Hata Durumları**: Beklenmeyen hatalar oluşabilir
- **Eksik Özellikler**: Bazı özellikler henüz tamamlanmamış olabilir
- **Performans**: Optimizasyon çalışmaları devam etmektedir
- **Güvenlik**: Security hardening henüz tamamlanmamıştır
- **Test Coverage**: Unit ve integration testler eksiktir

Herhangi bir sorunla karşılaştığınızda lütfen GitHub issue'sı açarak bildirin.

**SD Turizm** - Modern turizm yönetim sistemi 🏖️✈️🏨 