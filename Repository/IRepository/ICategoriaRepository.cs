﻿using ApiPelicula.Models;

namespace ApiPelicula.Repository.IRepository
{
    public interface ICategoriaRepository
    {

        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int categoriaId);
        bool ExisteCategoria(int categoriaId);
        bool ExisteCategoria(string nombre);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();

    }
}
