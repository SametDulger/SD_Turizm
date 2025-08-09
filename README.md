# SD Turizm - Turizm Yönetim Sistemi

.NET 9.0 ile geliştirilmiş kapsamlı turizm yönetim sistemi.

## 🏗️ Mimari

- **SD_Turizm.Core**: Domain entity'leri, interface'ler ve exception'lar
- **SD_Turizm.Infrastructure**: Entity Framework Core ile veri erişim katmanı
- **SD_Turizm.Application**: İş mantığı, servisler ve validasyonlar
- **SD_Turizm.API**: JWT authentication ile REST API
- **SD_Turizm.Web**: ASP.NET Core MVC web uygulaması
- **SD_Turizm.Tests**: Unit ve integration testler

## 🛠️ Teknoloji Stack'i

- **.NET 9.0**
- **Entity Framework Core**
- **SQL Server**
- **Redis Cache**
- **ASP.NET Core Web API & MVC**
- **JWT Authentication**
- **Health Checks**
- **FluentValidation**
- **xUnit & Moq**

## 🚀 Özellikler

### 🏢 İş Özellikleri
- ✅ **Otel Yönetimi** - Otel kaydı, oda tipleri, fiyatlandırma
- ✅ **Tur Operasyonları** - Tur planları, operatör yönetimi
- ✅ **Satış Yönetimi** - PNR, paket satışları, müşteri takibi
- ✅ **Mali İşlemler** - Cari hesaplar, döviz kurları
- ✅ **Raporlama** - Finansal ve operasyonel raporlar
- ✅ **Kullanıcı Yönetimi** - Rol tabanlı yetkilendirme

### 🛠️ Teknik Özellikler
- ✅ **Clean Architecture** - Katmanlı mimari
- ✅ **JWT Authentication** - Güvenli kimlik doğrulama
- ✅ **API Versioning** - API versiyonlama (v1, v2)
- ✅ **Validation** - FluentValidation ile veri doğrulama
- ✅ **Caching** - Redis ile önbellekleme
- ✅ **Rate Limiting** - API rate limiting
- ✅ **Health Checks** - Sistem sağlık kontrolü
- ✅ **Unit & Integration Tests** - Kapsamlı test coverage
- ✅ **Security Headers** - Güvenlik başlıkları
- ✅ **Error Handling** - Global exception handling
- ✅ **Logging** - Structured logging
- ✅ **Response Compression** - Gzip compression
- ✅ **Database Migrations** - EF Core migrations
- ✅ **Seed Data** - Demo veriler ile hızlı başlangıç

## 🗄️ Veritabanı

Sistem **39 tablo** ile kapsamlı bir veritabanı yapısına sahiptir:
- **Vendor Management** - Hotels, Tours, Airlines, Cruises, RentACar, Guides
- **Sales & Packages** - Sales, SaleItems, Packages, PackageItems  
- **Pricing** - Dinamik fiyatlandırma sistemi
- **User Management** - Role-based authentication
- **Financial** - Cari hesaplar, döviz kurları
- **Audit** - Sistem audit kayıtları

### Varsayılan Giriş
- **Kullanıcı**: `admin`
- **Şifre**: `Admin123!`

## 📚 Kurulum

Kurulum talimatları için [SETUP.md](SETUP.md) dosyasına bakın.

