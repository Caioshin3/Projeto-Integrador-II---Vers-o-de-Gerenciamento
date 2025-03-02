using Sistema_de_Armazenamento_de_Questões.Models;

namespace Sistema_de_Armazenamento_de_Questões.Repositório
{
    public interface IStudentRepository
    {
        void CreateStudent(StudentModel student);
        List<StudentModel> GetAllStudents();
        StudentModel GetStudentById(int id);
        void SaveStudentAnswer(StudentAnswerModel answer);
        List<StudentAnswerModel> GetStudentAnswers(int studentId, int examId);
        void AssignStudentToSchool(int studentId, int schoolId);
        void RemoveStudentFromSchool(int studentId, int schoolId);
        List<SchoolModel> GetSchoolsByStudent(int studentId);
        List<StudentModel> GetAllStudentsWithSchools();
    } 
}
