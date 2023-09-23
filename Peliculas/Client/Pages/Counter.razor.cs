using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System.Runtime.Serialization;

namespace Peliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject] ServicioSingleton servicioSingleton { get; set; } = null!;
        [Inject] ServicioTransient servicioTransient { get; set; } = null!;
        [Inject] IJSRuntime js { get; set; } = null!;

        IJSObjectReference jsObjectReference { get; set; } = null!;
        
        private int currentCount = 0;
        private static int currentCountStatic = 0;

        [JSInvokable]
        public async Task IncrementCount()
        {
            
            await jsObjectReference.InvokeVoidAsync("MostrarAlerta", 
                "Mostrando una alerta desde modulo de js");

            currentCount++;
            currentCountStatic = currentCount;
            servicioSingleton.Valor = currentCount;
            servicioTransient.Valor = currentCount;
            await js.InvokeVoidAsync("testNetStatic");
        }

     
        private async Task IncrementCountJavaScript()
        {            
            await js.InvokeVoidAsync("testNetInstancia",
                DotNetObjectReference.Create(this));
        }



        [JSInvokable]
        public static Task<int> IncrementCountStatic() {
            return Task.FromResult(currentCountStatic);
        }
    }
}
