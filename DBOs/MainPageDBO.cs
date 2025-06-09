namespace Cors.DBO;

public class MainPageDBO
{
    public class HomePage
    {
        public int Id { get; set; }
        public string header_tr { get; set; }
        public string description_tr { get; set; }
        public string header_en { get; set; }
        public string description_en { get; set; }
        public string main_image_base64 { get; set; }
    }
}