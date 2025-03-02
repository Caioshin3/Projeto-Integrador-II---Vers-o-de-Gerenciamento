using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly BancoContext _context;

        public SchoolRepository(BancoContext context)
        {
            _context = context;
        }

        public List<SchoolModel> GetAll()
        {
            return _context.Schools.ToList();
        }

        public SchoolModel GetById(int id)
        {
            return _context.Schools.Include(s => s.StudentSchools)
                                   .ThenInclude(ss => ss.Student)
                                   .FirstOrDefault(s => s.Id == id);
        }

        public void Add(SchoolModel school)
        {
            _context.Schools.Add(school);
            _context.SaveChanges();
        }

        public void Update(SchoolModel school)
        {
            var existingSchool = _context.Schools.FirstOrDefault(s => s.Id == school.Id);
            if (existingSchool != null)
            {
                existingSchool.Name = school.Name;
                existingSchool.City = school.City;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var school = _context.Schools.FirstOrDefault(s => s.Id == id);
            if (school != null)
            {
                _context.Schools.Remove(school);

                // Remove todas as associações de alunos dessa escola
                var studentSchools = _context.StudentSchools.Where(ss => ss.SchoolId == id);
                _context.StudentSchools.RemoveRange(studentSchools);

                _context.SaveChanges();
            }
        }

        // Método para associar um aluno a uma escola
        public void AddStudentToSchool(int studentId, int schoolId)
        {
            if (!_context.StudentSchools.Any(ss => ss.StudentId == studentId && ss.SchoolId == schoolId))
            {
                _context.StudentSchools.Add(new StudentSchool { StudentId = studentId, SchoolId = schoolId });
                _context.SaveChanges();
            }
        }

        // Método para remover um aluno de uma escola
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

        // Método para listar alunos de uma escola
        public List<StudentModel> GetStudentsBySchool(int schoolId)
        {
            return _context.StudentSchools
                .Where(ss => ss.SchoolId == schoolId)
                .Include(ss => ss.Student)
                .Select(ss => ss.Student)
                .ToList();
        }

        public List<SchoolModel> GetAllSchoolsWithStudents()
        {
            return _context.Schools
                .Include(s => s.StudentSchools)
                .ThenInclude(ss => ss.Student)
                .ToList();
        }
    }
}
