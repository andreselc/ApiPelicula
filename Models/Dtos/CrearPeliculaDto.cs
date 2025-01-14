using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Models.Dtos
{
    public class CrearPeliculaDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string? RutaImagen { get; set; }
        public IFormFile Imagen { get; set; }
        public enum CrearTipoClasificacion { G, PG, PG13, R, NC17 }
        public CrearTipoClasificacion Clasificacion { get; set; }
        public int categoriaId { get; set; }
    }
}
