namespace Cors.DBO;

public class SkillsDBO
{
    public class Skills
    {
        public int id { get; set; }
        public string SkillName { get; set; }
        public int proficiency { get; set; }
        public bool? IsDeleted { get; set; }
    }    

}