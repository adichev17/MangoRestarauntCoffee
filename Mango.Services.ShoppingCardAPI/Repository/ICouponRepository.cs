using Mango.Services.ShoppingCardAPI.Models.Dto;

namespace Mango.Services.ShoppingCardAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
