using AutoMapper;
using Peliculas.Shared.Entities;

namespace Peliculas.Server.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() {
            CreateMap<Actor, Actor>()
                .ForMember(x => x.Foto, option => option.Ignore());
        }
    }
}
