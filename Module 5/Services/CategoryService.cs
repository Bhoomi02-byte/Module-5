using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.Models.Entities;
using Module_5.DTO;
using MimeKit.Encodings;

namespace Module_5.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly BlogDbContext _context;
        public CategoryService(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(CategoryDto categoryDto, int userId)
        {
            var existingCategory= await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryDto.CategoryName);
            if (existingCategory!=null)
            {
                return false;
            }
            var newCategory = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Description = categoryDto.Description,
                AuthorId=userId
                
            };
            _context.Categories.Add(newCategory);
            var result=await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> DeleteAsync(int categoryId, int userId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return false;
            }
            if (category.AuthorId != userId)
            {
                return false;
            }
            _context.Categories.Remove(category);
            var result = await _context.SaveChangesAsync();
            return result > 0;
         }

        public async Task<List<object>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking() 
                .Select(c => new 
                     {   
                         CategoryId=c.Id,
                         c.CategoryName, 
                         c.Description
                     })
                 .ToListAsync<object>();
        }
        public async Task<CategoryDto?>UpdateAsync(CategoryDto categoryDto, int userId ,int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == categoryId);

            if (category == null || category.AuthorId != userId)
                return null;

            category.CategoryName = categoryDto.CategoryName;
            category.Description = categoryDto.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryName = category.CategoryName,
                Description=category.Description
            };
        }



    }

    
}
