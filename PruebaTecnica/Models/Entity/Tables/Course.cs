using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Models.Entity.Tables;

public class Course
{
    [Key]    
    public int IdCourse { get; set; }
    public string Course1 { get; set; } = null!;    
}
