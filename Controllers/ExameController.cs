using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;

namespace VirchowAspNetApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ExameController : ControllerBase
{
    private readonly ExameService _service;

    public ExameController(ExameService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Exame>> GetAll() => _service.GetAll();

}
