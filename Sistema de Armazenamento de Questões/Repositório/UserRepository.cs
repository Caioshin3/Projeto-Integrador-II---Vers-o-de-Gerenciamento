using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class UserRepository : IUserRepository
    {
        private readonly BancoContext _bancoContext;
        public UserRepository(BancoContext bancoContext) 
        {
            _bancoContext = bancoContext;
        }

        public UserModel ListarPorId(int id)
        {
            return _bancoContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public UserModel BuscarPorLogin(string login)
        {
            return _bancoContext.Users.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
        }

        public UserModel BuscarPorEmailELogin(string email, string login)
        {
            return _bancoContext.Users.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper() && x.Login.ToUpper() == login.ToUpper());
        }

        public List<UserModel> BuscarTodos()
        {
            return _bancoContext.Users.ToList();
        }

        public UserModel Add(UserModel user)
        {
            user.CreationDate = DateTime.Now;
            user.SetPasswordHash();
            _bancoContext.Users.Add(user);
            _bancoContext.SaveChanges();
            return user;
        }

        public UserModel Atualizar(UserModel user)
        {
            UserModel userDB = ListarPorId(user.Id);

            if(userDB == null) 
                throw new System.Exception("Houve um erro ao atualizar o usuário");


            userDB.Name = user.Name;
            userDB.Email = user.Email;
            userDB.Login = user.Login;
            userDB.Perfil = user.Perfil;
            userDB.UpdateDate = DateTime.Now;

            _bancoContext.Users.Update(userDB);
            _bancoContext.SaveChanges();

            return userDB;
        }

        public bool Apagar(int id)
        {
            UserModel userDB = ListarPorId(id);

            if (userDB == null)
                throw new System.Exception("Houve um erro ao deletar a usuário");

            _bancoContext.Users.Remove(userDB);
            _bancoContext.SaveChanges();

            return true;
        }
    }
}
