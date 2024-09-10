using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustetaan tietokantayhteys
        NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Hakee kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAllCustomers() 
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " +e.InnerException);
            }

        }

        [HttpGet("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);                
                return Ok(asiakas);
            }

            catch (Exception e)

            {
              //return BadRequest("Asiakasta id:llä" + id "ei löydy");
              return BadRequest($"Asiakasta id:llä {id} ei löydy. Lue lisää: "+e.InnerException);//string interpolation
            }

        }

     

    }
}
