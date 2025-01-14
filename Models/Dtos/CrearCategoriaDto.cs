﻿using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Models.Dtos
{
    public class CrearCategoriaDto
    {
        [Required (ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del nombre es 100 caracteres")]
        public string Nombre { get; set; }
    }
}
