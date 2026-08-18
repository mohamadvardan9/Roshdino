using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using DigitalMarketing.Services.DigitalMarketing.Services.Configuration;

namespace DigitalMarketing.DigitalMarketing.Services.Helpers.FileService
{
    public class FileUploadHelper : IFileUploadHelper
    {
        private readonly string _uploadsRoot;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB


        public FileUploadHelper(IOptions<UploadsOptions> uploadsOptions)
        {
            _uploadsRoot = uploadsOptions.Value.RootPath;

            if(string.IsNullOrWhiteSpace(_uploadsRoot))
                throw new InvalidOperationException(
                    "مسیر Uploads:RootPath در appsettings.json تنظیم نشده است.");
        }

        public async Task<(bool Success, string? Path, string? Error)> SaveImageAsync(IFormFile file,
            string subFolder)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                return (false, null, $"فرمت {extension} مجاز نیست.");

            if (file.Length > MaxFileSize)
                return (false, null, "حجم فایل نباید بیشتر از ۵ مگابایت باشد.");

            var folder = Path.Combine(_uploadsRoot , subFolder);
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return (true, $"/uploads/{subFolder}/{fileName}", null);
        }

        public void DeleteImage(string imageUrl)
        {
            // imageUrl چیزی مثل "/uploads/products/xxx.jpg" است؛ پیشوند "/uploads/" را برای رسیدن به مسیر فیزیکی حذف می‌کنیم
            var relaticePath = imageUrl.TrimStart('/');
            if (relaticePath.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
                relaticePath = relaticePath["uploads/".Length..];

            var fullPath = Path.Combine(_uploadsRoot, relaticePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }




    }
}
