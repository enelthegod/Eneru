namespace Eneru.Services
{
    public class ImageUploadService
    {
        private readonly IWebHostEnvironment _env;

        // IWebHostEnvironment gives us the physical path to wwwroot folder
        public ImageUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> SaveImageAsync(IFormFile? file)
        {
            // Return null if no file was provided
            if (file == null || file.Length == 0)
                return null;

            // Only allow image files for security
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return null;

            // Generate unique filename so files don't overwrite each other
            // Example: a3f5b2c1-4d6e-4f7a-8b9c-1d2e3f4a5b6c.jpg
            var fileName = $"{Guid.NewGuid()}{extension}";

            // Build full path to wwwroot/uploads/ folder
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

            // Create uploads folder if it doesn't exist yet
            Directory.CreateDirectory(uploadsFolder);

            // Full path where file will be saved on disk
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Copy uploaded file from memory to disk
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Return the URL path that will be stored in the database
            return $"/uploads/{fileName}";
        }
    }
}