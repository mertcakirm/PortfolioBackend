using Cors.DBO;

namespace Cors.DTO;

public class ProjectPageResponse
{
    public List<ProjectsDBO.Projects> Projects { get; set; }
    public int TotalCount { get; set; }
}