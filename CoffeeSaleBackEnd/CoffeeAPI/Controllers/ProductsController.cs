using AutoMapper;
using CoffeeAPI.DTO;
using CoffeeAPI.Models;
using CoffeeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(int pageIndex = 1, int pageSize = 10, string status = null)
    {
        var products = await _productService.GetProductsAsync(pageIndex, pageSize, status);
        var productDtos = _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        return Ok(productDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductResponseDto>(product);
            return Ok(productDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        var productDtos = _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        return Ok(productDtos);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(string name)
    {
        var products = await _productService.SearchProductsByNameAsync(name);
        var productDtos = _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        return Ok(productDtos);
    }

    [HttpGet("bestseller")]
    public async Task<IActionResult> GetBestSeller()
    {
        var bestSeller = await _productService.GetBestSellerAsync();
        if (bestSeller == null)
        {
            return NotFound();
        }
        var productDto = _mapper.Map<ProductResponseDto>(bestSeller);
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequestDto productRequest)
    {
        try
        {
            var product = _mapper.Map<Product>(productRequest);
            var createdProduct = await _productService.CreateProductAsync(product);
            var productResponse = _mapper.Map<ProductResponseDto>(createdProduct);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, productResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequestDto productRequest)
    {
        try
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (id != existingProduct.ProductId)
            {
                return BadRequest("Product ID mismatch.");
            }

            _mapper.Map(productRequest, existingProduct);
            await _productService.UpdateProductAsync(existingProduct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateProductStatus(int id, [FromBody] string status)
    {
        try
        {
            await _productService.UpdateProductStatusAsync(id, status);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}