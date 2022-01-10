using EmployeeManagement.Models;
using EmployeeManagement2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddNLog();
// Add services to the container.
builder.Services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDbConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireNonAlphanumeric = false;
});

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

////we want every user to be logged in to access controllers thats why we passed option parameters
//builder.Services.AddMvc(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();

//    options.Filters.Add(new AuthorizeFilter(policy));

//} ).AddXmlSerializerFormatters();

builder.Services.AddOptions();
builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
//builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
else
{
    app.UseExceptionHandler("/CustomError");
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
