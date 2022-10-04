using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NetKubernetes.Models;
using Microsoft.IdentityModel.Tokens;

namespace NetKubernetes.Token;

public class JwtGenerador: IJwtGenerador
{
    public string CrearToken(Usuario usuario)
    {

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
            new Claim("userId", usuario.Id),
            new Claim("email", usuario.Email!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
        var credeciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescripcion = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(30),
            SigningCredentials = credeciales
        };

        var tokenHandeler = new JwtSecurityTokenHandler();
        var token = tokenHandeler.CreateToken(tokenDescripcion);
        
        return tokenHandeler.WriteToken(token);

    }
}