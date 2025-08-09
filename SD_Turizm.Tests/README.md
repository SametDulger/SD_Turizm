# SD_Turizm Test Suite

Bu proje kapsamlı bir test suite'i içermektedir. Testler farklı kategorilerde organize edilmiştir.

## Test Kategorileri

### 1. Unit Tests (`/Unit`)

#### Services
- **AuthServiceTests.cs**: Authentication servisinin tüm fonksiyonlarını test eder
  - Login/Register işlemleri
  - Token yenileme ve doğrulama
  - Şifre değiştirme
  - 2FA (Two-Factor Authentication)
  - Kullanıcı rolleri ve oturumları

- **UserServiceTests.cs**: Kullanıcı yönetimi servisini test eder
  - CRUD işlemleri
  - Kullanıcı aktivasyonu/deaktivasyonu
  - Rol atama/kaldırma
  - Şifre sıfırlama
  - Sayfalama (pagination)

- **VendorServiceTests.cs**: Tedarikçi yönetimi servisini test eder
  - Tedarikçi CRUD işlemleri
  - Filtreleme ve arama
  - Durum değiştirme (aktif/pasif)
  - İstatistikler

- **HotelServiceTests.cs**: Otel yönetimi servisini test eder
  - Temel CRUD işlemleri
  - Varlık kontrolleri

- **SaleServiceTests.cs**: Satış yönetimi servisini test eder
  - Satış CRUD işlemleri
  - PNR ile arama
  - Varlık kontrolleri

- **ReportServiceTests.cs**: Raporlama servisini test eder
  - Satış raporları
  - Finansal raporlar
  - Müşteri raporları
  - Döviz kuru dönüşümleri

#### Validators
- **UserValidatorTests.cs**: Kullanıcı validation kurallarını test eder
- **HotelValidatorTests.cs**: Otel validation kurallarını test eder
- **VendorValidatorTests.cs**: Tedarikçi validation kurallarını test eder
- **SaleValidatorTests.cs**: Satış validation kurallarını test eder

#### Repositories
- **GenericRepositoryTests.cs**: Generic repository implementasyonunu test eder
  - CRUD işlemleri
  - Filtreleme
  - Soft delete
  - Queryable operations

- **UnitOfWorkTests.cs**: Unit of Work pattern implementasyonunu test eder
  - Repository factory
  - Transaction yönetimi
  - Değişiklikleri kaydetme

#### Middleware
- **GlobalExceptionHandlerTests.cs**: Global exception handling middleware'ini test eder
  - Farklı exception türleri
  - HTTP status code mapping
  - Error response formatting
  - Logging

- **ValidationMiddlewareTests.cs**: Validation middleware'ini test eder
  - FluentValidation integration
  - Error response formatting
  - Multiple validation errors

### 2. Integration Tests (`/Integration`)

#### Controllers
- **AuthControllerTests.cs**: Authentication controller'ını end-to-end test eder
  - Login/Register endpoints
  - Validation scenarios
  - Error handling

- **HotelsControllerTests.cs**: Hotels controller'ını test eder
  - CRUD endpoints
  - HTTP status codes

- **VendorControllerTests.cs**: Vendor controller'ını kapsamlı test eder
  - Tüm CRUD işlemleri
  - Filtreleme endpoints
  - Sayfalama
  - Validation scenarios

### 3. Performance Tests (`/Performance`)

- **LoadTests.cs**: Sistem performansını test eder
  - Büyük veri setleri ile test
  - Concurrent request handling
  - Bulk operations
  - Search performance
  - Pagination performance
  - Mixed workload scenarios
  - Memory usage stability

## Test Teknolojileri

- **xUnit**: Test framework
- **Moq**: Mocking framework
- **FluentAssertions**: Assertion library
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing
- **EntityFramework InMemory**: In-memory database for testing

## Test Verisi

Testler tutarlı sonuçlar için aşağıdaki stratejileri kullanır:

- **Fixed Seeds**: Rastgele data için sabit seed değerleri
- **In-Memory Database**: Her test için izole database
- **Test Data Builders**: Consistent test data creation
- **Setup/Teardown**: Proper test isolation

## Test Çalıştırma

```bash
# Tüm testleri çalıştır
dotnet test

# Sadece unit testleri çalıştır
dotnet test --filter "FullyQualifiedName~Unit"

# Sadece integration testleri çalıştır
dotnet test --filter "FullyQualifiedName~Integration"

# Sadece performance testleri çalıştır
dotnet test --filter "FullyQualifiedName~Performance"

# Verbose output ile çalıştır
dotnet test --verbosity normal

# Coverage raporu oluştur
dotnet test --collect:"XPlat Code Coverage"
```

## Test Coverage

Bu test suite'i aşağıdaki alanları kapsar:

- ✅ Services (Business Logic)
- ✅ Repositories (Data Access)
- ✅ Validators (Input Validation)
- ✅ Controllers (API Endpoints)
- ✅ Middleware (Request Pipeline)
- ✅ Exception Handling
- ✅ Authentication & Authorization
- ✅ Performance & Load Testing

## Test Prensipleri

1. **AAA Pattern**: Arrange, Act, Assert
2. **Single Responsibility**: Her test tek bir şeyi test eder
3. **Test Isolation**: Testler birbirinden bağımsız
4. **Meaningful Names**: Test isimleri açıklayıcı
5. **Fast Execution**: Testler hızlı çalışır
6. **Deterministic**: Testler her zaman aynı sonucu verir

## Continuous Integration

Bu testler CI/CD pipeline'ında otomatik olarak çalıştırılmalıdır:

- Pull request'lerde
- Master branch'e merge'den önce
- Deployment'dan önce
- Nightly builds'de

## Best Practices

1. Test yazarken edge case'leri düşünün
2. Happy path ve sad path'leri test edin
3. Performance testlerini düzenli çalıştırın
4. Test verilerini gerçekçi tutun
5. Test maintenance'ını ihmal etmeyin
