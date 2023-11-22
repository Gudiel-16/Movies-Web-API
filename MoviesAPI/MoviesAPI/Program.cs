using Microsoft.EntityFrameworkCore;
using MoviesAPI;
using MoviesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuracion
var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Configurando AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Servicio para guardar img en Azure
//builder.Services.AddTransient<IFileStorage, FileStorageAzure>();

// Servicio para guardar img local
builder.Services.AddTransient<IFileStorage, FileStorageLocal>();
builder.Services.AddHttpContextAccessor();

// Configurando el dbContext de nuestra app
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// para ver contenido estatico por medio de url, como img en wwwroot
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
