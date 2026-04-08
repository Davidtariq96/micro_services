using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

//Inherit from GRPC class in other to call the defined methods
public class DiscountService (DiscountContext discountDbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon =await discountDbContext
            .Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        if (coupon is null)
            coupon = new Coupon {ProductName ="No Discount",  Amount = 0, Description =  "No Description"};
        
        logger.LogInformation("Product is retrived for product name {ProductName} Amount{Amount}", coupon.ProductName, coupon.Amount);
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
      
        var coupon = request.Coupon?.Adapt<Coupon>();
        
        if (coupon is null)
           throw new RpcException (new Status (StatusCode.InvalidArgument,"Invalid request object"));
        
        discountDbContext.Coupons.Add(coupon);
        await discountDbContext.SaveChangesAsync();
        logger.LogInformation("Discount created successfully Product name {ProductName} Amount{Amount}", coupon.ProductName, coupon.Amount);
        
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon?.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException (new Status (StatusCode.InvalidArgument,"Invalid request object"));
        discountDbContext.Coupons.Update(coupon);
        await discountDbContext.SaveChangesAsync();
        logger.LogInformation("Discount updated successfully Product name {ProductName} Amount{Amount}", coupon.ProductName, coupon.Amount);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountDbContext.Coupons.FirstOrDefaultAsync(x =>
            x.ProductName == request.ProductName);
        if(coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
        
        discountDbContext.Coupons.Remove(coupon);
        await discountDbContext.SaveChangesAsync();
        logger.LogInformation("Discount deleted successfully Product name {ProductName} Amount{Amount}", coupon.ProductName, coupon.Amount);
        return new DeleteDiscountResponse {Success = true};

    }
}