using System.Security.Cryptography;
using System.Text;

namespace Sistema_de_Armazenamento_de_Questões.Helpter
{
    public static class Criptografia
    {
        public static string GenerateHash(this string value)
        {
            var hash = SHA1.Create();
            var encoding = new ASCIIEncoding();
            var array = encoding.GetBytes(value);

            array = hash.ComputeHash(array);

            var strHexa = new StringBuilder();

            foreach (var item in array) 
            {
                strHexa.Append(item.ToString("x2"));
            }

            return strHexa.ToString();
        }
    }
}
