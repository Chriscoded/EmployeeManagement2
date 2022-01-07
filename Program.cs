using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddNLog();
// Add services to the container.
builder.Services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDbConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();  

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddOptions();
builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
//builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();    
app.UseMvc(routes => routes.MapRoute("default", "{controller=home}/{action=index}/{id?}"));
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
