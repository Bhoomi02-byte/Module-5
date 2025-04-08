using Module_5.DTO;
using Module_5.Models.Entities;

namespace Module_5.Services
{
    public interface ICategoryService
    {
        Task<bool> CreateAsync(CategoryDto categoryDto, int userId);
        Task<bool>DeleteAsync(int CategoryId, int userId);
        Task<List<object>>GetAllAsync();
        Task<CategoryDto?> UpdateAsync(CategoryDto categoryDto, int userId, int categoryId);
        
    }
}
