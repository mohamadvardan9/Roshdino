namespace DigitalMarketing.Admin.Helpers
{
    public class FileUploadHelper
    {
        private readonly IWebHostEnvironment _environment;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB


        public FileUploadHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<(bool Success, string? Path, string? Error)> SaveImageAsync(IFormFile file,
            string subFolder)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                return (false, null, $"فرمت {extension} مجاز نیست.");

            if (file.Length > MaxFileSize)
                return (false, null, "حجم فایل نباید بیشتر از ۵ مگابایت باشد.");

            var folder = Path.Combine(_environment.WebRootPath, "uploads", subFolder);
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return (true, $"/uploads/{subFolder}/{fileName}", null);
        }

        public void DeleteImage(string imageUrl)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }




    }
}
