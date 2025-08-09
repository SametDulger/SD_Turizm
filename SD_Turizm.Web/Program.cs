using SD_Turizm.Web.Services;
using SD_Turizm.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure built-in logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpClient
builder.Services.AddHttpClient();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Configuration
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add API Client Services
builder.Services.AddScoped<IApiClientService, ApiClientService>();
builder.Services.AddScoped<IAuthApiService, AuthApiService>();
builder.Services.AddScoped<IHotelApiService, HotelApiService>();
builder.Services.AddScoped<ITourApiService, TourApiService>();
builder.Services.AddScoped<ISaleApiService, SaleApiService>();
builder.Services.AddScoped<IVendorApiService, VendorApiService>();
builder.Services.AddScoped<ITourOperatorApiService, TourOperatorApiService>();
builder.Services.AddScoped<IAirlineApiService, AirlineApiService>();
builder.Services.AddScoped<ICruiseApiService, CruiseApiService>();
builder.Services.AddScoped<IGuideApiService, GuideApiService>();
builder.Services.AddScoped<IRentACarApiService, RentACarApiService>();
builder.Services.AddScoped<ITransferCompanyApiService, TransferCompanyApiService>();

// Add new API services
builder.Services.AddScoped<IPackageApiService, PackageApiService>();
builder.Services.AddScoped<IPackageItemApiService, PackageItemApiService>();
builder.Services.AddScoped<IAddressApiService, AddressApiService>();
builder.Services.AddScoped<IDashboardApiService, DashboardApiService>();
builder.Services.AddScoped<IExchangeRateApiService, ExchangeRateApiService>();
builder.Services.AddScoped<ICariTransactionApiService, CariTransactionApiService>();
builder.Services.AddScoped<IUserApiService, UserApiService>();
builder.Services.AddScoped<IRoleApiService, RoleApiService>();
builder.Services.AddScoped<IAuditApiService, AuditApiService>();
builder.Services.AddScoped<IHotelPriceApiService, HotelPriceApiService>();
builder.Services.AddScoped<IAirlinePriceApiService, AirlinePriceApiService>();
builder.Services.AddScoped<ISalePersonApiService, SalePersonApiService>();
builder.Services.AddScoped<ISaleItemApiService, SaleItemApiService>();
builder.Services.AddScoped<IReportsApiService, ReportsApiService>();
builder.Services.AddScoped<ITourPriceApiService, TourPriceApiService>();
builder.Services.AddScoped<ICruisePriceApiService, CruisePriceApiService>();
builder.Services.AddScoped<IGuidePriceApiService, GuidePriceApiService>();
builder.Services.AddScoped<IRentACarPriceApiService, RentACarPriceApiService>();
builder.Services.AddScoped<ITransferPriceApiService, TransferPriceApiService>();
builder.Services.AddScoped<ILookupApiService, LookupApiService>();

// Add Authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Add global exception middleware
app.UseGlobalExceptionMiddleware();

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
