using ApiConsumeInProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ApiConsumeInProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = new List<Employee>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7014");
            HttpResponseMessage response = await client.GetAsync("api/Employee");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<List<Employee>>(result);
            }
            return View(employees);
        }


        public async Task<IActionResult> Details(int Id )
        {
            Employee employee = await GetEmployeeById(Id);
            return View(employee);
        }

        private static async Task<Employee> GetEmployeeById(int Id)
        {
            Employee employee = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7014");
            HttpResponseMessage response = await client.GetAsync($"api/Employee/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                employee = JsonConvert.DeserializeObject<Employee>(result);
            }

            return employee;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            //Employee employee = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7014");
            var response = await client.PostAsJsonAsync($"api/Employee",employee);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Delete(int Id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7014");
            HttpResponseMessage response = await client.DeleteAsync($"api/Employee/{Id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Employee employee = await GetEmployeeById(Id);
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7014");
            var response = await client.PutAsJsonAsync($"api/Employee/{employee.Id}", employee);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}