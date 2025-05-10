using Microsoft.EntityFrameworkCore;
using BookApi.Domain.Entities;




namespace BookApi.Infrastructure.Data

{
    //the main DB context for entity framework  and the constructor for config (like db connection string)
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        //dbset properties for each model
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}