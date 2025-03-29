using CoffeeAPI.Models;
using CoffeeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using CoffeeAPI.DTO;

namespace CoffeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartsController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCartByUser(int userId)
        {
            var cartItems = await _cartService.GetCartByUserAsync(userId);
            var cartDtos = _mapper.Map<IEnumerable<CartResponseDto>>(cartItems);
            return Ok(cartDtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartRequestDto cartRequest)
        {
            try
            {
                var cart = _mapper.Map<Cart>(cartRequest);
                var addedCartItem = await _cartService.AddToCartAsync(cart);
                var cartResponse = _mapper.Map<CartResponseDto>(addedCartItem);
                return CreatedAtAction(nameof(GetCartByUser), new { userId = addedCartItem.UserId }, cartResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{cartId}/quantity")]
        public async Task<IActionResult> UpdateCartQuantity(int cartId, [FromBody] int quantity)
        {
            try
            {
                var updatedCart = await _cartService.UpdateCartQuantityAsync(cartId, quantity);
                var cartResponse = _mapper.Map<CartResponseDto>(updatedCart);
                return Ok(cartResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(cartId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpPut("{cartId}/update")]
        public async Task<IActionResult> UpdateCartItem(int cartId, [FromBody] UpdateCartDto updateCartDto)
        {
            try
            {
                var updatedCart = await _cartService.UpdateCartItemAsync(
                    cartId,
                    updateCartDto.SelectedSize,
                    updateCartDto.IceLevel,
                    updateCartDto.SugarLevel,
                    updateCartDto.Quantity
                );

                var cartResponse = _mapper.Map<CartResponseDto>(updatedCart);
                return Ok(cartResponse);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Cart item not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}