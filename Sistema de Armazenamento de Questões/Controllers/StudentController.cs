using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Armazenamento_de_Questões.Data;
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
        private readonly BancoContext _context;

        public StudentController(IStudentRepository studentRepository, IStudentSchoolRepository studentSchoolRepository, ISchoolRepository schoolRepository, BancoContext context)
        {
            _studentRepo = studentRepository;
            _studentSchoolRepository = studentSchoolRepository;
            _schoolRepo = schoolRepository;
            _context = context;
        }

        public IActionResult Performance(int studentId, int? examId)
        {
            // Verificar se o aluno existe
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return NotFound("Aluno não encontrado");
            }

            // Obter todos os exames distintos respondidos pelo aluno
            var studentExams = _context.StudentAnswers
                .Where(sa => sa.StudentId == studentId)
                .Include(sa => sa.Exam)
                .Select(sa => sa.Exam)
                .GroupBy(e => e.Id) // Agrupar para eliminar duplicatas
                .Select(g => g.First())
                .ToList();

            if (!studentExams.Any())
            {
                // Nenhum exame encontrado para este aluno
                ViewData["StudentExams"] = new List<ExamModel>();
                return View(new List<StudentAnswerModel>());
            }

            ViewData["StudentExams"] = studentExams;

            // Selecionar o exame especificado ou o primeiro disponível
            var selectedExam = examId.HasValue
                ? studentExams.FirstOrDefault(e => e.Id == examId.Value)
                : studentExams.FirstOrDefault();

            if (selectedExam == null)
            {
                // Nenhum exame válido selecionado
                ViewData["SelectedExamId"] = 0;
                return View(new List<StudentAnswerModel>());
            }

            ViewData["SelectedExamId"] = selectedExam.Id;

            // Buscar respostas do aluno para o exame selecionado
            var studentAnswers = _context.StudentAnswers
                .Where(sa => sa.StudentId == studentId && sa.ExamId == selectedExam.Id)
                .Include(sa => sa.Question)
                .ToList();

            // Criar um objeto padrão com a mesma estrutura
            var defaultItem = new
            {
                Categorie = "Sem dados",
                Level = "N/A",
                Correct = 0,
                Total = 0
            };

            // Processando dados de desempenho por categoria e nível
            var performanceData = studentAnswers
                .Where(sa => sa.Question != null)
                .GroupBy(sa => new
                {
                    Categorie = sa.Question.Categorie ?? "Sem categoria",
                    Level = sa.Question.Level ?? "Sem nível"
                })
                .Select(g => new
                {
                    Categorie = g.Key.Categorie,
                    Level = g.Key.Level,
                    Correct = g.Count(sa => sa.IsCorrect == true),
                    Total = g.Count()
                })
                .ToList();

            // Verificar se temos dados para exibir
            if (!performanceData.Any())
            {
                // Como defaultItem já está com o tipo correto, podemos usá-lo diretamente
                performanceData = new[] { defaultItem }.ToList();
            }

            Console.WriteLine($"Performance data count: {performanceData.Count}");
            foreach (var item in performanceData)
            {
                Console.WriteLine($"Category: {item.Categorie}, Level: {item.Level}, Correct: {item.Correct}, Total: {item.Total}");
            }

            ViewData["PerformanceData"] = performanceData;

            return View(studentAnswers);
        }


        // No seu StudentController.cs
        public IActionResult Index()
        {
            // Carregar todos os alunos
            var students = _context.Students.ToList();

            // Carregar as escolas de cada aluno através da tabela de associação
            var studentSchoolsDictionary = new Dictionary<int, List<SchoolModel>>();

            // Buscar todas as associações estudante-escola
            var studentSchools = _context.StudentSchools
                .Include(ss => ss.School)
                .ToList();

            // Agrupar escolas por aluno
            foreach (var student in students)
            {
                var schoolsForStudent = studentSchools
                    .Where(ss => ss.StudentId == student.Id && ss.School != null)
                    .Select(ss => ss.School)
                    .ToList();

                studentSchoolsDictionary[student.Id] = schoolsForStudent;
            }

            ViewBag.StudentSchools = studentSchoolsDictionary;

            // Carregar os exames para cada aluno
            var studentExamsDictionary = new Dictionary<int, List<ExamModel>>();

            // Buscar os exames distintos que cada aluno respondeu
            var studentAnswers = _context.StudentAnswers
                .Include(sa => sa.Exam)
                .ToList();

            foreach (var student in students)
            {
                var examsForStudent = studentAnswers
                    .Where(sa => sa.StudentId == student.Id && sa.Exam != null)
                    .Select(sa => sa.Exam)
                    .GroupBy(e => e.Id) // Agrupar para eliminar duplicatas
                    .Select(g => g.First())
                    .ToList();

                studentExamsDictionary[student.Id] = examsForStudent;
            }

            ViewData["StudentExams"] = studentExamsDictionary;

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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _studentRepo.GetStudentById(id);
            if (student == null)
                return NotFound();

            ViewBag.Schools = _schoolRepo.GetAll();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentModel student, int SchoolId)
        {
            if (ModelState.IsValid)
            {
                _context.Update(student);

                // Atualizar a relação estudante-escola
                var studentSchool = _context.StudentSchools.FirstOrDefault(ss => ss.StudentId == student.Id);
                if (studentSchool != null)
                {
                    studentSchool.SchoolId = SchoolId;
                    _context.StudentSchools.Update(studentSchool);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Schools = _schoolRepo.GetAll();
            return View(student);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var student = _studentRepo.GetStudentById(id);
            if (student == null)
                return NotFound();

            var studentSchools = _context.StudentSchools
                .Include(ss => ss.School)
                .Where(ss => ss.StudentId == id)
                .Select(ss => ss.School)
                .ToList();

            ViewBag.Schools = studentSchools;
            return View(student);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _studentRepo.GetStudentById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _context.Remove(id);
            return RedirectToAction("Index");
        }

    }
}
