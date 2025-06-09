namespace Cors.DBO;

public class ProjectsDBO
{
    public class Projects
    {
        public int id { get; set; }
        public string title_tr { get; set; }
        public string description_tr { get; set; }
        public string title_en { get; set; }
        public string description_en { get; set; }
        public string image_base64 { get; set; }
        public string href { get; set; }
        public string Used_skills { get; set; }
    }
}