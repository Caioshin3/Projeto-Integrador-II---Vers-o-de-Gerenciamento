using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class StudentAnswerController : Controller
{
    private readonly BancoContext _context;

    public StudentAnswerController(BancoContext context)
    {
        _context = context;
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


    // Criar nova resposta para um exame
    public IActionResult Create(int examId)
    {
        ViewData["ExamId"] = examId;

        var exam = _context.Exams.FirstOrDefault(e => e.Id == examId);
        if (exam == null)
        {
            return NotFound();
        }

        ViewData["ExamTitle"] = exam.Title;
        ViewData["Students"] = new SelectList(_context.Students, "Id", "Name");
        ViewData["Questions"] = new SelectList(_context.Questions.Where(q => q.ExamModelId == examId), "Id", "Text");

        var studentAnswers = new List<StudentAnswerModel>();

        return View(studentAnswers);
    }

    [HttpPost]
    public IActionResult Create(StudentAnswerModel studentAnswer)
    {
        if (ModelState.IsValid)
        {
            _context.StudentAnswers.Add(studentAnswer);
            _context.SaveChanges();
            return RedirectToAction("Index", new { examId = studentAnswer.ExamId });
        }

        ViewData["Students"] = new SelectList(_context.Students, "Id", "Name", studentAnswer.StudentId);
        ViewData["Questions"] = new SelectList(_context.Questions.Where(q => q.ExamModelId == studentAnswer.ExamId), "Id", "Text", studentAnswer.QuestionId);
        return View(studentAnswer);
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
        _context.StudentAnswers.Remove(studentAnswer);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
