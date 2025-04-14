using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.Collections;
using Module_5.DTO;
using MimeKit.Encodings;
using MongoDB.Driver;

namespace Module_5.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoDbContext _db;
        public CategoryService(IMongoDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(CategoryDto categoryDto, string userId)
        {
            bool existingCategory = await _db.Categories.Find(c => c.CategoryName == categoryDto.CategoryName).AnyAsync();
            if (existingCategory)
            {
                return false;
            }
            var newCategory = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Description = categoryDto.Description,
                AuthorId = userId

            };
            await _db.Categories.InsertOneAsync(newCategory);

            return true;
        }
        public async Task<string> DeleteAsync(string categoryId, string userId)
        {
            var category = await _db.Categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();
            if (category == null || category.AuthorId != userId)
            {
                return JsonHelper.GetMessage(111);
            }
            bool hasPosts = await _db.Posts.Find(p => p.CategoryId == categoryId).AnyAsync();
            if (hasPosts) return JsonHelper.GetMessage(155);

            await _db.Categories.DeleteOneAsync(c => c.Id == categoryId);

            return JsonHelper.GetMessage(109);
        }

        public async Task<List<object>> GetAllAsync()
        {
            var categories = await _db.Categories.Find(_ => true).ToListAsync();
            return categories
                .Select(c => new
                {
                    CategoryId = c.Id,
                    c.CategoryName,
                    c.Description
                })
                 .Cast<object>()
                 .ToList();
        }
        public async Task<CategoryDto?> UpdateAsync(CategoryDto categoryDto, string userId, string categoryId)
        {
            var category = await _db.Categories.Find(p => p.Id == categoryId).FirstOrDefaultAsync();

            if (category == null || category.AuthorId != userId)
                return null;

            category.CategoryName = categoryDto.CategoryName;
            category.Description = categoryDto.Description;

            await _db.Categories.ReplaceOneAsync(c => c.Id == categoryId, category);
            return new CategoryDto
            {
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

    }


}
