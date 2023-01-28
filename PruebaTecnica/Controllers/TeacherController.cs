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
    [Route("teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly EducationalInstitutionContext _context;

        public TeacherController(EducationalInstitutionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<List<TeacherDTO>>> GetTeachers([FromQuery] FilterTeacherInputDTO filter)
        {
            var data = from t in this._context.Teachers
                       where
                       (!filter.IdTeacher.HasValue || t.IdTeacher == filter.IdTeacher) // opcion de filtrar por el id del Maestro
                       && (!filter.IdIdentificationNumber.HasValue || t.IdIdentificationNumber == filter.IdIdentificationNumber) // opcion de filtrar por el numero de identificacion
                       && (String.IsNullOrEmpty(filter.Name) || t.Name.Contains(filter.Name)) //opcion de filtrar a estudiantes por un nombre
                       select new TeacherDTO
                       {
                           IdTeacher = t.IdTeacher,
                           IdIdentificationNumber = t.IdIdentificationNumber,                           
                           Name = t.Name
                       };
            return data.ToList();
        }


        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<List<TeacherDTO>>> AddTeacher(TeacherInputDTO input)
        {
            var add = new Teacher()
            {
                IdIdentificationNumber = input.IdIdentificationNumber,
                Name = input.Name
            };
            this._context.Teachers.Add(add);
            await _context.SaveChangesAsync();
            return Ok(await _context.Teachers.ToListAsync());
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<List<TeacherDTO>>> UpdateStudent(TeacherInputDTO input)
        {
            var update = this._context.Teachers.Where(p => p.IdIdentificationNumber == input.IdIdentificationNumber).FirstOrDefault();
            if (update == null)
                return BadRequest("Profesor no encontrado");

            update.Name = input.Name;
            update.IdIdentificationNumber = input.IdIdentificationNumber;

            await _context.SaveChangesAsync();
            return Ok(await _context.Teachers.ToListAsync());
        }

        [HttpDelete("delete/{id}")]        
        public async Task<ActionResult<List<TeacherDTO>>> DeleteStudent(int id)
        {
            var delete = this._context.Teachers.Where(p => p.IdTeacher == id).FirstOrDefault();
            if (delete == null)
                return BadRequest("Profesor no encontrado");
            _context.Teachers.Remove(delete);
            await _context.SaveChangesAsync();
            return Ok(await _context.Teachers.ToListAsync());
        }
    }
}
 
