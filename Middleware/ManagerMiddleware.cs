using System.Net;
using Newtonsoft.Json;

namespace NetKubernetes.Middleware;

public class ManagerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManagerMiddleware> _logger;

    public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ManagerExceptionAsync(context, ex, _logger);
        }
    }

    private async Task ManagerExceptionAsync(HttpContext context, Exception ex, ILogger<ManagerMiddleware> logger)
    {
        object? errores = null;

        switch (ex)
        {
            case MiddlewareException me:
                logger.LogError(ex, "Middleware Error");
                errores = me.Errores;
                context.Response.StatusCode = (int)me.Codigo;
                break;
            case Exception e:
                logger.LogError(ex, "Error de Servidor");
                errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.ContentType = "application/json";
        var resultados = string.Empty;
        
        if(errores != null){
            resultados = JsonConvert.SerializeObject(new {errores});
        }

        await context.Response.WriteAsync(resultados);
    }

}