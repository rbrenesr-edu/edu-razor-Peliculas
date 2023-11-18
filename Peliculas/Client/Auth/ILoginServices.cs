using Peliculas.Shared.DTOs;

namespace Peliculas.Client.Auth
{
    public interface ILoginService
    {
        Task Login(UserTokenDTO userTokenDTO);
        Task Logout();
        Task ManejarRenovacionToken();
    }
}
