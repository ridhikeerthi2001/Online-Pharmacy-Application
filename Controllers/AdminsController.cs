using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy_DAO;
using System.Security.Policy;
using System.Text;

namespace Online_Pharmacy_App_MVC.Controllers
{
    public class AdminsController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: AdminsController
        public async Task<IActionResult> Default()
        {
            var response = await _httpClient.GetAsync("http://localhost:5278/api/Admins");
        if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var admin = JsonConvert.DeserializeObject<List<Admin>>(jsondata);
                return View(admin);
            }
            return View();
        }

        // GET: AdminsController/Details/5
        public async Task<IActionResult> GetAdminDetails(int? id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Admins/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var admin = JsonConvert.DeserializeObject<Admin>(jsondata);
                return View(admin);
            }
            return NotFound();
        }

        // GET: AdminsController/Create
        public ActionResult AddAdminDetails()
        {
            return View();
        }

        // POST: AdminsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdminDetails(AdminLoginViewModel model)
        {
            var response1 = await _httpClient.PostAsJsonAsync("http://localhost:5278/api/Logins", model.Login);
            var response2 = await _httpClient.PostAsJsonAsync("http://localhost:5278/api/Admins", model.Admin);

            return RedirectToAction(nameof(Login), "Logins");
        }

        // GET: AdminsController/Edit/5

        public async Task<IActionResult> UpdateAdminDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Admins/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var admin = JsonConvert.DeserializeObject<Admin>(jsondata);
                return View(admin);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdminDetails(int id, Admin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(admin);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"http://localhost:5278/api/Admins/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(admin);
        }
        // GET: AdminsController/Delete/5
        public ActionResult DeleteAdminDetails(int id)
        {
            return View();
        }

       
        public async Task<IActionResult> RemoveAdminDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Admins/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var admin = JsonConvert.DeserializeObject<Admin>(jsondata);
                return View(admin);
            }
            return NotFound();
        }

        [HttpPost, ActionName("RemoveAdminDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5278/api/Admins/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Default));
            }
            return View();

        }
    }
}
