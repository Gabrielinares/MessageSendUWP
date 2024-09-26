using MessageSendApi.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using MessageSendApi.Services.Interfaces;
using MessageSendApi.Repository.Interfaces;
using MessageSendApi.Services;
using MessageSendApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Environment Variables
Env.Load();

// Controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ISendingService, SendingService>();
builder.Services.AddScoped<ISendingRepository, SendingRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
