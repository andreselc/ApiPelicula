using System.Text;
using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.PeliculasMapper;
using ApiPelicula.Repository;
using ApiPelicula.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using XAct;
using XAct.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

builder.Services.AddControllers(opcion =>
{
    //Caché profile. Un caché global y así no habrá que colocarlo en todas partes   
    opcion.CacheProfiles.Add("PorDefecto20Segundos", new CacheProfile() { Duration = 20 });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme= "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                 },
                new List<string>()
            }
        });
        options.SwaggerDoc("v1", new OpenApiInfo
            { 
                Version = "v1.0",
                Title = "Peliculas Api V1",
                Description = "Api de Peliculas V1",
                TermsOfService = new Uri("https://render2web.com/promociones"),
                Contact = new OpenApiContact
                {
                    Name = "render2web",
                    Url = new Uri("https://render2web.com/promociones")
                },
                License = new OpenApiLicense
                {
                    Name = "Licencia Personal",
                    Url = new Uri("https://render2web.com/promociones")
                }
            }
        );
        options.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2.0",
            Title = "Peliculas Api V2",
            Description = "Api de Peliculas V2",
            TermsOfService = new Uri("https://render2web.com/promociones"),
            Contact = new OpenApiContact
            {
                Name = "render2web",
                Url = new Uri("https://render2web.com/promociones")
            },
            License = new OpenApiLicense
            {
                Name = "Licencia Personal",
                Url = new Uri("https://render2web.com/promociones")
            }
        }
        );

    }
    );


//Soporte para CORS
//Se pueden habilitar 1-Un dominio, 2- Múltiples dominios
//3- Todos los dominios (tener en cuenta la seguridad)
//Usaremmos de ejemplo el dominio http://localhost:3223, se de cambiar por el correcto
// Se usa (*) para todos los dominios

builder.Services.AddCors(p =>p.AddPolicy("PoliticaCors", build =>
{
    build.WithOrigins("http://localhost:3223").AllowAnyMethod().AllowAnyHeader();
}));

//Soporte para autenticación con .NET Identity
builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


//Soporte para cache
builder.Services.AddResponseCaching();

//Se agregan los repositorios

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

//Soporte para versionamiento de la API
var apiVersioningBuilder = builder.Services.AddApiVersioning(opcion =>
{
    opcion.AssumeDefaultVersionWhenUnspecified = true;
    opcion.DefaultApiVersion = new ApiVersion(1, 0);
    opcion.ReportApiVersions = true;
   // opcion.ApiVersionReader = ApiVersionReader.Combine(
   //    new QueryStringApiVersionReader("api-version"));//?api-version=1.0
                                                        //new HeaderApiVersionReader("X-Version")); //X-Version: 1.0
                                                        //new MediaTypeApiVersionReader("version"); //Accept: application/json; version=1.0
});

apiVersioningBuilder.AddApiExplorer(
        opciones =>
        {
            opciones.GroupNameFormat = "'v'VVV";
            opciones.SubstituteApiVersionInUrl = true;
        }
    );

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculaMapper));

//Configuramos la autenticación
builder.Services.AddAuthentication(
        x => {
               x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();
app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("v1/swagger.json", "ApiPeliculasV1");
        opciones.SwaggerEndpoint("v2/swagger.json", "ApiPeliculasV2");
    });
}
else
{
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("v1/swagger.json", "ApiPeliculasV1");
        opciones.SwaggerEndpoint("v2/swagger.json", "ApiPeliculasV2");
        opciones.RoutePrefix = "";
    });

}

//Soporte para archivos estáticos como imágenes
app.UseStaticFiles();
app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PoliticaCors");

//Soporte para Autenticación
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
