using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.DAL.Interfaces.ReviewInterface
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewofuser(Guid providerid, Guid userid);
        Task<List<Review>> GetUserReview(Guid userid);
        Task<List<Review>> GetProviderReview(Guid providerId);
        Task<bool> UpdateProvider(Review review);
    }
}
