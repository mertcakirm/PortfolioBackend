namespace Cors.DTO;

public class SkillsDTO
{
    public class RequestSkill
    {
        public int id { get; set; }
        public string SkillName { get; set; }
        public string proficiency { get; set; }
        public bool? IsDeleted { get; set; }
    }
}