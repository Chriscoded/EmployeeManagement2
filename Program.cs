
using EmployeeManagement2.Models;
using EmployeeManagement2.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;

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
    //options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<AppDbContext>()
/*.AddDefaultTokenProviders()*/;

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequiredLength = 10;
//    options.Password.RequireNonAlphanumeric = false;
//});

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

////we want every user to be logged in to access controllers thats why we passed option parameters
//builder.Services.AddMvc(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();

//    options.Filters.Add(new AuthorizeFilter(policy));

//} ).AddXmlSerializerFormatters();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
        {
            options.ClientId = "568069082839-qpthkhoinpanf0v204771914i94urbgc.apps.googleusercontent.com";
            options.ClientSecret = "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";
            options.CallbackPath = new PathString("/signin-google");
        })
    .AddFacebook(options =>
    {
        options.AppId = "1038041526752527";
        options.AppSecret = "47e5d25b3834b9cd8a3f7b80879dd0df";
    });

builder.Services.AddOptions();
builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddRazorPages();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteRolePolicy",
       //policy => policy.RequireClaim("Delete Role", "true"));
       policy => policy.RequireAssertion(context =>
       context.User.HasClaim(claim => claim.Type == "Delete role" && claim.Value == "true") ||
       context.User.IsInRole("Super Admin")
       ));

    options.AddPolicy("EditRolePolicy",
        policy => policy.RequireAssertion(context =>
        context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true") ||
        context.User.IsInRole("Super Admin")
        ));

    options.AddPolicy("EditingAdminNotSameWithEditedPolicy",
        policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirements()));

    options.AddPolicy("CreateRolePolicy",
        policy => policy.RequireAssertion(context =>
        context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true") ||
        context.User.IsInRole("Super Admin")
        ));

    options.AddPolicy("AdminRolePolicy",
        policy => policy.RequireAssertion(context =>
        context.User.IsInRole("Admin") ||
        context.User.IsInRole("Super Admin")
        ));


    options.AddPolicy("EditUserPolicy",
       policy => policy.RequireAssertion(context =>
       context.User.HasClaim(claim => claim.Type == "Edit User" && claim.Value == "true") ||
       context.User.IsInRole("Super Admin")
       ));

    options.AddPolicy("DeleteUserPolicy",
       policy => policy.RequireAssertion(context =>
       context.User.HasClaim(claim => claim.Type == "Delete User" && claim.Value == "true") ||
       context.User.IsInRole("Super Admin")
       ));
});

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
