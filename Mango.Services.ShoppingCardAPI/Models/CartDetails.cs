 using Mango.Services.ShoppingCardAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCardAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int? CartHeaderId { get; set; } = null;

        [ForeignKey("CartHeaderId")]
        public virtual CartHeader? CartHeader { get; set; } = null;
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int Count { get; set; }
    }
}
