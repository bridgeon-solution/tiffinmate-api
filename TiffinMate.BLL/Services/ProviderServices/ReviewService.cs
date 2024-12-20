﻿using System;
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
                    providername = review.provider?.username,
                    image=review.user.image

                }).ToList();

                return reviewDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching reviews: {ex.Message}");
            }
        }
        public async Task<List<AllReview>> GetAllProviderReview(Guid ProviderId)
        {
            try
            {
                var reviews = await _reviewRepository.GetProviderReview(ProviderId);

                if (reviews == null || !reviews.Any())
                {
                    throw new ArgumentException("No reviews found for the given provider .");
                }

                var reviewDtos = reviews.Select(review => new AllReview
                {
                    id= Guid.NewGuid(),
                    ProviderId = review.provider_id,
                    UserId = review.user_id,
                    review = review.review,
                    username = review.user?.name,
                    providername = review.Provider?.username,
                     image = review.user.image
                }).ToList();

                return reviewDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching reviews: {ex.Message}");
            }
        }

    }
}
