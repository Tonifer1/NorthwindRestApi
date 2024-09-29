using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Linq.Expressions;
namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustetaan tietokantayhteys. Perinteinen tapa
        //NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Dependency injektion tapa. Alustetaan tyhjänä db.
        private readonly NorthwindOriginalContext db;

        //Metodi on nimetty Controllerin mukaan. 
        public CustomersController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }


        //GET = READ Hakee kaikki asiakkaat. Ei parametreja.
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
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }

        }

        //Hakee asiakkaan pääavaimella.Tässä tapauksessa string id,
        //koska Northwind tietokannassa asiakkaan pääavain on merkkijono
        //e virheenkäsittelyssä Exeption e = e on oma luotu muuttuja
        //GET = READ Hakee Id:n perusteella asiakkaan.
        [HttpGet("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                // Yritetään hakea asiakas tietokannasta
                var asiakas = db.Customers.Find(id);

                // Tarkistetaan löytyykö asiakas
                if (asiakas == null)
                {
                    return NotFound($"Asiakasta id:llä {id} ei löytynyt.");
                }

                // Jos asiakas löytyy, palautetaan se
                return Ok(asiakas);
            }
            catch (Exception e)
            {
                // Jos tapahtuu virhe, palautetaan virheilmoitus
                return BadRequest($"Tapahtui virhe haettaessa asiakasta id:llä {id}. Lisätietoja: {e.Message}");
            }


        }

        //Uuden asiakkaan lisääminen. customer on alias nimi
        //POST = CREATE Lisää uuden asiakkaan. Ei parametreja.
        [HttpPost]
        public ActionResult AddNewCustomer([FromBody] Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            //Tämä on front endille näytettävä viesti
            return Ok($"Lisätty uusi asiakas: {customer.CompanyName}");
        }

        //Poistaminen url parametrina annettavan  id:n perusteella. Huom! string muoto
        //DELETE = DELETE Poistaa asiakkaan id:n perusteella. 
        [HttpDelete("{id}")]
        public ActionResult DeleteOneCustomerById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas == null)
                {
                    //return NotFound("Asikasta id:ll" + id + "ei löytynyt");
                    return NotFound($"Asikasta id:llä  {id}  ei löytynyt");
                }
                else
                {
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    return Ok($"Poistettiin {asiakas.CompanyName} poistettu");
                }

            }
            catch (Exception e)
            {
              //return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
                return BadRequest($"Tapahtui virhe. Lue lisää:  + {e.Message}");
            }



            //try
            //{
            //    var asiakas = db.Customers.Find(id);
            //  //var asiakas = db.Customers.Where(c=>c.CustomerID == id);
            //    return Ok(asiakas);
            //}

            //catch (Exception e)

            //{
            // return BadRequest("Asiakasta id:llä" + id "ei löydy");
            // return BadRequest($"Asiakasta id:llä {id} ei löydy. Lue lisää: "+e.InnerException);//string interpolation
            //}


        }

        //Asiakkaan tietojen muokkaaminen
        //ottaa vastaan kaksi parametria:Urlista id(string) ja customer objekti http bodyn osasta.
        //From body tarkoittaa kaikkia asiakkaan tietoja
        //Haetaan id:N perusteella vanha asiakasobjekti
        //PUT= UPDATE Päivittää asiakkaan tiedot id:n perusteella.
        //Hakee asiakkaan pääavaimella.Tässä tapauksessa string id.
        [HttpPut("{id}")]
        public ActionResult EditCustomer(string id,[FromBody] Customer customer)
        {
             var asiakas = db.Customers.Find(id);
             if (asiakas != null)
             {
                //em. asiakasobjektiin sulautetaann parametrina saadut asiakkaan tiedot
                    asiakas.CompanyName = customer.CompanyName;
                    asiakas.ContactName = customer.ContactName;
                    asiakas.Address = customer.Address;
                    asiakas.City = customer.City;
                    asiakas.Region = customer.Region;
                    asiakas.PostalCode = customer.PostalCode;
                    asiakas.Country = customer.Country;
                    asiakas.Phone = customer.Phone;
                    asiakas.Fax = customer.Fax;
                    db.Customers.Update(asiakas); // Päivitä muokattu asiakasobjekti
                    db.SaveChanges();
                    return Ok($"Asiakkaan {asiakas.CompanyName} tiedot päivitetty");

             }
            return NotFound($"Asiakasta id:llä {id} ei löytynyt");
        }

        // Hakee nimen osalla: /api/companyname/hakusana
        //GET = READ Hakee nimen osalla asiakkaan.
        [HttpGet("companyname/{cname}")]
        public ActionResult GetByName(string cname)
        {
            try
            {
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname));

                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; //<-- sama mutta traditional


                // var cust = db.Customers.Where(c => c.CompanyName == cname);// <--- perfect match

                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}
