using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using ApiPelicula.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;


namespace ApiPelicula.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly ApplicationDbContext _db;
        private string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioRepository(ApplicationDbContext db, 
            IConfiguration config,
            UserManager<AppUsuario> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IMapper mapper)
        {
            _db = db;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _db.AppUsuario.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _db.AppUsuario.OrderBy(u => u.UserName).ToList();
        }

        public bool isUniqueUser(string usuario)
        {
            var usuarioBd = _db.AppUsuario.FirstOrDefault(u => u.UserName == usuario);
            if ( usuarioBd == null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            var usuario = _db.AppUsuario.FirstOrDefault(
                u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());
              
            bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);
            
            //Validamos si el usuario no existe con la combinación de usuario y contraseña

            if (usuario == null || isValid ==false)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //Aquí existe el usuario, y luego podremos procesar el Login
            var Roles = await _userManager.GetRolesAsync(usuario);
            var manejadoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, Roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = manejadoToken.CreateToken(tokenDescriptor);
            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadoToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario),
            };

            return usuarioLoginRespuestaDto;
        }

        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            //var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            AppUsuario usuario = new AppUsuario()
            {
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                Nombre = usuarioRegistroDto.Nombre
            };

            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("usuario"));
                }
                await _userManager.AddToRoleAsync(usuario, "admin");
                var usuarioRetornado = _db.AppUsuario.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }
            
            return new UsuarioDatosDto();
        }

        //Método para encripatr la contraseña con MD5, se usa tanto en elacceso como en el registro
       // public static string obtenermd5(string valor)
       // {
         //   MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
         //   byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
         //   data = x.ComputeHash(data);
         //   string resp = "";
         //   for (int i = 0; i < data.Length; i++)
         //       resp += data[i].ToString("x2").ToLower();
         //   return resp;
      //  }

    }
}
