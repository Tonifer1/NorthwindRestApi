using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;
using System;
using System.Threading.Tasks;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        private readonly NorthwindOriginalContext _context;
        private const string KeyCode = "12345"; //  Avainkoodi

        public DocumentationController(NorthwindOriginalContext context)
        {
            _context = context;
        }

        [HttpGet("{keycode}")]
        public async Task<IActionResult> GetDocumentation(string keycode)
        {
            if (keycode == KeyCode)
            {
                var documentation = await _context.Documentations.ToListAsync();
                return Ok(documentation);
            }
            else
            {
                var result = new
                {
                    Date = DateTime.UtcNow,
                    Message = "Documentation missing"
                };
                return NotFound(result);
            }
        }
    }
}
