using AppsWave.DTO;
using AppsWave.Entites;
using AppsWave.Services.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppsWave.Services.Controllers
{
    [ApiController]
    [Route("api/product")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDTO _response = new();

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseDTO>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var products = await _unitOfWork.Products.GetAllAsync(
                filter: p => !p.IsDeleted,
                orderBy: q => q.OrderBy(p => p.EnglishName),
                skip: (page - 1) * pageSize,
                take: pageSize
            );

            var dtos = products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                EnglishName = p.EnglishName,
                ArabicName = p.ArabicName,
                Price = p.Price
            }).ToList();

            _response.IsSuccess = true;
            _response.Result = dtos;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> GetProduct(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(p => p.ProductId == id && !p.IsDeleted);

            if (product == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Product not found.";
                return NotFound(_response);
            }

            _response.Result = new ProductDTO
            {
                ProductId = product.ProductId,
                EnglishName = product.EnglishName,
                ArabicName = product.ArabicName,
                Price = product.Price
            };

            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDTO>> CreateProduct([FromBody] UpsertProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.Message = "Validation failed.";
                return BadRequest(_response);
            }

            var exists = await _unitOfWork.Products.GetAsync(p =>
                p.EnglishName.Trim().ToLower() == dto.EnglishName.Trim().ToLower() && !p.IsDeleted);

            if (exists != null)
            {
                _response.IsSuccess = false;
                _response.Message = "Product with this English name already exists.";
                return BadRequest(_response);
            }

            var product = new Product
            {
                EnglishName = dto.EnglishName.Trim(),
                ArabicName = dto.ArabicName.Trim(),
                Price = dto.Price
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();

            var createdDto = new ProductDTO
            {
                ProductId = product.ProductId,
                EnglishName = product.EnglishName,
                ArabicName = product.ArabicName,
                Price = product.Price
            };

            _response.Result = createdDto;
            _response.Message = "Product created successfully.";

            return CreatedAtRoute("GetProduct", new { id = product.ProductId }, _response);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpsertProductDTO dto)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest("Invalid data.");

            var product = await _unitOfWork.Products.GetAsync(p => p.ProductId == id && !p.IsDeleted, tracked: true);
            if (product == null)
                return NotFound("Product not found.");

            // Prevent duplicate English name (excluding current product)
            var duplicate = await _unitOfWork.Products.GetAsync(p =>
                p.ProductId != id &&
                p.EnglishName.Trim().ToLower() == dto.EnglishName.Trim().ToLower() &&
                !p.IsDeleted);

            if (duplicate != null)
                return BadRequest("Another product with this English name already exists.");

            product.EnglishName = dto.EnglishName.Trim();
            product.ArabicName = dto.ArabicName.Trim();
            product.Price = dto.Price;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDTO>> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(p => p.ProductId == id && !p.IsDeleted, tracked: true);
            if (product == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Product not found or already deleted.";
                return NotFound(_response);
            }

            product.IsDeleted = true;
            await _unitOfWork.SaveAsync();

            _response.IsSuccess = true;
            _response.Message = "Product deleted successfully (soft delete).";
            return Ok(_response);
        }
    }
}
