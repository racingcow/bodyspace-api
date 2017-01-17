using Microsoft.AspNetCore.Mvc;

namespace bodyspace_api.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        // GET api/Ping
        [HttpGet]
        public string Get()
        {
            return "API is up. See readme.md at https://github.com/racingcow/bodyspace-api for documentation.";
        }
    }
}
