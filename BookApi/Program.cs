using BookApi.Database;
using BookApi.Mapper;
using BookApi.Services;
using BookApi.Services.Contracts;
using MessageBus;
using MessageBus.Contracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbConnection = builder.Configuration.GetConnectionString("DefaultDbConnection");
var rabbitConnection = builder.Configuration.GetConnectionString("RabbitMQConnection");

var services = builder.Configuration.GetSection("ServicesUrl");
var gateway = services.GetValue<string>("Gateway");

if (string.IsNullOrEmpty(gateway) || string.IsNullOrEmpty(rabbitConnection))
{
    throw new AggregateException(nameof(builder.Configuration));
}

var corsOrigins = new string[] 
{
    gateway
};

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(dbConnection);
});
builder.Services.AddTransient<IBookService, BookService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

builder.Services.AddSingleton<IMessageBus, RabbitBus>((provider) => new RabbitBus(rabbitConnection));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        config.RoutePrefix = string.Empty;
    });
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