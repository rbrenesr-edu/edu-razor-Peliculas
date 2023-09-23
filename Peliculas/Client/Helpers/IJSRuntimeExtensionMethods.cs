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
    }
}
