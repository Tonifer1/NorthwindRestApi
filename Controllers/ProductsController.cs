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
    // Määrittää reitin tämän API:n kutsuille, esimerkiksi "api/products"
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Dependency Injection: alustetaan  _context tyhjänä.
        private readonly NorthwindOriginalContext _context;

        // Tässä on Konstruktori, jossa otetaan dependency injectionin kautta NorthwindOriginalContext ja asetetaan _context-muuttujaan.
        //context on parametri. 
        public ProductsController(NorthwindOriginalContext context)
        {
            _context = context; // Tämän avulla voidaan tehdä tietokantakyselyt ja CRUD-operaatiot
        }

        // GET: api/Products
        // Tämä metodi hakee kaikki tuotteet tietokannasta.
        [HttpGet] // HTTP GET-pyyntö, joka hakee kaikki tuotteet.
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Jos Products-taulu on null (tietokantaa ei ole määritelty), palautetaan 404 NotFound-vastaus.
            if (_context.Products == null)
            {
                return NotFound(); // Ei löytynyt tuotteita
            }
            // Palauttaa kaikki tuotteet listana asynkronisesti tietokannasta.
            return await _context.Products.ToListAsync();
        }

        // Hakee tuotteita hinnan perusteella: /api/products/price/{price}
        [HttpGet("low/{price}")] // GET-pyyntö, joka hakee tuotteet annetun hinnan perusteella.
        public ActionResult GetByLowPrice(decimal price)
        {
            try
            {
                // Haetaan tuotteet, joiden UnitPrice on yhtä suuri tai alle annetun hinnan.
                var products = _context.Products.Where(p => p.UnitPrice <= price);

                // Palautetaan löydetyt tuotteet.
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Jos tapahtuu virhe, palautetaan BadRequest ja virheen viesti.
                return BadRequest(ex.Message);
            }
        }

        // Hakee tuotteita hinnan perusteella: /api/products/price/{price}
        [HttpGet("high/{price}")] // GET-pyyntö, joka hakee tuotteet annetun hinnan perusteella.
        public ActionResult GetByHighPrice(decimal price)
        {
            try
            {
                // Haetaan tuotteet, joiden UnitPrice on yhtä suuri tai yli annetun hinnan.
                var products = _context.Products.Where(p => p.UnitPrice >= price);

                // Palautetaan löydetyt tuotteet.
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Jos tapahtuu virhe, palautetaan BadRequest ja virheen viesti.
                return BadRequest(ex.Message);
            }
        }

        // Hakee tuotteita hinnan perusteella: /api/products/price/{price}
        [HttpGet("{minprice},{maxprice}")] // GET-pyyntö, joka hakee tuotteet annetun hinnan perusteella.
        public ActionResult GetByPrice(decimal minprice, decimal maxprice)
        {
            try
            {
                // Haetaan tuotteet, joiden UnitPrice on yhtä suuri tai yli annetun hinnan.
                var products = _context.Products.Where(p => p.UnitPrice >= minprice && p.UnitPrice <= maxprice);

                // Palautetaan löydetyt tuotteet.
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Jos tapahtuu virhe, palautetaan BadRequest ja virheen viesti.
                return BadRequest(ex.Message);
            }
        }



        //async tarkoittaa, että metodi on asynkroninen, eli se voi suorittaa operaatioita rinnakkain muiden tehtävien kanssa
        //odottamatta niiden valmistumista heti.
        //Task<ActionResult<Product>>:
        //Task: Tämä palauttaa asynkronisen tehtävän(task). Koska metodi on asynkroninen, sen palautusarvona on Task,
        //joka edustaa tehtävää, joka suoritetaan tulevaisuudessa.Task sisältää myös metodin varsinaisen palautusarvon, kun operaatio on valmis.
        //ActionResult<Product>: Tämä määrittää metodin palauttaman tyypin, kun asynkroninen tehtävä on valmis.


        // POST: api/Products
        // Lisää uuden tuotteen tietokantaan. 
        [HttpPost] // HTTP POST-pyyntö, joka luo uuden tuotteen
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Tarkistetaan, että Products-taulu on olemassa.
            if (_context.Products == null)
            {
                return Problem("Entity set 'NorthwindOriginalContext.Products' is null.");
            }
            // Lisätään uusi tuote tietokantaan.
            _context.Products.Add(product);
            // Tallennetaan muutokset tietokantaan.
            await _context.SaveChangesAsync();

            // Palautetaan vastaus, jossa kerrotaan, että tuote on luotu ja sisältää luodun tuotteen tiedot.
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // GET: api/Products/5
        // Hakee tuotteen tietokannasta sen id:n perusteella.
        [HttpGet("{id}")] // HTTP GET-pyyntö, joka hakee tuotteen sen id:n perusteella.
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Jos Products-taulu on null, palautetaan NotFound-vastaus.
            if (_context.Products == null)
            {
                return NotFound();
            }
            // Etsitään tuote tietokannasta sen id:n perusteella.
            var product = await _context.Products.FindAsync(id);

            // Jos tuotetta ei löydy, palautetaan NotFound-vastaus.
            if (product == null)
            {
                return NotFound();
            }

            // Palautetaan haettu tuote.
            return product;
        }

        // DELETE: api/Products/5
        // Poistaa tuotteen tietokannasta id:n perusteella.
        [HttpDelete("{id}")] // HTTP DELETE-pyyntö, joka poistaa tuotteen sen id:n perusteella.
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Tarkistetaan, että Products-taulu on olemassa.
            if (_context.Products == null)
            {
                return NotFound();
            }
            // Etsitään poistettava tuote id:n perusteella.
            var product = await _context.Products.FindAsync(id);
            // Jos tuotetta ei löydy, palautetaan NotFound-vastaus.
            if (product == null)
            {
                return NotFound();
            }

            // Poistetaan tuote tietokannasta.
            _context.Products.Remove(product);
            // Tallennetaan muutokset tietokantaan.
            await _context.SaveChangesAsync();

            // Palautetaan NoContent-vastaus, mikä tarkoittaa, että operaatio onnistui, mutta mitään sisältöä ei palauteta.
            return NoContent();
        }

        // PUT: api/Products/5
        // Päivittää olemassa olevan tuotteen tiedot.
        [HttpPut("{id}")] // HTTP PUT-pyyntö, joka päivittää tuotteen id:n perusteella.
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            // Tarkistetaan, että päivitettävän tuotteen id vastaa parametrina tullutta id:tä.
            if (id != product.ProductId)
            {
                return BadRequest(); // Id:t eivät täsmää, palautetaan virhevastaus.
            }

            // Merkitään tuote muutetuksi (Modified), jotta Entity Framework tietää päivittää sen.
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                // Tallennetaan muutokset tietokantaan.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Jos päivityksessä tapahtuu virhe (esim. tuote poistettu toisen käyttäjän toimesta), tarkistetaan, onko tuote yhä olemassa.
                if (!ProductExists(id))
                {
                    return NotFound(); // Jos tuotetta ei enää ole, palautetaan NotFound-vastaus.
                }
                else
                {
                    throw; // Muut virheet heitetään eteenpäin.
                }
            }

            // Päivitys onnistui, palautetaan NoContent-vastaus.
            return NoContent();
        }

        // Apumetodi, joka tarkistaa, onko tuote olemassa tietokannassa.
        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        // Hakee tuotteita nimen perusteella: /api/productname/hakusana
        [HttpGet("productname/{pname}")] // GET-pyyntö, joka hakee tuotteet osittaisen nimen perusteella.
        public ActionResult GetByName(string pname)
        {
            try
            {
                // Haetaan tuotteet, joiden nimi sisältää annetun hakusanan.
                var prod = _context.Products.Where(p => p.ProductName.Contains(pname));

                // Palautetaan löydetyt tuotteet.
                return Ok(prod);
            }
            catch (Exception ex)
            {
                // Jos tapahtuu virhe, palautetaan BadRequest ja virheen viesti.
                return BadRequest(ex.Message);
            }
        }
    }
}
