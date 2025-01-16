using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IRatingService
    {
        Task<bool> PostRating(RatingRequestDto ratingDto);
    }
}
