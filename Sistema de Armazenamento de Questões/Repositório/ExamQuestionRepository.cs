using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class ExamQuestionRepository : IExamQuestionRepository
    {
        private readonly BancoContext _context;

        public ExamQuestionRepository(BancoContext context)
        {
            _context = context;
        }

        public void AddQuestionsToExam(int examId, List<int> questionIds)
        {
            var examQuestions = questionIds.Select(qId => new ExamQuestion
            {
                ExamId = examId,
                QuestionId = qId
            }).ToList();

            _context.ExamQuestions.AddRange(examQuestions);
            _context.SaveChanges();
        }

        public List<QuestionModel> GetQuestionsByExamId(int examId)
        {
            return _context.ExamQuestions
                .Where(eq => eq.ExamId == examId)
                .Include(eq => eq.Question)  // Incluir a questão associada
                .Select(eq => eq.Question)   // Selecionar a questão diretamente
                .ToList();
        }
    }
}
