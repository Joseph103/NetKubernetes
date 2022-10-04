using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class LoadDatabase
{
    public static async Task InsertarData(AppDbContext context, UserManager<Usuario> usuarioManager)
    {
        if (!usuarioManager.Users.Any())
        {
            var usuario = new Usuario{
                Nombre = "Vaxi",
                Apellido = "Drez",
                Email = "vaxi.drez.social@gmail.com",
                UserName = "vaxi.drez",
                Telefono = "98142545"
            };

            await usuarioManager.CreateAsync(usuario, "PasswordVxidrez123$");
        }

        if (!context.Inmuebles!.Any())
        {
            context.Inmuebles!.AddRange(
                new Inmueble{
                    Nombre = "Casa de Playa",
                    Direccion = "Av. El Sol 32",
                    Precio = 4500M,
                    FechaCreacion = DateTime.Now
                },
                new Inmueble{
                    Nombre = "Casa de Invierno",
                    Direccion = "Av. La Roca 101",
                    Precio = 3500M,
                    FechaCreacion = DateTime.Now
                }
            );
        }

        context.SaveChanges();
    }
}