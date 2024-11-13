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

        //if (_context.Users == null) varmistaa, että sovelluksella on pääsy Users-tauluun NorthwindOriginalContext-kontekstin kautta
        //Get All
        // GET: api/Users
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
            Console.WriteLine($"Claims: {string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"))}");
            Console.WriteLine($"accesLevelIdClaim: {accesLevelIdClaim}");

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
