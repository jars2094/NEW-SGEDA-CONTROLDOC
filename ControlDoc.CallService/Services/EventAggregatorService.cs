namespace ControlDoc.Services.Services
{
    public class EventAggregatorService
    {


        #region Atributos
        public event Func<Task>? LanguageChangedEvent;

        #endregion

        #region Metodos
        public async Task PublishLanguageChanged()
        {
            if (LanguageChangedEvent != null)
            {
                await LanguageChangedEvent.Invoke();
            }
        }
        #endregion
    }
}
