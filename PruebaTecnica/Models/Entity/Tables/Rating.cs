using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Models.Entity.Tables;

public partial class Rating
{
    [Key]
    public int IdRating { get; set; }
    public int IdCourse { get; set; }
    public int IdTeacher { get; set; }
    public int IdStudent { get; set; }
    public decimal Rating1 { get; set; }
    public DateTime Date { get; set; }

}
