using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Peliculas.Client.Helpers;
using Peliculas.Client.Repositorios;
using Peliculas.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Peliculas.Client.Auth
{
    public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginService
    {
        public static readonly string TOKENKEY = "token";
        public static readonly string EXPITARIONTOKENKEY = "expirationtoken";
        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;
        private readonly IRepositorio repositorio;
        private AuthenticationState Anonimo = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));


        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient, IRepositorio repositorio)
        {
            this.js = js;
            this.httpClient = httpClient;
            this.repositorio = repositorio;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.ObtenerDelLocalStorage(TOKENKEY);

            //if (!string.IsNullOrEmpty(token.ToString()))
            if (token is null)
            { //representa un usaurio anónimo
                return Anonimo;
            }

            var tiempoExpiracionObject = await js.ObtenerDelLocalStorage(EXPITARIONTOKENKEY);
            DateTime tiempoExpiracion;

            if (tiempoExpiracionObject is null)
            {
                await Limpiar();
                return Anonimo;
            }

            if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
            {
                if (TokenExpirado(tiempoExpiracion))
                {
                    await Limpiar();
                    return Anonimo;
                }

                if (DebeRenovarToken(tiempoExpiracion))
                {
                    token = await RenovarToken(token.ToString()!);
                }
            }


            return CrearAuthenticationState(token.ToString()!);
            
        }



        private bool TokenExpirado(DateTime tiempoExpiracion) {
            return tiempoExpiracion <= DateTime.UtcNow;
        }

        private bool DebeRenovarToken(DateTime tiempoExpiracion)
        {
            return tiempoExpiracion.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(5);
        }

        private async Task<string> RenovarToken(string token) { 
            Console.WriteLine("renovando token...");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var nuevoTokenResponse = await repositorio.Get<UserTokenDTO>("api/cuentas/RenovarToken");
            var nuevoToken = nuevoTokenResponse.Response!;

            await js.GuardarEnLocalStorage(TOKENKEY, nuevoToken.Token);
            await js.GuardarEnLocalStorage(EXPITARIONTOKENKEY, nuevoToken.Expitarion.ToString());
            return nuevoToken.Token.ToString();
        }

        public async Task ManejarRenovacionToken() {

            var tiempoExpiracionObject = await js.ObtenerDelLocalStorage(EXPITARIONTOKENKEY);
            DateTime tiempoExpiracion;
            if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
            {
                if (TokenExpirado(tiempoExpiracion))
                {
                    await Logout();
                }

                if (DebeRenovarToken(tiempoExpiracion))
                {
                    var token = await js.ObtenerDelLocalStorage(TOKENKEY);
                    var nuevoToken = await RenovarToken(token.ToString()!);
                    var authState = CrearAuthenticationState(nuevoToken);
                    NotifyAuthenticationStateChanged(Task.FromResult(authState));
                }
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


        public async Task Login(UserTokenDTO userTokenDTO)
        {
            await js.GuardarEnLocalStorage(TOKENKEY, userTokenDTO.Token);
            await js.GuardarEnLocalStorage(EXPITARIONTOKENKEY, userTokenDTO.Expitarion.ToString());

            var authState = CrearAuthenticationState(userTokenDTO.Token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await Limpiar();
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }

        private async Task Limpiar() {
            await js.RemoverDelLocalStorage(TOKENKEY);
            await js.RemoverDelLocalStorage(EXPITARIONTOKENKEY);
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

}
