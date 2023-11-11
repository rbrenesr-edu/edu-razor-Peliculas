namespace Peliculas.Client.Auth
{
    public interface ILoginServices
    {
        Task Login(string token);
        Task Logout();
    }
}
