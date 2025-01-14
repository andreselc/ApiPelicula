using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiPelicula.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

    }
}
