using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class ExamRepository : IExamRepository
    {
        private readonly BancoContext _context;

        public ExamRepository(BancoContext context)
        {
            _context = context;
        }

        public List<ExamModel> GetAllExams()
        {
            return _context.Exams
                .Include(e => e.ExamQuestions)  // Carregar questões relacionadas
                .ThenInclude(eq => eq.Question)  // Carregar questões associadas
                .ToList();
        }

        public ExamModel GetExamById(int id)
        {
            return _context.Exams
                .Include(e => e.ExamQuestions)  // Carregar as questões associadas
                .ThenInclude(eq => eq.Question)  // Carregar as questões
                .FirstOrDefault(e => e.Id == id);
        }

        public void CreateExam(ExamModel exam)
        {
            // Criação da prova
            _context.Exams.Add(exam);
            _context.SaveChanges();  // Salvar no banco de dados
        }
    }
}
