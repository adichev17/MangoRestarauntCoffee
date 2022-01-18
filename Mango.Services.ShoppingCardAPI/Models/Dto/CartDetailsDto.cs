using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCardAPI.Models.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public virtual CartHeaderDto? CartHeader { get; set; } = null;
        public int ProductId { get; set; }
        public virtual ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
