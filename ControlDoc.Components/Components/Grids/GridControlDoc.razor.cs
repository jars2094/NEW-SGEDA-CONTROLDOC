using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Components.Components.Grids
{
    public partial class GridControlDoc<T> : ComponentBase where T : class
    {
        #region Injects

        [Inject]
        private IJSRuntime Js { get; set; }

        #endregion

        #region Component Parameters

        // Lista de objetos de tipo genérico T que se mostrarán en la grilla.
        [Parameter] public List<T> DataObjectList { get; set; }

        // Lista de títulos de columna que se utilizarán en la grilla.
        [Parameter] public List<string> TitlesList { get; set; }

        // Lista de nombres de propiedades que se utilizarán como campos de datos en la grilla.
        [Parameter] public List<string> PropertyNames { get; set; }

        
        

        // Tamaño de la página en la grilla que determina cuántos registros se mostrarán por página.
        [Parameter] public int PageSize { get; set; }

        // Parametro para saber si se muestra o no los botones de borrar
        [Parameter] public bool IsEditColumn { get; set; }

        // Evento que se invoca cuando se selecciona un registro en la grilla.
        // El valor seleccionado se pasa como parámetro en el evento.
        [Parameter] public EventCallback<MyEventArgs<T>> OnRecordSelected { get; set; }
        [Parameter] public EventCallback<MyEventArgs<T>> OnRecordSelectedDelete { get; set; }

        #endregion


        #region Private Variables

        private T SelectedRecord;

        #endregion

        #region Component Methods

        private async Task ShowModalEdit(T response)
        {
            SelectedRecord = response;

            var eventArgs = new MyEventArgs<T>
            {
                Data = SelectedRecord,
                ModalStatus = true // Aquí puedes configurar el valor de modalStatus como desees
            };

            await OnRecordSelected.InvokeAsync(eventArgs);
            //await Task.Delay(10);
            //ShowModalsService.ShowModal(Js, ModalId);
        }

        private async Task DeleteRecord(T response)
        {
            SelectedRecord = response;
            var evetArgs = new MyEventArgs<T>
            {
                Data = SelectedRecord,
                ModalStatus = true
            };

            await OnRecordSelectedDelete.InvokeAsync(evetArgs);
            await Task.Delay(10);
        }

        #endregion

    }
}
  

