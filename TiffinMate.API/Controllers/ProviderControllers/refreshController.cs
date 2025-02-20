﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.API.ApiRespons;
using Asp.Versioning;
using TiffinMate.BLL.Interfaces;

namespace TiffinMate.API.Controllers.ProviderControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class RefreshController : ControllerBase
    {
        private readonly RefreshInterface _refreshService;

        public RefreshController(RefreshInterface providerService)
        {
            _refreshService = providerService;
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] refreshTokenDto request)
        {
          
            if (string.IsNullOrEmpty(request?.refresh_token))
            {
                return BadRequest(new ApiResponse<string>(
                    "failure",                          
                    "Refresh token is missing",        
                    null,                              
                    HttpStatusCode.BadRequest,          
                    "The refresh token was not provided or is invalid" 
                ));
            }

            try
            {
                
                var result = await _refreshService.GetRefreshToken(request.refresh_token);

              
                return Ok(new ApiResponse<ProviderLoginResponse>(   
                    "success",                                         
                    "Refresh token retrieved successfully",           
                    result,                                             
                    HttpStatusCode.OK,                                 
                    string.Empty                                      
                ));
            }
            catch (Exception ex)
            {
             
                var response = new ApiResponse<string>(
                    "failed",                                          
                    "Internal server error occurred",                
                    null,                                              
                    HttpStatusCode.InternalServerError,                
                    ex.Message                                         
                );
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
