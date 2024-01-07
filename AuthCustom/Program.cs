using AuthCustom.Authorization.AuthorizationHandlers;
using AuthCustom.Authorization.AuthorizationPolicyProviders;
using AuthCustom.Data;
using AuthCustom.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AuthCustomContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthCustomContext") ?? throw new InvalidOperationException("Connection string 'AuthCustomContext' not found.")));

builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Account/Forbidden";
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthorizationHandler, PermisosAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, MyAuthorizationPolicyProvider>();

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor(); // para poder usar componentes blazor en un proyecto de Razor Pages

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub(); // para poder usar componentes blazor en un proyecto de Razor Pages

app.Run();
