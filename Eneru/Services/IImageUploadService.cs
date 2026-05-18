namespace Eneru.Services
{
    public interface IImageUploadService
    {
        Task<string?> SaveImageAsync(IFormFile? file);
    }
}