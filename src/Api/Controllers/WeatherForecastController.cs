using System.Net;
using Application.Dtos.Responses;
using Application.Services;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(OpenWeatherSimpleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] string location, [FromServices] GetWeatherForecastService service)
    {
        var result = await service.GetWeatherAsync(location, "");
        return Ok(result);
    }
    
    [HttpGet("extended")]
    [ProducesResponseType(typeof(OpenWeatherExtendedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExtended([FromQuery] string location, [FromServices] GetWeatherForecastExtendedService service)
    {
        var result = await service.GetWeatherAsync(location, "");
        return Ok(result);
    }
}