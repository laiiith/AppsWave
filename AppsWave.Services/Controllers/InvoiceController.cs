using AppsWave.DTO;
using AppsWave.Entites;
using AppsWave.Services.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppsWave.Services.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ResponseDTO _response;

        public InvoiceController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _response = new ResponseDTO();
        }

        [HttpPost("purchase")]
        public async Task<ActionResult<ResponseDTO>> Purchase([FromBody] PurchaseRequestDTO request)
        {
            if (request?.Items == null || !request.Items.Any())
                return BadRequest("Cart is empty.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var invoice = new Invoice
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                Details = new List<InvoiceDetail>()
            };

            foreach (var item in request.Items)
            {
                var product = await _unitOfWork.Products.GetAsync(p =>
                    p.ProductId == item.ProductId && !p.IsDeleted);

                if (product == null)
                    return BadRequest($"Product with ID {item.ProductId} not found or deleted.");

                if (item.Quantity <= 0)
                    return BadRequest($"Invalid quantity for product {item.ProductId}.");

                invoice.Details.Add(new InvoiceDetail
                {
                    ProductId = product.ProductId,
                    Price = product.Price,
                    Product = product,
                    Quantity = item.Quantity
                });
            }

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveAsync();

            var responseDto = new InvoiceResponseDTO
            {
                InvoiceId = invoice.InvoiceId,
                Date = invoice.Date,
                UserName = user?.FullName ?? "User",
                TotalAmount = invoice.TotalAmount,
                Items = invoice.Details.Select(d => new InvoiceItemDTO
                {
                    ProductEnglishName = d.Product?.EnglishName ?? "Unknown",
                    ProductArabicName = d.Product?.ArabicName ?? "غير معروف",
                    Price = d.Price,
                    Quantity = d.Quantity
                }).ToList()
            };

            _response.IsSuccess = true;
            _response.Result = responseDto;
            _response.Message = "Purchase completed successfully!";

            return Ok(_response);
        }

        [HttpGet("my-invoices")]
        public async Task<ActionResult<ResponseDTO>> GetMyInvoices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var invoices = await _unitOfWork.Invoices.GetUserInvoicesAsync(userId);

            var result = invoices.Select(i => new InvoiceResponseDTO
            {
                InvoiceId = i.InvoiceId,
                Date = i.Date,
                TotalAmount = i.TotalAmount,
                Items = i.Details.Select(d => new InvoiceItemDTO
                {
                    ProductEnglishName = d.Product.EnglishName,
                    ProductArabicName = d.Product.ArabicName,
                    Price = d.Price,
                    Quantity = d.Quantity
                }).ToList()
            }).ToList();

            _response.IsSuccess = true;
            _response.Result = result;
            return Ok(_response);
        }
    }
}
