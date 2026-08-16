using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.Data.DigitalMarketing.Data.Repositories;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Data;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.Services.DigitalMarketing.Services.Implementations;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.Services.DigitalMarketing.Services.Validators.ContactMessage;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MyDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddControllersWithViews()
    // For New Views Rout
    .AddRazorOptions(opt =>
    {
        opt.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
        opt.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    });



// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();



// Services
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();





// Fluent Validations
builder.Services.AddValidatorsFromAssembly(Assembly.Load("DigitalMarketing.Services")); // Validator ها رو باید اضافه کنم



// AutoMapper Profiles
builder.Services.AddAutoMapper(am =>
{
    am.AddMaps(typeof(ProductCategoryProfile));
    am.AddMaps(typeof(ArticleCategoryProfile));
    am.AddMaps(typeof(ProductProfile));
    am.AddMaps(typeof(ArticleProfile));
});




// Helpers
builder.Services.AddScoped<IFileUploadHelper, FileUploadHelper>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


// Get files from DigitalMarketing.Admin
var adminUploadsPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "..",
    "DigitalMarketing.Admin",
    "wwwroot",
    "uploads"
);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.GetFullPath(adminUploadsPath)
    ),
    RequestPath = "/uploads"
});

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
