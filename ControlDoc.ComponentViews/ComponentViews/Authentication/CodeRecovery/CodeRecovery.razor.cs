using ControlDoc.Components.Components.DropDown;
using ControlDoc.Components.Components.Input;
using ControlDoc.Components.Components.Modals;
using ControlDoc.Components.Components.Timers;
using ControlDoc.Models.Models.Authentication.CodeRecovery.Request;
using ControlDoc.Models.Models.Authentication.Login.Request;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Sockets;
using System.Net;
using ControlDoc.Models.Enums;

namespace ControlDoc.ComponentViews.ComponentViews.Authentication.CodeRecovery
{
    public partial class CodeRecovery : ComponentBase
    {
        #region Variables
        #region Injects
        [Inject]
        private EventAggregatorService EventAggregator { get; set; }

        [Inject] private ICallService CallService { get; set; }

        [Inject]
        private AuthenticationStateContainer authenticationStateContainer { get; set; }
        [Inject]
        private IJSRuntime js { get; set; }
        [Inject]
        private SweetAlertService swal { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject] private IAuthenticationJWT AuthenticationJWT { get; set; }
        #endregion
        #endregion

        private FormModel formModel = new FormModel();
        private CodeInputComponent codeInputComponent = new CodeInputComponent();
        private bool formSubmitted = false;
        CodeRecoveryDtoRequest codeRecoveryDtoRequest = new CodeRecoveryDtoRequest();
        private ModalComponent? modal { get; set; }

        [Parameter] public EventCallback<string> BotonClick { get; set; }


        private void CambiarComponente()
        {
            authenticationStateContainer.SelectedComponentChanged("PasswordRecovery");
        }
        protected override void OnInitialized()
        {
            EventAggregator.LanguageChangedEvent += HandleLanguageChanged;
            var adsdas = authenticationStateContainer.User;
            var adsdaas = authenticationStateContainer.Uuid;
            var adsdaaas = authenticationStateContainer.Ip;
        }

        private async Task HandleLanguageChanged()
        {
            StateHasChanged();
        }

        private string setKeyName(string key)
        {
            return DropDownLanguageComponent.GetText(key);
        }

        private async Task HandleValidSubmit()
        {
            if (!codeInputComponent.IsInvalid)
            {
                
                await CodeRecoverySubmit();
                //ResetForm();
            }
            
        }
        private async Task CodeRecoverySubmit()
        {
            codeRecoveryDtoRequest.code = codeInputComponent?.InputValue.ToUpper() ?? string.Empty;
            codeRecoveryDtoRequest.uuid = authenticationStateContainer.Uuid;
            codeRecoveryDtoRequest.ip = authenticationStateContainer.Ip;

            var headers = new Dictionary<string, dynamic>
                {
                    { "usernameOrEmail", authenticationStateContainer.User },
                    { "typeValidation", 2 }
                };
            //var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWVyTyI6ImV5SmhiR2NpT2lKU1UwRXRUMEZGVUNJc0ltVnVZeUk2SWtFeU5UWkRRa010U0ZNMU1USWlMQ0owZVhBaU9pSktWMVFpTENKamRIa2lPaUpLVjFRaWZRLlJRNFFFRnRVRC1sd3J6UEw4bU5qcU5kZGRpblBQN3JUVjMyUGRSNW1TX2VKU1dQNWEyUHFIdmxkcG1IUWwwMnZXcGh0MWd4SW92MFc5a3YzZURQSUNiR3VjMWc2MG9HWERKUGM2dWR6X3hWbkswbXZhanNhaHJRdTd0Nmx0a1FGdVV2YkIzUnA3cGl1LXNGWFR3VzZNLVJFeUxtWExSaWJqS05LQi1QeXdlZlVINjd5S1RzcmI5NjlTMExLekVkTndMakRlaDYyLXBrek9OalhqNjY3U0xCSWgzRm1XNW95SVFYUlVHd3RKY3Rsb1NRRHdLN3FkTElBek1FQmZRWjJLM1Bhb19uQkx2R3didEFzNTNzOVhmaDRBNWF1MXBKQVpyNENDb3VnZkd0eUZzdzlITWxSc1NaeDR2QjExc2VyOTZhcFZyQmV3ZTNYeHg5UVhXWWZGNTFOa3V2Z25iazVrb2FpbXJQMXNqRzliX0JsWk41eFJMdDRkZ0pUVzAzbkRGZUowcnRpek1oUmdRN1dqM2FQM3NCM3V0Q21ad0dNVGtzTlJHbFVQS0otRXVOckluUnk1Y01tQ0Mwb3VsM1hxQ29hM2VIaWJLZXVaZWdJT2FzWDlONVZXQlF5dUZCRG1pNjJaLTdnVkRvTVFPYUtGYk5EaDdpZHY2Z3dFV2xPSUhxWmJJbUlLOHR6Znk5ZDVsQ2pISGJtUzc1YmtlTDBmbGx6dUNKdzY4QjQtNWdPaWlCT1RqRVdFVDdiYVhaTktZam9uZmFRVlU5ZDFRcllzSWd5STRGcFc4dEFLSHU4NmFmbWVmZ2pydGtHWnFzYlFvcGlhM2lxM1pLbG1MaHJBdHNUcUFIT25XZVI0LS00UmpEbEVabTNFalk5c1ZGUFR5RlNrdmdyNDNvLmpnc3c2NDA0OENoWm1YM3VnbUM5cVEuak4wR1NrQlBuMG5PUUMwb1I5bmZrQWplR2RRdVhZU2ZXdHJOcUNXa0tWak8td29SUTA2aFpuWTZrdDBWdndmd2wwektiT3NDcDFieGJCOWFCb3V2V24weWd0cENaSHRNdzNBM3JibGhUM0taYlc1dUZCVDRJOV8yckZJbDFKUzE0Nk41TF9XVWJrT1RhRVA4eTY0QXRMeXdKVDNwZXpFYWNMSERfV3N4MXNqVmJUM011Q1hlZVRMYXZfa0loM1V1bW5pRUVJdmxVNXpPV2kzZ0FWanVMQnZCZWZyQ3ZwLWRYSTJITHBGVWJRWDlNdUlaNHJwc0JDZ1RGQkVONFVrdWNJS1VqU2dGbDdLNDM4S2xZSXdzZ1QxTnlmMDlYUFltdUJpaGFuSExtSVhyWjlacHJ1YjJrWm5MMkR6SFhtTEIyZzdNdXY3QnFyd3d2VnhTdExTRmtsRjE4Slp3dEFMZGtDRXBzM0daT2VJcUVuOXYyM2Rwc19aRFJYcmpzbU96NWhIQm5OZHh1WVZGUm1DWEQydi04d0ZBalhubTlKQWNWWWRZTkdLb1I5SEYyazZpcmxFS2tGMlBscVFMSFhlNThSMVRzM2NLTll2YU9YV0JXdXdHUEo3eDU0b1d6Mms2ZG4tNDIxeVJWM1kuSzBPNDQtRGt0ZWE2cG5tRnpMcnVSaWZGd2QxbXAwbUJNQ1F3bzhzWWNmQSIsIklkZW50aWZpZXJUIjoiZXlKaGJHY2lPaUpTVTBFdFQwRkZVQ0lzSW1WdVl5STZJa0V5TlRaRFFrTXRTRk0xTVRJaUxDSjBlWEFpT2lKS1YxUWlMQ0pqZEhraU9pSktWMVFpZlEuRXhja2FGZjVCeXh3bkNlVHh0NnQyMmpYRHVlRUF1WlM2Rm1hd04yVWZSMmcxMVJ4cFV2S3dsUWZXb0kzZHkyNG9wanlEWEUtenN6U3dOSkZEOEJ0NkMzdjU5ek8yWTYyRTgzNV81ME9IbHRUdmkzQnU3WVVWQ2M1cDE0S2p3dHZNWEx5V1FQOEJmckp3QWVTUjFnRlROQ1Ita1RHY0hydVlNbHlvRzVmLTVZQTFObWdaR3VDdGIwYml0Y2lSQ09EaC1FQ3RPQUVmRm5Td0NsMWZmRmlWaUdKNDEtQWJrUnNpN2JMZk91TDM5UEtlSE9OVlQ4WVE4QkFvTUxkV3o2TWNGVS1OSTI3OVcwemxLYXZqM2tGblYwTVc0ajVISnYzUmViX1VUVHBkZ2lOWmVpNm9GZ2VGdlRycFA2SFphWDFtaW9DYTI3T0ZTeFVab2NHdzVIYXI0TlpVU2VqT0JLMFp5Wm5CM1QxN3RycmQxZFRMcFRXOWNfbFNNZDhTeE5iRDZBNjlrS0d0NFFyWUx5dkFrbDBtU1JSNENyNDVGN21pcXhBMExwNzFUMUlhYUd6d3lPRTU1cHRqNlNNeVRraGpyNWhuZ2FNRW00aVYxeDgyazRPNXpIZzFDanhYQ1ZNc3FCTmpVRDJaX1lQWG9HLU44Mk1TU3VVVWI4ZzFvVV9xOXdkaDlYQTB2bXV0TWgyeHA5NUdXYjVpYnZnYi1Hb2dxaFhGWkJEQTktX3JUdTVfVW1ZV1RJb1BIUkh3eGEtci1jNGttZnl5eVZwTkVvb3hEQnBiMDgtNFBzRzZrQXE4ZUFZbmkwM19pS3RoNnJObW0xQnJJS1V2UlJDRUxMWUFtSUtvdHIyTVNFNHIzd2ZNdEdIYzE0RmpRMm1GR3lycWNUM3NnaFptckkuSGVGbjlkbFdMa3NtdUprOHVYeUtTUS5uUEoyY1E2NFlZc1c2VzVTMUFWMi1PZmh5NXZSai1qQm52ZWhtM2RoaTR6S28wM1JreTBZRkJ3UjVnOVY1NmZ0bVlXYUdpLWczNUwtUHRPQnFQVlhWOVVNanVxSzg1TzdfVFRHUWpEMVZxWWh2WHZMcHotVTYwTldBU01iemFuMFAzUkVHblp1UnI2Z3VOdUhHdFMwaEsxRUNaMnFJQTZmWmtiTlZPT1JIX09GXzQwTW5qRG9fSmJWcl9YdTUwam45ZXAtNjdsRkE4MFlDVmRHYXBhZHFZMDlrUmpqWm4zQVk3c3N0bWc3aDAtZm1ZeUJuTUNwXzUzN2RuVGpMRUxaVDdTOEp5WW9KLU4yaGtHaV9jSF9nZjc1OUJwYlQ4N3Z2VTZnRGRxOUlneEFlakQ3QVpsSTIzSHRRVGZyeUNkV09xSGJhZVVPMjhWYXRodWtvNGJ0d1JUQTY4Mkw4a0ZVU2NmSk9jLVdaSDRDd1Z0a1llM2RUYTlrbi0xNGtELVBwaG1QdmZzZDBmNHBhWEdZMXkwYk1ZS1ltcElyc29nMFBoSWFZc05SR0s3XzZlVENSaVAxS0RkNGlKejJKRVhxMHZoTzl4dlZ2aTFjY01KSG9POV8xU19pS0tSa0wwekk4X0UwdDNrcFk3VG9jV1lXeFRXUE05Z2tFdXJpd2VIOXZBVW5mQktPUmRPVWlPR0JjckdVTm0xdW1DbHBkcTRDbWJZeFhiT2h5WndmR29qWEdzc0dla2p4TkFRNVZ1VzBha1hwUlR5SkE4MkRRZWlEUHhkVVI4UGRBQjNGd2Vwei04Rjg0SVVoSjRaMzVyeVk0MTZZMGVwN1RfeDMyWkxjWjQzUGh4ZFNHUjN6MVNDaExwdUVVdHF3aTJTYkdKR21xVWFLcWJ4MENsVkJkWU0yaFZCTWN1UU4yVkttUzNPSmRzZm9Fem41R3F0aXJtR0xaX0I1MmR2Y0lSNEdDOHZoT0xScWExbkV5ZkdzOW1QSldWNnVMa25KLUh4dFB3MU9LalJaWjdVZE5ST3ZmSFRGSWRVS0RtNnRqdnpDM3pxekluNzVjYk50bXZJQ0xqM0JrdXZudnNlSndjVWdVR0FBbTMxaGFVUllLekRfWVhQMVlXWnlEbmFmYkpmU2Q5MlR2NVl0c05uVDVCWVZmRG9mT3JCS2RkWkZsV1pjQWh0dHVfek90R2d6Q1FIS3JPS3NiT2oyT05Fc25LUVRyYmFoTWFsVV9LdmJXbWgxX2RRcFlSdE9nUmh1ZzBkN2ZZMzBvYmJtd0N3UTZoT2lPV29TREZ5TFFEaHJ3ZEtYM3lOQzJSZm1EVHd6Z1l5S0VkUGZyckwwR0o0dWNRV2sycDFFcjhEOEZvMDRfcFBNdmlMR3d0WFViNFBLYnFzMWFiakE3UXZPUDdMNzdQVVR5aDNLcDNsZGxYTXJNNmx3dUI2a29Ebmd0dnlHcEM0ejZRSi1sSU1fdXQzY2hxZEUtM1VFaGE5QmRnVzNlTDZXUndaUTVNTUZNZmxyTG5mZnBvZnA0YTFnWWFvM1Y3Y0ZncGNScDFIQ240Q1pKZXBNblJ0UjE1TjREbFJWMkJTTlk4VG9CSlRoUlNGUHZrZlVPQk1FcVA1bDZvU0pZdlFKSmpuZk5kTENGWWYya281YkJ3cnlERjRlbm1pY3dXaGpjYmt6azFpVG1kNzg5Mk5aTDA4QnBzMDJZSGlHT1R0T2FtTDBLMXhEb0lXeFludTQ3RzQ2aGhZUFJueUw0OUc5Q2xpY0ZTZk52ck5yNXRZRk9aeDNtaWlmOHpnUklwQjIyVkNQWWtNbG5IZ19KVEZtVTZDWlZMZGxTaGczQURxLVFZRXJaZ1RBaFFMbVNMQmFFSVdqckIteUs0ZTdVNHU2ZWhrYW92ZHozMEdlWHVNaFdJZnhGN1JUVGRoRm43cU1xelRTRzRzTFh4N0QzWjZvRDdyVE1kSmpEOWpJN0xrbEpXT1RIa3FzZG9EN2ZSY292YTh1emt5UWNMYVBaWnNtam52QnQwcnBiNGpGUWtHNHIwMmpSQ283aGk1ZGplR25VWjV3NElDSWFaX3gweThSMVlIbjBBbDNxMFRVZlhlZ2VaeHZRYUFncnA0bEZjNFByZ3pjelZhRlV3d2NtRk9PSkE0dFh0eGFXeXFtV3VlU0M3bTJ1R3ZWZ25acEdVZWhBR2VpUC12MkhwbEFhbTI0UmJTQUUyN01laEFZbWs4VnFTamFyVlBiazNPUjI3RHRqRGlrSi1naUhzQ05KbnFrLW1SbmRtM0F4aUFGVjhXZnRnNVFCVWo3c1U5ZmtsSFM0d3BkdUJQNXhVUmdtTU9kOFJfQ2VaX2QzMEhROVFWMVQ5WDAwTW5ycXU4cmRMYWRsYVpDcVJ5UGgtVW5oem5NVmFvVURubkpXeUpwTlNwZjJheHo1aTU4N2JqZGw4Y3ZrMUlUcHY0WUxBMjZmVXEtTlhxSlpCRVZLd3FxQlJpOVExTHhJV21jakJtQWYwcUt3aHotc3lDV0NkeTg1M1ZVYk1wZTI5QXcxNTdjTHJoSFhNT0t5dnBoN2tyYlV2a3pBS0c3Q2RCSl9zOGMzMHNhckp1ZEdVbFczaTZ1Z1h3RzJ4bzhmUkZEcHZNR1ZUYndFWWtQd19hRnpqenpzSEZFQTh3cTVNMHR3MWxFTjAxbVotWWw5bTBkcW9aZ2hPN1hpRFNFTU1vcWNFVFVDQ0RPLUIwNDcyZWVNOU1Lb1Q5TVY2aWxhWDgtZkhxWHEzcFFSNThmcV9nSUxKYzRLNHJlbVI1R1FkMG9pcTQ1eVNvZjRXRjJ0RkJOeS11RVlnU21LYjBKZmhsQmUzQkh4SEs4alhwblFxdHVod1pDaW1wSzY1ejZnZXdLdTV2SUxfVlkzaEJUWlJtRHhMUUU5NmlNRlphRDBvUUpNZmhRWE0yaldaUXJXenJkU3ZuaWEyLU55RTBDS1JLMjNkS0pMTFc0Wml5NzhRLThEbm1wMk1zVVdtR2VsRjFXQ0laTjM5bGhRbEJoYTRzWVlsQko4UHJlWGRZRWFFZTl6VGdWbmZNcmZFTklRWDV6NkZObldBZ2JYVEpLeEJGZ0RMVnd1ckdaMzBkUmV1d1NxMzJKa0l5M2otbkVkV1o3TUs2M25VdHpvRDJadmNiSXUxeXJJQ3FIekJueFpRN0R0UGx6LWdKOWJxd1JPMkR0ejJub0N2VlAySWRabE5XR1IxUzJVS1hSdVludHc2cXM5OFgxMXFJb2o3UEdYRGlBenZjeG1YSDhJTkQ4THNMemZJX3BzRkxsaWpvU1RMMGZpSEpab2NFS0RvNFRwaWt5Q0c2Y0RGVnlnRnpHS1dwWFBiV2RYT2lERWNGZEJocnBid1E0OUxDenByaUtVYUZUOW1RTlQtei16aXZTUXFSUHVEUlBjUzIzeXJURVcxeFk1bEtLOUU4S0pSLTlRNEs1aUhzWWZGVTRyLWo3MlNPY19wN1ZCUGpEQXRoaDlHSDlNeFdaa09id2g4Z1J3NHU3WUZybEdsZVNuYWxOd2J0Zl9uZkNZZGRsVVZyY2k0SUlLRVJWczQxUDBEZmJVbXZqaFh1Tjh0QUYtYnBwdW81NDRVQzVHbkU3Q1RyTkNCRlVjclFEQkhFMVpsaHJ5aGlJNHlMUXRlZlZEcVV3dlZucGd0RE94SzV3eWNhVDBNZ25qc0VWYnpDMGhBLUlqTmFhMGx3c2xad0RLa2tBbklJZVFZU3RpZ1BvTktXd0xuSGJfOEZDdWU3MmVDSjFYT3lVTGdDOVFIZ1VaY3pic3BuMTVJYWVta1N4bTZLeURIWVlBS0FHM1gyMy1wdFYuaXM0dUpBVVRyZ3U0R3VmNWZVa3prSEpYUE9aRk44cHJYUENmTjNoZVR2NCIsIm5iZiI6MTcwMzYyMTM4MiwiZXhwIjoxNzAzNjY0NTgyLCJpc3MiOiJodHRwOi8vMjUuNTYuMTYwLjIwMzo3MDAwIiwiYXVkIjoiaHR0cDovLzI1LjU2LjE2MC4yMDM6NzAwMCJ9.HTBi3GP8mh_LucjoJ9_SSOLS9XD3mkeIlCw3O3JVVsEMCyeMd0K8Vd4c4S5EG9XFbGbQmUIFxuLOVkptLIpmhw";

            //await AuthenticationJWT.LoginToken(token);
            //NavigationManager.NavigateTo("Home");

            try
            {
                var CodeRecoveryResponse = await CallService.Put<string, CodeRecoveryDtoRequest>("security/Session/UpdateRecoveryCode", codeRecoveryDtoRequest, headers);
                if (CodeRecoveryResponse.Succeeded)
                {
                    await AuthenticationJWT.LoginToken(CodeRecoveryResponse.Data);
                    NavigationManager.NavigateTo("Home");
                }
                else
                {
                    await modal!.MostrarNotificacion("Error!", "Codigo incorrecto.", ModalComponent.Icons.error, "Aceptar", "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al con el codigo de verificación: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            
            
            formSubmitted = false;
            codeInputComponent.Reset();

            // Implementar el método HandleCodeRecoverySubmit
        }

        private async Task HandleCodeRecoverySubmit()
        {
            //ToDoHandleRegisterSubmit
        }



        private class FormModel
        {
            public string Name { get; set; } = string.Empty;

            public string Password { get; set; } = string.Empty;
        }

        #region Metodos
        #region Envio al login
        private void ReturnLogin()
        {
            authenticationStateContainer.SelectedComponentChanged("Login");
        }
        #endregion
        #endregion
    }
}
