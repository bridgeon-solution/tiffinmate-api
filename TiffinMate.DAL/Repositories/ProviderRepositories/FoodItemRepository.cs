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

            return await _context.FoodItems.Include(f => f.category).Include(f => f.menu).ToListAsync();
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
            var existingcategory=await _context.Categories.FirstOrDefaultAsync(c=>c.category_name== category.category_name);
            if (existingcategory != null) {
                return "category with same name already exists.";
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return "Category added";
        }

        public async Task<List<FoodItem>> GetByProviderAsync(Guid providerId, Guid? menuId)
        {

            var items = _context.FoodItems
              .Include(f => f.category)
              .Include(c => c.menu)
              .Where(f => f.provider_id == providerId);
                  

            if (menuId != null)
            {
                items = items.Where(f => f.menu_id == menuId);

            }
            return await items.ToListAsync();
            
        
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

        public async Task<string> AddMenuAsync(Menu menus)
        {
            var existingcategory = await _context.menus.FirstOrDefaultAsync(c => c.name == menus.name);
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
        public async Task<decimal> GetTotalAmount(List<Guid> categoryIds, string day)
        {
            return await _context.FoodItems
                .Where(item => categoryIds.Contains(item.category_id) && item.day == day)
                .SumAsync(item => item.price);
        }

        





    } 
}
