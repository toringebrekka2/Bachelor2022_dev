using System.Collections;using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Straisimulator.AppTools;
using Straisimulator.Data;

var builder = WebApplication.CreateBuilder(args);

IConfigurationBuilder confbuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
var configuration = confbuilder.Build();

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    opts.UseSqlServer(configuration.GetConnectionString("Default"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


/* ---- App specific ---- */
AllOrderQues allOrderQues = new AllOrderQues();







/* ---------------------- */

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();