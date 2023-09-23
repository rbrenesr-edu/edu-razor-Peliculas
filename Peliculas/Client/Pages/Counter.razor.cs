using Microsoft.AspNetCore.Components;

namespace Peliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject] ServicioSingleton servicioSingleton { get; set; } = null!;
        [Inject] ServicioTransient servicioTransient { get; set; } = null!;

        private int currentCount = 0;

        private void IncrementCount()
        {
            currentCount++;
            servicioSingleton.Valor = currentCount;
            servicioTransient.Valor = currentCount;
        }
    }
}
