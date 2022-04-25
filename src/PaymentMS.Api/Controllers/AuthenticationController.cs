using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentMS.DAL.CloudServices;

namespace PaymentMS.Api.Controllers
{
    public class AuthenticationController : BaseApiController
    {
        private IKeyVaultStorage KeyVault { get; }
        public AuthenticationController(IKeyVaultStorage keyVault)
        {
            KeyVault = keyVault;
        }

        [HttpPost]
        [Route("token")]
        public IActionResult GetToken(Auth auth)
        {
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("key-vault/tmp")]
        public async Task<IActionResult> GetTmp()
        {
            var result = await KeyVault.GetSecretByNameAsync("tmp");
            return Ok(result);
        }
    }

    public class Auth
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}
