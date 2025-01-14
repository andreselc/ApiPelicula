using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiPelicula.Repository
{
    public class PeliculaRepository: IPeliculaRepository
    {
        private readonly ApplicationDbContext _db;

        public PeliculaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
           pelicula.FechaCreacion = DateTime.Now;
            //Arreglar problema del PATCH
            var peliculaExistente = _db.Pelicula.Find(pelicula.Id);
            if (peliculaExistente != null)
            {
                _db.Entry(peliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else
            {
                _db.Pelicula.Update(pelicula);
            }
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _db.Pelicula.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _db.Pelicula;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _db.Pelicula.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(int id)
        {
            return _db.Pelicula.Any(p => p.Id == id);
        }

        public bool ExistePelicula(string nombre)
        {
            bool valor = _db.Pelicula.Any(p => p.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _db.Pelicula.FirstOrDefault(p => p.Id == peliculaId);
        }

        //V1
        //public ICollection<Pelicula> GetPeliculas()
        //{
        //    return _db.Pelicula.OrderBy(p => p.Nombre).ToList();
        //}

        //v2
        public ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize)
        {
            return _db.Pelicula.OrderBy(p => p.Nombre)
                .Skip((pageNumber -1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
        {

            return _db.Pelicula.Include(c => c.Categoria).Where(c => c.categoriaId == catId).ToList();

        }

        public int GetTotalPeliculas()
        {
            return _db.Pelicula.Count();
        }

        public bool Guardar()
        {
           return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
