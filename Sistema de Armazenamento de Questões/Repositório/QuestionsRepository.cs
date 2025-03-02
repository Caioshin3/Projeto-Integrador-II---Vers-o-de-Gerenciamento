using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly BancoContext _bancoContext;

        public QuestionsRepository(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public QuestionModel ListarPorId(int id)
        {
            return _bancoContext.Questions
                .Include(q => q.ExamQuestions)
                .ThenInclude(eq => eq.Exam)
                .FirstOrDefault(q => q.Id == id);
        }

        public List<QuestionModel> BuscarPorCategoria(string categoria)
        {
            return _bancoContext.Questions
                .Where(q => q.Categorie == categoria)
                .Include(q => q.ExamQuestions)
                .ThenInclude(eq => eq.Exam)
                .ToList();
        }

        public List<QuestionModel> BuscarTodos()
        {
            return _bancoContext.Questions
                .Include(q => q.ExamQuestions)
                .ThenInclude(eq => eq.Exam)
                .ToList();
        }

        public QuestionModel Add(QuestionModel question)
        {
            // Garante que a lista de ExamQuestions seja inicializada
            question.ExamQuestions ??= new List<ExamQuestion>();

            _bancoContext.Questions.Add(question);
            _bancoContext.SaveChanges();
            return question;
        }

        public QuestionModel Atualizar(QuestionModel questionAtua)
        {
            QuestionModel questionDB = ListarPorId(questionAtua.Id);

            if (questionDB == null)
                throw new System.Exception("Houve um erro ao atualizar a questão");

            questionDB.Question = questionAtua.Question;
            questionDB.Level = questionAtua.Level;
            questionDB.CorrectAlternative = questionAtua.CorrectAlternative;
            questionDB.Categorie = questionAtua.Categorie;

            _bancoContext.Questions.Update(questionDB);
            _bancoContext.SaveChanges();

            return questionDB;
        }

        public bool Apagar(int id)
        {
            QuestionModel questionDB = ListarPorId(id);

            if (questionDB == null)
                throw new System.Exception("Houve um erro ao deletar a questão");

            // Remove apenas as relações se houver alguma
            var examQuestions = _bancoContext.ExamQuestions
                .Where(eq => eq.QuestionId == id)
                .ToList();

            if (examQuestions.Any())
            {
                _bancoContext.ExamQuestions.RemoveRange(examQuestions);
            }

            _bancoContext.Questions.Remove(questionDB);
            _bancoContext.SaveChanges();

            return true;
        }
    }
}
