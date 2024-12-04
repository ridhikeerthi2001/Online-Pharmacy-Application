using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy_DAO;
using System.Text;

namespace Online_Pharmacy_App_MVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly HttpClient _httpClient;
        public OrdersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: AdminsController
        public async Task<IActionResult> Default()
        {
            var response = await _httpClient.GetAsync("http://localhost:5278/api/Orders");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<List<Order>>(jsondata);
                return View(order);
            }
            return View();
        }
        
        public async Task<IActionResult> FetchOrder(int? id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(jsondata);
                return View(order);
            }
            return NotFound();
        }

        // GET: OrdersController/Create
        public ActionResult AddOrder()
        {
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(order);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5278/api/Orders", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(order);            
        }
        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(jsondata);
                return View(order);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(order);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"http://localhost:5278/api/Orders/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(order);
        }

       

        // GET: OrdersController/Delete/5
        public ActionResult DeleteOrder(int id)
        {
            return View();
        }
        public async Task<IActionResult> RemoveOrder(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(jsondata);
                return View(order);
            }
            return NotFound();
        }

        [HttpPost, ActionName("RemoveOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5278/api/Orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Default));
            }
            return View();

        }
    }
}
