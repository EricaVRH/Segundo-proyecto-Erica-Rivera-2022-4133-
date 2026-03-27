using FincaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;


namespace FincaMVC.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmpleadosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            var client = _httpClientFactory.CreateClient("API");
            
            var departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos") ?? new();
            var puestos = await client.GetFromJsonAsync<List<Puesto>>("api/puestos") ?? new();
                        
            ViewBag.Departamentos = departamentos;
            ViewBag.Puestos = puestos;

            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            var client = _httpClientFactory.CreateClient("API");

            if (ModelState.IsValid)
            {
                empleado.IdEstado = 1; // Activo por defecto

                // Logica de la Foto
                if (empleado.Foto != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(empleado.Foto.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await empleado.Foto.CopyToAsync(stream);
                    }
                    empleado.FotoRuta = "/img/" + fileName;
                }

                var response = await client.PostAsJsonAsync("api/empleados", empleado);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
                        
            ViewBag.Departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos") ?? new();
            ViewBag.Puestos = await client.GetFromJsonAsync<List<Puesto>>("api/puestos") ?? new();

            return View(empleado); 
        }

        // GET Edit
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("API");

            var empleado = await client.GetFromJsonAsync<Empleado>($"api/empleados/{id}");
            if (empleado == null)
                return RedirectToAction("Index");
                        
            var departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos") ?? new List<Departamento>();
            var puestos = await client.GetFromJsonAsync<List<Puesto>>("api/puestos") ?? new List<Puesto>();
            
            ViewBag.Departamentos = departamentos;
            ViewBag.Puestos = puestos;
            
            return View(empleado);
        }

        // POST Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Empleado empleado)
        {
            var client = _httpClientFactory.CreateClient("API");
                       
            if (empleado.Foto != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(empleado.Foto.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await empleado.Foto.CopyToAsync(stream);

                empleado.FotoRuta = "/img/" + fileName;
            }
            // Si no sube foto, mantener la ruta existente
            else
            {
                var empleadoActual = await client.GetFromJsonAsync<Empleado>($"api/empleados/{empleado.IdEmpleado}");
                empleado.FotoRuta = empleadoActual?.FotoRuta;
            }

            var response = await client.PutAsJsonAsync($"api/empleados/{empleado.IdEmpleado}", empleado);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
                        
            var departamentos = await client.GetFromJsonAsync<List<Departamento>>("api/departamentos") ?? new List<Departamento>();
            var puestos = await client.GetFromJsonAsync<List<Puesto>>("api/puestos") ?? new List<Puesto>();
            var estados = await client.GetFromJsonAsync<List<EstadoEmpleado>>("api/estados") ?? new List<EstadoEmpleado>();

            ViewBag.Departamentos = departamentos;
            ViewBag.Puestos = puestos;
            ViewBag.Estados = estados;

            return View(empleado);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("API");

            await client.DeleteAsync($"api/empleados/{id}");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Activar(int id)
        {
            var client = _httpClientFactory.CreateClient("API");

            var response = await client.PutAsync($"api/empleados/activar/{id}", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo activar el empleado.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Despedir(int id)
        {
            var client = _httpClientFactory.CreateClient("API");

            var response = await client.PutAsync($"api/empleados/despedir/{id}", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo despedir al empleado.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(
            string nombre = "",
            string apellido = "",
            DateTime? fechaContratacion = null,
            int page = 1,
            int pageSize = 5)
        {
            var client = _httpClientFactory.CreateClient("API");

            // query string con los filtros
            var url = $"api/empleados?nombre={nombre}&apellido={apellido}&page={page}&pageSize={pageSize}";
            if (fechaContratacion.HasValue)
                url += $"&fechaContratacion={fechaContratacion:yyyy-MM-dd}";

            var response = await client.GetFromJsonAsync<ApiResponse>(url);

            var empleados = response?.data ?? new List<Empleado>();

            ViewBag.CurrentPage = response?.page ?? 1;
            ViewBag.TotalPages = (int)Math.Ceiling((double)(response?.total ?? 0) / pageSize);

            ViewBag.Nombre = nombre;
            ViewBag.Apellido = apellido;
            ViewBag.FechaContratacion = fechaContratacion?.ToString("yyyy-MM-dd");
            
            return View(empleados);
        }

        [HttpGet]
        public async Task<JsonResult> GetPuestos(int departamentoId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var puestos = await client.GetFromJsonAsync<List<Puesto>>($"api/puestos/por-departamento/{departamentoId}");
            return Json(puestos);
        }

    }
}
