using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IQuestionsRepository
    {
        QuestionModel ListarPorId(int id);
        List<QuestionModel> BuscarTodos();

        List<QuestionModel> BuscarPorCategoria(string categoria);
        QuestionModel Add(QuestionModel questions);
        QuestionModel Atualizar(QuestionModel questionAtua);
        bool Apagar(int id);

    }
}
