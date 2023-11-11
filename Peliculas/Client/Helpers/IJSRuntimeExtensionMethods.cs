using Microsoft.JSInterop;
using Peliculas.Shared.Entities;

namespace Peliculas.Client.Helpers
{
    public static class IJSRuntimeExtensionMethods
    {
        public static async ValueTask<bool> Confirm(this IJSRuntime iJSRuntime, string message) { 
        
            return await iJSRuntime.InvokeAsync<bool>("confirm", message);
        }

        public static void Clg(this IJSRuntime iJSRuntime, string message)
        {
            iJSRuntime.InvokeVoidAsync("console.log", message);
        }

        public static ValueTask<object> GuardarEnLocalStorage(this IJSRuntime js, string llave, string contenido) {
            return js.InvokeAsync<object>("localStorage.setItem", llave, contenido);
        }

        public static ValueTask<object> ObtenerDelLocalStorage(this IJSRuntime js, string llave)
        {
            return js.InvokeAsync<object>("localStorage.getItem", llave);
        }

        public static ValueTask<object> RemoverDelLocalStorage(this IJSRuntime js, string llave)
        {
            return js.InvokeAsync<object>("localStorage.removeItem", llave);
        }

    }
}
