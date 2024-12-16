using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;

namespace TiffinMate.DAL.Repositories.ProviderRepositories
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly AppDbContext _context;

        public FoodItemRepository(AppDbContext context)
        {
            _context = context;
        }

        //getall
        public async Task<List<FoodItem>> GetAllAsync()
        {
           
            return await _context.FoodItems.Include(f => f.category).ToListAsync();
        }

        //getbyid
        public async Task<FoodItem> GetByIdAsync(Guid id)
        {
            
            return await _context.FoodItems.Include(f => f.category).FirstOrDefaultAsync(c => c.Id == id);
        }

        //additem
        public async Task<string> AddItemAsync(FoodItem item)
        {
            
            await _context.FoodItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return " food item added";
        }

        public async Task<string> AddCategoryAsync(Categories category)
        {
            var existingcategory=await _context.Categories.FirstOrDefaultAsync(c=>c.categoryname== category.categoryname);
            if (existingcategory != null) {
                return "category with same name already exists.";
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return "Category added";
        }

        public async Task<List<FoodItem>> GetByProviderAsync(Guid providerId)
        {
            
             return await _context.FoodItems
                    .Include(f => f.category)
                    .Where(f=>f.providerid==providerId)
                    .ToListAsync();
        
        }

        public async Task<List<Categories>> GetAllCategory()
        {

            return await _context.Categories.ToListAsync();
        }


    } 
}
