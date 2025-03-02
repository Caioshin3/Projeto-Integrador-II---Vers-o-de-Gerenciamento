using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRepository _questionRepository;

        public QuestionsController(IQuestionsRepository questionsRepository, IExamRepository examRepository)
        {
            _questionRepository = questionsRepository;
        }

        public IActionResult Index()
        {
            List<QuestionModel> questionsView = _questionRepository.BuscarTodos();
            return View(questionsView);
        }

        public IActionResult QuestionDisplay(string categoria)
        {
            var questoes = string.IsNullOrEmpty(categoria)
                ? _questionRepository.BuscarTodos()
                : _questionRepository.BuscarPorCategoria(categoria);

            ViewBag.Categoria = categoria;
            return View(questoes);
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
            try
            {
                bool apagado = _questionRepository.Apagar(id);
                if (apagado)
                    TempData["MensagemSucesso"] = "Questão excluída com sucesso";
                else
                    TempData["MensagemErro"] = "Ops, não conseguimos excluir a questão, tente novamente!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir a questão: {ex.Message}";
                return RedirectToAction("Index");
            }
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
                TempData["MensagemErro"] = $"Erro ao cadastrar questão: {ex.Message}";
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
                TempData["MensagemErro"] = $"Erro ao editar questão: {ex.Message}";
                return RedirectToAction("Index");
            }

            return View("Edit");
        }

        // 🔹 Criar uma Prova Selecionando Questões
        

}
}
