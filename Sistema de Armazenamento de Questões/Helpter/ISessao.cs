using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Helpter
{
    public interface ISessao
    {
        void CriarSessaoDoUsuario(UserModel user);

        void RemoverSessaoDoUsuarios();

        UserModel BuscarSessaoDoUsuario();
    }
}
