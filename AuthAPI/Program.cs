var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ ADD THIS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// ✅ ADD THIS (before MapControllers)
app.UseCors("AllowAngular");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
