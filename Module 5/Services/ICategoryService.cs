using Module_5.DTO;
using Module_5.Collections;

namespace Module_5.Services
{
    public interface ICategoryService
    {
        Task<bool> CreateAsync(CategoryDto categoryDto, string userId);
        Task<string> DeleteAsync(string CategoryId, string userId);
        Task<List<object>> GetAllAsync();
        Task<CategoryDto?> UpdateAsync(CategoryDto categoryDto, string userId, string categoryId);

    }
}
