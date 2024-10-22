﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;//Tietokantamallit. Nimi tulee projektin nimestä.
using System.Linq.Expressions;
namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustetaan tietokantayhteys. Perinteinen tapa
        //NorthwindOriginalContext db = new NorthwindOriginalContext(); Voi jättää pois. Pelkkä new riittää perässä.

        //Dependency injektion tapa. Alustetaan tyhjänä db.
        private readonly NorthwindOriginalContext db;

        //Metodi on nimetty Controllerin mukaan. 
        public CustomersController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }


        //GET = READ Hakee kaikki asiakkaat. Ei parametreja.
        [HttpGet]//Tämä on filtteri, joka ohjaa pyynnön tähän metodiin.
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

        //Hakee asiakkaan pääavaimella.{id}
        //Tässä tapauksessa string id,jonka on oltava saman niminen parametrina.kts(string id)
        //koska Northwind tietokannassa asiakkaan pääavain on merkkijono
        //e virheenkäsittelyssä Exeption e = e on oma luotu muuttuja
        //GET = READ Hakee Id:n perusteella asiakkaan.
        [HttpGet("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                // Yritetään hakea asiakas tietokannasta Find metodilla voidaan hakea tietoa pääavaimella.
                var asiakas = db.Customers.Find(id);

                // Tarkistetaan löytyykö asiakas. Eli jos ei ole null, niin asiakas löytyy.
                if (asiakas != null)
                {
                    // Jos asiakas löytyy, palautetaan se
                    return Ok(asiakas);
                }
                else
                {
                    // Jos asiakasta ei löydy, palautetaan 404 Not Found
                    return NotFound($"Asiakasta id:llä {id} ei löytynyt");
                }


            }
            catch (Exception e)
            {
                // Jos tapahtuu virhe,esim. väärä connection string palautetaan virheilmoitus 400.
                // $-merkki on string interpolation.Muuttuja arvo aaltosulkeissa. 
                return BadRequest($"Tapahtui virhe haettaessa asiakasta id:llä {id}. Lisätietoja: {e.Message}");
            }


        }


        //POST = CREATE Lisää uuden asiakkaan. customer on alias nimi Ei parametreja.
        //Tämä ottaa vastaan parametrina http pyynnön body osasta Customer luokkaa vastaavan olion joka on aliasoitu customeriksi.
        [HttpPost]
        public ActionResult AddNewCustomer([FromBody] Customer customer)
        {
            try
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return Ok($"Lisätty uusi asiakas: {customer.CompanyName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }

        }



        //DELETE = DELETE Poistaa yhden asiakkaan id:n perusteella. Huom! string muoto
        [HttpDelete("{id}")]
        public ActionResult DeleteOneCustomerById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas == null)
                {
                    //return NotFound("Asikasta id:ll" + id + "ei löytynyt");
                    return NotFound($"Asiakasta id:llä  {id}  ei löytynyt");
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


        }

        //Asiakkaan tietojen muokkaaminen
        //ottaa vastaan kaksi parametria:Urlista id(string) ja customer objekti http bodyn osasta.
        //From body tarkoittaa kaikkia asiakkaan tietoja
        //Haetaan id:N perusteella vanha asiakasobjekti
        //PUT= UPDATE Päivittää asiakkaan tiedot id:n perusteella.
        //Hakee asiakkaan pääavaimella.Tässä tapauksessa string id.
        [HttpPut("{id}")]
        public ActionResult EditCustomer(string id, [FromBody] Customer customer)
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
