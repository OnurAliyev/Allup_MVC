using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_MiniProject_Allup.Models;

namespace MVC_MiniProject_Allup.DataAccesLayer;

public class AllupDbContext:IdentityDbContext<AppUser>
{
    public AllupDbContext(DbContextOptions<AllupDbContext> options) : base(options) { }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImages> ProductImages { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
}
