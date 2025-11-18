using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserAPI.Enums;
using UserAPI.Exceptions;
using UserAPI.Interfaces;
using UserAPI.Models;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartnerController(IUserService userService, IAuthService authService, IJournalService journalService,
    ILogger<PartnerController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IAuthService _authService = authService;
    private readonly IJournalService _journalService = journalService;
    private readonly ILogger<PartnerController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> RememberMe([Required] string code)
    {
        try
        {
            await _userService.GetAsync(code);

            var token = _authService.GenerateJwtToken(code);

            return Ok(new TokenInfoModel
            {
                Token = token,
            });
        }
        catch (SecureException ex)
        {
            _logger.LogError(ex.Message);

            var journal = await _journalService.CreateAsync(ExceptionType.Secure, ex.Message);

            var errorResponse = new
            {
                id = journal == null ? DateTime.UtcNow.Ticks.ToString() : journal.Id.ToString(),
                type = ExceptionType.Secure,
                data = new
                {
                    message = ex.Message
                }
            };

            return StatusCode(500, errorResponse);
        }
    }
}
