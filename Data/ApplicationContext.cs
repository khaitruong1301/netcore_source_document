using Microsoft.EntityFrameworkCore;
using netflixdemo.Models;

namespace netflixdemo.Data;

public class ApplicationContext:DbContext
{
    public ApplicationContext(DbContextOptions dbContextOptions):base(dbContextOptions)
    {
        
    }
    public DbSet<User> User{get;set;}
    public DbSet<Product> Product{get;set;}

}



