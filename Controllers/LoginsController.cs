using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using DNTCaptcha.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy_DAO;
using System.Text;

namespace Online_Pharmacy_App_MVC.Controllers
{
    public class LoginsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IDNTCaptchaValidatorService _validatorService;
        public LoginsController(HttpClient httpClient, IDNTCaptchaValidatorService validatorService)
        {
            _httpClient = httpClient;
            _validatorService = validatorService;
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string username)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Logins/{username}");
            if (response.IsSuccessStatusCode)
            {
                if (ModelState.IsValid)
                {
                    if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                    {
                        this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");
                    }
                    else
                    {
                        var password = await response.Content.ReadAsStringAsync();
                        if (password == "")
                        {
                            return View();
                        }
                        else
                        {
                            ViewBag.Password = password;
                            return View();
                        }
                    }

                }
            }
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please Enter Valid Captcha",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> Login(string username, string password)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Logins/{username}/{password}");
            if (response.IsSuccessStatusCode)
            {
                if (ModelState.IsValid)
                {
                    if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                    {
                        this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");
                    }
                    else
                    {
                        var logintype = await response.Content.ReadAsStringAsync();
                        //var logintype = JsonConvert.DeserializeObject<string>(jsondata);
                        if (logintype == "")
                        {
                            return View();
                        }
                        else if (logintype == "C")
                        {
                            return RedirectToAction(nameof(Default), "Customers");
                        }
                        else if (logintype == "A")
                        {
                            return RedirectToAction(nameof(Default), "Admins");
                        }
                        else
                        {
                            return RedirectToAction(nameof(Login), logintype);
                        }
                    }
                }
                return View();

            }
            return View();
        }
        // GET: AdminsController   
        public async Task<IActionResult> Default()
        {
            var response = await _httpClient.GetAsync("http://localhost:5278/api/Logins");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var login = JsonConvert.DeserializeObject<List<Login>>(jsondata);
                return View(login);
            }
            return View();
        }



        // GET: LoginsController/Details/5
        public ActionResult GetLoginDetails(int id)
        {
            return View();
        }

        // GET: LoginsController/Create
        public ActionResult AddLoginDetails()
        {
            return View();
        }

        // POST: LoginsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLoginDetails(Login login)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5278/api/Logins", login);
            return RedirectToAction(nameof(Default));
        }

        
        public async Task<IActionResult> UpdateLoginDetails(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/Logins/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var login = JsonConvert.DeserializeObject<Login>(jsondata);
                return View(login);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLoginDetails(int id, Login login)
        {
            if (id != login.LoginId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var jsondata = JsonConvert.SerializeObject(login);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"http://localhost:5278/api/Logins/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Default));
                }
            }
            return View(login);
        }
        public ActionResult Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login), "Logins");
        }

        // GET: LoginsController/Delete/5
        public ActionResult DeleteLoginDetails(int id)
        {
            return View();
        }

        // POST: LoginsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLoginDetails(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
