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

        public async Task<List<FoodItemDto>> GetFoodItemsAsync()
        {
            var result=await _foodItemRepository.GetAllAsync();
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

           
            var foodItem = _mapper.Map<FoodItem>(foodItemDto);
            foodItem.image = imageUrl;
            if (string.IsNullOrEmpty(foodItem.image))
            {
                return false;
            }
            await _foodItemRepository.AddItemAsync(foodItem);
            return true; 
        }

        public async Task<string> AddCategories(CategoryDto category)
        {
            var Catgory=_mapper.Map<Categories>(category);
            await _foodItemRepository.AddCategoryAsync(Catgory);
            return "Added successfully";

        }

        public async Task<List<FoodItemDto>> GetByProviderAsync(Guid id)
        {
            var result = await _foodItemRepository.GetByProviderAsync(id);
            return _mapper.Map<List<FoodItemDto>>(result);
        }

    }
}
