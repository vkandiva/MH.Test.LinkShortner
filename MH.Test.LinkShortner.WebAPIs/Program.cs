using MH.Test.LinkShortner.WebAPIs.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        bldr => bldr.WithOrigins("https://localhost:4200") // Allow requests only from this origin
            .AllowAnyMethod()
            .AllowAnyHeader());
});

RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowSpecificOrigin");

app.Run();

void RegisterServices(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddSingleton<InMemoryDbContextFactory>();

    webApplicationBuilder.Services.AddScoped<IMortgageHouseDbRepository, MortgageHouseDbRepository>(provider =>
    {
        var factory = provider.GetRequiredService<InMemoryDbContextFactory>();
        return new MortgageHouseDbRepository(factory.GetDbContext());
    });
}