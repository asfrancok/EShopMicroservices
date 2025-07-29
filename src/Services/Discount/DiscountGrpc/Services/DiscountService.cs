using DiscountGrpc.Data;
using DiscountGrpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DiscountGrpc.Services
{
    public class DiscountService(DiscountContext _dbContext, ILogger<DiscountService> _logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if(coupon is null) 
                coupon = new Models.Coupon { ProductName = "No discount", Amount = 0, Description = "No discount" };

            _logger.LogInformation("Discoun is retrieved for ProductName:{ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Model.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            _dbContext.Coupons.Add(coupon);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully created. ProductName:{ProductName}, Amount:{Amount}", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Model.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            _dbContext.Coupons.Update(coupon);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully updated. ProductName:{ProductName}, Amount:{Amount}", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();
        }

        public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await  _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} not found."));

            _dbContext.Coupons.Remove(coupon);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully deleted. ProductName={ProductName}", request.ProductName);

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
