using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.Data.DigitalMarketing.Data.Repositories;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Data;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.DigitalMarketing.Services.Validators.Article;
using DigitalMarketing.DigitalMarketing.Services.Validators.ProductCategory;
using DigitalMarketing.Services.DigitalMarketing.Services.Implementations;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;






var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Add services to the container.
builder.Services.AddControllersWithViews();




// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();




// Services
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();








// Fluent Validations
builder.Services.AddValidatorsFromAssembly(Assembly.Load("DigitalMarketing.Services"));
//builder.Services.AddValidatorsFromAssembly(typeof(CreateArticleDtoValidator).Assembly);





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










// Test in other diveces
// http://10.218.61.209:5079
//builder.WebHost.ConfigureKestrel(opt =>
//{
//    opt.ListenAnyIP(5079);
//});



var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // new added

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
