namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class StudentSchool
    {
        public int StudentId { get; set; }
        public StudentModel? Student { get; set; }

        public int SchoolId { get; set; }
        public SchoolModel? School { get; set; }
    }
}
