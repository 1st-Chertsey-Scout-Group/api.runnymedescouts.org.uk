using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RunnymedeScouts.API.DTOs;
using RunnymedeScouts.API.Extensions;
using RunnymedeScouts.API.Filters;
using RunnymedeScouts.API.Services;

namespace RunnymedeScouts.API.Controllers;

[Route("[controller]")]
[ApiController]
public class EnquiriesController(ILogger<EnquiriesController> logger, ISmtpService smtpService) : ControllerBase
{
    private readonly ILogger<EnquiriesController> _logger = logger;
    private readonly ISmtpService _smtpService = smtpService;

    [HttpPost]
    [EnableRateLimiting("Fixed")]
    [ValidateReferrer]
    public IActionResult Post([FromBody] PostEnquiryDto request)
    {
        _logger.LogInformation("{method} - Called.", nameof(Post));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.FriendlyMessage());
        }

        if (!string.IsNullOrEmpty(request.Number))
        {
            // Bot honeypot triggered.
            return Ok();
        }

        var emailSent = _smtpService.SendEmail(request.Email, request.Name, request.Subject, request.Message);
        if (emailSent)
        {
            return Ok();
        }
        return Problem("Unable to send enquiry");
    }


}

