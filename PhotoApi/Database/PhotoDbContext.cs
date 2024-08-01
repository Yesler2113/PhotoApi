using Microsoft.EntityFrameworkCore;
using PhotoApi.entities;

namespace PhotoApi.Database
{
    public class PhotoDbContext : DbContext
    {
        public PhotoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PhotoEntity> Photos { get; set; }
    
    }  
}

