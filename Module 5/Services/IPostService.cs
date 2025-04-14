using Module_5.DTO;
using Module_5.Collections;
using Module_5.Utilities;

namespace Module_5.Services
{
    public interface IPostService
    {
        Task<ApiResponse> CreateAsync(string userId, string categoryId, PostDto postDto);
        Task<List<object>> GetAllAsync();
        Task<List<object>> GetAsync(string postId, string userId, string userRole);
        Task<bool> DeleteAsync(string postId, string userId);
        Task<PostDto> UpdateAsync(string postId, string userId, PostDto postDto);
        Task<bool> PublishAsync(string postId, string userId);
        Task<bool> UnPublishAsync(string postId, string userId);
        Task<string> UploadImageAsync(string postId, string userId, IFormFile image, HttpRequest request);

    }
}
