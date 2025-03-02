using Sistema_de_Armazenamento_de_Questões.Data;
using Sistema_de_Armazenamento_de_Questões.Models;
using System.Collections.Generic;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface ISchoolRepository
    {
        List<SchoolModel> GetAll();
        SchoolModel GetById(int id);
        void Add(SchoolModel school);
        void Update(SchoolModel school);
        void Delete(int id);
        void AddStudentToSchool(int studentId, int schoolId);
        void RemoveStudentFromSchool(int studentId, int schoolId);
        List<SchoolModel> GetAllSchoolsWithStudents();
        List<StudentModel> GetStudentsBySchool(int schoolId);

    }
}
