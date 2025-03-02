using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IExamRepository
    {
        List<ExamModel> GetAllExams();
        ExamModel GetExamById(int id);
        void CreateExam(ExamModel exam);


    }
}
