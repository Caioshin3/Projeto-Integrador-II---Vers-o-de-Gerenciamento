using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamRepository _examRepo;
        private readonly IQuestionsRepository _questionRepo;
        private readonly IExamQuestionRepository _examQuestionRepo;

        public ExamController(IExamRepository examRepo, IQuestionsRepository questionRepo, IExamQuestionRepository examQuestionRepo)
        {
            _examRepo = examRepo;
            _questionRepo = questionRepo;
            _examQuestionRepo = examQuestionRepo;
        }

        // Ação para listar todas as provas
        public IActionResult Index()
        {
            var exams = _examRepo.GetAllExams();
            return View(exams);
        }

        // Ação para visualizar detalhes de uma prova
        public IActionResult Details(int id)
        {
            var exam = _examRepo.GetExamById(id);
            if (exam == null) return NotFound();
            return View(exam);
        }

        // Ação para exibir o formulário de criação de uma nova prova
        public IActionResult Create()
        {
            var model = new ExamModel
            {
                // Carregar todas as questões para seleção ao criar uma prova
                ExamQuestions = _questionRepo.BuscarTodos().Select(q => new ExamQuestion
                {
                    QuestionId = q.Id,
                    Question = q // Associando a questão ao modelo de exame
                }).ToList()
            };
            return View(model);
        }

        // Ação para processar a criação de uma nova prova
        [HttpPost]
        public IActionResult Create(ExamModel exam, List<int>? QuestionIds)
        {
            try
            {
                if (QuestionIds == null || !QuestionIds.Any())
                {
                    TempData["ErrorMessage"] = "A prova deve conter pelo menos uma questão.";
                    exam.ExamQuestions = _questionRepo.BuscarTodos().Select(q => new ExamQuestion
                    {
                        QuestionId = q.Id,
                        Question = q
                    }).ToList();
                    return View(exam);
                }

                // Criando a prova
                _examRepo.CreateExam(exam);

                // Adicionando as questões à prova usando o repositório
                _examQuestionRepo.AddQuestionsToExam(exam.Id, QuestionIds);

                TempData["SuccessMessage"] = "Prova criada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao criar a prova: {ex.Message}";
                exam.ExamQuestions = _questionRepo.BuscarTodos().Select(q => new ExamQuestion
                {
                    QuestionId = q.Id,
                    Question = q
                }).ToList();
                return View(exam);
            }
        }


        // Ação para gerar o PDF da prova
        public IActionResult GenerateExamPdf(int id)
        {
            var exam = _examRepo.GetExamById(id);
            if (exam == null) return NotFound();

            // Usando o repositório para obter as questões associadas à prova
            var questions = _examQuestionRepo.GetQuestionsByExamId(id);

            using (MemoryStream stream = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter.GetInstance(doc, stream);
                doc.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                doc.Add(new Paragraph($"Prova: {exam.Title}", titleFont));
                doc.Add(new Paragraph("\n"));

                int count = 1;
                foreach (var question in questions)
                {
                    doc.Add(new Paragraph($"{count}. {question.Question}", normalFont));
                    doc.Add(new Paragraph("\n"));
                    count++;
                }

                doc.Close();
                byte[] fileBytes = stream.ToArray();
                return File(fileBytes, "application/pdf", "Prova.pdf");
            }
        }
    }
}
