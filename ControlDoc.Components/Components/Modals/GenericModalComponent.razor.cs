using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Components.Components.Modals
{
    public partial class GenericModalComponent : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public bool IsVisible { get; set; } = false;

        [Parameter]
        public string Width { get; set; }

        [Parameter]
        public string CancelText { get; set; } = "Cancel";

        [Parameter]
        public string ConfirmText { get; set; } = "Ok";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public string Id { get; set; }

        [Parameter]
        public bool Show { get; set; }
        [Inject]
        private IJSRuntime Js { get; set; }

        [Parameter]
        public EventCallback<bool> OnModalClosed { get; set; }

        private string cosa = "1000px";

        private async Task CloseModal()
        {
            //ShowModalsService.hideModal(Js, Id);
            await OnModalClosed.InvokeAsync(false); 
        }

        //private void OnCloseButtonClick()
        //{
        //    ShowModalsService.ShowModal(Js, Id);
        //    IsVisible = false;
        //}



    }
}
