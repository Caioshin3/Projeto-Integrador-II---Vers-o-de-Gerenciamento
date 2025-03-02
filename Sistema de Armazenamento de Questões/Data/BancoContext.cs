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
        public DbSet<ExamModel> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<SchoolModel> Schools { get; set; }
        public DbSet<StudentAnswerModel> StudentAnswers { get; set; }
        public DbSet<StudentSchool> StudentSchools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentSchool>()
                .HasKey(ss => new { ss.StudentId, ss.SchoolId }); // Definição da chave composta

            modelBuilder.Entity<StudentSchool>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSchools)
                .HasForeignKey(ss => ss.StudentId);

            modelBuilder.Entity<StudentSchool>()
                .HasOne(ss => ss.School)
                .WithMany(s => s.StudentSchools)
                .HasForeignKey(ss => ss.SchoolId);

            // Relacionamento entre ExamModel e QuestionModel via a entidade de junção ExamQuestion
            modelBuilder.Entity<ExamQuestion>()
                .HasKey(eq => new { eq.ExamId, eq.QuestionId }); // Chave composta

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamId);

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionId);
        }
    }
}
