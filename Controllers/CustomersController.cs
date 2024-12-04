using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy_DAO;
using System.Text;

namespace Online_Pharmacy_App_MVC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IDNTCaptchaValidatorService _validatorService;
        public CustomersController(HttpClient httpClient, IDNTCaptchaValidatorService validatorService)
        {
            _httpClient = httpClient;
            _validatorService = validatorService;
        }
        // GET: AdminsController
        public async Task<IActionResult> Default()
        {
            var response = await _httpClient.GetAsync("http://localhost:5278/api/Customers");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<List<Customer>>(jsondata);
                return View(customer);
            }
            return View();
        }


        public async Task<IActionResult> GetCustomerDetails(int? id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Customers/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(jsondata);
                return View(customer);
            }
            return NotFound();
        }

        // GET: CustomersController/Create
        public ActionResult AddCustomerDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please Enter Valid Captcha",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> AddCustomerDetails(CustomerLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");
                }
                else
                {
                    var response1 = await _httpClient.PostAsJsonAsync("http://localhost:5278/api/Logins", model.Login);
                    var response2 = await _httpClient.PostAsJsonAsync("http://localhost:5278/api/Customers", model.Customer);
                    return RedirectToAction(nameof(Login), "Logins");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateCustomerDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Customers/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(jsondata);
                return View(customer);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCustomerDetails(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(customer);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"http://localhost:5278/api/Customers/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(customer);
        }

                    

        // GET: CustomersController/Delete/5
        public ActionResult DeleteCustomerDetails(int id)
        {
            return View();
        }

    
        public async Task<IActionResult> RemoveCustomerDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Customers/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(jsondata);
                return View(customer);
            }
            return NotFound();
        }

        [HttpPost, ActionName("RemoveCustomerDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5278/api/Customers/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Default));
            }
            return View();

        }
    }
}
