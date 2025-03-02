using System.Collections.Generic;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IExamQuestionRepository
    {
        void AddQuestionsToExam(int examId, List<int> questionIds);
        List<QuestionModel> GetQuestionsByExamId(int examId);
    }
}
