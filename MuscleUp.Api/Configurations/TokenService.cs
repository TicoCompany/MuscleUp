using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Api.Configurations
{
    public static class TokenService
    {
        public static string GenerateToken(Usuario aluno)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(App.ChaveSecreta);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, aluno.Nome),
            new Claim(ClaimTypes.Email, aluno.Email),
            new Claim(ClaimTypes.NameIdentifier, aluno.Id.ToString()),
             new Claim("IdAcademia", aluno!.IdAcademia.ToString() ?? string.Empty)
            }
            .Where(c => !string.IsNullOrWhiteSpace(c.Value)).ToList();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
