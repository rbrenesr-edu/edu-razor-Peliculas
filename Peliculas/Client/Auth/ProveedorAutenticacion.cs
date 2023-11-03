using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Peliculas.Client.Auth
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            var anonimo = new ClaimsIdentity();
            return await Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(anonimo))
                );
        }
    }
}
