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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                var categoryDto = _mapper.Map<CategoryResponseDto>(category);
                return Ok(categoryDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var category = await _categoryService.GetCategoryByNameAsync(name);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryResponseDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto categoryRequest)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryRequest);
                var createdCategory = await _categoryService.CreateCategoryAsync(category);
                var categoryResponse = _mapper.Map<CategoryResponseDto>(createdCategory);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, categoryResponse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDto categoryRequest)
        {
            try
            {
                var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound();
                }

                if (id != existingCategory.CategoryId)
                {
                    return BadRequest("Category ID mismatch.");
                }

                _mapper.Map(categoryRequest, existingCategory);
                await _categoryService.UpdateCategoryAsync(existingCategory);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}