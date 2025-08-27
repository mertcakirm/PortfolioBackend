using System;
using System.Collections.Generic;

namespace ASPNetProject.Entities;

public partial class Education
{
    public int Id { get; set; }

    public string? EducationText { get; set; }

    public string? Egitim { get; set; }

    public bool? IsDeleted { get; set; }
}
