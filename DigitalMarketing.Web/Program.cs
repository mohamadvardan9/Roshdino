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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Reflection;






/*////////////////////////////
            builder  
////////////////////////////*/

var builder = WebApplication.CreateBuilder(args);



// <<<   Database Options   >>> //

// MyDbContext
// ثبت MyDbContext برای سازگاری با Repositoryها و Serviceهایی
// که مستقیماً MyDbContext را از طریق DI دریافت می‌کنند.
builder.Services.AddDbContextFactory<MyDbContext>(d =>
d.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DbContextFactory
// برای ایجاد DbContextهای مستقل و مناسب برای عملیات هم‌زمان
builder.Services.AddScoped<MyDbContext>(sp =>
    sp.GetRequiredService<IDbContextFactory<MyDbContext>>().CreateDbContext());





// <<<   AddContollerWithView   >>> //

builder.Services.AddControllersWithViews();




// <<<   Repositories   >>> //

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddScoped<IMainRepository, MainRepository>();


// <<<   Services   >>> //

builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IMainService, MainService>();



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

// دریافت تنظیمات مسیر فیزیکی و مسیر URL فایل‌های آپلودشده
var uploadsOptions = app.Services.GetRequiredService<IOptions<UploadsOptions>>().Value;

// سرو فایل‌های آپلودشده از مسیر مشترک بین Admin و Web
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsOptions.RootPath),
    RequestPath = uploadsOptions.RequestPath
});





app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





// <<<   app.Run();   >>> //
app.Run();
