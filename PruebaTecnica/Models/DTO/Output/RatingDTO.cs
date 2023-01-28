namespace PruebaTecnica.Models.DTO.Input
{
    public class RatingDTO: RatingInputDTO
    {

        public int IdRating { get; set; }
        public string? TeacherName { get; set; }
        public string? StudentName { get; set; }
        public string? CourseName { get; set; }  
        
    }
}
