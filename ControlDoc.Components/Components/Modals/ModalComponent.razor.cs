using CurrieTechnologies.Razor.SweetAlert2;

namespace ControlDoc.Components.Components.Modals

{

    public partial class ModalComponent
    {
        #region Fields
        private string imagenUrl = "../img/hoja-controldoc-modal.png";
        public enum Icons
        {
            error,
            success,
            warning,
            info,
            question
        }
        #endregion

        #region Methods

        public async Task MostrarNotificacion(string titulo, string message, Icons tipo, string ConfirmButtonText, string DeniedButtonText)
        {
            SweetAlertResult result = new SweetAlertResult();
            switch (tipo)
            {
                case (Icons.success):
                    Console.WriteLine("resultados");
                    result = await Swal.FireAsync(new SweetAlertOptions
                    {

                        Title = titulo,
                        Text = message,
                        ImageUrl = imagenUrl,
                        ImageHeight = 150,
                        ImageWidth = 200,
                        Icon = SweetAlertIcon.Success,
                        ShowCancelButton = false,
                        ShowConfirmButton = false,
                        Timer = 1000000,
                        Width = "50%",
                        CustomClass = new SweetAlertCustomClass
                        {
                            Popup = "custom-popup-style",
                            Container = "custom-image-position"
                        }


                    });


                    break;

                case (Icons.error):
                    result = await Swal.FireAsync(new SweetAlertOptions
                    {

                        Title = titulo,
                        Text = message,
                        ImageUrl = imagenUrl,
                        ImageHeight = 150,
                        ImageWidth = 200,
                        Icon = SweetAlertIcon.Error,
                        ShowCancelButton = false,
                        ConfirmButtonText = ConfirmButtonText,
                        Width = "50%",
                        CustomClass = new SweetAlertCustomClass
                        {
                            Popup = "custom-popup-style .swal2-popup",
                            Container = "custom-image-position"
                        }


                    });

                    break;

                case (Icons.info):
                    result = await Swal.FireAsync(new SweetAlertOptions
                    {

                        Title = titulo,
                        Text = message,
                        ImageUrl = imagenUrl,
                        ImageHeight = 150,
                        ImageWidth = 200,
                        Icon = SweetAlertIcon.Info,
                        ShowCancelButton = false,
                        ConfirmButtonText = ConfirmButtonText,
                        Width = "50%",
                        CustomClass = new SweetAlertCustomClass
                        {
                            Popup = "custom-popup-style .swal2-popup",
                            Container = "custom-image-position"
                        }


                    });

                    break;


                case (Icons.warning):
                    result = await Swal.FireAsync(new SweetAlertOptions
                    {

                        Title = titulo,
                        Text = message,
                        ImageUrl = imagenUrl,
                        ImageHeight = 150,
                        ImageWidth = 200,
                        Icon = SweetAlertIcon.Warning,
                        ShowCancelButton = false,
                        ConfirmButtonText = ConfirmButtonText,
                        Width = "50%",
                        CustomClass = new SweetAlertCustomClass
                        {
                            Popup = "custom-popup-style .swal2-popup",
                            Container = "custom-image-position"
                        }


                    });
                    break;

                case (Icons.question):
                    result = await Swal.FireAsync(new SweetAlertOptions
                    {

                        Title = titulo,
                        Text = message,
                        ImageUrl = imagenUrl,
                        ImageHeight = 150,
                        ImageWidth = 200,
                        Icon = SweetAlertIcon.Question,
                        ShowCancelButton = false,
                        ShowDenyButton = true,
                        ConfirmButtonText = ConfirmButtonText,
                        DenyButtonText = DeniedButtonText,
                        Width = "50%",
                        CustomClass = new SweetAlertCustomClass
                        {
                            Popup = "custom-popup-style .swal2-popup",
                            Container = "custom-image-position"
                        }


                    });


                    break;

            }

        }
    }
    #endregion
}
