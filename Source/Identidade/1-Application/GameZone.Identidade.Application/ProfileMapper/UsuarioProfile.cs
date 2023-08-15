using AutoMapper;
using GameZone.Identidade.Application.DTOs;
using GameZone.Identidade.Application.DTOs.Response;
using GameZone.Identidade.Domain.Entities;

namespace GameZone.Identidade.Application.ProfileMapper
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUsuarioDto, Usuario>().ReverseMap();               
            CreateMap<LoginUsuarioDto, Usuario>().ReverseMap();               
            CreateMap<LoginUsuario, LoginUsuarioDto>().ReverseMap();               
            CreateMap<UsuarioDto, Usuario>().ReverseMap();               
        }
    }
}
