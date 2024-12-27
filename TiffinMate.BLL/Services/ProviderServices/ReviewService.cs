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
using Twilio.Annotations;

namespace TiffinMate.BLL.Services.ProviderServices
{
    public class ReviewService : IReviewService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IProviderRepository providerRepository, IUserRepository userRepository, IReviewRepository reviewRepository)
        {
            _providerRepository = providerRepository;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
        }
        public async Task<bool> Reviews(ReviewDto reviewDto)
        {
            var provider = await _providerRepository.GetProviderById(reviewDto.ProviderId);
            if (provider == null)
            {
                throw new ArgumentException("Invalid Provider ID");
            }

            var user = await _userRepository.GetUserById(reviewDto.UserId);
            if (user == null)
            {

                throw new ArgumentException("Invalid User ID");
            }
            var review = new Review
            {
                provider_id = reviewDto.ProviderId,
                user_id = reviewDto.UserId,
                review = reviewDto.review
            };
            await _reviewRepository.UpdateProvider(review);


            await _providerRepository.SaveChangesAsync();

            return true;

        }
        public async Task<List<AllReview>> GetAllReview(Guid userId)
        {
            try
            {
                var reviews = await _reviewRepository.GetUserReview(userId);

                if (reviews == null || !reviews.Any())
                {
                    throw new ArgumentException("No reviews found for the given provider and user.");
                }

                var reviewDtos = reviews.Select(review => new AllReview
                {
                    id = Guid.NewGuid(),
                    ProviderId = review.provider_id,
                    UserId = review.user_id,
                    review = review.review,
                    username = review.user?.name,
                    providername = review.provider?.user_name,
                    image = review.user.image,
                    created_at = review.created_at

                }).ToList();

                return reviewDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching reviews: {ex.Message}");
            }
        }
        public async Task<List<AllReview>> GetAllProviderReview(Guid providerid)
        {
            try
            {
                var reviews = await _reviewRepository.GetProviderReview(providerid);

                if (reviews == null || !reviews.Any())
                {
                    return [];
                }

                var reviewDtos = reviews.Select(review => new AllReview
                {
                    id = Guid.NewGuid(),
                    ProviderId = review.provider_id,
                    UserId = review.user_id,
                    review = review.review,
                    username = review.user?.name,
                    providername = review.provider?.user_name,
                    image = review.user.image,
                    created_at = review.created_at
                }).ToList();

                return reviewDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching reviews: {ex.Message}");
            }
        }
        public async Task<PaginationReview> ReviewsList(Guid ProviderId, int page, int pageSize, string search = null, string filter = null)
        {
            var reviews = (await _reviewRepository.GetProviderReview(ProviderId)).ToList();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                reviews = reviews
                    .Where(u => u.user.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                u.user.email.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // sort filter
            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.ToLower() == "true")
                {
                    reviews = reviews.OrderByDescending(u => u.created_at).ToList();
                }
                else if (filter.ToLower() == "false")
                {
                    reviews = reviews.OrderBy(u => u.created_at).ToList();
                }
            }

            var totalCount = reviews.Count;

            // pagination
            var pagedReviews = reviews
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var reviewDtos = pagedReviews.Select(review => new AllReview
            {
                id = review.id,
                ProviderId = review.provider_id,
                UserId = review.user_id,
                review = review.review,
                username = review.user?.name,
                providername = review.provider?.user_name,
                image = review.user.image,
                created_at = review.created_at
            }).ToList();

            var result = new PaginationReview
            {
                TotalCount = totalCount,
                reviews = reviewDtos
            };

            return result;
        }

    }
}
