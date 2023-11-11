using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Peliculas.Client.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Peliculas.Client.Auth
{
    public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginServices
    {
        public static readonly string TOKENKEY = "token";
        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;
        private AuthenticationState Anonimo = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));


        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient)
        {
            this.js = js;
            this.httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.ObtenerDelLocalStorage(TOKENKEY);

            //if (!string.IsNullOrEmpty(token.ToString()))
            if (token is null)
            { //representa un usaurio anónimo
                return Anonimo;
            }
            else {
                return CrearAuthenticationState(token.ToString()!);
            }
        }

      

        private AuthenticationState CrearAuthenticationState(string token) {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var claims = ParsearClaimsDelJWT(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));

        }

        private IEnumerable<Claim> ParsearClaimsDelJWT(string token) { 
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenDeserealizado = jwtSecurityTokenHandler.ReadJwtToken(token);
            return tokenDeserealizado.Claims;
        }


        public async Task Login(string token)
        {
            await js.GuardarEnLocalStorage(TOKENKEY, token);
            var authState = CrearAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await js.RemoverDelLocalStorage(TOKENKEY);
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }
    }

}
