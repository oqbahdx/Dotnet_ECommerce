

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
        options.RoutePrefix = string.Empty; // ?? this makes Swagger UI the default page
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Important: map controllers
app.MapControllers();

app.Run();
