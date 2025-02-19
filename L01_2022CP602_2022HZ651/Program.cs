var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//Inyeccion por dependencoa del string de conexion al contexto 
builder.Services.AddDbContext<RestauranteContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("RestauranteDbConnection")
        )
);

//Inyeccion nueva
builder.Services.AddDbContext<RestauranteContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("RestauranteCarolina")
        )
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
