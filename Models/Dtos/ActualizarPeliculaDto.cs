using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Models.Dtos
{
    public class ActualizarPeliculaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string? RutaImagen { get; set; }
        public string? RutaLocalImagen { get; set; }
        public IFormFile Imagen { get; set; }
        public enum TipoClasificacion { G, PG, PG13, R, NC17 }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int categoriaId { get; set; }
    }
}
