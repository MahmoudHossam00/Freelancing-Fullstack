using Freelancing.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllPaymentsController(IPaymentService _paymentService,ApplicationDbContext _context) : ControllerBase
    {
        [Authorize]
        [HttpGet("payments")]
        public async Task<ActionResult<List<PaymentDTO>>> GetUserPayments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();

            }

            var payments = await _paymentService.GetUserPaymentsAsync(userId);
            return Ok(payments);
        }

     

      
    }
}
