var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the INumberStore and ICalculator services
builder.Services.AddSingleton<WillsCalculator.Server.Interfaces.INumberStore, WillsCalculator.Server.Services.NumberStore>();
builder.Services.AddTransient<WillsCalculator.Server.Interfaces.ICalculator, WillsCalculator.Server.Services.Calculator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:4200") // Replace with the URL of your Angular app
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
