using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Helpter
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Sessao(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public UserModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContextAccessor.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) 
                return null;

                return JsonConvert.DeserializeObject<UserModel>(sessaoUsuario);
        }

        public void CriarSessaoDoUsuario(UserModel user)
        {
            string valor = JsonConvert.SerializeObject(user);
            _httpContextAccessor.HttpContext.Session.SetString("sessaoUsuarioLogado", valor);      
        }

        public void RemoverSessaoDoUsuarios()
        {
            _httpContextAccessor.HttpContext.Session.Remove("sessaoUsuarioLogado");
        }
    }
}
