using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }


        [HttpPost("fooditem")]
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


        [HttpPost("categery")]
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


        [HttpGet("fooditem")]
        public async Task<IActionResult> GetFoodItems(Guid? menuId,Guid?category_id)
        {
            if (menuId.HasValue && category_id.HasValue)
            {
                var res = await _foodItemService.GetFoodItemByMenu(menuId,category_id);
                if (res == null)
                {
                    return NotFound(new ApiResponse<string>("failure", "No food items found for the specified menu.", null, HttpStatusCode.NotFound, "No food items available."));
                }

                return Ok(new ApiResponse<List<FoodItemDto>>("success", "Food item retrieved successfully.", res, HttpStatusCode.OK, ""));
            }

            var result = await _foodItemService.GetFoodItemsAsync();
            if (result == null)
            {
                return NotFound(new ApiResponse<string>("failure", "No food items found.", null, HttpStatusCode.NotFound, "No food items available."));
            }

            return Ok(new ApiResponse<List<FoodItemResponceDto>>("success", "Food items retrieved successfully.", result, HttpStatusCode.OK, ""));
        }



        [HttpGet("{id}")]
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

        [HttpGet("providerid/{id}")]
        public async Task<IActionResult> GetByProvider(Guid id)
        {
            var result = await _foodItemService.GetByProviderAsync(id);
            if (result == null || !result.Any())
            {
                return Ok(new ApiResponse<string>("failure", "No food items found for the given provider. ", null, HttpStatusCode.NotFound, "No food items found for the given provider."
            ));

            }

            var responce = new ApiResponse<List<FoodItemResponceDto>>("success", "Food items retrieved successfully", result, HttpStatusCode.OK, "");
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

            var responce = new ApiResponse<List<Categories>>("success", "categories retrieved successfully", result, HttpStatusCode.OK, "");
            return Ok(responce);
        }

        [HttpGet("menu")]
        public async Task<IActionResult> Getmenu([FromQuery] Guid? providerId)
        {
            var result = await _foodItemService.GetMenuAsync(providerId);
            if (result == null || !result.Any())
            {
                return NotFound(new ApiResponse<string>("failure", "No menu found", null, HttpStatusCode.NotFound, "No menu found"
            ));

            }

            var responce = new ApiResponse<List<MenuDto>>("success", "menu retrieved successfully", result, HttpStatusCode.OK, "");
            return Ok(responce);
        }

        [HttpGet("menu/{id:guid}")]
        public async Task<IActionResult> GetByProviderMenu(Guid id)
        {
            var result = await _foodItemService.ByProvider(id);
            if (result == null || !result.Any())
            {
                return NotFound(new ApiResponse<string>("failure", "No menu found for the given provider. ", null, HttpStatusCode.NotFound, "No menu found for the given provider."
            ));

            }

            var responce = new ApiResponse<List<MenuDto>>("success", "menu retrieved successfully", result, HttpStatusCode.OK, "");
            return Ok(responce);

        }


        [HttpPost("menu")]
        public async Task<IActionResult> AddMenu([FromForm] MenuRequestDto menu, IFormFile image)
        {
            var response = await _foodItemService.AddMenuAsync(menu, image);
            if (!response)
            {
                return BadRequest(new ApiResponse<string>("failure", "Addition failed", null, HttpStatusCode.BadRequest, "Menu with the same name already exists under this provider or image upload failed."));
            }
            var result = new ApiResponse<bool>("success", "Addition Successful", response, HttpStatusCode.OK, "");
            return Ok(result);
        }

        [HttpPost("total-amount")]
        public async Task<IActionResult> CalculateTotal([FromBody] PlanRequest request,bool is_subscription)
        {
            try
            {
                var totalAmount = await _foodItemService.CalculateTotal(request,is_subscription);
                return Ok(new ApiResponse<decimal>("success", "total calculated succesfully", totalAmount, HttpStatusCode.OK, ""));

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpPost("menu-category")]
        public async Task<IActionResult> GetAllFoodItem(Guid? menuId, [FromBody] List<Guid> categoryIds)
        {
            try
            {
               
                if (!menuId.HasValue)
                {
                    return BadRequest(new ApiResponse<string>("failed", "menuId is required", null, HttpStatusCode.BadRequest, "Invalid input"));
                }

                if (categoryIds == null || !categoryIds.Any())
                {
                    return BadRequest(new ApiResponse<string>("failed", "categoryIds list is required", null, HttpStatusCode.BadRequest, "Invalid input"));
                }

                var res = await _foodItemService.GetAllFoodItems(menuId.Value, categoryIds);

                if (res == null || !res.Any())
                {
                    return NotFound(new ApiResponse<string>("failed", "No food items found for the given menu and categories", null, HttpStatusCode.NotFound, ""));
                }

                return Ok(new ApiResponse<List<AllFoodItemResponseDTO>>("success", "Food items retrieved successfully", res, HttpStatusCode.OK, ""));
            }
            catch (Exception ex)
            {
               

                var response = new ApiResponse<string>("failed", "An error occurred", ex.Message, HttpStatusCode.InternalServerError, "Error occurred");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }


    }

}
