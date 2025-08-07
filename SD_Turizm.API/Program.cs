using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Infrastructure.Data;
using SD_Turizm.Infrastructure.Repositories;
using SD_Turizm.API.Middleware;

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

// Add Authentication Service
builder.Services.AddScoped<IAuthService, AuthService>();

// Add Cache Service
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

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

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Global Exception Handler
app.UseMiddleware<GlobalExceptionHandler>();

// Add Validation Middleware
app.UseMiddleware<ValidationMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
