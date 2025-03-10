﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using static Supabase.Gotrue.Constants;

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

            var query = _context.FoodItems.Include(f => f.category).Include(f => f.menu);

            return await query.ToListAsync();
        }

        public async Task<List<FoodItem>> GetByMenu(Guid? menuId, Guid? category_id)
        {
            return await _context.FoodItems
                .Include(f => f.category)
                .Include(f => f.menu)
                .Where(f => f.menu_id == menuId && f.category_id == category_id)
                .ToListAsync();
        }


        //getbyid
        public async Task<FoodItem> GetByIdAsync(Guid id)
        {
            
            return await _context.FoodItems.Include(f => f.category).FirstOrDefaultAsync(c => c.id == id);
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
            try
            {
                var existingcategory = await _context.Categories.FirstOrDefaultAsync(c => c.category_name == category.category_name);
                if (existingcategory != null)
                {
                    return "category with same name already exists.";
                }
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return "Category added";
            }
            catch(Exception ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }

        public async Task<List<FoodItem>> GetByProviderAsync(Guid providerId)
        {

            return await _context.FoodItems
              .Include(f => f.category)
              .Include(c => c.menu)
              .Where(f => f.provider_id == providerId).ToListAsync();       
        
        }

        public async Task<List<Categories>> GetAllCategory()
        {

            return await _context.Categories.ToListAsync();
        }


        public async Task<List<Menu>> GetAllMenuAsync(Guid?providerId)
        {
            if (providerId != null)
            {
                return await _context.menus.Where(e=>e.provider_id == providerId).ToListAsync();
            }
            else
            {
                return await _context.menus.ToListAsync();
            }
        }
        //add menu
        public async Task<string> AddMenuAsync(Menu menus)
        {
            var existingcategory = await _context.menus.FirstOrDefaultAsync(c => c.name == menus.name && c.provider_id==menus.provider_id);
            if (existingcategory != null)
            {
                return "menu with same name already exists.";
            }
            await _context.menus.AddAsync(menus);
            await _context.SaveChangesAsync();
            return "menu added";
        }

        public async Task<List<Menu>> GetMenuByProviderAsync(Guid providerId)
        {

            return await _context.menus
                   .Where(f => f.provider_id == providerId)
                   .ToListAsync();

        }
        public async Task<decimal> GetTotalAmount(Guid menuId, List<Guid> categoryIds, string day)
        {
            return await _context.FoodItems.Where(item => item.menu_id == menuId && categoryIds.Contains(item.category_id) && item.day == day)
                .SumAsync(item => item.price);
        }
        public async Task<decimal> GetMonthlyTotalAmount(Guid menuId)
        {
            return await _context.menus.Where(m => m.id == menuId).Select(m => m.monthly_plan_amount).FirstOrDefaultAsync();

        }

        public async Task<List<FoodItem>> GetAllFoodItem(Guid? menuId, List<Guid> category_id)
        {
            return await _context.FoodItems
                .Include(f => f.category)
                .Include(f => f.menu)
                .Where(f => f.menu_id == menuId && category_id.Contains(f.category_id))
                .ToListAsync();
        }
        public async Task <bool>DeleteMenu(Guid id)
        {
           var menu= await _context.menus.FirstOrDefaultAsync(u => u.id == id);
             _context.menus.Remove(menu);
            _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteFooditem(Guid id)
        {
            var FoodItems = await _context.FoodItems.FirstOrDefaultAsync(u => u.id == id);
            _context.FoodItems.Remove(FoodItems);
            _context.SaveChangesAsync();
            return true;
        }
    } 
}
