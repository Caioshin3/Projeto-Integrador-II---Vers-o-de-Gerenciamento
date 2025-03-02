using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Sistema_de_Armazenamento_de_Questões.Repositório;

public class StudentAnswerController : Controller
{
    private readonly BancoContext _context;
    private readonly IExamQuestionRepository _examQuestionRepository;
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IStudentRepository _studentRepository;

    public StudentAnswerController(BancoContext context, IExamQuestionRepository examQuestionRepository, IQuestionsRepository questionsRepository, IStudentRepository studentRepository)
    {
        _context = context;
        _examQuestionRepository = examQuestionRepository;
        _questionsRepository = questionsRepository;
        _studentRepository = studentRepository;
    }

    public IActionResult Index(int examId)
    {
        var studentAnswers = _context.StudentAnswers
                                      .Include(sa => sa.Student)
                                      .Include(sa => sa.Question)
                                      .Where(sa => sa.ExamId == examId)
                                      .ToList();

        var exam = _context.Exams.FirstOrDefault(e => e.Id == examId);
        if (exam == null)
        {
            return NotFound();
        }

        ViewData["ExamTitle"] = exam.Title;
        ViewData["ExamId"] = examId;  // Adicionado o exameId na ViewData para o link de criar resposta
        return View(studentAnswers);
    }

    public IActionResult Create(int examId)
    {
        // Obtenha os alunos e questões do banco de dados ou repositório
        var students = _studentRepository.GetAllStudents();
        var questions = _examQuestionRepository.GetQuestionsByExamId(examId);

        // Verificar se a lista de alunos ou questões está vazia
        if (students == null || !students.Any())
        {
            ModelState.AddModelError(string.Empty, "Não há alunos cadastrados.");
        }

        if (questions == null || !questions.Any())
        {
            ModelState.AddModelError(string.Empty, "Não há questões para este exame.");
        }

        // Passando as listas para a View via ViewBag
        ViewBag.Students = students != null ? new SelectList(students, "Id", "Name") : new SelectList(new List<StudentModel>());
        ViewBag.Questions = questions != null ? new SelectList(questions, "Id", "Question") : new SelectList(new List<QuestionModel>());

        // Passar o ID do exame para a View
        ViewData["ExamId"] = examId;

        // Para cada questão, extraímos as alternativas e a resposta correta
        if (questions != null && questions.Any())
        {
            var question = questions.FirstOrDefault(); // Pega a primeira questão, ou você pode adicionar lógica para pegar a questão correta conforme necessário

            // Extrair alternativas da questão
            var alternatives = question.Question.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Where(line => line.Contains(")"))
                                                     .ToList();

            // Passar as alternativas e a resposta correta para a view
            ViewData["Alternatives"] = alternatives;
            ViewData["CorrectAnswer"] = question.CorrectAlternative; // Supondo que exista um campo 'CorrectAnswer' que contém a resposta correta
        }

        // Verifica se há algum erro de modelo
        if (!ModelState.IsValid)
        {
            return View(new StudentAnswerModel());  // Retorna à view com mensagens de erro
        }

        return View(new StudentAnswerModel());
    }


    [HttpPost]
    public IActionResult Create(StudentAnswerModel studentAnswer)
    {
        if (ModelState.IsValid)
        {
            // Encontrar a questão e a resposta correta
            var question = _context.Questions.FirstOrDefault(q => q.Id == studentAnswer.QuestionId);

            // Validar a resposta
            if (question != null)
            {
                // Extrair a letra da alternativa correta (ex: "A", "B", etc.)
                var correctAnswer = question.CorrectAlternative.ToUpper().Trim(); // A, B, etc.

                // Extrair apenas a letra da resposta do aluno (ex: "A", "B", etc.) da resposta completa "A) Algum texto"
                var studentAnswerLetter = studentAnswer.GivenAnswer.Split(')')[0].Trim().ToUpper();

                // Verificar se a resposta do aluno corresponde à alternativa correta (A, B, C, etc.)
                studentAnswer.IsCorrect = correctAnswer.Equals(studentAnswerLetter, StringComparison.OrdinalIgnoreCase);
            }

            // Salvar a resposta do aluno no banco de dados
            _context.StudentAnswers.Add(studentAnswer);
            _context.SaveChanges();

            return RedirectToAction("Index", new { examId = studentAnswer.ExamId });
        }

        // Caso o modelo não seja válido, repasse os dados para a view
        ViewData["ExamId"] = studentAnswer.ExamId;
        ViewData["ExamTitle"] = _context.Exams.FirstOrDefault(e => e.Id == studentAnswer.ExamId)?.Title;
        ViewData["Students"] = new SelectList(_context.Students, "Id", "Name", studentAnswer.StudentId);
        ViewData["Questions"] = new SelectList(_context.Questions
            .Where(q => q.ExamQuestions != null && q.ExamQuestions.Any(eq => eq.ExamId == studentAnswer.ExamId)),
            "Id", "Question", studentAnswer.QuestionId);

        return View(studentAnswer);
    }






    public IActionResult Delete(int id)
    {
        var studentAnswer = _context.StudentAnswers.Include(sa => sa.Student).Include(sa => sa.Exam).Include(sa => sa.Question).FirstOrDefault(sa => sa.Id == id);
        if (studentAnswer == null)
        {
            return NotFound();
        }

        return View(studentAnswer);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var studentAnswer = _context.StudentAnswers.Find(id);
        if (studentAnswer != null)
        {
            _context.StudentAnswers.Remove(studentAnswer);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    // Editar resposta
    public IActionResult Edit(int id)
    {
        var studentAnswer = _context.StudentAnswers.Include(sa => sa.Student).Include(sa => sa.Exam).Include(sa => sa.Question).FirstOrDefault(sa => sa.Id == id);
        if (studentAnswer == null)
        {
            return NotFound();
        }

        ViewData["Students"] = new SelectList(_context.Students, "Id", "Name", studentAnswer.StudentId);
        ViewData["Exams"] = new SelectList(_context.Exams, "Id", "Name", studentAnswer.ExamId);
        ViewData["Questions"] = new SelectList(_context.Questions, "Id", "Text", studentAnswer.QuestionId);

        return View(studentAnswer);
    }

    [HttpPost]
    public IActionResult Edit(int id, StudentAnswerModel studentAnswer)
    {
        if (id != studentAnswer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(studentAnswer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(studentAnswer);
    }

    // Excluir resposta

}
