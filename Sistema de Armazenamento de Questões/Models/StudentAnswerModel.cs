namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class StudentAnswerModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public string GivenAnswer { get; set; }
        public bool IsCorrect { get; set; }  // Define se a resposta está correta ou não

        public StudentModel Student { get; set; }
        public ExamModel Exam { get; set; }
        public QuestionModel Question { get; set; }
    }
}
