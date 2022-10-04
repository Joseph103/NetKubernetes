using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UsuarioDtos;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _singManager;
    private readonly IJwtGenerador _jwtGenerador;
    private readonly AppDbContext _contexto;
    private readonly IUsuarioSesion _usuarioSesion;


    public UsuarioRepository(UserManager<Usuario> userManager,
        SignInManager<Usuario> singManager,
        IJwtGenerador jwtGenerador,
        AppDbContext contexto,
        IUsuarioSesion usuarioSesion
    )
    {
        _userManager = userManager;
        _singManager = singManager;
        _jwtGenerador = jwtGenerador;
        _contexto = contexto;
        _usuarioSesion = usuarioSesion;
    }


    private UsuarioResponseDto TransformUserToUserDto(Usuario usuario)
    {
        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Telefono = usuario.Telefono,
            Email = usuario.Email,
            UserName = usuario.UserName,
            Token = _jwtGenerador.CrearToken(usuario)
        };
    }

    public async Task<UsuarioResponseDto> GetUsuario()
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
        return TransformUserToUserDto(usuario!);
    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto resquest)
    {
        var usuario = await _userManager.FindByEmailAsync(resquest.Email!);
        await _singManager.CheckPasswordSignInAsync(usuario!, resquest.Password!, false);
        return TransformUserToUserDto(usuario!);
    }

    public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto request)
    {
        var usuario = new Usuario {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName,
         };

         await _userManager.CreateAsync(usuario!, request.Password!);

         return TransformUserToUserDto(usuario);
    }
}