namespace Peliculas.Client.Repositorios
{
    public class HttpResponseWrapper<T>
    {
        
        public HttpResponseWrapper(bool error, T? response, HttpResponseMessage? httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage? HttpResponseMessage { get; set; }
    }
}
