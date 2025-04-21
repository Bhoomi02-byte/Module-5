namespace Module_5.Services
{
    public interface IS3Service
    {
        Task<string> UploadImageAsync(int postId, int userId, IFormFile image, HttpRequest request);
        Task<string> GeneratePresignedUrl(string fileName);
    }

}

