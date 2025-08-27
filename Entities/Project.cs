using System;
using System.Collections.Generic;

namespace ASPNetProject.Entities;

public partial class Project
{
    public int Id { get; set; }

    public string TitleEn { get; set; } = null!;

    public string TitleTr { get; set; } = null!;

    public string DescriptionEn { get; set; } = null!;

    public string DescriptionTr { get; set; } = null!;

    public string? ImageBase64 { get; set; }

    public string Href { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? UsedSkills { get; set; }

    public bool? IsDeleted { get; set; }
}
