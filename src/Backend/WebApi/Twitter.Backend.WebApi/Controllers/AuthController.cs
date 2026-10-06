using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TwitterClone.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("redirect")]
    public IActionResult LoginRedirect([FromQuery] string returnUrl)
    {
        // VULNERABILITY: Open Redirect (CWE-601)
        // Attackers can trick users into clicking a link to the real API that redirects to a malicious site
        return Redirect(returnUrl);
    }

    [HttpPost("deserialize")]
    public IActionResult ProcessWebhook([FromBody] string untrustedJson)
    {
        // VULNERABILITY: Insecure Deserialization (CWE-502)
        // Using TypeNameHandling.All allows an attacker to instantiate arbitrary classes
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        var obj = JsonConvert.DeserializeObject(untrustedJson, settings);
        
        return Ok();
    }
}