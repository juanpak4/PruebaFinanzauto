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
    [Route("course")]
    public class CourseController : ControllerBase
    {
        private readonly EducationalInstitutionContext _context;

        public CourseController(EducationalInstitutionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get")]
        //Metodo para obtener las materias registradas en la bd asi como su posible filtrado por nombre o id
        //https://localhost:7270/course/get?IdCourse=3&Course=Fisica ejemplo para probar en postman
        public async Task<ActionResult<List<CourseDTO>>> GetCourse([FromQuery] FilterCourseInputDTO filter) 
        {
            var data = from c in this._context.Courses
                       where
                       (!filter.IdCourse.HasValue || c.IdCourse  == filter.IdCourse) // opcion de filtrar por el id de la materia                       
                       && (String.IsNullOrEmpty(filter.Course) || c.Course1.Contains(filter.Course)) //opcion de filtrar las materias por un nombre
                       select new CourseDTO
                       {
                           IdCourse = c.IdCourse,
                           Course = c.Course1,                                                      
                       };
            return data.ToList();
        }


        [HttpPost]
        [Route("add")]
        //metodo para agregar una materia
        public async Task<ActionResult<List<CourseDTO>>> AddCourse(CourseInputDTO input) 
        {
            var add = new Course()
            {
                Course1 = input.Course,                
            };
            this._context.Courses.Add(add);
            await _context.SaveChangesAsync();
            return Ok(await _context.Courses.ToListAsync());
        }

        [HttpPut]
        [Route("update/{idCourse}")]
        //metodo para actualizar un materia por su id
        public async Task<ActionResult<List<CourseDTO>>> UpdateCurse(int idCourse,CourseInputDTO input)
        {
            var update = this._context.Courses.Where(c => c.IdCourse == idCourse).FirstOrDefault();
            if (update == null)
                return BadRequest("Curso no encontrado");

            update.Course1 = input.Course;
            await _context.SaveChangesAsync();

            return Ok(await _context.Courses.ToListAsync());
        }

        [HttpDelete("delete/{id}")]    
        //metodo para eliminar una materia
        public async Task<ActionResult<List<CourseDTO>>> DeleteCurse(int id)
        {
            var delete = this._context.Courses.Where(c => c.IdCourse == id).FirstOrDefault();
            if (delete == null)
                return BadRequest("Curso no encontrado");
            _context.Courses.Remove(delete);
            await _context.SaveChangesAsync();
            return Ok(await _context.Courses.ToListAsync());
        }
    }
}
 
