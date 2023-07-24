using E_commerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_commerce.Models;

namespace E_commerce.Data;

public class AppDbContext : IdentityDbContext<E_commerceUser>
{
    public DbSet<Products_ViewModel> ProductDb_Ecommerce{get; set;}
    public DbSet<Add_To_Cart_View_Model> CartDb_Ecommerce{get; set;}

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
