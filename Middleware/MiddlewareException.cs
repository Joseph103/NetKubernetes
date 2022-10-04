using System.Net;
namespace NetKubernetes.Middleware;

public class MiddlewareException : Exception
{
    public HttpStatusCode Codigo { get; set; }
    public object? Errores { get; set; }

    public MiddlewareException(HttpStatusCode codigo, object? errores = null)
    {
        Codigo = codigo;
        Errores = errores;
    }
}