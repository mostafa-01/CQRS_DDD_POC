using CQRSAndDDD.App.Handlers;
using CQRSAndDDD.Infrastrucure.Data;
using CQRSAndDDD.Infrastrucure.Repos.Boundry;
using CQRSAndDDD.Infrastrucure.Repos.Service;
using CQRSAndDDD_POC.Common;
using CQRSAndDDD_POC.Kafka;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddDbContext<AppDbContext>(options =>
{
    //should be configurable 
    options.UseOracle(Constatns.CONFIG_DB_CONNECTION);
});

builder.Services.AddScoped<ITicketRepo, TicketRepoImpl>();
builder.Services.AddScoped<CreateTicketCommandHandler>();

builder.Services.AddHostedService<Consumer>();
builder.Services.AddScoped<IProducer, Producer>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
