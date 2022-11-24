using BasketApi.Mapper;
using BasketApi.Database;
using Microsoft.EntityFrameworkCore;
using MessageBus;
using MessageBus.Contracts;
using BasketApi.Services.Host;
using BasketApi.Services.Microservices;
using BasketApi.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

var dbConnection = builder.Configuration.GetConnectionString("DefaultDbConnection");
var rabbintConnection = builder.Configuration.GetConnectionString("RabbitMQConnection");

var services = builder.Configuration.GetSection("ServicesUrl");
var gateway = services.GetValue<string>("Gateway");
var bookApi = services.GetValue<string>("BookApi");

if (string.IsNullOrEmpty(gateway) || string.IsNullOrEmpty(bookApi))
{
    throw new ArgumentException(nameof(services));
}

var corsOrigins = new string[] 
{
    gateway,
    bookApi
};

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(config =>
{
    config.UseNpgsql(dbConnection);
});


builder.Services.AddHttpClient<BookService>(config =>
{
    config.BaseAddress = new Uri(bookApi);
});

builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IBasketService, BasketService>();

//builder.Services.AddSingleton<IMessageBus, RabbitBus>((provider) => new RabbitBus(rabbintConnection));
//builder.Services.AddHostedService<SubsriberService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(config =>
{
    config
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins(corsOrigins);
});

app.MapControllers();

app.Run();