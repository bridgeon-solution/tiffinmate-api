using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;


namespace TiffinMate.DAL.DbContexts
{
    public class AppDbContext:DbContext
    {
       
        public DbSet<ApiLog> ApiLogs { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
    }
}
