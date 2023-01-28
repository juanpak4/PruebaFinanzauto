using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Models.DTO.Input;
using PruebaTecnica.Models;
using PruebaTecnica.Models.Entity;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models.Entity.Tables;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using NuGet.Protocol;
using System;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    [Route("rating")]
    public class RatingController : ControllerBase
    {        

        private readonly EducationalInstitutionContext _context;

        public RatingController(EducationalInstitutionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get")]
        //metodo que permite buscar todas la calificaciones
        public async Task<ActionResult<List<TeacherDTO>>> GetRating([FromQuery] FilterRatingInputDTO filter)
        {
            var data = from r in this._context.Ratings
                       join c in this._context.Courses
                       on r.IdCourse equals c.IdCourse
                       join t in this._context.Teachers
                       on r.IdTeacher equals t.IdTeacher
                       join s in this._context.Students
                       on r.IdStudent equals s.IdStudent
                       where
                       (!filter.IdentificationStudent.HasValue || s.IdentifactionNumber == filter.IdentificationStudent) // opcion de filtrar por el numero de identificacion
                       && (String.IsNullOrEmpty(filter.NameStudent) || s.Name.Contains(filter.NameStudent)) //opcion de filtrar a estudiantes por un nombre
                       && (!filter.IdentificationTeacher.HasValue || t.IdIdentificationNumber == filter.IdentificationTeacher) // opcion de filtrar por el numero de identificacion
                       && (String.IsNullOrEmpty(filter.NameTeacher) || t.Name.Contains(filter.NameTeacher)) //opcion de filtrar a estudiantes por un nombre
                       && (!filter.IdCourse.HasValue || c.IdCourse == filter.IdCourse) // opcion de filtrar por el id de la materia                       
                       && (String.IsNullOrEmpty(filter.Course) || c.Course1.Contains(filter.Course)) //opcion de filtrar las materias por un nombre
                       select new RatingDTO
                       {
                           IdCourse = c.IdCourse,
                           IdentificationStudent = s.IdentifactionNumber,
                           IdentificationTeacher = t.IdIdentificationNumber,
                           IdRating = r.IdRating,
                           Rating = r.Rating1,
                           Date = r.Date,
                           StudentName = s.Name,
                           TeacherName = t.Name,
                           CourseName = c.Course1
                       };
            return Ok(data.ToList());
        }


        [HttpPost]
        [Route("add")]
        //metodo que permite agregar una calificacion
        public async Task<ActionResult<List<RatingDTO>>> AddRating(RatingInputDTO input)
        {
            StudentController student = new StudentController(_context);// instancia de la clase Estudiante
            TeacherController teacher = new TeacherController(_context);// instancia de la clase Maestro
            CourseController course = new CourseController(_context);// instancia de la clase Curso

            //se busca el estudiante por su numero de identificacion donde en caso de no encontrarlo 
            //la condicion enviara un mensaje en el cual no se encuentra el estudiante
            //esta funcion se repite pero con cada una de las tablas necesarias como lo son maestro y curso
            var idStudent = student.GetStudents(filter: new FilterStudentInputDTO() { IdentifactionNumber = input.IdentificationStudent }).Result.Value; 
            if(idStudent == null) {
                return BadRequest("no se encontro el estudiante para realizar la asignacion de la claificacion");
            }
            var idTeacher = teacher.GetTeachers(new FilterTeacherInputDTO() { IdIdentificationNumber = input.IdentificationTeacher }).Result.Value;
            if (idTeacher == null)
            {
                return BadRequest("no se puede crear una calificacion por que no se encontro el maestro que la genero");
            }
            var idcourse = course.GetCourse(new FilterCourseInputDTO() { IdCourse = input.IdCourse }).Result.Value;
            if (idcourse == null) {
                return BadRequest("la materia no existe");
            }
            //crea objeto de tipo Rating para ser agregado a la tabla Calificaicones
            var add = new Rating()
            {
                IdCourse= input.IdCourse,
                IdStudent = idStudent.First().IdStudent,
                IdTeacher = idTeacher.First().IdTeacher,
                Rating1 = input.Rating,
                Date = DateTime.Now
            };
            this._context.Ratings.Add(add);
            await _context.SaveChangesAsync();
            return Ok(await _context.Ratings.ToListAsync());
        }

        [HttpPut]
        [Route("update/{idRating}")]
        public async Task<ActionResult<List<RatingDTO>>> UpdateRating(int idRating, RatingInputDTO input)
        {
            StudentController student = new StudentController(_context);
            TeacherController teacher = new TeacherController(_context);
            CourseController course = new CourseController(_context);

            var idStudent = student.GetStudents(filter: new FilterStudentInputDTO() { IdentifactionNumber = input.IdentificationStudent }).Result.Value;

            if (idStudent == null)
            {
                return BadRequest("no se encontro el estudiante para realizar la asignacion de la claificacion");
            }
            var idTeacher = teacher.GetTeachers(new FilterTeacherInputDTO() { IdIdentificationNumber = input.IdentificationTeacher }).Result.Value;
            if (idTeacher == null)
            {
                return BadRequest("no se puede crear una calificacion por que no se encontro el maestro que la genero");
            }
            var idcourse = course.GetCourse(new FilterCourseInputDTO() { IdCourse = input.IdCourse }).Result.Value;
            if (idcourse == null)
            {
                return BadRequest("la materia no existe");
            }
            //realiza la buscqueda de la calificacion por su id de no encontrarla devolvera un mensaje en le cual se especifica que no existe
            var update = this._context.Ratings.Where(p => p.IdRating == idRating).FirstOrDefault();
            if (update == null)
                return BadRequest("Calificacion no encontrada");
            //actualizacion de los parametros definidos
            update.Rating1 = input.Rating;  
            update.Date = input.Date;
            update.IdStudent = idStudent.First().IdStudent;
            update.IdTeacher = idTeacher.First().IdTeacher;
            update.IdCourse = input.IdCourse;

            await _context.SaveChangesAsync();
            return Ok(await _context.Ratings.ToListAsync());
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<List<RatingDTO>>> DeleteRating(int id)
        {
            //se busca la calificacion para que en caso de ser encontrada la borre mediante la funcion remove
            var delete = this._context.Ratings.Where(p => p.IdRating == id).FirstOrDefault();
            if (delete == null)
                return BadRequest("La calificacion no existe");
            _context.Ratings.Remove(delete);
            await _context.SaveChangesAsync();
            return Ok(await _context.Ratings.ToListAsync());
        }

    }

}

