namespace Api.Controllers;

[ApiController]
public sealed class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("{ \"status\": \"available\" }");
    }
}
