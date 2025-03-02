using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;
using System.Collections.Generic;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentSchoolRepository _studentSchoolRepository;
        private readonly ISchoolRepository _schoolRepo;

        public StudentController(IStudentRepository studentRepository, IStudentSchoolRepository studentSchoolRepository, ISchoolRepository schoolRepository)
        {
            _studentRepo = studentRepository;
            _studentSchoolRepository = studentSchoolRepository;
            _schoolRepo = schoolRepository;
        }

        public IActionResult Performance(int studentId, int examId)
        {
            var answers = _studentRepo.GetStudentAnswers(studentId, examId);
            if (answers == null)
                return NotFound("Respostas não encontradas para este aluno e prova.");

            return View(answers);
        }

        public IActionResult Index()
        {
            List<StudentModel> students = _studentRepo.GetAllStudentsWithSchools();
            return View(students);
        }

        public IActionResult SelectSchoolForStudent()
        {
            // Obtém a lista de escolas
            ViewBag.Schools = _schoolRepo.GetAll();

            // Retorna a view de cadastro de aluno
            return View("CreateStudent");
        }

        public IActionResult CreateStudent()
        {
            var schools = _schoolRepo.GetAll();
            if (schools.Count == 0)
            {
                TempData["Message"] = "Nenhuma escola disponível. Cadastre uma escola primeiro.";
                return RedirectToAction("Index", "School");
            }
            ViewBag.Schools = schools;
            return View(new StudentModel()); // Initialize a new model
        }

        // Ação para processar o formulário e cadastrar o aluno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStudent(StudentModel student, int SchoolId)
        {
            if (ModelState.IsValid)
            {
                // Cria o aluno
                _studentRepo.CreateStudent(student);

                // Associa o aluno à escola (por meio da tabela auxiliar StudentSchool)
                var studentSchool = new StudentSchool
                {
                    StudentId = student.Id,
                    SchoolId = SchoolId
                };
                _studentSchoolRepository.Add(studentSchool);

                // Redireciona para a lista de alunos ou outra ação
                return RedirectToAction("Index");
            }

            // Se o modelo não for válido, retorna à view com os dados existentes
            return View(student);
        }


        [HttpPost]
        public IActionResult SaveStudentAnswer(StudentAnswerModel answer)
        {
            _studentRepo.SaveStudentAnswer(answer);
            return Ok(new { message = "Resposta registrada!" });
        }

        [HttpGet]
        public IActionResult GetStudentAnswers(int studentId, int examId)
        {
            var answers = _studentRepo.GetStudentAnswers(studentId, examId);
            if (answers == null)
                return NotFound("Respostas não encontradas.");

            return Ok(answers);
        }
    }
}
