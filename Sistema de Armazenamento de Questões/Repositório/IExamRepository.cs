using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IExamRepository
    {
        void CreateExam(ExamModel exam, List<int> questionIds);
        ExamModel GetExamById(int id);
        List<ExamModel> GetAllExams();
    }
}
