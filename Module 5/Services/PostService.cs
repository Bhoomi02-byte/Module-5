using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.DTO;
using Module_5.Models.Entities;
using Module_5.Utilities;

namespace Module_5.Services
{
    public class PostService:IPostService
    {
       
        private readonly BlogDbContext _context;

        public PostService(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> CreateAsync(int userId, int categoryId, PostDto postDto)
        {
            
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return new ApiResponse(false, 404, JsonHelper.GetMessage(107), null);

            
            bool titleExists = await _context.Posts.AnyAsync(p => p.Title == postDto.Title);
            if (titleExists)
                return new ApiResponse(false, 400, JsonHelper.GetMessage(112), null);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new ApiResponse(false, 404, JsonHelper.GetMessage(102), null);

            
            var newPost = new Post
            {
                Title = postDto.Title,
                Content = postDto.Content,
                ImageUrl = postDto.ImageUrl,
                AuthorId = userId,
                CategoryId = categoryId,
                IsPublished = false
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            var response = new
            {
                newPost.Id,
                newPost.Title,
                newPost.Content,
                newPost.ImageUrl,
                AuthorName = user.Name,
                category.CategoryName,

            };
            return new ApiResponse(true, 201, "Post created successfully.", response);
           
        }
        public async Task<List<object>> GetAllAsync(int userId, string userRole)
        {
            return await _context.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Where(p => userRole == "Author" && p.AuthorId == userId || p.IsPublished)
                .Select(p => new
                {
                    postId=p.Id,
                    p.Title,
                    p.Content,
                    p.ImageUrl,
                    AuthorName =  p.Author.Name ,
                    CategoryName = p.Category != null ? p.Category.CategoryName : "Uncategorized",
                    p.IsPublished
                })
                .ToListAsync<object>();
        }
        public async Task<List<object>> GetAsync(int postId, int userId, string userRole)
        {
            return await _context.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Where(p => p.Id == postId &&
                   ((userRole == "Author" && p.AuthorId == userId) || p.IsPublished))
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Content,
                    p.ImageUrl,
                    AuthorName = p.Author.Name,
                    CategoryName = p.Category != null ? p.Category.CategoryName : "Uncategorized",
                    p.IsPublished
                })
                .ToListAsync<object>();
        }
        public async Task<bool> DeleteAsync(int postId, int userId)
        {
            
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null|| post.AuthorId != userId)
                return false; 

             _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return true; 
        }
        public async Task<PostDto?> UpdateAsync(int postId, int userId, PostDto postDto)
        {
           
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null|| post.AuthorId != userId)
                return null; 

            post.Title = postDto.Title;
            post.Content = postDto.Content;
            post.ImageUrl = postDto.ImageUrl;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return new PostDto
            {
               
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl
            };
        }

        public async Task<bool> PublishAsync(int postId, int userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null|| post.AuthorId != userId)
                return false;

            post.IsPublished = true;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();


            return true;
           }
        public async Task<bool> UnPublishAsync(int postId, int userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null || post.AuthorId != userId)
                return false;

            post.IsPublished = false;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return true;
           
            
              
           
        }

        





    }
}

   