using AutoMapper;
using GameZone.Blog.Application.DTOs;
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

            CreateMap<Noticia, CreateNoticiaDTO>()
                 .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Conteudo));

            CreateMap<CreateNoticiaDTO, Noticia>()
                .ForMember(dest => dest.Conteudo, opt => opt.MapFrom(src => src.Descricao));

        }
    }
}
