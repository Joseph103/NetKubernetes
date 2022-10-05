namespace NetKubernetes.Dtos.InmuebleDtos;

public class InmuebleRequestDto
{
    public string? Nombre { get; set; }
    public string? Direccion {get; set;}
    public decimal Precio { get; set; }
    public string? Picture { get; set; }
}