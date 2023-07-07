using AutoMapper;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Domain.Entidades;

namespace GameZone.Identidade.Application.ProfileMapper
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUsuarioDto, Usuario>().ReverseMap();               
            CreateMap<LoginUsuarioDto, Usuario>().ReverseMap();               
        }
    }
}
