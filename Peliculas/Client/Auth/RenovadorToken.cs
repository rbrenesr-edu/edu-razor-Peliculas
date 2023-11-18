using System.Timers;
using Timer = System.Timers.Timer;

namespace Peliculas.Client.Auth
{
    public class RenovadorToken : IDisposable
    {
        private readonly ILoginService loginService;
        Timer? timer;

        public RenovadorToken(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        public void Dispose()
        {
           timer?.Dispose();
        }

        public void Iniciar() {
            timer = new Timer();
            timer.Interval = 1000 * 60 * 4; //4 minutos
            //timer.Interval = 1000 * 4; //4 minutos
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            loginService.ManejarRenovacionToken();
        }
    }
}
