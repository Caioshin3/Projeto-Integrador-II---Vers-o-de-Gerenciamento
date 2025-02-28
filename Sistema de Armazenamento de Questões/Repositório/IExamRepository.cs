using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IExamRepository
    {
        void Add(ExamModel exam);
        List<ExamModel> GetAll();
        ExamModel GetById(int id);
    }
}
