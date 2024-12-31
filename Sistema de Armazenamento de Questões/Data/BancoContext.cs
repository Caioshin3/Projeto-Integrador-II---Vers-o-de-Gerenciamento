using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<QuestionModel> Questions { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
