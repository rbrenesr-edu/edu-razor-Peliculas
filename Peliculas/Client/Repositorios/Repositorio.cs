using Peliculas.Shared.Entities;
using System.Text;
using System.Text.Json;

namespace Peliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        private readonly HttpClient httpClient;
        public Repositorio(HttpClient httpClient) {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar) {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(!responseHttp.IsSuccessStatusCode, null,responseHttp);
        }

        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
            {
                new Pelicula{ 
                    Titulo = "Wakanda Forever", 
                    Lanzamiento = new DateTime(2016,11,25),
                    Poster="https://upload.wikimedia.org/wikipedia/en/thumb/3/3b/Black_Panther_Wakanda_Forever_poster.jpg/220px-Black_Panther_Wakanda_Forever_poster.jpg"
                },
                new Pelicula{ 
                    Titulo = "Moana",
                    Lanzamiento = new DateTime(2020,10,25),
                    Poster="https://upload.wikimedia.org/wikipedia/en/thumb/2/26/Moana_Teaser_Poster.jpg/220px-Moana_Teaser_Poster.jpg"
                },
                new Pelicula{ 
                    Titulo = "Inception",
                    Lanzamiento = new DateTime(2023,9,15),
                    Poster="https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg"
                }
            };
        }
    }
}
