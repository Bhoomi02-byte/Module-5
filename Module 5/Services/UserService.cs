using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.DTO;
using Module_5.Models.Entities;

namespace Module_5.Services
{
    public class UserService : IUserService
    {
        private readonly BlogDbContext _context;

        public UserService(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<string> LikePost(int userId, int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            var user = await _context.Users.FindAsync(userId);
            if (post == null || user == null)
            {
                return "Post not found.";
            }

            bool alreadyLiked = await _context.Likes.AnyAsync(l => l.UserId == userId && l.PostId == postId);

            if (alreadyLiked)
                return "You have already liked this post.";

            var like = new Like
            {
                UserId = userId,
                PostId = postId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return "Post liked successfully";

        }

        public async Task<string> UnLikePost(int userId, int postId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

            if (like == null)
                return "You have not liked this post.";

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return "Post Unliked successfully";
        }

        public async Task<string> CreateAsync(CommentDto commentDto,int userId, int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return "Post does not exist";
            }
            var newComment = new Comment
            {
                Text = commentDto.text,
                UserId = userId,
                PostId = postId,

            };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return "Comment Added successfully";
        }

        public async Task<List<object>> GetAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .Include(c => c.Post)
                .Select(c => new 
                {
                    UserName = c.User.Name,
                    PostTitle = c.Post.Title,
                     Comments = c.Text
                })
                .ToListAsync<object>();
        }

        public async Task<string> DeleteAsync(int postId, int userId,int commentId)
        {
            var comment = await _context.Comments
        .FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == postId);

            if (comment == null)
                return "Comment not found.";

            if (comment.UserId != userId)
                return "You are not authorized to delete this comment.";

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return "Comment deleted successfully.";

        }



    }
}
