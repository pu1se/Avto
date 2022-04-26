using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avto.Api.Controllers
{
    public class AuthenticationController : BaseApiController
    {
        public AuthenticationController()
        {
        }

        [HttpPost]
        [Route("token")]
        public IActionResult GetToken(Auth auth)
        {
            return Ok();
        }
    }

    public class Auth
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}
