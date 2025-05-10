using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;

namespace VirchowAspNetApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DiagnosticoController : ControllerBase
{
    private readonly DiagnosticoService _service;

    public DiagnosticoController(DiagnosticoService service)
    {
        _service = service;
    }

    [HttpGet("{exameId}")]
    public ActionResult<List<Diagnostico>> GetByExameId(int exameId)
    {
        var diagnostico = _service.GetByExameId(exameId);
        return diagnostico is null ? NotFound() : Ok(diagnostico);
    }
}
