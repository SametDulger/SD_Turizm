# SD Turizm - Kurulum Rehberi

SD Turizm turizm yönetim sisteminin kurulum ve çalıştırma talimatları.

## 📋 Gereksinimler

### Gerekli Yazılımlar
- **.NET 9.0 SDK** - [İndir](https://dotnet.microsoft.com/download/dotnet/9.0)
- **SQL Server** - LocalDB, Express veya tam instance
- **Visual Studio 2022** veya **VS Code** (C# extension ile)
- **Entity Framework Tools** - Migration'lar için global tool

## 🚀 Kurulum Adımları

### 1. Repository'yi Klonlayın

```bash
git clone https://github.com/SametDulger/SD_Turizm.git
cd SD_Turizm
```

### 2. .NET Tools Kurulumu

```bash
# Entity Framework Tools'u global olarak yükleyin
dotnet tool install --global dotnet-ef

# Kurulumu doğrulayın
dotnet ef --version
```

### 3. NuGet Paketlerini Yükleyin

```bash
# Tüm paketleri yükleyin
dotnet restore
```

### 4. Yapılandırma Dosyalarını Hazırlayın

**SD_Turizm.API/appsettings.example.json** dosyasını kopyalayıp `appsettings.json` olarak yeniden adlandırın, sonra connection string'i güncelleyin.

**SD_Turizm.Web/appsettings.example.json** dosyasını kopyalayıp `appsettings.json` olarak yeniden adlandırın.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=SD_Turizm;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

#### Connection String Örnekleri

**LocalDB:**
```
Server=(localdb)\\mssqllocaldb;Database=SD_Turizm;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true
```

**SQL Server Express:**
```
Server=YOUR_PC\\SQLEXPRESS;Database=SD_Turizm;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true
```

### 5. Veritabanını Oluşturun

```bash
# API projesine gidin
cd SD_Turizm.API

# Migration oluşturun
dotnet ef migrations add InitialCreate --project ../SD_Turizm.Infrastructure --startup-project .

# Veritabanını güncelleyin
dotnet ef database update --project ../SD_Turizm.Infrastructure --startup-project .
```

### 6. Projeyi Derleyin

```bash
# Solution root'a dönün
cd ..

# Projeyi derleyin
dotnet build
```

### 7. Uygulamaları Çalıştırın

**Terminal 1 - API:**
```bash
cd SD_Turizm.API
dotnet run
```

**Terminal 2 - Web:**
```bash
cd SD_Turizm.Web
dotnet run
```

## 🌐 Erişim URL'leri

Başarılı başlatma sonrası:

- **API Swagger UI**: https://localhost:7001/swagger
- **API Base URL**: https://localhost:7001/api
- **Web Uygulaması**: https://localhost:5001
- **Web HTTP**: http://localhost:5000

## 🔐 Varsayılan Giriş Bilgileri

- **Kullanıcı Adı**: `admin`
- **Şifre**: `Admin123!`

## 🗄️ Veritabanı Yönetimi

### Yeni Migration Oluşturma

```bash
cd SD_Turizm.API
dotnet ef migrations add MigrationAdi --project ../SD_Turizm.Infrastructure --startup-project .
```

### Veritabanını Güncelleme

```bash
dotnet ef database update --project ../SD_Turizm.Infrastructure --startup-project .
```

## 🐛 Sorun Giderme

### Yaygın Sorunlar

#### 1. Derleme Hataları
```bash
dotnet clean
dotnet restore
dotnet build
```

#### 2. Veritabanı Bağlantı Sorunları
- SQL Server'ın çalıştığını doğrulayın
- Connection string formatını kontrol edin
- Veritabanı izinlerini kontrol edin

#### 3. Port Çakışmaları
5000, 5001, 7000 veya 7001 portları kullanımdaysa:
- Her projedeki `Properties/launchSettings.json` dosyasını düzenleyin
- Port numaralarını değiştirin

#### 4. Entity Framework Sorunları
```bash
dotnet ef database drop --project ../SD_Turizm.Infrastructure --startup-project .
dotnet ef migrations remove --project ../SD_Turizm.Infrastructure --startup-project .
dotnet ef migrations add InitialCreate --project ../SD_Turizm.Infrastructure --startup-project .
dotnet ef database update --project ../SD_Turizm.Infrastructure --startup-project .
```

