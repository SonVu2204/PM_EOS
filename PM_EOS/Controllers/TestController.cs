using Microsoft.AspNetCore.Mvc;

namespace PM_EOS.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
