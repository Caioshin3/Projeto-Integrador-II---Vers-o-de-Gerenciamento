using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Digite o seu login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite a sua senha")]
        public string Senha { get; set; }
    }
}
