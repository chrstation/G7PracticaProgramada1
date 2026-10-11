using GestorClientes.Consola;
using GestorClientes.Datos;
using GestorClientes.Negocio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// Configuración de la inyección de dependencias.
var servicios = new ServiceCollection();
servicios.AddDbContext<AppDbContext>(opciones => opciones.UseSqlite("Data Source=clientes.db"));
servicios.AddScoped<IClienteRepositorio, ClienteRepositorio>();
servicios.AddScoped<IClienteServicio, ClienteServicio>();
servicios.AddScoped<MenuClientes>();

using var proveedor = servicios.BuildServiceProvider();
using var alcance = proveedor.CreateScope();

// Crea la base de datos si todavía no existe.
alcance.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();

await alcance.ServiceProvider.GetRequiredService<MenuClientes>().EjecutarAsync();
