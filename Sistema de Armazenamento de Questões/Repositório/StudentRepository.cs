using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class StudentRepository : IStudentRepository
    {
        private readonly BancoContext _context;

        public StudentRepository(BancoContext context)
        {
            _context = context;
        }

        public void CreateStudent(StudentModel student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public List<StudentModel> GetAllStudents()
        {
            return _context.Students.Include(s => s.StudentSchools)
                                    .ThenInclude(ss => ss.School)
                                    .ToList();
        }

        public StudentModel GetStudentById(int id)
        {
            return _context.Students.Include(s => s.StudentSchools)
                                    .ThenInclude(ss => ss.School)
                                    .FirstOrDefault(s => s.Id == id);
        }

        public void SaveStudentAnswer(StudentAnswerModel answer)
        {
            if (!_context.StudentAnswers.Any(a => a.StudentId == answer.StudentId &&
                                                  a.ExamId == answer.ExamId &&
                                                  a.QuestionId == answer.QuestionId))
            {
                _context.StudentAnswers.Add(answer);
                _context.SaveChanges();
            }
        }

        public List<StudentAnswerModel> GetStudentAnswers(int studentId, int examId)
        {
            return _context.StudentAnswers
                .Where(a => a.StudentId == studentId && a.ExamId == examId)
                .Include(a => a.Question)
                .ToList();
        }

        // Método para associar um aluno a uma escola
        public void AssignStudentToSchool(int studentId, int schoolId)
        {
            if (!_context.StudentSchools.Any(ss => ss.StudentId == studentId && ss.SchoolId == schoolId))
            {
                _context.StudentSchools.Add(new StudentSchool { StudentId = studentId, SchoolId = schoolId });
                _context.SaveChanges();
            }
        }

        // Método para remover a associação de um aluno com uma escola
        public void RemoveStudentFromSchool(int studentId, int schoolId)
        {
            var studentSchool = _context.StudentSchools
                .FirstOrDefault(ss => ss.StudentId == studentId && ss.SchoolId == schoolId);
            if (studentSchool != null)
            {
                _context.StudentSchools.Remove(studentSchool);
                _context.SaveChanges();
            }
        }

        public List<StudentModel> GetAllStudentsWithSchools()
        {
            return _context.Students
                .Include(s => s.StudentSchools)
                .ThenInclude(ss => ss.School)
                .ToList();
        }

        // Método para obter todas as escolas associadas a um aluno
        public List<SchoolModel> GetSchoolsByStudent(int studentId)
        {
            return _context.StudentSchools
                .Where(ss => ss.StudentId == studentId)
                .Include(ss => ss.School)
                .Select(ss => ss.School)
                .ToList();
        }
    }
}
