using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirchowAspNetApi.Models;
using VirchowAspNetApi.Services;

namespace VirchowAspNetApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TipoLaudoController : ControllerBase
{
    private readonly TipoLaudoService _service;

    public TipoLaudoController(TipoLaudoService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<TipoLaudo>> GetAll() => _service.GetAll();

}
