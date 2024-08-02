using Ecomm_project_01.DataAccess.Data;
using Ecomm_project_01.DataAccess.Repository;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Ecomm_project_01.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("con");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddScoped<iCategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<iCoverTypeRepository, CoverTypeRepository>();

builder.Services.AddScoped<iUnitOfWork, UnitOfWork>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication().AddFacebook(Options =>
{
    Options.AppId = "1430883451135071";
    Options.AppSecret = "1b67d205113a03e32fefd804aeda1296";
});
builder.Services.AddAuthentication().AddGoogle(Options =>
{
    Options.ClientId = "512091175257-murvd1iagsctomr4kq4udr9llrfttilj.apps.googleusercontent.com";
    Options.ClientSecret = "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";
});

builder.Services.AddAuthentication().AddTwitter(options =>
{
    options.ConsumerKey = builder.Configuration[key:"Authentication:Twitter:Apikey"];
    options.ConsumerSecret = builder.Configuration[key: "Authentication:Twitter:Apisecret"];
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));

builder.Services.ConfigureApplicationCookie(Options =>
{
    Options.LoginPath = $"/Identity/Account/Login";
    Options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    Options.LogoutPath = $"/Identity/Account/Logout";
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISMSSender, SMSSender>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly= true;
    options.Cookie.IsEssential= true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSettings")["SecretKey"];

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
