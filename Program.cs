using System.Text;
using ApiEcommerce.Const;
using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(dbConnectionString)
  .UseSeeding((context, _) =>
  {
    var appContext = (ApplicationDbContext)context;
    // Seeding de Roles
    
    DataSeeder.SeedData(appContext);
  })
);
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024;
    options.UseCaseSensitivePaths = true;
});
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
TypeAdapterConfig.GlobalSettings.Scan(typeof(ApiEcommerce.Mapping.CategoryProfile).Assembly);
builder.Services.AddMapster();
MapsterConfig.RegisterMappings();
builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();



var secretKey = builder.Configuration.GetValue<string> ("ApiSettings:SecretKey");
if(string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("SecretKy no esta configuurada");
}
builder.Services.AddAuthentication(Options =>
{
    
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(Options =>
{
    Options.RequireHttpsMetadata = false;
    Options.SaveToken = true;
    // Evita que .NET renombre "role" a "http://schemas.microsoft.com/.../role"
    Options.MapInboundClaims = false;
    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = "role"
    };
}
);

builder.Services.AddControllers( option =>
{
    option.CacheProfiles.Add(CacheProfiles.Default10, CacheProfiles.profile10);
    option.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.profile20);
  
}
 ); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
  {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
      Description = "Nuestra API utiliza la Autenticación JWT usando el esquema Bearer. \n\r\n\r" +
                    "Ingresa la palabra a continuación el token generado en login.\n\r\n\r" +
                    "Ejemplo: \"12345abcdef\"",
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.Http,
      Scheme = "Bearer"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
      [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
    options.SwaggerDoc("v1",new OpenApiInfo
    {
      Version = "v1",
      Title = "Api Ecommerce",
      Description = "Api para gestionar productos y ususarios",
      TermsOfService =  new Uri("htt://examsple.com/term"),
      Contact = new OpenApiContact
      {
        Name ="Diegood",
        Url = new Uri("http://devtalles.com")
      },
      License = new OpenApiLicense
      {
        Name = "Licencia de uso",
        Url = new Uri("htt://examsple.com/license")
      }
    });
     options.SwaggerDoc("v2",new OpenApiInfo
    {
      Version = "v2",
      Title = "Api Ecommerce v2",
      Description = "Api para gestionar productos y ususarios",
      TermsOfService =  new Uri("htt://examsple.com/term"),
      Contact = new OpenApiContact
      {
        Name ="Diegood",
        Url = new Uri("http://devtalles.com")
      },
      License = new OpenApiLicense
      {
        Name = "Licencia de uso",
        Url = new Uri("htt://examsple.com/license")
      }
    });
  }
);
var AppiVersioningBuilder = builder.Services.AddApiVersioning(Options =>
{
  Options.AssumeDefaultVersionWhenUnspecified = true;
  Options.DefaultApiVersion = new ApiVersion(1,0);
  Options.ReportApiVersions = true;
  //Options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"));
});
AppiVersioningBuilder.AddMvc().AddApiExplorer(Options =>
{
  Options.GroupNameFormat = "'v'VVV"; //v1.v2,v3 etc
  Options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddCors(Options =>
{
    Options.AddPolicy(PolicyNames.AllowSpecificOrigin,
    builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }
    );
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
      options.SwaggerEndpoint ("/swagger/v1/swagger.json","v1");
      options.SwaggerEndpoint ("/swagger/v2/swagger.json","v2");
    });
    
}               
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseResponseCaching();
        
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
