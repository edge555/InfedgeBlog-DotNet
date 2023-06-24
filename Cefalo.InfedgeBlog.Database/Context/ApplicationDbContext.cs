﻿using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.InfedgeBlog.Database.Context
{
    public  class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Story> Stories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
