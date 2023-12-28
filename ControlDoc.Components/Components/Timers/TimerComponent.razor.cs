using Microsoft.AspNetCore.Components;


namespace ControlDoc.Components.Components.Timers
{
    public partial class TimerComponent : ComponentBase
    {
        [Parameter]
        public TimeSpan TimeDuration { get; set; } = TimeSpan.FromSeconds(30); // Tiempo por defecto
        [Parameter]
        public string ResendButtonText { get; set; } = "Reenviar código"; // Texto por defecto del botón
        [Parameter]
        public EventCallback OnTimerEnd { get; set; }

        private CancellationTokenSource cts = new CancellationTokenSource();
        private TimeSpan timeLeft;
        private bool isButtonDisabled = true;

        protected override void OnInitialized()
        {
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timeLeft = TimeDuration;
            isButtonDisabled = true;
            RunTimer();
        }

        private async void RunTimer()
        {
            while (timeLeft > TimeSpan.Zero)
            {
                try
                {
                    await Task.Delay(1000, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    return; // Cancelar si el token de cancelación se activa
                }

                timeLeft = timeLeft.Add(TimeSpan.FromSeconds(-1));
                StateHasChanged(); // Re-render the component
            }

            isButtonDisabled = false;
            StateHasChanged(); // Re-render the component

            if (OnTimerEnd.HasDelegate)
            {
                await OnTimerEnd.InvokeAsync(null);
            }
        }

        private async Task ResendCode()
        {
            if (!isButtonDisabled)
            {
                // Ejecutar lógica para reenviar código aquí
                // ...

                // Reiniciar el temporizador
                cts.Cancel();
                cts.Dispose();
                cts = new CancellationTokenSource();
                InitializeTimer();
            }
        }

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
        }
    }
}
