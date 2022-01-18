using Mango.Web;
using Mango.Web.Services;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddHttpClient<IProductService, ProductService>();
SD.ProductAPIBase = configuration["ServiceUrls:ProductAPI"];
SD.ShoppingCartAPIBase = configuration["ServiceUrls:ShoppingCartAPI"];
SD.CouponAPIBase = configuration["ServiceUrls:CouponAPI"];

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddRazorRuntimeCompilation(); // me
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})//Corresponds to the values from SD Mango.Identity
    .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(15)).AddOpenIdConnect("oidc", options =>
{
    options.Authority = configuration["ServiceUrls:IdentityAPI"];
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClientId = "mango";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.ClaimActions.MapJsonKey("role", "role", "role");
    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
    options.Scope.Add("mango");
    options.SaveTokens = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
