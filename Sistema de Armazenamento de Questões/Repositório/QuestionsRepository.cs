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
            return _bancoContext.Questions.FirstOrDefault(x => x.Id == id);
        }

        public List<QuestionModel> BuscarTodos()
        {
            return _bancoContext.Questions.ToList();
        }

        public QuestionModel Add(QuestionModel question)
        {
            _bancoContext.Questions.Add(question);
            _bancoContext.SaveChanges();
            return question;
        }

        public QuestionModel Atualizar(QuestionModel questionAtua)
        {
            QuestionModel questionDB = ListarPorId(questionAtua.Id);

            if(questionDB == null) 
                throw new System.Exception("Houve um erro ao atualizar a questão");


            questionDB.Question = questionAtua.Question;
            questionDB.Level = questionAtua.Level;
            questionDB.CorrectAlternative = questionAtua.CorrectAlternative;

            _bancoContext.Questions.Update(questionDB);
            _bancoContext.SaveChanges();

            return questionDB;
        }

        public bool Apagar(int id)
        {
            QuestionModel questionDB = ListarPorId(id);

            if (questionDB == null)
                throw new System.Exception("Houve um erro ao deletar a questão");

            _bancoContext.Questions.Remove(questionDB);
            _bancoContext.SaveChanges();

            return true;
        }
    }
}
