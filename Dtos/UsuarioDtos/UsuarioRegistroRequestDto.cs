namespace NetKubernetes.Dtos.UsuarioDtos;

public class UsuarioRegistroRequestDto
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}