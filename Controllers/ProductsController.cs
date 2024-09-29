using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Dependency injektion tapa. Alustetaan tyhjänä _context.
        private readonly NorthwindOriginalContext _context;

        //Metodi on nimetty Products Controllerin mukaan.
        public ProductsController(NorthwindOriginalContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]//GET = READ Hakee kaikki tuotteet. Ei parametreja.
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]//POST = CREATE Lisää uuden tuotteen.
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'NorthwindOriginalContext.Products'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]//GET = READ Hakee Id:n perusteella tuotteen.
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]//DELETE = DELETE Poistaa tuotteen.
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]//PUT = UPDATE Päivittää tuotteen.
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        

       

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

       

        // Hakee nimen osalla tuotteen: /api/productname/hakusana
        //GET = READ Hakee nimen osalla tuotteen.
        [HttpGet("productname/{pname}")]
        public ActionResult GetByName(string pname)
        {
            try
            {
                var prod = _context.Products.Where(p => p.ProductName.Contains(pname));

                //var prod = from p in _context.Product where p.ProductName.Contains(pname) select p; //<-- sama mutta traditional


                // var cust = _context.Customers.Where(c => c.CompanyName == cname);// <--- perfect match

                return Ok(prod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
