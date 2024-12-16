﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.BLL.Services.ProviderServices;
using TiffinMate.DAL.Entities.ProviderEntity;
using Twilio.Http;
using Twilio.Rest.Trunking.V1;

namespace TiffinMate.API.Controllers.ProviderControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }


        [HttpPost("FoodItem")]
        public async Task<IActionResult> AddItem([FromForm] FoodItemDto foodItemDto, IFormFile image)
        {
            var response = await _foodItemService.AddFoodItemAsync(foodItemDto, image);
            if (!response)
            {
                return BadRequest(new ApiResponse<string>("failure", "Addition failed", null, HttpStatusCode.BadRequest, "Image is not uploaded"));
            }
            var result = new ApiResponse<bool>("success", "Addition Successful", response, HttpStatusCode.OK, "");
            return Ok(result);
        }


        [HttpPost("Categery")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            var response= await _foodItemService.AddCategories(category);
            if (response == "category with same name already exists.")
            {
                return BadRequest(new ApiResponse<string>("failure", "Addition failed", null, HttpStatusCode.BadRequest, "category with same name already exists. "));
            }
            var result = new ApiResponse<string>("success", "Addition Successful", response, HttpStatusCode.OK, "");
            return Ok(result);
        }


        [HttpGet("FoodItem")]
        public async Task<IActionResult> GetFoodItems()
        {
            var result=await _foodItemService.GetFoodItemsAsync();
            if(result== null)
            {
                return NotFound(new ApiResponse<string>("failure","No food items found", null,HttpStatusCode.NotFound, "No food items available"
            ));

           }

            var responce = new ApiResponse<List<FoodItemDto>>("success", "Food items retrieved successfully",result, HttpStatusCode.OK,"");
            return Ok(responce);
        }


        [HttpGet("ById")]
        public async Task<IActionResult> GetFoodItem(Guid id)
        {
            var result = await _foodItemService.GetFoodItemAsync(id);
            if (result == null)
            {
                return NotFound(new ApiResponse<string>("failure", "No food items found", null, HttpStatusCode.NotFound, "No food items available"));
            }

            var response = new ApiResponse<FoodItemDto>("success", "Food items retrieved successfully", result, HttpStatusCode.OK, "");
            return Ok(response);
        }

        [HttpGet("ByProvider")]
        public async Task<IActionResult> GetByProvider(Guid id)
        {
            var result = await _foodItemService.GetByProviderAsync(id);
            if (result == null || !result.Any())
            {
                return NotFound(new ApiResponse<string>("failure", "No food items found for the given provider. ", null, HttpStatusCode.NotFound, "No food items found for the given provider."
            ));

            }

            var responce = new ApiResponse<List<FoodItemDto>>("success", "Food items retrieved successfully", result, HttpStatusCode.OK, "");
            return Ok(responce);

        }

        [HttpGet("category")]
        public async Task<IActionResult> Getcategory()
        {
            var result = await _foodItemService.GetCategoryAsync();
            if (result == null || !result.Any())
            {
                return NotFound(new ApiResponse<string>("failure", "No categories found", null, HttpStatusCode.NotFound, "No categories found"
            ));

            }

            var responce = new ApiResponse<List<CategoryDto>>("success", "categories retrieved successfully", result, HttpStatusCode.OK, "");
            return Ok(responce);
        }



    }
}
