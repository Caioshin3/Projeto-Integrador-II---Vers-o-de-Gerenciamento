using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Helpter;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessao _sessao;

        public LoginController(IUserRepository userRepository
                               ,ISessao sessao)
        {
            _userRepository = userRepository;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuarios();

            return RedirectToAction("Index", "Login");   
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = _userRepository.BuscarPorLogin(loginModel.Login);

                    if (user != null)
                    {
                        if (user.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(user);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["MensagemErro"] = $"Senha do usuário é inválida, tente novamente.";
                    }
                    else
                    {
                        TempData["MensagemErro"] = $"Ops, usuário e/ou senhas inválido(s)";
                    }
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar o seu login, tente novamente, detalhe do erro:{ex.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
