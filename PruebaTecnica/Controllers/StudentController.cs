using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Models.DTO.Input;
using PruebaTecnica.Models;
using PruebaTecnica.Models.Entity;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models.Entity.Tables;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private readonly EducationalInstitutionContext _context;

        public StudentController(EducationalInstitutionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<List<StudentDTO>>> GetStudents([FromQuery] FilterStudentInputDTO filter)// metodo get para obtener a un listado de estudiantes el filtrado se puede dejar vacio para traer todos los estudiantes
        {
            var data = from s in _context.Students // consulta a la tabla estudiantes
                       where
                       (!filter.IdStudent.HasValue || s.IdStudent == filter.IdStudent) // opcion de filtrar por el id del estudiante
                       && (!filter.IdentifactionNumber.HasValue || s.IdentifactionNumber == filter.IdentifactionNumber) // opcion de filtrar por el numero de identificacion
                       && (String.IsNullOrEmpty(filter.Name) || s.Name.Contains(filter.Name)) //opcion de filtrar a estudiantes por un nombre

                       select new StudentDTO 
                       {
                           IdentifactionNumber = s.IdentifactionNumber,
                           IdStudent = s.IdStudent,
                           Name = s.Name
                       };
            return data.ToList();
        }


        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<List<StudentDTO>>> AddStudent(StudentInputDTO input)
        {
            var add = new Student()
            {
                IdentifactionNumber = input.IdentifactionNumber,
                Name = input.Name
            };
            this._context.Students.Add(add);
            await _context.SaveChangesAsync();
            return Ok(await _context.Students.ToListAsync());
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<List<StudentDTO>>> UpdateStudent(StudentInputDTO input)
        {
            var update = this._context.Students.Where(p => p.IdentifactionNumber == input.IdentifactionNumber).FirstOrDefault();
            if (update == null)
                return BadRequest("Persona no encontrada");

            update.Name = input.Name;
            update.IdentifactionNumber = input.IdentifactionNumber;

            await _context.SaveChangesAsync();
            return Ok(await _context.Students.ToListAsync());
        }

        [HttpDelete("delete/{id}")]        
        public async Task<ActionResult<List<StudentDTO>>> DeleteStudent(int id)
        {
            var delete = this._context.Students.Where(p => p.IdStudent == id).FirstOrDefault();
            if (delete == null)
                return BadRequest("Persona no encontrada");
            _context.Students.Remove(delete);
            await _context.SaveChangesAsync();
            return Ok(await _context.Students.ToListAsync());
        }
    }
}
 
