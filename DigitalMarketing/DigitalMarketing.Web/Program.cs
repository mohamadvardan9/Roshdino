using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Data;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.DigitalMarketing.Services.Validators.ProductCategory;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MyDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
//builder.Services.AddControllersWithViews();



builder.Services.AddControllersWithViews()
    // For New Views Rout
    .AddRazorOptions(opt =>
    {
        opt.ViewLocationFormats.Add("/DigitalMarketing.Web/Views/{1}/{0}.cshtml");
        opt.ViewLocationFormats.Add("/DigitalMarketing.Web/Views/Shared/{0}.cshtml");
    });



// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();



// Services
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IArticleCategoryService, ArticleCategoryService>();





// Fluent Validations
builder.Services.AddValidatorsFromAssembly(Assembly.Load("DigitalMarketing"));





// AutoMapper Profiles
builder.Services.AddAutoMapper(am =>
{
    am.AddMaps(typeof(ProductCategoryProfile));
    am.AddMaps(typeof(ArticleCategoryProfile));
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



// New wwwroot location(route)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "DigitalMarketing.Web/wwwroot")),
    RequestPath = ""
});


app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
