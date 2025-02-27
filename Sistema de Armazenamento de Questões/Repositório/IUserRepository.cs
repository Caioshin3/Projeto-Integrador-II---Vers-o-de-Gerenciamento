using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IUserRepository
    {
        UserModel BuscarPorLogin(string login);
        UserModel BuscarPorEmailELogin(string email, string login); 
        UserModel ListarPorId(int id);
        List<UserModel> BuscarTodos();
        UserModel Add(UserModel user);
        UserModel Atualizar(UserModel user);
        bool Apagar(int id);
    }
}
