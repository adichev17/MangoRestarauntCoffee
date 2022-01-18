using Mango.Services.ShoppingCardAPI.Models.Dto;

namespace Mango.Services.ShoppingCardAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserId(string userId);
        Task<CartDto> CreateUpdateAsync(CartDto cartDto);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ApplyCoupon(string UserId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
