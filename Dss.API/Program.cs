using Confluent.Kafka;
using Dss.API.kafkaEvents.UserRegistration.Consumers;
using Dss.API.kafkaEvents.UserRegistration.Handlers;
using Dss.application.Interfaces;
using Dss.Application.Interfaces;
using Dss.Application.Kafka.Messages.UserRegistration;
using Dss.Application.Services;
using Dss.Infrastructure.Persistence;
using Kafka.Consumer;
using Kafka.Interfaces;
using Kafka.Producer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Connect to PostgreSQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IAzureStorage, AzureStorage>();

var clientConfig = new ClientConfig()
{
    BootstrapServers = builder.Configuration["Kafka:ClientConfigs:BootstrapServers"]
};

var producerConfig = new ProducerConfig(clientConfig);
var consumerConfig = new ConsumerConfig(clientConfig)
{
    GroupId = "DSSAPI",
    EnableAutoCommit = true,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    StatisticsIntervalMs = 5000,
    SessionTimeoutMs = 6000
};

builder.Services.AddSingleton(producerConfig);
builder.Services.AddSingleton(consumerConfig);


builder.Services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));

builder.Services.AddScoped<IKafkaHandler<string, RegisterUser>, RegisterUserHandler>();
builder.Services.AddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));
builder.Services.AddHostedService<RegisterUserConsumer>();


// Add Serilog
builder.Host.UseSerilog((_, config) =>
{
    config.WriteTo.Console()
    .WriteTo.File("logs/dsslogs.txt", rollingInterval: RollingInterval.Day)
    .ReadFrom.Configuration(builder.Configuration);
});

//set uploadsize large files
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

//set uploadsize large files
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(120);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(120);
    //options.Limits.MaxRequestBodySize = 500_000_000;
    //options.Limits.MaxRequestBufferSize = null;
    //options.Limits.MaxResponseBufferSize = null;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Shows UseCors with CorsPolicyBuilder.
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthorization();
app.MapControllers();

app.Run();

