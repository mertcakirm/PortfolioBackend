using System;
using System.Collections.Generic;

namespace ASPNetProject.Entities;

public partial class BlogContent
{
    public int? Id { get; set; }

    public string TitleEn { get; set; } = null!;

    public string TitleTr { get; set; } = null!;

    public string ContentEn { get; set; } = null!;

    public string ContentTr { get; set; } = null!;

    public string? ImageBase64 { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? Blogid { get; set; }

    public virtual Blog? Blog { get; set; }
}
