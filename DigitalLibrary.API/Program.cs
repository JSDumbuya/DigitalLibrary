using DigitalLibrary.API.Data;
using DigitalLibrary.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DbContext
builder.Services.AddDbContext<DigitalLibraryContext>(options => options.UseSqlite("Data Source=YLDatabase.db"));

//Controllers
builder.Services.AddControllers();

//Repos
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<LibraryRepository>();

//Services
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LibraryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*
app.UseAuthorization();
app.MapControllers();*/

app.Run();