using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Controllers
builder.Services.AddControllers();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AuthAPI",
        Version = "v1"
    });
});

// ✅ CORS (Angular Local + EC2)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins(
                "http://localhost:4200",      // ✅ Angular local
                "http://13.127.194.181"        // ✅ Angular hosted on EC2
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

// ✅ Swagger enabled in EC2 also
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthAPI v1");
    c.RoutePrefix = "swagger";
});

// ✅ Redirect HTTP to HTTPS (optional)
// If you are only using HTTP behind Nginx, you can comment this line
app.UseHttpsRedirection();

// ✅ Apply CORS before mapping controllers
app.UseCors("AllowAngular");

app.MapControllers();

app.Run();
