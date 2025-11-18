using MiAppCharca.Configuration;

var builder = WebApplication.CreateBuilder(args);

// ===== AGREGAR CONTROLLERS =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ===== CONFIGURACIÓN CENTRALIZADA =====
builder.Services.AddApiServices(builder.Configuration);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
// ===== BUILD APP =====
var app = builder.Build();

// ===== CONFIGURAR MIDDLEWARE =====
app.ConfigureApiMiddleware();

app.Run();