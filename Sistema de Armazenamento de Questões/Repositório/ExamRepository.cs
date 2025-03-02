using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class ExamRepository : IExamRepository
    {
        private readonly BancoContext _context;

        public ExamRepository(BancoContext context)
        {
            _context = context;
        }

        public void CreateExam(ExamModel exam, List<int> questionIds)
        {
            _context.Exams.Add(exam);
            _context.SaveChanges();

            foreach (var questionId in questionIds)
            {
                _context.ExamQuestions.Add(new ExamQuestion
                {
                    ExamId = exam.Id,
                    QuestionId = questionId
                });
            }

            _context.SaveChanges();
        }

        public ExamModel GetExamById(int id)
        {
            return _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.Id == id);
        }

        public List<ExamModel> GetAllExams()
        {
            return _context.Exams.Include(e => e.Questions).ToList();
        }
    }
}
