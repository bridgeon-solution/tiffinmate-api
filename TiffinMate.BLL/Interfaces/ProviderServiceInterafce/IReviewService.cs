using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IReviewService
    {
        Task<bool> Reviews(ReviewDto review);
        Task<List<AllReview>> GetAllReview(Guid userId);
        Task<List<AllReview>> GetAllProviderReview(Guid ProviderId);
    }
}
