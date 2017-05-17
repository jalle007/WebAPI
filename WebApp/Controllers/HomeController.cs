using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        string apiURL= "http://localhost:7368/";
        string currentUser = "user1";

        public  IActionResult Index()
        {
            var message = GetImages().Result;

            var result = new List<ImageView>();
            result= JsonConvert.DeserializeObject<List<ImageView>>(message);

            ViewBag.message=message;
            return View(result);
        }
        
        public async Task<string> GetImages () { 
           string textResult;
           using (var client = new HttpClient())
              {
                  var response = await client.GetAsync(apiURL+"api/getimages/user/" + currentUser);
                  textResult = await response.Content.ReadAsStringAsync();
              }
            return textResult;
        }

          public  IActionResult Like(int id, bool like)
        {
            var response = CallLike(id, like).Result;

           
            return    RedirectToAction("Index");
        }

         public async Task<string> CallLike (int imageId, bool like) { 
           string textResult;
         //string userId, int platformId, int imageId, bool likes
         //var data = new { 
         //   userId=currentUser,
         //   platformId=1,
         //   imageId=imageId,
         //   likes=like
         //};
         //var stringContent = new StringContent(data.ToString());

           using (var client = new HttpClient())
              {
                  var response = await client.PostAsync(apiURL+"api/likes/user1/1/" + imageId + "/" + like, null);
                  textResult = await response.Content.ReadAsStringAsync();
              }
            return textResult;
        }

        public IActionResult About(){return View();}
        public IActionResult Contact(){return View();}
        public IActionResult Error(){return View();}
    }
}
