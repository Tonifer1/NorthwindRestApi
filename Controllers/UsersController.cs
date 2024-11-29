using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;
using NuGet.Protocol.Plugins;

namespace NorthwindRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NorthwindOriginalContext _context;

        public UsersController(NorthwindOriginalContext context)
        {
            _context = context;
        }

        //Get All
        // GET: api/Users
        // IEnumerable<User> tarkoittaa, että palautetaan kokoelma User-objekteja.
        //if (_context.Users == null) varmistaa, että sovelluksella on pääsy Users-tauluun NorthwindOriginalContext-kontekstin kautta
        //ActionResult<IEnumerable<User>>: Metodi palauttaa ActionResult-tyypin,joka mahdollistaa HTTP-vastauksen hallinnan.

        //User.Claims on lista käyttäjän claims-tiedoista. Claim on yksittäinen väite (esimerkiksi tunnistautuminen tai rooli), joka liittyy käyttäjään.
        //FirstOrDefault hakee ensimmäisen claimin, joka vastaa ehtoa Type == "acceslevelId" (eli käyttäjän accesslevelId).
        //?.Value: Tämä tarkoittaa, että jos kyseinen claim löytyy, sen Value (arvo) otetaan. Jos claim ei löydy, palautetaan null.
        //int.TryParse yrittää muuntaa accesLevelIdClaim-arvon kokonaisluvuksi (int).
        //Jos accesLevelIdClaim on kelvollinen kokonaisluku ja se on 2, jatketaan suorittamista.
        //Jos accesLevelIdClaim Ei ole kelvollinen luku, tai se ei ole 2, päädytään else-osioon
        //Jos käyttäjä on tasolla 2, käydään läpi kaikki käyttäjät tietokannasta ja asetetaan heidän Password-kenttänsä null-arvoksi,
        //jotta salasana ei paljastu.
        //return await _context.Users.ToListAsync(): Palautetaan kaikkien käyttäjien lista tietokannasta (lukuoperaatio) asynkronisesti.

        //var accesLevelIdClaim = User.Claims.FirstOrDefault(c => c.Type == "acceslevelId")?.Value;
        //Tämä rivi hakee User.Claims-kokoelmasta claimin, jonka Type on "acceslevelId".
        //Jos tällaista claimia ei löydy, palautetaan null
        //?.Value palauttaa itse claimin arvon string-muodossa, jos claim löytyy. Muuten palautetaan null.
        //int accesLevelId; Tämä on muuttuja, johon yritetään muuntaa accesLevelIdClaim-arvo kokonaisluvuksi.



        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            Console.WriteLine("GetUsers method called.");
            if (_context.Users == null)
            {
              return NotFound();
            }

            var accesLevelIdClaim = User.Claims.FirstOrDefault(c => c.Type == "acceslevelId")?.Value;
            int accesLevelId;

            if (int.TryParse(accesLevelIdClaim, out accesLevelId) && accesLevelId == 2)
            {
                // Käyttäjä on tasolla 2
                foreach (var user in _context.Users)
                {
                    user.Password = null;
                }
                return await _context.Users.ToListAsync();

            }
            else
            {
                return Unauthorized("Access denied: Only level 2 users are allowed.");
            }
        

        }

        //Get by Id
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var accesLevelIdClaim = User.Claims.FirstOrDefault(c => c.Type == "acceslevelId")?.Value;
            int accesLevelId;
            if (!(int.TryParse(accesLevelIdClaim, out accesLevelId) && accesLevelId == 2))
            {
                return Unauthorized("Access denied: Only level 2 users are allowed.");
            }

            if (_context.Users == null)
            {
              return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //Update by Id
        // PUT: api/Users/5
        //Apumuuttujaa UserExists käytetään:
        //Tietokannan eheyden takaamiseksi, jotta ei tehdä muutoksia olemattomaan tietueeseen.
        //Concurrency-tilanteiden hallitsemiseksi, koska päivitysoperaatiossa on mahdollista, että tietuetta muokataan samanaikaisesti.

        private bool UserExists(int id)
        {
            return _context.Users?.Any(e => e.UserId == id) ?? false;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            var accesLevelIdClaim = User.Claims.FirstOrDefault(c => c.Type == "acceslevelId")?.Value;
            int accesLevelId;
            if (!(int.TryParse(accesLevelIdClaim, out accesLevelId) && accesLevelId == 2))
            {
                return Unauthorized("Access denied: Only level 2 users are allowed.");
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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


        //Create
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var accesLevelIdClaim = User.Claims.FirstOrDefault(c => c.Type == "acceslevelId")?.Value;
            int accesLevelId;
            if (!(int.TryParse(accesLevelIdClaim, out accesLevelId) && accesLevelId == 2))
            {
                return Unauthorized("Access denied: Only level 2 users are allowed.");
            }

            if (_context.Users == null)
            {
                return Problem("Entity set 'NorthwindOriginalContext.Users' is null.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        //Delete by Id
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var accesLevelIdClaim = User.Claims.FirstOrDefault(c => c.Type == "acceslevelId")?.Value;
            int accesLevelId;
            if (!(int.TryParse(accesLevelIdClaim, out accesLevelId) && accesLevelId == 2))
            {
                return Unauthorized("Access denied: Only level 2 users are allowed.");
            }

            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
