using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MemberPortal.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using MemberPortal.Models;
using Microsoft.CodeAnalysis.Operations;

namespace MemberPortal.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            _log4net.Info(nameof(Index) + " method invoked.");
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]MemberLogin memberDetail)
        {
            try
            {
                _log4net.Info(nameof(Login) + " method invoked by the Username :" + memberDetail.Username);
                string token = GetToken("https://localhost:44392/api/Auth/Login", memberDetail);

                if (token != null)
                {
                    using (var client = new HttpClient())
                    {
                        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                        client.DefaultRequestHeaders.Accept.Add(contentType);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpContext.Session.SetString("Username", memberDetail.Username);

                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                return View("Index");
            }
            catch(Exception e)
            {

                ErrorViewModel error = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
                _log4net.Info("Error occured in " + nameof(Login) + " Error message:  " + error.ErrorMessage);
                return View("Error",error);
            }
           
        }

      

        public string GetToken(string url, MemberLogin user)
        {
            try
            {
                _log4net.Info(nameof(GetToken) + " method invoked.");
                var jsonData = JsonConvert.SerializeObject(user);
                var encodedData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = client.PostAsync(url, encodedData).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string tokenString = response.Content.ReadAsStringAsync().Result;
                    return tokenString;
                }
                return null;
            }
            catch(Exception e)
            {
                _log4net.Info("Error occured in " + nameof(Login) + " Error message:  " + e.Message);
                throw e;
            }
           
            
        }

    }
}
