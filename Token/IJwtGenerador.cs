using NetKubernetes.Models;

namespace NetKubernetes.Token;

public interface IJwtGenerador
{
 string CrearToken(Usuario usuario);
}