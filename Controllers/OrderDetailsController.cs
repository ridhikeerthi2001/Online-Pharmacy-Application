using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy_DAO;
using System.Text;

namespace Online_Pharmacy_App_MVC.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly HttpClient _httpClient;
        public OrderDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: AdminsController
        public async Task<IActionResult> Default()
        {
            var response = await _httpClient.GetAsync("http://localhost:5278/api/OrderDetails");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var orderdetails = JsonConvert.DeserializeObject<List<OrderDetail>>(jsondata);
                return View(orderdetails);
            }
            return View();
        }

        public async Task<IActionResult> GetOrderDetails(int? id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/OrderDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var orderdetail = JsonConvert.DeserializeObject<OrderDetail>(jsondata);
                return View(orderdetail);
            }
            return NotFound();
        }


        // GET: OrderDetailsController/Create
        public ActionResult AddOrderDetails()
        {
            return View();
        }

        // POST: OrderDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrderDeatils(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(orderdetail);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5278/api/OrderDetails", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(orderdetail);
        }



        public async Task<IActionResult> UpdateOrderDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/OrderDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var orderdetail = JsonConvert.DeserializeObject<OrderDetail>(jsondata);
                return View(orderdetail);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetails(int id, OrderDetail orderdetail)
        {
            if (id != orderdetail.OrderDetailId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(orderdetail);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"http://localhost:5278/api/OrderDetails/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(orderdetail);
        }

        // GET: OrderDetailsController/Delete/5
        public ActionResult DeleteOrderDetails(int id)
        {
            return View();
        }
        public async Task<IActionResult> RemoveOrderDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/OrderDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var orderdetail = JsonConvert.DeserializeObject<OrderDetail>(jsondata);
                return View(orderdetail);
            }
            return NotFound();
        }

        [HttpPost, ActionName("RemoveOrderDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5278/api/OrderDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Default));
            }
            return View();

        }
    }
}
