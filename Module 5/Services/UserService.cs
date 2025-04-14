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

        public async Task<string> LikePost(int postId, int userId)
        {
            var post = await _context.Posts
       .FirstOrDefaultAsync(p => p.Id == postId);


            if (post == null || post.IsPublished == false)
                return JsonHelper.GetMessage(138);


            var alreadyLiked = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

            if (alreadyLiked!=null)
                return JsonHelper.GetMessage(143);

            var newLike = new Like
            {
                UserId = userId,
                PostId = postId
            };

            _context.Likes.Add(newLike);
            await _context.SaveChangesAsync();

            return JsonHelper.GetMessage(142);

        }

        public async Task<string> UnLikePost(int postId, int userId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

            if (like == null)
                return JsonHelper.GetMessage(141);

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return JsonHelper.GetMessage(140);
        }

        public async Task<string> CreateAsync(CommentDto commentDto,int postId, int userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

           if (post == null)
            {
                return JsonHelper.GetMessage(138);
            }
            var newComment = new Comment
            {
                Text = commentDto.text,
                UserId = userId,
                PostId = postId,

            };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return JsonHelper.GetMessage(137);
        }

        public async Task<List<object>> GetAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId && c.Post.IsPublished)
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
                return JsonHelper.GetMessage(136);

            if (comment.UserId != userId)
                return JsonHelper.GetMessage(135);
               

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return JsonHelper.GetMessage(134);

        }



    }
}
