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
        Task<List<FoodItemDto>> GetFoodItemsAsync();
        Task<FoodItemDto> GetFoodItemAsync(Guid id);
        Task<bool> AddFoodItemAsync(FoodItemDto foodItemDto, IFormFile image);

        Task<string> AddCategories( CategoryDto category);
        Task<List<FoodItemDto>> GetByProviderAsync(Guid id);
        Task<List<Categories>> GetCategoryAsync();



    }
}
