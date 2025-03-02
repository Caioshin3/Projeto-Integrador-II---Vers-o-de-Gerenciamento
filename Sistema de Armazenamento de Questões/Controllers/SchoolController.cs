using Microsoft.AspNetCore.Mvc;
using Sistema_de_Armazenamento_de_Questões.Models;
using Sistema_de_Armazenamento_de_Questões.Repositório;

namespace Sistema_de_Armazenamento_de_Questões.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolController(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        // GET: School
        public IActionResult Index()
        {
            var schools = _schoolRepository.GetAll();
            return View(schools);
        }

        // GET: School/Details/5
        public IActionResult Details(int id)
        {
            var school = _schoolRepository.GetStudentsBySchool(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        // GET: School/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: School/Create
        [HttpPost]
        public IActionResult Create(SchoolModel school)
        {
            if (ModelState.IsValid)
            {
                _schoolRepository.Add(school);
                TempData["SuccessMessage"] = "Escola cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        // GET: School/Edit/5
        public IActionResult Edit(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        // POST: School/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SchoolModel school)
        {
            if (id != school.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _schoolRepository.Update(school);
                TempData["SuccessMessage"] = "Escola atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        // GET: School/Delete/5
        public IActionResult Delete(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        // POST: School/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school != null)
            {
                _schoolRepository.Delete(id);
                TempData["SuccessMessage"] = "Escola excluída com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao excluir a escola. Tente novamente.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
