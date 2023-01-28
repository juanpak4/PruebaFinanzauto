using PruebaTecnica.Models.Entity.Tables;
using System;

namespace PruebaTecnica.Models.DTO.Input
{
    public class RatingInputDTO
    {
    public int IdCourse { get; set; }
    public int IdentificationTeacher { get; set; }
	public int IdentificationStudent { get; set; }
    public decimal Rating { get; set; }
	public DateTime Date { get; set; }
    }
}
