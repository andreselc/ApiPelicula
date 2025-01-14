using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using ApiPelicula.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPelicula.Controllers.V2
{
    //[ResponseCache(Duration = 20)]
    [Route("api/v{version:apiVersion}/categorias")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CategoriasController : ControllerBase
    {

        private readonly ICategoriaRepository _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {

            return new string[] { "jose", "valor2", "valor3" };
        }


    }
}
