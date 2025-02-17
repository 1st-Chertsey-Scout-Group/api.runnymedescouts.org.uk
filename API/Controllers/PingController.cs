using Microsoft.AspNetCore.Mvc;

namespace RunnymedeScouts.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController(ILogger<PingController> logger, IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<PingController> _logger = logger;

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation($"{nameof(Get)} - Called.");
        return Ok(_configuration.GetValue<string>("AppSettings:Ping") ?? "Ok");
    }
}
