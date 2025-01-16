using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Interfaces.ReviewInterface;
using TiffinMate.DAL.Interfaces.UserInterfaces;

namespace TiffinMate.BLL.Services.ProviderServices
{
    public class RatingService:IRatingService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRaingRepository _ratingRepository;
        public RatingService(IProviderRepository providerRepository,IUserRepository userRepository,IReviewRaingRepository ratingRepository)
        {
            _providerRepository = providerRepository;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
        }
        public async Task<bool> PostRating(RatingRequestDto ratingDto)
        {
            var provider = await _providerRepository.GetProviderById(ratingDto.ProviderId);
            if (provider == null)
            {
                return false;
            }

            var user = await _userRepository.GetUserById(ratingDto.UserId);
            if (user == null)
            {

                throw new ArgumentException("Invalid User ID");
            }
            var rating = new Rating
            {
                id = Guid.NewGuid(),
                provider_id = ratingDto.ProviderId,
                user_id = ratingDto.UserId,
                rating = ratingDto.Rating,

            };

            await _ratingRepository.AddRating(rating);

            return true;

        }
    }
}
