using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.CloudinaryInterface;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using Twilio.Rest.Trunking.V1;
using static Supabase.Gotrue.Constants;

namespace TiffinMate.BLL.Services.ProviderServices
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _foodItemRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinary;
        private readonly IConfiguration _config;
       
        public FoodItemService(IFoodItemRepository foodItemRepository,IMapper mapper, ICloudinaryService cloudinary, IConfiguration config)
        {
            _foodItemRepository = foodItemRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _config = config;
           
        }

        public async Task<List<FoodItemResponceDto>> GetFoodItemsAsync()
        {
            var result=await _foodItemRepository.GetAllAsync();
            var foodItemsDto = result.Select(e =>
            {
                var dto = _mapper.Map<FoodItemResponceDto>(e);
                dto.category_name = e.category?.category_name;
                dto.menu_name = e.menu?.name;
                dto.menu_id = e.menu.id;
                return dto;
            }).ToList();
            return foodItemsDto;
        }

        public async Task<List<FoodItemDto>> GetFoodItemByMenu(Guid? menuId,Guid? category_id)
        {
            var result = await _foodItemRepository.GetByMenu(menuId,category_id);
            return _mapper.Map<List<FoodItemDto>>(result);

        }

        public async Task<FoodItemDto> GetFoodItemAsync(Guid id)
        {
           var result=await _foodItemRepository.GetByIdAsync(id);
            return _mapper.Map<FoodItemDto>(result);

        }
        public async Task<bool> AddFoodItemAsync(FoodItemDto foodItemDto, IFormFile image)
        {
            if (foodItemDto == null)
                throw new ArgumentNullException(nameof(foodItemDto));

            if (image == null)
                throw new ArgumentNullException(nameof(image));

           
            var imageUrl = await _cloudinary.UploadDocumentAsync(image);

            if (string.IsNullOrEmpty(imageUrl))
            {
                
                return false;
            }

            var foodItem = _mapper.Map<FoodItem>(foodItemDto);
            foodItem.image = imageUrl; 

            await _foodItemRepository.AddItemAsync(foodItem);
            return true;
        }


        public async Task<string> AddCategories(CategoryDto category)
        {
            try
            {
                var Catgory = _mapper.Map<Categories>(category);
                await _foodItemRepository.AddCategoryAsync(Catgory);
                return "Added successfully";
            }
            catch(Exception ex)
            {
                return $"Error occurred: {ex.Message}";
            }

        }
        //foodbyprovider
        public async Task<List<FoodItemResponceDto>> GetByProviderAsync(Guid id)
        {
            var result = await _foodItemRepository.GetByProviderAsync(id);
            var foodItemsDto = result.Select(e =>
            {
                var dto = _mapper.Map<FoodItemResponceDto>(e);
                dto.category_name = e.category?.category_name;
                dto.menu_name = e.menu?.name;
                //dto.menu_id = e.menu.id;
                return dto;
            }).ToList();
            return foodItemsDto;
            
        }


        public async Task<List<Categories>> GetCategoryAsync()
        {
            var result = await _foodItemRepository.GetAllCategory();
            if (result==null)
            {
                return null;
            }
            return _mapper.Map<List<Categories>>(result);
            
            
        }

        public async Task<List<MenuDto>> GetMenuAsync(Guid? providerId)
        {
            var result = await _foodItemRepository.GetAllMenuAsync(providerId);
            if (result == null)
            {
                return null;
            }
            return _mapper.Map<List<MenuDto>>(result);


        }
        public async Task<List<MenuDto>> ByProvider(Guid id)
        {
            var result = await _foodItemRepository.GetMenuByProviderAsync(id);
            return _mapper.Map<List<MenuDto>>(result);
        }

        public async Task<bool> AddMenuAsync(MenuRequestDto menu, IFormFile image)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            if (image == null)
                throw new ArgumentNullException(nameof(image));


            var imageUrl = await _cloudinary.UploadDocumentAsync(image);

            if (string.IsNullOrEmpty(imageUrl))
            {

                return false;
            }

            var menuitem = _mapper.Map<Menu>(menu);
            
            menuitem.image = imageUrl;

           var menuResponce= await _foodItemRepository.AddMenuAsync(menuitem);
            if (menuResponce != "menu added")
            {
                return false;
            }
            return true;
        }


        public async Task<decimal> CalculateTotal(PlanRequest request, bool is_subscription)
        {
            decimal totalAmount;
            if (is_subscription)
            {
                var dayOfMonth = DateTime.Parse(request.date).Day;
                var remainingDays = 30 - dayOfMonth + 1;
                var total = await _foodItemRepository.GetMonthlyTotalAmount(request.menuId);
                var totalForMonth = total / 3 * request.categories.Count();
                totalAmount= totalForMonth / 30 * remainingDays;
            }
            else
            {
                var day = DateTime.Parse(request.date).DayOfWeek.ToString();
                totalAmount = await _foodItemRepository.GetTotalAmount(request.menuId, request.categories, day);
            }
            return Math.Ceiling(totalAmount);
        }

        public async Task<List<AllFoodItemResponseDTO>> GetAllFoodItems(Guid? menuId, List<Guid> category_id)
        {
            var fooditems = await _foodItemRepository.GetAllFoodItem(menuId, category_id);

            var foodItemResponses = fooditems.Select(f => new AllFoodItemResponseDTO
            {
                category_id = f.category_id,
                menu_id = f.menu_id,
                food_name = f.food_name,
                price = f.price,
                description = f.description,
                day = f.day,
                image = f.image,
                category_name=f.category.category_name
                
            }).ToList();

            return foodItemResponses;
        }







    }
}
