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
        Task<FoodItem> GetByIdAsync(Guid id);
        Task<string> AddItemAsync(FoodItem item);
        Task<string> AddCategoryAsync(Categories category);
        Task<List<FoodItem>> GetByProviderAsync(Guid providerId);
        Task<List<Categories>> GetAllCategory();
        Task<List<Menu>> GetAllMenuAsync();
        Task<string> AddMenuAsync(Menu menus);
    }
}
