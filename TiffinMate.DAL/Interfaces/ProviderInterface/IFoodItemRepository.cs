using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Interfaces.ProviderInterface
{
    public interface IFoodItemRepository
    {
        Task<List<FoodItem>> GetAllAsync();
        Task<List<FoodItem>> GetByMenu(Guid? menuId, Guid? category_id);
        Task<FoodItem> GetByIdAsync(Guid id);
        Task<string> AddItemAsync(FoodItem item);
        Task<string> AddCategoryAsync(Categories category);
        Task<List<FoodItem>> GetByProviderAsync(Guid providerId);
        Task<List<Categories>> GetAllCategory();
        Task<List<Menu>> GetAllMenuAsync(Guid?providerId);
        Task<string> AddMenuAsync(Menu menus);
        Task<List<Menu>> GetMenuByProviderAsync(Guid providerId);
        Task<decimal> GetTotalAmount(Guid menuId,List<Guid> categoryIds, string day);
        Task<decimal> GetMonthlyTotalAmount(Guid menuId);
        Task<List<FoodItem>> GetAllFoodItem(Guid? menuId, List<Guid> category_id);
        Task<bool> DeleteMenu(Guid id);
        Task<bool> DeleteFooditem(Guid id);
    }
}
