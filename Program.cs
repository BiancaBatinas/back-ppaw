using Microsoft.EntityFrameworkCore;

using ppawproject.Database;
using ppawproject.Interfaces;
using ppawproject.Services;

var builder = WebApplication.CreateBuilder(args);


// Configurează conexiunea la baza de date
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));




// Adaugă serviciile
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMarketplaceService, MarketplaceService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configurează CORS pentru a permite toate originile
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Permite toate originile
              .AllowAnyHeader() // Permite toate anteturile
              .AllowAnyMethod(); // Permite toate metodele (GET, POST, PUT, DELETE etc.)
    });
});

// Add services to the container.
builder.Services.AddControllers();
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

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

