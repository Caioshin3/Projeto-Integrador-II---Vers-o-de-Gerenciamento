namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public string schoolYear { get; set; }
        public List<StudentSchool>? StudentSchools { get; set; }
    }
}
