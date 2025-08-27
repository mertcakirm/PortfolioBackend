using System;
using System.Collections.Generic;

namespace ASPNetProject.Entities;

public partial class Homepage
{
    public int Id { get; set; }

    public string HeaderEn { get; set; } = null!;

    public string HeaderTr { get; set; } = null!;

    public string DescriptionEn { get; set; } = null!;

    public string DescriptionTr { get; set; } = null!;

    public string? MainImageBase64 { get; set; }
}
