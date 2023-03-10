using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Repositories;
using MortgageCalculator.Core.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
builder.Services.Configure<ConnectionStringsConfig>(config.GetSection(ConnectionStringsConfig.Section));
builder.Services.Configure<DatabaseNamesConfig>(config.GetSection(DatabaseNamesConfig.Section));
builder.Services.Configure<CollectionNamesConfig>(config.GetSection(CollectionNamesConfig.Section));

builder.Services.AddTransient(s =>
{
    var connString = s.GetService<IOptions<ConnectionStringsConfig>>()?.Value.Local
        ?? throw new InvalidDataException($"Connection String '{nameof(ConnectionStringsConfig.Local)}' has not been provided in appsettings.");

    var client = new MongoClient(connString);
    var databaseName = s.GetService<IOptions<DatabaseNamesConfig>>()?.Value.Mortgage
        ?? throw new InvalidDataException($"Database Name '{nameof(DatabaseNamesConfig.Mortgage)}' has not been provided in appsettings.");

    return client.GetDatabase(databaseName);
});

builder.Services.AddTransient<ISeedRepositoryData, SeedRepositoryData>();
builder.Services.AddTransient<IMongoRepository<UsefulLink>, UsefulLinksRepository>();
builder.Services.AddTransient<IMongoRepository<MortgagePayment>, MortgagePaymentsRepository>();
builder.Services.AddTransient<IMongoRepository<Mortgage>, MortgageRepository>();

builder.Services.AddTransient<IMortgagePaymentsRepository, MortgagePaymentsRepository>();
builder.Services.AddTransient<IMortgageRepository, MortgageRepository>();

builder.Services.AddTransient<IInterestCalculator, InterestCalculator>();
builder.Services.AddTransient<IMortgagePayments, MortgagePayments>();
builder.Services.AddTransient<IPublicHolidayChecker, PublicHolidayChecker>();
builder.Services.AddTransient<IMortgageForecaster, MortgageForecaster>();

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

app.UseCors(opt => opt
    .AllowAnyOrigin()
    .WithMethods("GET", "POST", "OPTIONS", "PATCH", "DELETE")
    .WithHeaders("Authorization", "Content-Type"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var seeder = app.Services.GetService<ISeedRepositoryData>()
    ?? throw new InvalidOperationException($"The dependency '{nameof(ISeedRepositoryData)}' has not been set up.");
await seeder.SeedData();

app.Run();
