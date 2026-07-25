using Microsoft.EntityFrameworkCore;
using SchoolERP.Infrastructure.Data;
using SchoolERP.Infrastructure.Extensions;
using SchoolERP.Application.Extensions;  // ✅ إضافة
using SchoolERP.Application.Mappings;
using SchoolERP.Application.Interfaces.Services;  // ✅ إضافة
using SchoolERP.Application.Services;  // ✅ إضافة



var builder = WebApplication.CreateBuilder(args);

// ➕ إضافة Controllers with Views
builder.Services.AddControllersWithViews();

// ➕ إضافة Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// ➕ إضافة Application Services
builder.Services.AddApplicationServices();

// ➕ إضافة AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

// ➕ إضافة Authentication (Cookie-based)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// ➕ إضافة Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ➕ إضافة HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ➕ Configure Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
pattern: "{controller=Auth}/{action=Login}/{id?}");

// ✅ Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // ✅ Apply migrations
    await dbContext.Database.MigrateAsync();  // ✅ MigrateAsync بدلاً من Migrate

    // ✅ Seed data
    await SeedData3.SeedAsync(dbContext);
}

app.Run();


