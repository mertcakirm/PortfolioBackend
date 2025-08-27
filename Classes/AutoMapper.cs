using AutoMapper;
using ASPNetProject.Entities;
using Cors.DBO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Blog, BlogDBO.Blog>()
            .ForMember(dest => dest.Blog_image_base64, opt => opt.MapFrom(src => src.BlogImageBase64))
            .ForMember(dest => dest.Blog_description, opt => opt.MapFrom(src => src.BlogDescription))
            .ForMember(dest => dest.BLOG_Name_tr, opt => opt.MapFrom(src => src.BlogNameTr))
            .ForMember(dest => dest.BLOG_desc_tr, opt => opt.MapFrom(src => src.BlogDescTr))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.HasValue 
                                ? src.CreatedDate.Value.ToDateTime(TimeOnly.MinValue) 
                                : (DateTime?)null))
            .ForMember(dest => dest.blog_Contents, opt => opt.MapFrom(src => src.BlogContents));

        CreateMap<BlogContent, BlogDBO.Blog_Contents>()
            .ForMember(dest => dest.image_base64, opt => opt.MapFrom(src => src.ImageBase64))
            .ForMember(dest => dest.title_en, opt => opt.MapFrom(src => src.TitleEn))
            .ForMember(dest => dest.title_tr, opt => opt.MapFrom(src => src.TitleTr))
            .ForMember(dest => dest.content_en, opt => opt.MapFrom(src => src.ContentEn))
            .ForMember(dest => dest.content_tr, opt => opt.MapFrom(src => src.ContentTr));
    }
}