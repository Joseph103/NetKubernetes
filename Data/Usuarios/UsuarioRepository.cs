using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Dtos.UsuarioDtos;
using NetKubernetes.Middleware;
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
        if (usuario is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.Unauthorized,
                new { mensaje = "El usuario del token no existe en la base de datos" }
                );
        }
        return TransformUserToUserDto(usuario!);
    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto resquest)
    {
        var usuario = await _userManager.FindByEmailAsync(resquest.Email!);

        if (usuario is null)
        {
            throw new MiddlewareException(
                HttpStatusCode.Unauthorized,
                new { mensaje = "El email del ususario no existe en mi base de datos" }
                );
        }

        var resultado = await _singManager.CheckPasswordSignInAsync(usuario!, resquest.Password!, false);

        if (resultado.Succeeded)
        {
            return TransformUserToUserDto(usuario!);
        }

        throw new MiddlewareException(
            HttpStatusCode.Unauthorized,
            new { mensaje = "Las credenciales son incorrectas" }
        );

    }

    public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto request)
    {
        var existeEmail = await _contexto.Users.Where(x => x.Email == request.Email).AnyAsync();

        if (existeEmail)
        {
            throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { mensaje = "Las email del usuario ya existe en la base de datos" }
            );
        }
        
        var existeUsername = await _contexto.Users.Where(x => x.UserName == request.UserName).AnyAsync();

        if (existeUsername)
        {
            throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { mensaje = "El username del usuario ya existe en la base de datos" }
            );
        }

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName,
        };

        var resultado = await _userManager.CreateAsync(usuario!, request.Password!);

        if(resultado.Succeeded)
        {
            return TransformUserToUserDto(usuario);
        }
        throw new Exception("No se pudo registrar el usuario");

    }
}