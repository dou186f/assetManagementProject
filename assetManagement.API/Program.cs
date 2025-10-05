using assetManagement.API.Interfaces;
using assetManagement.API.Data;
using assetManagement.API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddScoped<IAssetRepo, AssetRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

const string MauiCors = "allow_maui";
builder.Services.AddCors(o =>
{
    o.AddPolicy(MauiCors, p =>
        p.WithOrigins("https://307637fc6699.ngrok-free.app").AllowAnyHeader().AllowAnyMethod()); //ngrok http https://localhost:7145
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MauiCors);

app.UseAuthorization();

app.MapControllers();

app.Run();