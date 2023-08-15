using AutoMapper;
using GameZone.Blog.Application.DTOs;
using GameZone.Blog.Application.DTOs.Request;
using GameZone.Blog.Application.DTOs.Response;
using GameZone.Blog.Domain.Entities;

namespace GameZone.Blog.Application.ProfileMapper
{
    public class NoticiaProfile : Profile
    {
        public NoticiaProfile()
        {
            CreateMap<Noticia, NoticiaDTO>()
                 .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Conteudo));

            CreateMap<NoticiaDTO, Noticia>()
                .ForMember(dest => dest.Conteudo, opt => opt.MapFrom(src => src.Descricao));

            CreateMap<Noticia, UpdateNoticiaDTO>()
                 .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Conteudo));

            CreateMap<UpdateNoticiaDTO, Noticia>()
                .ForMember(dest => dest.Conteudo, opt => opt.MapFrom(src => src.Descricao));

            CreateMap<Noticia, CreateNoticiaDTO>()
                 .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Conteudo));

            CreateMap<CreateNoticiaDTO, Noticia>()
                .ForMember(dest => dest.Conteudo, opt => opt.MapFrom(src => src.Descricao));

            CreateMap<Comentarios, ComentarioDTO>().ReverseMap();
            
            CreateMap<Comentarios, CreateComentarioDTO>().ReverseMap();
            
            CreateMap<Comentarios, UpdateComentarioDTO>().ReverseMap();
        }
    }
}
