using GestorClientes.Negocio;
using GestorClientes.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestorClientes.Web.Controllers;

/// <summary>
/// Controlador MVC para la gestión de clientes. Solo se comunica con la capa de negocio.
/// </summary>
public class ClientesController : Controller
{
    private readonly IClienteServicio _servicio;

    public ClientesController(IClienteServicio servicio)
    {
        _servicio = servicio;
    }

    // GET: /Clientes
    public async Task<IActionResult> Index()
    {
        var clientes = await _servicio.ListarAsync();

        return View(clientes.Select(ClienteViewModel.DesdeEntidad).ToList());
    }

    // GET: /Clientes/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var cliente = await BuscarClienteAsync(id);

        return cliente is null ? NotFound() : View(cliente);
    }

    // GET: /Clientes/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new ClienteViewModel { Telefonos = { new TelefonoViewModel() } });
    }

    // POST: /Clientes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        try
        {
            await _servicio.RegistrarAsync(modelo.ComoEntidad());
            TempData["Mensaje"] = "Cliente registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidacionException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            return View(modelo);
        }
    }

    // GET: /Clientes/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var cliente = await BuscarClienteAsync(id);

        return cliente is null ? NotFound() : View(cliente);
    }

    // POST: /Clientes/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ClienteViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        try
        {
            await _servicio.ModificarAsync(modelo.ComoEntidad());
            TempData["Mensaje"] = "Cliente modificado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ValidacionException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            return View(modelo);
        }
    }

    // GET: /Clientes/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await BuscarClienteAsync(id);

        return cliente is null ? NotFound() : View(cliente);
    }

    // POST: /Clientes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _servicio.BorrarAsync(id);
            TempData["Mensaje"] = "Cliente borrado correctamente.";
        }
        catch (ValidacionException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error()
    {
        return View();
    }

    private async Task<ClienteViewModel?> BuscarClienteAsync(int id)
    {
        try
        {
            var cliente = await _servicio.ObtenerDetalleAsync(id);

            return ClienteViewModel.DesdeEntidad(cliente);
        }
        catch (ValidacionException)
        {
            return null;
        }
    }
}
