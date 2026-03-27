using Microsoft.AspNetCore.Mvc;
using FincaMVC.Models;
using System.Net.Http.Json;

namespace FincaMVC.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DepartamentosController(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("API");
            var departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos");
            return View(departamentos.Where(d => d.IdEstado != 2).ToList()); 
        }

        public IActionResult Create()
        {
            return View(new Departamento()); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(Departamento departamento)
        {
            var client = _httpClientFactory.CreateClient("API");
            
            departamento.IdEstado = 1;
            await client.PostAsJsonAsync("api/departamentos", departamento);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var departamento = await client.GetFromJsonAsync<Departamento>($"api/departamentos/{id}");
            if (departamento == null) return NotFound();
            return View(departamento);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Departamento departamento)
        {
            var client = _httpClientFactory.CreateClient("API");
            await client.PutAsJsonAsync($"api/departamentos/{departamento.IdDepartamento}", departamento);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            await client.DeleteAsync($"api/departamentos/{id}");
            return RedirectToAction("Index");
        }
    }
}