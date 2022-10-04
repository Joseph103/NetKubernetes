using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Inmuebles;

public class InmuebleRepository : IInmuebleRepository
{
    private readonly AppDbContext _contexto;
    private readonly IUsuarioSesion _usuarioSesion;
    private readonly UserManager<Usuario> _userManager;

    public InmuebleRepository(AppDbContext contexto, IUsuarioSesion sesion, UserManager<Usuario> userManager)
    {
        _contexto = contexto;
        _usuarioSesion = sesion;
        _userManager = userManager;
    }

    public async Task CreateInmueble(Inmueble inmueble)
    {
        var usuario = await _userManager.FindByNameAsync( _usuarioSesion.ObtenerUsuarioSesion());

        inmueble.FechaCreacion = DateTime.Now;
        inmueble.UsuarioId = Guid.Parse(usuario!.Id);
    }

    public void DeleteInmueble(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Inmueble> GetAllInmuebles()
    {
        throw new NotImplementedException();
    }

    public Inmueble GetInmuebleById(int id)
    {
        throw new NotImplementedException();
    }

    public bool SaveChanges()
    {
        throw new NotImplementedException();
    }
}