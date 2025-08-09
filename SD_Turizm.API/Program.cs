using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Infrastructure.Data;
using SD_Turizm.Infrastructure.Repositories;
using SD_Turizm.API.Middleware;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Core.DTOs;
using SD_Turizm.Application.Validators;
using System.Threading.RateLimiting;
using StackExchange.Redis;
using SD_Turizm.API.HealthChecks;
using SD_Turizm.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Services
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<ITourOperatorService, TourOperatorService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ITransferCompanyService, TransferCompanyService>();
builder.Services.AddScoped<IRentACarService, RentACarService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IAirlineService, AirlineService>();
builder.Services.AddScoped<ICruiseService, CruiseService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICariTransactionService, CariTransactionService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Add User Management Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

// Add Audit Service
builder.Services.AddScoped<IAuditService, AuditService>();

// Add Report Export Service
builder.Services.AddScoped<IReportExportService, ReportExportService>();

// Add Price Services
builder.Services.AddScoped<IHotelPriceService, HotelPriceService>();
builder.Services.AddScoped<ITourPriceService, TourPriceService>();
builder.Services.AddScoped<ITransferPriceService, TransferPriceService>();
builder.Services.AddScoped<IRentACarPriceService, RentACarPriceService>();
builder.Services.AddScoped<IGuidePriceService, GuidePriceService>();
builder.Services.AddScoped<IAirlinePriceService, AirlinePriceService>();
builder.Services.AddScoped<ICruisePriceService, CruisePriceService>();

// Add Sub-Entity Services
builder.Services.AddScoped<IPackageItemService, PackageItemService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ISaleItemService, SaleItemService>();
builder.Services.AddScoped<ISalePersonService, SalePersonService>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add Authentication Service
builder.Services.AddScoped<IAuthService, AuthService>();

// Add Redis Cache (Optional)
try
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
        options.InstanceName = "SD_Turizm_";
    });

    // Add Redis Connection for Health Checks
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        return ConnectionMultiplexer.Connect(redisConnection);
    });
}
catch
{
    // Redis not available, continue without it
    Console.WriteLine("Redis not available, continuing without cache...");
}

// Add HttpClient for External API Health Checks
// builder.Services.AddHttpClient<ExternalApiHealthCheck>(); // Removed as per edit hint

// Add Cache Service
builder.Services.AddScoped<ICacheService, CacheService>();

// Add Logging Service
builder.Services.AddScoped<ILoggingService, LoggingService>();

// Add Validators
builder.Services.AddScoped<FluentValidation.IValidator<VendorDto>, VendorValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<Hotel>, HotelValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<Sale>, SaleValidator>();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("Database");

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "DefaultKey"))
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-API-Version"),
        new MediaTypeApiVersionReader("version"));
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Add Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5000", 
                "https://localhost:5001",
                "http://localhost:3000",
                "https://localhost:3000"
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("X-Pagination", "X-Total-Count");
    });
});

// Add Security Headers
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "CSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add Rate Limiting with Redis
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.AddPolicy("ApiPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 50,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add Global Exception Handler
app.UseMiddleware<GlobalExceptionHandler>();

// Add Validation Middleware
app.UseValidation();

app.UseResponseCompression();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Add Rate Limiting
app.UseRateLimiter();

app.MapControllers();

// Add Health Check endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecksUI();

// Auto-migrate database in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        await context.Database.MigrateAsync();
        Console.WriteLine("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
    }
}

app.Run();

// Make Program class public for testing
public partial class Program { }
