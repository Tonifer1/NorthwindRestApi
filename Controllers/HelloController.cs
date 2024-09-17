using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//Tässä Hellocontroller perii ControllBase luokan ominaisuudet
namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World!";

        }

        [HttpPost]
        public int LaskeYhteen(int a,int b)
        {
            return a + b;
        }

        
    }
}
