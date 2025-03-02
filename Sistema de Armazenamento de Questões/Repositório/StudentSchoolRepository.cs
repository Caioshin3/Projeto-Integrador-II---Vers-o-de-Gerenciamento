using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class StudentSchoolRepository : IStudentSchoolRepository
    {
        private readonly BancoContext _context;

        public StudentSchoolRepository(BancoContext context)
        {
            _context = context;
        }

        // Adiciona a associação aluno-escola
        public void Add(StudentSchool studentSchool)
        {
            _context.StudentSchools.Add(studentSchool);
            _context.SaveChanges();
        }

        // Obtém a associação por ID de aluno
        public StudentSchool GetByStudentId(int studentId)
        {
            return _context.StudentSchools
                .FirstOrDefault(ss => ss.StudentId == studentId);
        }

        // Obtém a associação por ID de escola
        public StudentSchool GetBySchoolId(int schoolId)
        {
            return _context.StudentSchools
                .FirstOrDefault(ss => ss.SchoolId == schoolId);
        }

        // Remove a associação aluno-escola
        public void Remove(int studentId, int schoolId)
        {
            var studentSchool = _context.StudentSchools
                .FirstOrDefault(ss => ss.StudentId == studentId && ss.SchoolId == schoolId);
            if (studentSchool != null)
            {
                _context.StudentSchools.Remove(studentSchool);
                _context.SaveChanges();
            }
        }
    }
}
