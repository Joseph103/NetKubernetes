using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Inmuebles;
using NetKubernetes.Dtos.InmuebleDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;

namespace NetKubernetes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InmuebleController : ControllerBase
{
    private readonly IInmuebleRepository _repository;
    private IMapper _mapper;

    public InmuebleController(IInmuebleRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<InmuebleResponseDto>> GetInmuebles()
    {
        var inmuebles = _repository.GetAllInmuebles();
        return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles));
    }
    
    [HttpGet("{id}", Name="GetInmuebleById")]
    public ActionResult<IEnumerable<InmuebleResponseDto>> GetInmuebleById(int id)
    {
        var inmueble = _repository.GetInmuebleById(id);

        if (inmueble is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.NotFound,
                new {mensaje = $"No se encontro el inmueble por este id {id}"}
            );
        }

        return Ok(_mapper.Map<InmuebleResponseDto>(inmueble));
    }

    [HttpPost]
    public ActionResult<InmuebleResponseDto> CreateInmueble([FromBody] InmuebleRequestDto inmueble)
    {
        var inmuebleModel = _mapper.Map<Inmueble>(inmueble);
        _repository.CreateInmueble(inmuebleModel);
        _repository.SaveChanges();

        var inmuebleResponse = _mapper.Map<InmuebleResponseDto>(inmuebleModel);
        return CreatedAtRoute(nameof(GetInmuebleById), new {inmuebleResponse.Id}, inmuebleResponse);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteInmueble(int id)
    {
        _repository.DeleteInmueble(id);
        _repository.SaveChanges();
        return Ok();
    }

}