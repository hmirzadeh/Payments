using PaymentGateway.Application.Features.Payment.Services;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using PaymentGateway.Application.Features.Payment.CommandModels;
using PaymentGateway.Domain.Configurations;
using PaymentGateway.Application.Infrastructure;
using PaymentGateway.Application.Common.Validators;
using PaymentGateway.Application.Common.Mappers;
using PaymentGateway.Infrastructure.External;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddTransient<IValidator<ProcessPaymentCommand>, PaymentValidator>();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(PaymentMapping).Assembly, Assembly.GetExecutingAssembly());
builder.Services.AddOptions();
builder.Services.Configure<AcquiringBank>(
    builder.Configuration.GetSection("AcquiringBank"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddSingleton<IHttpProcessor, HttpProcessor>();

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
