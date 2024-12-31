using Microsoft.AspNetCore.Mvc;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
