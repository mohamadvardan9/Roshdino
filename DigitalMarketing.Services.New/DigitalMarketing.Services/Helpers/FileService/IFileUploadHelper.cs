using Microsoft.AspNetCore.Http;

namespace DigitalMarketing.DigitalMarketing.Services.Helpers.FileService
{
    public interface IFileUploadHelper
    {
        Task<(bool Success, string? Path, string? Error)> SaveImageAsync(IFormFile file, string subFolder);
        void DeleteImage(string imageUrl);
    }
}
