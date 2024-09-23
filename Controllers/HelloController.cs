using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//Tässä Hellocontroller perii ControllBase luokan ominaisuudet
namespace NorthwindRestApi.Controllers
{
    
    //C# puhutaan yleensä metodeista, koska ne kuuluvat luokkiin.Esimerkiksi sekä GetJSON että LaskeYhteen ovat metodeja,
    //koska ne ovat osa HelloController-luokkaa.

        //Haku tehdään /api/hello. Eli Controller sana jää pois
        [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        //GET = READ    Tähän voi ohjautua vain GET pyynnöt. Palauttaa stringin, eli Hello World!
        //Aina kun on return, niin se palauttaa jotain. Parametria ei ole tässä.
        [HttpGet]
        public string Get()
        {
            return "Hello World!";

        }
        //GET = READ    Tähän voi ohjautua vain GET pyynnöt. Palauttaa JSON muodossa, eli Hello World!Ei parametria.
        [HttpGet("json")]
        public IActionResult GetJSON()
        {
            var response = new { Message = "Hello World!" };
            return Ok(response); // Tämä palauttaa JSON-muodossa
        }

        //POST = CREATE    Tähän voi ohjautua vain POST pyynnöt.Palautta int muotoisen datan.Tässä on kaksi parametria.
        // Tämä metodi ottaa kaksi parametria, a ja b, jotka ovat kokonaislukuja(int).
        // Nämä parametrit tulevat POST-pyynnön mukana (esimerkiksi pyynnön rungossa, body).

        [HttpPost]
        public int LaskeYhteen(int a,int b)
        {
            return a + b;
        }

        
    }
}
