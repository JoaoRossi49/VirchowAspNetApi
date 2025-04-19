using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;

namespace VirchowAspNetApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PacienteController : ControllerBase
{
    private readonly PacienteService _service;

    public PacienteController(PacienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Paciente>> GetAll() => _service.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Paciente> GetById(int id)
    {
        var paciente = _service.GetById(id);
        return paciente is null ? NotFound() : Ok(paciente);
    }

    [HttpPost]
    public ActionResult<Paciente> Create(Paciente paciente)
    {
        var novoProduto = _service.Add(paciente);
        return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Paciente paciente)
    {
        var atualizado = _service.Update(id, paciente);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var removido = _service.Delete(id);
        return removido ? NoContent() : NotFound();
    }
}
