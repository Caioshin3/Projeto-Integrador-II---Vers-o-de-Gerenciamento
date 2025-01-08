using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Filters;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    [PaginaRestritaAdm]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            List<UserModel> user = _userRepository.BuscarTodos();

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            UserModel user = _userRepository.ListarPorId(id);
            return View(user);
        }

        public IActionResult Edit(int id)
        {
            UserModel user = _userRepository.ListarPorId(id);
            return View(user);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool deleted = _userRepository.Apagar(id);

                if (deleted) TempData["MensagemSucesso"] = "Usuário apagado com sucesso!";
                else TempData["MensagemErro"] = "Ops, não conseguimos apagar o seu usuário";

                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar o seu usuário, tente novamente, detalhe do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepository.Add(user);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View("Create", user);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, usuário não foi cadastrado, tente novamente, detalhe do erro:{ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(UserNotPasswordModel usernotpass)
        {
            try
            {
                UserModel user = null;

                if (ModelState.IsValid)
                {
                    user = new UserModel()
                    {
                        Id = usernotpass.Id,
                        Name = usernotpass.Name,
                        Login = usernotpass.Login,
                        Email = usernotpass.Email,
                        Perfil = usernotpass.Perfil
                    };

                   user = _userRepository.Atualizar(user);
                    TempData["MensagemSucesso"] = "Usuário editado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, usuário não foi editado, tente novamente, detalhe do erro:{ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
