﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPelicula.Migrations
{
    /// <inheritdoc />
    public partial class SoporteParaSubidaImagenesPeliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RutaLocalImagen",
                table: "Pelicula",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaLocalImagen",
                table: "Pelicula");
        }
    }
}
