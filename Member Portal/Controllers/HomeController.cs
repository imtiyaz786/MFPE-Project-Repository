using MemberMicroservice.Models;
using MemberPortal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MemberPortal.Controllers
{


    public class HomeController : Controller
    {


        private static int count = 1;
        private static MockDatabase _data = new MockDatabase();
        private readonly MockDbContext _context;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(HomeController));

        public HomeController(MockDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (CheckStatus())
            {
                _log4net.Info(nameof(Index) + " method invoked.");
                ViewBag.UserName = HttpContext.Session.GetString("Username");
                _log4net.Info(ViewBag.UserName + " logged in.");
                return View();
            }

            return RedirectToAction("Index", "Login");
        }


        private bool CheckStatus()
        {
            if (HttpContext.Session.GetString("Username") != null)
                return true;
            return false;
        }

        public IActionResult ViewBill()
        {
            try
            {
                if (CheckStatus())
                {
                    if (_data.PolicyID != 0)
                    {
                        _log4net.Info(nameof(ViewBill) + " method invoked!");
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage response = new HttpResponseMessage();
                            response = client.GetAsync("https://localhost:44355/api/Members/viewBills?policyID=" + _data.PolicyID + "&memberID=" + _data.MemberID).Result;
                            var data = JsonConvert.DeserializeObject<MemberPremium>(response.Content.ReadAsStringAsync().Result);

                            return View(data);
                        }
                    }
                    return View();
                }
                return RedirectToAction("Index", "Login");
            }
            catch (Exception e)
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
                _log4net.Error("Error Ocuured in " + nameof(ViewBill) + " Error message : " + e.Message);
                return View("Error", error);
            }



        }

        public IActionResult SubmitClaim()
        {
            if (CheckStatus())
            {
                _log4net.Info(nameof(SubmitClaim) + " method invked.");
                return View();
            }
            return RedirectToAction("Index", "Login");

        }


        public IActionResult ClaimStatus()
        {
            if (CheckStatus())
            {
                _log4net.Info(nameof(ClaimStatus) + " method invked.");
                return View();
            }
            return RedirectToAction("Index", "Login");

        }

        [HttpPost]
        public IActionResult SubmitStatus([FromForm] MockDatabase data)
        {
            try
            {
                _log4net.Info(nameof(SubmitStatus) + " mehod invoked with Memeber Id " + data.MemberID);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = client.GetAsync("https://localhost:44355/api/Members/getClaimStatus?claimID=" + data.ClaimID + "&policyID=" + data.PolicyID + "&memberID=" + data.MemberID).Result;
                    var responseData = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                    ViewBag.Status = responseData;
                    return View("Status");
                }
            }
            catch (Exception e)
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
                _log4net.Error("Error Ocuured in " + nameof(SubmitStatus) + " Error message : " + e.Message);
                return View("Error", error);
            }

        }

        [HttpPost]
        public IActionResult SaveClaim([FromForm] MockDatabase data)
        {
            try
            {
                _data.TransactionID = Guid.NewGuid();
                _data.BenefitID = data.BenefitID;
                _data.PolicyID = data.PolicyID;
                _data.MemberID = data.MemberID;
                _data.BenefitName = data.BenefitName;
                _data.ClaimID = count++;
                _data.ClaimAmount = data.ClaimAmount;
                _data.HospitalId = data.HospitalId;
                _context.MockDatabases.Add(_data);
               // _context.SaveChanges();


                _log4net.Info(nameof(SaveClaim) + " method invoked.");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = new HttpResponseMessage();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                    response = client.PostAsync("https://localhost:44355/api/Members/submitClaim?policyID=" + data.PolicyID + "&memberID=" + data.MemberID + "&benefitID=" + data.BenefitID + "&hospitalID=" + data.HospitalId + "&claimAmt=" + data.ClaimAmount + "&benefit=\"" + data.BenefitName + "\"", content).Result;
                    var responseData = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                    // ViewBag.Status = responseData;
                    // return View("Status");
                    ViewBag.Message1 = data.PolicyID;
                    ViewBag.Message2 = data.MemberID;
                    ViewBag.Message3 = data.BenefitID;
                    ViewBag.Message4 = data.HospitalId;
                    ViewBag.Message5 = data.ClaimAmount;
                    ViewBag.Message6 = data.BenefitName;
                    return View("Claim");
                }
            }
            catch (Exception e)
            {

                ErrorViewModel error = new ErrorViewModel
                {
                    ErrorMessage = e.Message
                };
                _log4net.Error("Error Ocuured in " + nameof(SaveClaim) + " Error message : " + e.Message);
          
                return View("Error", error);
               
            }

        }


        public IActionResult Logout()
        {
            _log4net.Info(nameof(Logout) + " method invoked. " + ViewBag.UserName + " logged out.");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }


    }
}
