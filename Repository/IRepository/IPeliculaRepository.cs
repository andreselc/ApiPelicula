using ApiPelicula.Models;

namespace ApiPelicula.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        //V1
        //ICollection<Pelicula> GetPeliculas();
        //V2
        ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize);
        int GetTotalPeliculas();
        ICollection<Pelicula> GetPeliculasEnCategoria(int catId);
        IEnumerable<Pelicula> BuscarPelicula(string nombre);
        Pelicula GetPelicula(int peliculaId);
        bool ExistePelicula(int peliculaId);
        bool ExistePelicula(string nombre);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        bool Guardar();

    }
}
