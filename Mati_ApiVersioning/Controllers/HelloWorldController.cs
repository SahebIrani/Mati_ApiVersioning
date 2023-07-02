using Microsoft.AspNetCore.Mvc;

namespace Mati_ApiVersioning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public string Get() => $"Controller = {GetType().Name}";
}