namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class ExamQuestion
    {
        public int ExamId { get; set; }
        public ExamModel Exam { get; set; }

        public int QuestionId { get; set; }
        public QuestionModel Question { get; set; }
    }
}
