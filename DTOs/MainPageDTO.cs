namespace Cors.DTO;

public class MainPageDTO
{
    
    public class RequestMain
    {
        public int Id { get; set; }
        public string header_tr { get; set; }
        public string description_tr { get; set; }
        public string header_en { get; set; }
        public string description_en { get; set; }
    }

    public class Image_base64{
        public string main_image_base64 { get; set; }

    }
}