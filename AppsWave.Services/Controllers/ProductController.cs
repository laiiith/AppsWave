using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppsWave.Services.Controllers
{
    [ApiController]
    [Route("api/product")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
