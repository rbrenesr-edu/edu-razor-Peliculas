using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Peliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject] ServicioSingleton servicioSingleton { get; set; } = null!;
        [Inject] ServicioTransient servicioTransient { get; set; } = null!;

        [Inject] IJSRuntime js { get; set; } = null!;



        private int currentCount = 0;
        private static int currentCountStatic = 0;

        private async void IncrementCount()
        {
            currentCount++;
            currentCountStatic = currentCount;
            servicioSingleton.Valor = currentCount;
            servicioTransient.Valor = currentCount;
            await js.InvokeVoidAsync("testNetStatic");
        }


        [JSInvokable]
        public static Task<int> IncrementCountStatic() {
            return Task.FromResult(currentCountStatic);
        }
    }
}
