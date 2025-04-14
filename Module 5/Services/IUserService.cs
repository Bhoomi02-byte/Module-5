using Module_5.DTO;

namespace Module_5.Services
{
    public interface IUserService
    {
        Task<string> LikePost(string userId, string postId);
        Task<string> UnLikePost(string userId, string postId);
        Task<string> CreateAsync(CommentDto commentDto, string userId, string postId);
        Task<List<object>> GetAsync(string postId);
        Task<string> DeleteAsync(string postId, string userId, string commentId);
    }
}
