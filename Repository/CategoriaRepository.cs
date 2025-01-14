using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.Repository.IRepository;

namespace ApiPelicula.Repository
{
    public class CategoriaRepository: ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
           categoria.FechaCreacion = DateTime.Now;
            //Arreglar problema del PUT 
            var categoriaExistente = _db.Categoria.Find(categoria.Id);
            if (categoriaExistente != null)
            {
                _db.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
            }
            else
            {
                _db.Categoria.Update(categoria);
            }
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _db.Categoria.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categoria.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(int categoriaId)
        {
            return _db.Categoria.Any(c => c.Id == categoriaId);
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _db.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _db.Categoria.FirstOrDefault(c => c.Id == categoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _db.Categoria.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
           return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
