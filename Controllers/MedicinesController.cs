using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy_DAO;
using System.Text;

namespace Online_Pharmacy_App_MVC.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly HttpClient _httpClient;
        public MedicinesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: AdminsController
        public async Task<IActionResult> Default()
        {
            var response = await _httpClient.GetAsync("http://localhost:5278/api/Medicines");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var medicine = JsonConvert.DeserializeObject<List<Medicine>>(jsondata);
                return View(medicine);
            }
            return View();
        }
        public async Task<IActionResult> UserMedList()
        {
            var response = await _httpClient.GetAsync("http://localhost:5278/api/Medicines");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var medicine = JsonConvert.DeserializeObject<List<Medicine>>(jsondata);
                return View(medicine);
            }
            return View();
        }


        public async Task<IActionResult> GetMedicineDetails(int? id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Medicines/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var medicine = JsonConvert.DeserializeObject<Medicine>(jsondata);
                return View(medicine);
            }
            return NotFound();
        }

        // GET: MedicinesController/Create
        public ActionResult AddMedicineDetails()
        {
            return View();
        }

        // POST: MedicinesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicineDetails(Medicine medicine)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5278/api/Medicines", medicine);
            return RedirectToAction(nameof(Default));
        }

        public async Task<IActionResult> UpdateMedicineDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Medicines/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var medicine = JsonConvert.DeserializeObject<Medicine>(jsondata);
                return View(medicine);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMedicineDetails(int id, Medicine medicine)
        {
            if (id != medicine.MedicineId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(medicine);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"http://localhost:5278/api/Medicines/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(medicine);
        }
        // GET: MedicinesController/Delete/5
        public ActionResult DeleteMedicineDetails(int id)
        {
            return View();
        }

        public async Task<IActionResult> RemoveMedicineDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Medicines/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var medicine = JsonConvert.DeserializeObject<Medicine>(jsondata);
                return View(medicine);
            }
            return NotFound();
        }

        [HttpPost, ActionName("RemoveMedicineDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5278/api/Medicines/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Default));
            }
            return View();

        }
    }
}
