using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IStudentSchoolRepository
    {
        void Add(StudentSchool studentSchool);  // Adicionar associação aluno-escola
        StudentSchool GetByStudentId(int studentId);  // Obter a associação por aluno
        StudentSchool GetBySchoolId(int schoolId);  // Obter a associação por escola
        void Remove(int studentId, int schoolId);  // Remover associação
    }
}
