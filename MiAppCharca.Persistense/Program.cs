using MiAppCharca.Configuration;

var builder = WebApplication.CreateBuilder(args);

// ===== AGREGAR CONTROLLERS =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ===== CONFIGURACIÓN CENTRALIZADA =====
builder.Services.AddApiServices(builder.Configuration);

// ===== BUILD APP =====
var app = builder.Build();

// ===== CONFIGURAR MIDDLEWARE =====
app.ConfigureApiMiddleware();

app.Run();