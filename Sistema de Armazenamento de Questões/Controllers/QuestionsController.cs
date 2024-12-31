using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRepository _questionRepository;
        public QuestionsController(IQuestionsRepository questionsRepository)
        {
            _questionRepository = questionsRepository;
        }

        public IActionResult Index()
        {
            List<QuestionModel> questionsView = _questionRepository.BuscarTodos();

            return View(questionsView);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            QuestionModel questionEdit = _questionRepository.ListarPorId(id);
            return View(questionEdit);
        }

        public IActionResult Delete(int id)
        {
            QuestionModel questionEdit = _questionRepository.ListarPorId(id);
            return View(questionEdit);
        }

        public IActionResult Apagar(int id)
        {
            _questionRepository.Apagar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Criar(QuestionModel questions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _questionRepository.Add(questions);
                    TempData["MensagemSucesso"] = "Questão cadastrada com sucesso";
                    return RedirectToAction("Index");
                }

                return View(questions);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, questão não foi cadastrada, tente novamente, detalhe do erro:{ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(QuestionModel questions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _questionRepository.Atualizar(questions);
                    TempData["MensagemSucesso"] = "Questão editada com sucesso";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, questão não foi editada, tente novamente, detalhe do erro:{ex.Message}";
                return RedirectToAction("Index");
            }

            return View("Edit");
        }
    }
}
