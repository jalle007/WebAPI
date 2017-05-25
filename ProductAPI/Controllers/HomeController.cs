using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Service;

namespace ProductAPI.Controllers {
  public class HomeController : Controller
    {
        private readonly ProductLikesContext _context;
        public HomeController(ProductLikesContext context)
        {
            _context = context;    
        }
 
        public async Task<IActionResult> Index()
        {
            return View();
        }
     }
}
