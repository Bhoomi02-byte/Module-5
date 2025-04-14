using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.DTO;
using Module_5.Collections;
using MongoDB.Driver;

namespace Module_5.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoDbContext _db;

        public UserService(IMongoDbContext db)
        {
            _db = db;
        }

        public async Task<string> LikePost(string postId, string userId)
        {
            var post = await _db.Posts
       .Find(p => p.Id == postId).FirstOrDefaultAsync();


            if (post == null || post.IsPublished == false)
                return JsonHelper.GetMessage(138);


            var alreadyLiked = await _db.Likes.Find(l => l.UserId == userId && l.PostId == postId).FirstOrDefaultAsync();

            if (alreadyLiked != null)
                return JsonHelper.GetMessage(143);

            var newLike = new Like
            {
                UserId = userId,
                PostId = postId
            };

           await _db.Likes.InsertOneAsync(newLike);

            return JsonHelper.GetMessage(142);

        }

        public async Task<string> UnLikePost(string postId, string userId)
        {
            var like = await _db.Likes
                .Find(l => l.UserId == userId && l.PostId == postId).FirstOrDefaultAsync();

            if (like == null)
                return JsonHelper.GetMessage(141);

           await _db.Likes.DeleteOneAsync(l => l.Id == like.Id);

            return JsonHelper.GetMessage(140);
        }

        public async Task<string> CreateAsync(CommentDto commentDto, string postId, string userId)
        {
            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null || !post.IsPublished)
            {
                return JsonHelper.GetMessage(138);
            }
            var newComment = new Comment
            {
                Text = commentDto.text,
                UserId = userId,
                PostId = postId,

            };
           await _db.Comments.InsertOneAsync(newComment);
            return JsonHelper.GetMessage(137);
        }

        public async Task<List<object>> GetAsync(string postId)
        {
           
            var comments = await _db.Comments
                .Find(c => c.PostId == postId)
                .ToListAsync();

            if (!comments.Any())
                return new List<object>();

            
            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();
            if (post == null || !post.IsPublished)
                return new List<object>();

            
            var userIds = comments.Select(c => c.UserId).Distinct().ToList();
            var users = await _db.Users.Find(u => userIds.Contains(u.Id)).ToListAsync();

            return comments.Select(c => new
            {
                UserName = users.FirstOrDefault(u => u.Id == c.UserId)?.Name,
                PostTitle = post.Title,
                Comments = c.Text
            }).Cast<object>().ToList();
        }


        public async Task<string> DeleteAsync(string postId, string userId, string commentId)
        {
            var comment = await _db.Comments
        .Find(c => c.Id == commentId && c.PostId == postId).FirstOrDefaultAsync();

            if (comment == null)
                return JsonHelper.GetMessage(136);

            if (comment.UserId != userId)
                return JsonHelper.GetMessage(135);


            await  _db.Comments.DeleteOneAsync(c => c.Id == commentId);
         
            return JsonHelper.GetMessage(134);

        }



    }
}
