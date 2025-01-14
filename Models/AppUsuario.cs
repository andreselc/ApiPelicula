using Microsoft.AspNetCore.Identity;

namespace ApiPelicula.Models
{
    public class AppUsuario: IdentityUser
    {
        public string Nombre { get; set; }
    }
}
