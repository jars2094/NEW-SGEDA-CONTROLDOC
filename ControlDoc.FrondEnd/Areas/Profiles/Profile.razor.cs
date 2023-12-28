using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using System.Linq;
using System;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ControlDoc.Models.Models.Generic;

namespace ControlDoc.FrondEnd.Areas.Profiles
{
    public partial class Profile
    {
        #region Variables
        #region Injects
        [Inject] 
        private ICallService CallService { get; set; }
        #endregion
        #endregion

        #region Attributes
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private List<VUserDtoResponse> userList;
        private List<PerfilesDtoResponse> profileList;
        private string profiles = "";
        private VUserDtoResponse userDto = new();
        private Meta meta;

        #endregion Attributes

        #region OnInitialize
        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await GetUserProfiles();
        }

        #endregion OnInitialize

        #region Methods
        public async Task GetUserInfo()
        {
            try
            {
                int prueba = 3042;//por el momento para validar info
                Dictionary<string, dynamic> user = new()
                {
                    {"userId", prueba }
                };

                var response = await CallService.Get<List<VUserDtoResponse>>("generalviews/VUser/ByFilter", user);
                userList = response.Data;

                if(userList.Count != 0)
                {
                    userDto = userList[0];
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener informacion de usuario: {ex.Message}");
            }
        }

        public async Task GetUserProfiles()
        {
            try
            {
                int prueba = 3042;
                Dictionary<string, dynamic> userProfiles = new()
                {
                    {"ProUserid", prueba }
                };

                var response = await CallService.Get<List<PerfilesDtoResponse>>("permission/Profile/ByFilterUserId", userProfiles);
                profileList = response.Data;

                if (profileList.Count != 0)
                {
                    foreach (var p in profileList)
                    {
                        profiles += p.profile1 + ",";
                    }

                    profiles = profiles.Remove(profiles.Length - 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener informacion de usuario: {ex.Message}");
            }
        }

        private async Task TriggerFileInputClick()
        {
            await JSRuntime.InvokeVoidAsync("triggerClick", "inputFileElement");
        }

        async Task OnChange(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                await ConvertFileToFileInfo(file);
            }
        }

        private async Task<FileInfoData> ConvertFileToFileInfo(IBrowserFile file)
        {
            using var stream = file.OpenReadStream(10 * 1024 * 1024);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var base64Data = Convert.ToBase64String(ms.ToArray());

            return new FileInfoData
            {
                Name = Path.GetFileNameWithoutExtension(file.Name),
                Extension = Path.GetExtension(file.Name),
                Size = file.Size,
                Base64Data = base64Data,        
            };
        }

        #endregion Methods

    }
}
