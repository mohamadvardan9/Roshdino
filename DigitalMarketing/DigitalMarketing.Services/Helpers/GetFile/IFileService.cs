namespace DigitalMarketing.DigitalMarketing.Services.Helpers.GetFile
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
        Task DeleteAsync(string path);
    }
}
