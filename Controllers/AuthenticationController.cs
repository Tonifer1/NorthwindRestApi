using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Services.Interfaces;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //Alustetaan tyhjänä _authenticateService
        private IAuthenticateService _authenticateService;

        //Dependency Injection
        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        // Tähän tulee Front endin kirjautumisyritys
        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukse)
        {            
            var loggedUser = _authenticateService.Authenticate(tunnukse.Username ?? "", tunnukse.Password ?? "");

            if (loggedUser != null)
            {
                return Ok(loggedUser);// Palautus front endiin (sis. vain loggedUser luokan mukaiset kentät)
            }
                return BadRequest("Käyttäjätunnus tai salasana on virheellinen");            
        }
    }
}
