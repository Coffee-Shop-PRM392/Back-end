using CoffeeAPI.Models;
using CoffeeAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using CoffeeAPI.DTO;

namespace CoffeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;

        public OrderItemsController(IOrderItemService orderItemService, IMapper mapper)
        {
            _orderItemService = orderItemService;
            _mapper = mapper;
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderItemsByOrder(int orderId)
        {
            var orderItems = await _orderItemService.GetOrderItemsByOrderAsync(orderId);
            var orderItemDtos = _mapper.Map<IEnumerable<OrderItemResponseDto>>(orderItems);
            return Ok(orderItemDtos);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetOrderItemsByProduct(int productId)
        {
            var orderItems = await _orderItemService.GetOrderItemsByProductAsync(productId);
            var orderItemDtos = _mapper.Map<IEnumerable<OrderItemResponseDto>>(orderItems);
            return Ok(orderItemDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemRequestDto orderItemRequest)
        {
            try
            {
                var orderItem = _mapper.Map<OrderItem>(orderItemRequest);
                var createdOrderItem = await _orderItemService.CreateOrderItemAsync(orderItem);
                var orderItemResponse = _mapper.Map<OrderItemResponseDto>(createdOrderItem);
                return CreatedAtAction(nameof(GetOrderItemsByOrder), new { orderId = createdOrderItem.OrderId }, orderItemResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            try
            {
                await _orderItemService.DeleteOrderItemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}