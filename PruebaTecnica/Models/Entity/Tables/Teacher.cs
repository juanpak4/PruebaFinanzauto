using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PruebaTecnica.Models.Entity.Tables;

public partial class Teacher
{
    [Key]
    public int IdTeacher { get; set; }
    public int IdIdentificationNumber { get; set; }
    public string Name { get; set; } = null!;    
}
