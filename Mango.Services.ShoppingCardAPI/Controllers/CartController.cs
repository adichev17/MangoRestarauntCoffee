using Mango.Services.ShoppingCardAPI.Messages;
using Mango.Services.ShoppingCardAPI.Models.Dto;
using Mango.Services.ShoppingCardAPI.Repository;
using MangoMessageBus;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCardAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMessageBus _messageBus;
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto _responseDto;

        public CartController(ICartRepository cartRepository, IMessageBus messageBus, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _messageBus = messageBus;
            _couponRepository = couponRepository;
            this._responseDto = new ResponseDto();  
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserId(userId);
                _responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString()  };
            }
            return _responseDto;
        }

        [HttpPost("AddCart")]
        public async Task<ResponseDto> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartRepository.CreateUpdateAsync(cartDto);

                _responseDto.Result = cartDt;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }

        [HttpPost("UpdateCart")]
        public async Task<ResponseDto> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartRepository.CreateUpdateAsync(cartDto);

                _responseDto.Result = cartDt;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }


        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);

                _responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }

        [HttpPost("ClearCart")]
        public async Task<ResponseDto> ClearCart([FromBody] string userId)
        {
            try
            {
                bool isSuccess = await _cartRepository.ClearCart(userId);

                _responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }


        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                bool isSuccess = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode); 

                _responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveCoupon(userId);

                _responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }


        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDto checkoutHeader)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserId(checkoutHeader.UserId);
                if (cartDto == null)
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
                {
                    CouponDto coupon = await _couponRepository.GetCoupon(checkoutHeader.CouponCode);
                    if (checkoutHeader.DiscountTotal != coupon.DiscountAmount)
                    {
                        _responseDto.IsSuccess = false;
                        _responseDto.ErrorMessages = new List<string>() { "Coupon Price has changed, please confirm" };
                        _responseDto.DisplayMessage = "Coupon Price has changed, please confirm";
                        return _responseDto;
                    }
                }

                checkoutHeader.CartDetails = cartDto.CartDetails;

                //logic to add message to process order.
                await _messageBus.PublishMessage(checkoutHeader, "checkoutqueue");
                await _cartRepository.ClearCart(checkoutHeader.UserId);

                ////rabbitMQ
                //_rabbitMQCartMessageSender.SendMessage(checkoutHeader, "checkoutqueue");
             //   await _cartRepository.ClearCart(checkoutHeader.UserId);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }

    }
}
