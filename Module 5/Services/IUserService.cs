using Module_5.DTO;

namespace Module_5.Services
{
    public interface IUserService
    {
        Task<string> LikePost(int userId, int postId);
        Task<string> UnLikePost(int userId, int postId);
        Task<string> CreateAsync(CommentDto commentDto, int userId, int postId);
        Task<List<object>> GetAsync(int postId);
        Task<string> DeleteAsync(int postId, int userId, int commentId);
    }
}
