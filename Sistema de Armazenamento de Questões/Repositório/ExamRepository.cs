using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class ExamRepository : IExamRepository
    {
        private static List<ExamModel> exams = new List<ExamModel>();

        public void Add(ExamModel exam)
        {
            exam.Id = exams.Count + 1;
            exams.Add(exam);
        }

        public List<ExamModel> GetAll()
        {
            return exams;
        }

        public ExamModel GetById(int id)
        {
            return exams.FirstOrDefault(e => e.Id == id);
        }
    }
}
