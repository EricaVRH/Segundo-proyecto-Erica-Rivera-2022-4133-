using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FincaMVC.Models;

namespace FincaMVC.Controllers
{
    public class PuestosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PuestosController(IHttpClientFactory clientFactory) => _httpClientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("API");
            var puestos = await client.GetFromJsonAsync<List<Puesto>>("api/puestos");
            return View(puestos.Where(p => p.IdEstado != 2).ToList()); // Oculta los despedidos
        }

        public async Task<IActionResult> Create()
        {
            var client = _httpClientFactory.CreateClient("API");
            var departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos");
            ViewBag.Departamentos = new SelectList(departamentos.Where(d => d.IdEstado == 1), "IdDepartamento", "Nombre");
            return View(new Puesto()); // Pasamos un modelo vacío para evitar null
        }

        [HttpPost]
        public async Task<IActionResult> Create(Puesto puesto)
        {
            var client = _httpClientFactory.CreateClient("API");
            await client.PostAsJsonAsync("api/puestos", puesto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("API");

            var puesto = await client.GetFromJsonAsync<Puesto>($"api/puestos/{id}");
            if (puesto == null)
                return RedirectToAction("Index");

            // Traer departamentos para el combo
            var departamentos = new List<Departamento>();
            try
            {
                departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos") ?? new();
            }
            catch
            {
                departamentos = new List<Departamento>(); // nunca nulo
            }
            ViewBag.Departamentos = departamentos;

            return View(puesto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Puesto puesto)
        {
            var client = _httpClientFactory.CreateClient("API");

            var response = await client.PutAsJsonAsync($"api/puestos/{puesto.IdPuesto}", puesto);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
                       
            var departamentos = new List<Departamento>();
            try
            {
                departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos") ?? new();
            }
            catch
            {
                departamentos = new List<Departamento>();
            }
            ViewBag.Departamentos = departamentos;

            return View(puesto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            await client.DeleteAsync($"api/puestos/{id}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Activar(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            await client.PutAsync($"api/puestos/activar/{id}", null);
            return RedirectToAction("Index");
        }
    }
}