using Microsoft.EntityFrameworkCore;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.ReviewInterface;

namespace TiffinMate.DAL.Repositories.ReviewRepository
{
   
    public class ReviewRepository:IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Review>> GetReviewofuser(Guid providerid, Guid userid)
        {
            return await _context.Reviews
         .Where(r => r.ProviderId == providerid && r.UserId == userid)
         .Include(r => r.User)
         .Include(r => r.Provider)
         .ToListAsync();
        }
        public async Task<List<Review>> GetUserReview(Guid userid)
        {
            return await _context.Reviews
         .Where(r => r.UserId == userid)
         .Include(r => r.User)
         .Include(r => r.Provider)
         .ToListAsync();
        }
        public async Task<List<Review>> GetProviderReview(Guid providerId)
        {
            return await _context.Reviews
         .Where(r => r.ProviderId == providerId)
         .Include(r => r.User)
         .Include(r => r.Provider)
         .ToListAsync();
        }
        public async Task<bool> UpdateProvider(Review review)
        {
            _context.Reviews.Add(review);
            int changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

    }
}
