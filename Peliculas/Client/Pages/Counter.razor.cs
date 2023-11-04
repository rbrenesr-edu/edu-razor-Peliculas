using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Peliculas.Client.Helpers;
using System.Runtime.Serialization;

namespace Peliculas.Client.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;
        [Inject] private IJSRuntime js { get; set; }
        [CascadingParameter] private Task<AuthenticationState> auth { get; set; } = null!;

        public async void IncrementCount()
        {
            var arreglo = new double[] { 1, 2, 3, 4, 5 };
            var max = arreglo.Maximum();
            var min = arreglo.Minimum();

            //await js.InvokeVoidAsync("alert", $"El max es { max }, el min es { min }");

            var authState = await auth;
            var userAuth = authState.User.Identity.IsAuthenticated;

            if (userAuth)
            {
                currentCount++;

            }
            else
            {
                currentCount--;
            }

        }
    }
}
