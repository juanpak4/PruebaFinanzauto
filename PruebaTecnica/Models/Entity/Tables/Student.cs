using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Models.Entity.Tables;

public class Student
{
    [Key]
    public int IdStudent { get; set; }
    public int IdentifactionNumber { get; set; }
    public string Name { get; set; } = null!;    
}
