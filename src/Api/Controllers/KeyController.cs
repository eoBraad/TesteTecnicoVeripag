using System.Net;
using Application.Dtos.Responses;
using Application.Services;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeyController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromQuery] string key, [FromServices] CreateApiKeyService service)
    {
        var result = await service.CreateApiKeyAsync(key);
        
        return Created("", result);
    }
}