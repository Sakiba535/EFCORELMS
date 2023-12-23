using Microsoft.EntityFrameworkCore;

namespace WebApplication1LMS.Models
{
    public class LibraryContext:DbContext
    {
        public DbSet<Student> students { get; set; }
        public DbSet<Book> books { get; set; }
        public LibraryContext(DbContextOptions options):base(options) 
        { 
            
        }
    }
}
