using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Filters;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    [PaginaUserLogado]
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
