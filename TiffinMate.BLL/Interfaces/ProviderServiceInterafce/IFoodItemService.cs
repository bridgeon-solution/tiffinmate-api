using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IFoodItemService
    {
        Task<List<FoodItemResponceDto>> GetFoodItemsAsync();
        Task<List<FoodItemDto>>GetFoodItemByMenu(Guid? menuId,Guid?category_id);
        Task<FoodItemDto> GetFoodItemAsync(Guid id);
        Task<bool> AddFoodItemAsync(FoodItemDto foodItemDto, IFormFile image);

        Task<string> AddCategories( CategoryDto category);
        Task<List<FoodItemResponceDto>> GetByProviderAsync(Guid id);
        Task<List<Categories>> GetCategoryAsync();

        Task<List<MenuDto>> GetMenuAsync(Guid?providerId);

        Task<bool> AddMenuAsync(MenuRequestDto menu, IFormFile image);

        Task<List<MenuDto>> ByProvider(Guid id);
        Task<decimal> CalculateTotal(PlanRequest request,bool is_subscription);
        Task<List<AllFoodItemResponseDTO>> GetAllFoodItems(Guid? menuId, List<Guid> category_id);
        Task<bool> DeleteMenu(Guid menu_id);
        Task<bool> DeleteFoodItems(Guid fooditem_id);


    }
}
