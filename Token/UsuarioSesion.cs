using System.Security.Claims;
namespace NetKubernetes.Token;

public class UsuarioSesion : IUsuarioSesion
{
    private readonly IHttpContextAccessor _httpContextAccesor;

    public UsuarioSesion(IHttpContextAccessor httpContextAccesor)
    {
        _httpContextAccesor = httpContextAccesor;
    }

    public string ObtenerUsuarioSesion()
    {
        var userName = _httpContextAccesor.HttpContext!.User?.Claims?
                            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        return userName!;                            
    }
}