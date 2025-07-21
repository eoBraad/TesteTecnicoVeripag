using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string location, [FromServices] GetWeatherForecastService service)
    {
        var result = await service.GetWeatherAsync(location, "");
        return Ok(result);
    }
}