using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Armazenamento_de_Questões.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Digite o seu login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite o seu e-mail")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        public string Email { get; set; }
    }
}
