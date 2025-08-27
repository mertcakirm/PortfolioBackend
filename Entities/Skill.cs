using System;
using System.Collections.Generic;

namespace ASPNetProject.Entities;

public partial class Skill
{
    public int Id { get; set; }

    public string SkillName { get; set; } = null!;

    public int Proficiency { get; set; }

    public bool? IsDeleted { get; set; }
}
