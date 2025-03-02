namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class SchoolModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<StudentSchool>? StudentSchools { get; set; }
    }
}
