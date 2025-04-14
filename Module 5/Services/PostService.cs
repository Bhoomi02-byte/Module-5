using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.DTO;
using Module_5.Collections;
using Module_5.Utilities;
using MongoDB.Driver;
using Microsoft.Extensions.Hosting;

namespace Module_5.Services
{
    public class PostService : IPostService
    {

        private readonly IMongoDbContext _db;

        public PostService(IMongoDbContext db)
        {
            _db = db;
        }
        public async Task<ApiResponse> CreateAsync(string userId, string categoryId, PostDto postDto)
        {

            var category = await _db.Categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();
            if (category == null)
                return new ApiResponse(false, 404, JsonHelper.GetMessage(107), null);


            bool titleExists = await _db.Posts.Find(p => p.Title == postDto.Title).AnyAsync();
            if (titleExists)
                return new ApiResponse(false, 400, JsonHelper.GetMessage(112), null);

            var user = await _db.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            var newPost = new Post
            {
                Title = postDto.Title,
                Content = postDto.Content,
                ImageUrl = postDto.ImageUrl,
                AuthorId = userId,
                CategoryId = categoryId,
                IsPublished = false
            };

           await _db.Posts.InsertOneAsync(newPost);
           
            var response = new
            {
                newPost.Id,
                newPost.Title,
                newPost.Content,
                newPost.ImageUrl,
                AuthorName = user.Name,
                category.CategoryName,

            };
            return new ApiResponse(true, 201, JsonHelper.GetMessage(113), response);

        }
        public async Task<List<object>> GetAllAsync()
        {
           
            var posts = await _db.Posts.Find(p => p.IsPublished).ToListAsync();

            if (!posts.Any())
                return new List<object>();

            var authorIds = posts.Select(p => p.AuthorId).Distinct().ToList();
            var categoryIds = posts.Select(p => p.CategoryId).Distinct().ToList();

            
            var authors = await _db.Users.Find(u => authorIds.Contains(u.Id)).ToListAsync();
            var categories = await _db.Categories.Find(c => categoryIds.Contains(c.Id)).ToListAsync();

            return posts.Select(p => new
            {
                postId = p.Id,
                p.Title,
                p.Content,
                p.ImageUrl,
                AuthorName = authors.FirstOrDefault(a => a.Id == p.AuthorId)?.Name,
                CategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.CategoryName,
                p.IsPublished
            }).Cast<object>().ToList();
        }


        public async Task<List<object>> GetAsync(string postId, string userId, string userRole)
        {
            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return new List<object>();

            
            if (post.AuthorId != userId && !post.IsPublished)
                return new List<object>();

           
            var author = await _db.Users.Find(u => u.Id == post.AuthorId).FirstOrDefaultAsync();
            var category = await _db.Categories.Find(c => c.Id == post.CategoryId).FirstOrDefaultAsync();

            return new List<object>
          {
           new
          {
            postId = post.Id,
            post.Title,
            post.Content,
            post.ImageUrl,
            AuthorName = author?.Name,
            CategoryName = category?.CategoryName,
            post.IsPublished
          }
         };
        }

        public async Task<bool> DeleteAsync(string postId, string userId)
        {


            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null || post.AuthorId != userId)
                return false;

           await _db.Posts.DeleteOneAsync(p => p.Id == postId);

           await _db.Likes.DeleteManyAsync(l=> l.PostId == postId);

           await _db.Comments.DeleteManyAsync(l => l.PostId == postId);
            
            return true;
        }
        public async Task<PostDto?> UpdateAsync(string postId, string userId, PostDto postDto)
        {
            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null || post.AuthorId != userId)
                return null;

            post.Title = postDto.Title;
            post.Content = postDto.Content;
            post.ImageUrl = postDto.ImageUrl;

           await _db.Posts.ReplaceOneAsync(p => p.Id==postId,post);
           
            return new PostDto
            {

                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl
            };
        }

        public async Task<bool> PublishAsync(string postId, string userId)
        {
            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null || post.AuthorId != userId)
                return false;

            post.IsPublished = true;

             await _db.Posts.ReplaceOneAsync(p => p.Id == postId,post);
       
             return true;
        }
        public async Task<bool> UnPublishAsync(string postId, string userId)
        {
            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null || post.AuthorId != userId)
                return false;

            post.IsPublished = false;
            await  _db.Posts.ReplaceOneAsync(p => p.Id == postId,post);
           
            return true;


        }
        public async Task<string> UploadImageAsync(string postId, string userId, IFormFile image, HttpRequest request)
        {
            var post = await _db.Posts.Find(p => p.Id == postId).FirstOrDefaultAsync();
            if (post == null) return JsonHelper.GetMessage(138);

            if (post.AuthorId != userId) return JsonHelper.GetMessage(150);


            var extension = Path.GetExtension(image.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(extension.ToLower()))
                return JsonHelper.GetMessage(151);

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var imageUrl = $"{request.Scheme}://{request.Host}/uploads/{fileName}";


            post.ImageUrl = imageUrl;
            await _db.Posts.ReplaceOneAsync(p => p.Id == postId,post);

            return JsonHelper.GetMessage(152);
        }


    }
}

