namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }

        public string Question{ get; set; }

        public string Level { get; set; }

        public string Categorie { get; set; }

        public string CorrectAlternative { get; set; }
    }
}
