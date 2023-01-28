using PruebaTecnica.Models.Entity.Tables;
using System;

namespace PruebaTecnica.Models.DTO.Input
{
    public class FilterRatingInputDTO
    {
    public int? IdCourse { get; set; }
    public int? IdentificationTeacher { get; set; }
	public int? IdentificationStudent { get; set; }
	public string? NameStudent { get; set; }
	public string? NameTeacher { get; set; }
	public string? Course { get; set; }
    
    }
}
