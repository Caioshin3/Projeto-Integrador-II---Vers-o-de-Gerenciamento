using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamRepository _examRepo;
        private readonly IQuestionsRepository _questionRepo;

        public ExamController(IExamRepository examRepo, IQuestionsRepository questionRepo)
        {
            _examRepo = examRepo;
            _questionRepo = questionRepo;
        }

        public IActionResult Index()
        {
            var exams = _examRepo.GetAllExams();
            return View(exams);
        }

        public IActionResult Details(int id)
        {
            var exam = _examRepo.GetExamById(id);
            if (exam == null) return NotFound();
            return View(exam);
        }

        public IActionResult Create()
        {
            var model = new ExamModel
            {
                // Exemplo de adição de questões ao modelo. Isso pode ser feito conforme necessário.
                Questions = _questionRepo.BuscarTodos() // Supondo que você tenha uma lógica para obter todas as questões
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult Create(ExamModel exam, List<int> QuestionIds)
        {
            try
            {
                _examRepo.CreateExam(exam, QuestionIds);
                TempData["SuccessMessage"] = "Prova criada com sucesso!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocorreu um erro ao criar a prova.";
            }
            return RedirectToAction("Index");
        }


        public IActionResult GenerateExamPdf(int id)
        {
            var exam = _examRepo.GetExamById(id);
            if (exam == null) return NotFound();

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
                foreach (var question in exam.Questions)
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