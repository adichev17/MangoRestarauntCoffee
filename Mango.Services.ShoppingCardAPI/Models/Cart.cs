using Mango.Services.ShoppingCardAPI.Models.Dto;

namespace Mango.Services.ShoppingCardAPI.Models
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}
