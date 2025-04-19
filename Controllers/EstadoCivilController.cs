using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;

namespace VirchowAspNetApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EstadoCivilController : ControllerBase
{
    private readonly EstadoCivilService _service;

    public EstadoCivilController(EstadoCivilService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<EstadoCivil>> GetAll() => _service.GetAll();

}
