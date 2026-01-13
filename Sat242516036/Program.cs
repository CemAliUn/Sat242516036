using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// Hocanýn mimarisi ve Custom Namespace'ler:
using MyDbModels;
using Providers;
using QuestPDF.Infrastructure; // PDF Lisansý için
using Sat242516036.Components;
using Sat242516036.Components.Account;
using Sat242516036.Data;
using Sat242516036.Services; // ReportService ve EmailSender için gerekli
using Extensions;
using UnitOfWorks;
using DbContexts;

var builder = WebApplication.CreateBuilder(args);

// 0. QUESTPDF LÝSANS AYARI
QuestPDF.Settings.License = LicenseType.Community;

// 1. VERÝTABANI BAÐLANTISI
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// HOCANIN MÝMARÝSÝ ÝÇÝN: DB Context
builder.Services.AddDbContext<MyDbModel_DbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. IDENTITY (KÝMLÝK DOÐRULAMA) AYARLARI
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// LOGLAMA SERVÝSLERÝ (Madde 19)
builder.Services.AddScoped<Sat242516036.Loggers.DbLogger>();
builder.Services.AddScoped<Sat242516036.Loggers.FileLogger>();

// RAPORLAMA SERVÝSÝ (Madde 24)
builder.Services.AddScoped<ReportService>();

// --- EKLENEN KISIM: EMAIL SERVÝSÝ (Register Hatasý Çözümü) ---
// Bu satýr olmazsa Register sayfasý açýlmaz.
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, Sat242516036.Services.EmailSender>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    // --- EMAIL DOÐRULAMA KAPATMA ---
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    // Þifre Kurallarý (Basitleþtirilmiþ)
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// 3. HOCANIN MÝMARÝSÝ ÝÇÝN SERVÝS KAYITLARI (Madde 22)
builder.Services.AddScoped<IMyDbModel_UnitOfWork, MyDbModel_UnitOfWork<MyDbModel_DbContext>>();
builder.Services.AddScoped<IMyDbModel_Provider, MyDbModel_Provider>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Pipeline Ayarlarý
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

// 4. OTOMATÝK ROL VE ADMÝN OLUÞTURMA (Seeding)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Rolleri Ekle
    string[] roles = { "Admin", "Personel", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Admin Kullanýcýsýný Ekle
    var adminEmail = "admin@sakarya.edu.tr";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = "Sistem Yöneticisi"
        };
        // Þifre: Sau.123
        await userManager.CreateAsync(newAdmin, "Sau.123");
        await userManager.AddToRoleAsync(newAdmin, "Admin");
    }
}

app.Run();