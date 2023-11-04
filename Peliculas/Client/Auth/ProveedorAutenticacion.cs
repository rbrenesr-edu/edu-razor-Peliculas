using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Peliculas.Client.Auth
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(1000);
            var anonimo = new ClaimsIdentity();

            var user = new ClaimsIdentity(
                
                new List<Claim> 
                {
                    new Claim("id","304140227"),    
                    new Claim(ClaimTypes.Name,"Rafael Brenes"),
                    new Claim(ClaimTypes.Role, "admins")

                },
                authenticationType: "test");

            return await Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(user))
                );
        }
    }
}
