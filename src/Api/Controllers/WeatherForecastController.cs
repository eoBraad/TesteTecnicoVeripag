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
    public async Task<IActionResult> Get([FromQuery] string location, [FromQuery] string apikey,[FromServices] GetWeatherForecastService service)
    {
        var result = await service.GetWeatherAsync(location, apikey);
        return Ok(result);
    }
    
    [HttpGet("extended")]
    [ProducesResponseType(typeof(OpenWeatherExtendedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExtended([FromQuery] string location, [FromQuery] string apikey,[FromServices] GetWeatherForecastExtendedService service)
    {
        var result = await service.GetWeatherAsync(location, apikey);
        return Ok(result);
    }
}