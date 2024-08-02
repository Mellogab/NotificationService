using NotificationService.Core.Patterns.Factory;
using NotificationService.Core.Services;
using NotificationService.Core.UseCases.Notification;
using NotificationService.Core;
using _NotificationService.Presenters;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.AutoMapper;
using NotificationService.Infrastructure.Patterns.Factory;
using Serilog;
using Serilog.Events;
using NotificationService.Core.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/**
 * 1. UseCase Injections
 * **/

builder.Services.AddTransient<INotificationUseCase, NotificationUseCase>();

builder.Services.AddTransient<NotificationService.Core.Presenters.DefaultPresenter<UseCaseResponseMessage>>();
builder.Services.AddTransient(typeof(NotificationService.Core.Presenters.DefaultPresenter<>), typeof(NotificationService.Core.Presenters.DefaultPresenter<>));
builder.Services.AddTransient<DefaultPresenter<UseCaseResponseMessage>>();
builder.Services.AddTransient(typeof(DefaultPresenter<>), typeof(DefaultPresenter<>));

/**
 * 2. Services Injections
 * **/
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddMemoryCache();

/**
 * 3. AutoMapper
 * **/

builder.Services.AddAutoMapper(mapper =>
{
    mapper.AddProfile<MappingProfile>();
});

/**
 * 4. Patterns
 * **/

builder.Services.AddSingleton<INotificationSenderFactory, NotificationSenderFactory>();


/**
 * 5. ClientsAPIs Injections
 * **/

builder.Services.AddSingleton(builder.Configuration.GetSection("SMPTConfigurations").Get<SMPTConfigurations>());

/**
 * 6 Configure logs for application
 * **/
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

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
