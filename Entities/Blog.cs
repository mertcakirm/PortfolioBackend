using System;
using System.Collections.Generic;

namespace ASPNetProject.Entities;

public partial class Blog
{
    public int? Blogid { get; set; }

    public string? BlogName { get; set; }

    public string? BlogImageBase64 { get; set; }

    public string? BlogDescription { get; set; }

    public string? BlogNameTr { get; set; }

    public string? BlogDescTr { get; set; }

    public bool? ShowBlog { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<BlogContent> BlogContents { get; set; } = new List<BlogContent>();
}
