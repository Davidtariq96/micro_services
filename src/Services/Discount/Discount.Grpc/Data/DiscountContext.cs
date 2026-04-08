using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext :DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;

    public DiscountContext(DbContextOptions<DiscountContext> options)
        :base(options)
    {
        
    }
//Seeding the SQLite database with initial data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductName = "Iphone XR",Description = "A brand new phone in town",Amount = 30},
            new Coupon { Id = 2, ProductName = "Iphone 11Pro max",Description = "A brand new phone in town",Amount = 20}
                );
    }
}