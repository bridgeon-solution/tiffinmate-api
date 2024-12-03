using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TiffinMate.DAL.DbContexts
{
    public class AppDbContext:DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
    }
}
