using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirchowAspNetApi.DTOs;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;

namespace VirchowAspNetApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LaudoController : ControllerBase
{
    private readonly LaudoService _service;

    public LaudoController(LaudoService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Laudo>> GetAll() => _service.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Laudo> GetById(int id)
    {
        var laudo = _service.GetById(id);
        return laudo is null ? NotFound() : Ok(laudo);
    }

    [HttpPost]
    public ActionResult<LaudoRequest> Create(LaudoRequest laudo)
    {
        var novoLaudo = _service.Add(laudo);
        return CreatedAtAction(nameof(GetById), new { id = novoLaudo.Id }, novoLaudo);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Laudo laudo)
    {
        var atualizado = _service.Update(id, laudo);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpPatch("invalidar/{id}")]
    public IActionResult Inavalidate(int id, [FromBody] InvalidaLaudoRequest request)
    {
        var removido = _service.Invalidate(id, request.UsuarioId);
        return removido ? NoContent() : NotFound();
    }

    [HttpPost("complementar/")]
    public ActionResult<LaudoComplementarRequest> Complementar(LaudoComplementarRequest laudo)
    {
        var novoLaudo = _service.Add(laudo);
        return CreatedAtAction(nameof(GetById), new { id = novoLaudo.Id }, novoLaudo);
    }
}
