using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Peliculas.Client.Helpers;
using System.Runtime.Serialization;

namespace Peliculas.Client.Pages
{
    public partial class Counter
    {                      
        private int currentCount = 0;
        
        public void IncrementCount()
        {                        
            currentCount++;                       
        }     
    }
}
