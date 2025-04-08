using Module_5.DTO;
using Module_5.Models.Entities;
using Module_5.Utilities;

namespace Module_5.Services
{
    public interface IPostService
    {
        Task<ApiResponse> CreateAsync(int userId, int categoryId, PostDto postDto);
        Task<List<object>> GetAllAsync();
        Task<List<object>> GetAsync(int postId ,int userId, string userRole);
        Task<bool> DeleteAsync(int postId, int userId);
        Task<PostDto> UpdateAsync(int postId, int userId, PostDto postDto);
        Task<bool> PublishAsync(int postId, int userId);
        Task<bool> UnPublishAsync(int postId, int userId);
        Task<string> UploadImageAsync(int postId,int userId, IFormFile image, HttpRequest request);

    }
}
