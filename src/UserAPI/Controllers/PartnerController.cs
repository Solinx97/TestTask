using Microsoft.AspNetCore.Mvc;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartnerController : ControllerBase
{
    [HttpGet]
    public IActionResult RememberMe(int code)
    {
        return Ok();
    }
}
