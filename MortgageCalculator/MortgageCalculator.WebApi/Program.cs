using MortgageCalculator.Core.Config;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
builder.Services.Configure<ConnectionStringsConfig>(config.GetSection(ConnectionStringsConfig.Section));
builder.Services.Configure<DatabaseNamesConfig>(config.GetSection(DatabaseNamesConfig.Section));
builder.Services.Configure<CollectionNamesConfig>(config.GetSection(CollectionNamesConfig.Section));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
