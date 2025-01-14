using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;

namespace ApiPelicula.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<AppUsuario> GetUsuarios();
        AppUsuario GetUsuario(string usuarioId);
        bool isUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}
