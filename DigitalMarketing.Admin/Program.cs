using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.Data.DigitalMarketing.Data.Repositories;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Data;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.Services.DigitalMarketing.Services.Configuration;
using DigitalMarketing.Services.DigitalMarketing.Services.Implementations;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.Services.DigitalMarketing.Services.Mapping;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Reflection;



/*////////////////////////////
            builder  
////////////////////////////*/

var builder = WebApplication.CreateBuilder(args);


// <<<   Database Options   >>> //

builder.Services.AddDbContextFactory<MyDbContext>(d =>
d.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<MyDbContext>(sp =>
    sp.GetRequiredService<IDbContextFactory<MyDbContext>>().CreateDbContext());




// <<<   AddContollerWithView   >>> //

builder.Services.AddControllersWithViews(options =>
{
    // این یه فیلتر سراسریه که همه چیز رو به پیش فرض [Authorize] کنه
    // فقط روی کنترولرها یا اکشن هایی که 
    // [AllowAnonymous]
    // دارند کار نمیکنه
    // خودت فهمیدی دایی :)
    var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
});





// <<<   Add Authentication   >>> //

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Account/Login";
        opt.LogoutPath = "/Account/Logout";
        opt.AccessDeniedPath = "/Account/Login";
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
        opt.SlidingExpiration = true;
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; // فقط HTTPS
    });







// <<<   Repositories   >>> //

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IMainRepository, MainRepository>();



// <<<   Services   >>> //

builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
builder.Services.AddScoped<IMainService, MainService>();
builder.Services.AddScoped<IAdminSearchService, AdminSearchService>();




// <<<   Fluent Validations   >>> //

builder.Services.AddValidatorsFromAssembly(Assembly.Load("DigitalMarketing.Services"));




// <<<   AutoMapper Profiles   >>> //

builder.Services.AddAutoMapper(am =>
{
    am.AddMaps(typeof(ProductCategoryProfile));
    am.AddMaps(typeof(ArticleCategoryProfile));
    am.AddMaps(typeof(ProductProfile));
    am.AddMaps(typeof(ArticleProfile));
    am.AddMaps(typeof(ContactMessageProfile));
});





// <<<   Uploads Configuration   >>> //

builder.Services.Configure<UploadsOptions>(builder.Configuration.GetSection(UploadsOptions.SectionName));


// <<<   Heplers   >>> //

builder.Services.AddScoped<IFileUploadHelper, FileUploadHelper>();











/*////////////////////////////
            app  
////////////////////////////*/




var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}





app.UseHttpsRedirection();



// <<< Static files from Web's wwwroot >>> //

app.UseStaticFiles();



// <<< Uploaded files shared between Admin and Web >>> //

var uploadsOptions = app.Services.GetRequiredService<IOptions<UploadsOptions>>().Value;
// Ensure the shared upload directory exists
Directory.CreateDirectory(uploadsOptions.RootPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsOptions.RootPath),
    RequestPath = uploadsOptions.RequestPath
});





app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");





// <<<   app.Run();   >>> //
app.Run();
