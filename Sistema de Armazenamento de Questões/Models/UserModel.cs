using System.ComponentModel.DataAnnotations;
using Sistema_de_Armazenamento_de_Questões.Enums;

namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Digite o nome do usuário")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Digite o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite o e-mail do usuário")]

        [EmailAddress(ErrorMessage ="O e-mail informado não é válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite a senha do usuário")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Informe o Perfil do usuário")]
        public PerfilEnum? Perfil { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool SenhaValida(string senha)
        {
            return Password == senha;
        }
    }
}
