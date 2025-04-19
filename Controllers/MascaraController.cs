using Microsoft.AspNetCore.Mvc;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;

namespace VirchowAspNetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascaraController : ControllerBase
{
    private readonly MascaraService _service;

    public MascaraController(MascaraService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Mascara>> GetAll() => _service.GetAll();

}
