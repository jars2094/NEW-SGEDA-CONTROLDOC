using ControlDoc.Components.Components.Modals;
using ControlDoc.ComponentViews.ComponentViews.Search;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Documents.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Numerics;

namespace ControlDoc.ComponentViews.ComponentViews.GenericViews
{
    public partial class GenericSearchModal
    {
        #region Parameters

        [Parameter] public string? Title { get; set; }
        [Parameter] public bool showParamTrdDdl { get; set; } = true;

        [Parameter] public bool showCopiesColumn { get; set; } = true;
        [Parameter] public bool showCarge { get; set; } = true;
        [Parameter] public bool multipleSelection { get; set; } = true;
        [Parameter] public bool showNameField { get; set; } = true;
        [Parameter] public bool showLastNameField { get; set; } = true;
        [Parameter] public EventCallback<MyEventArgs<VUserDtoResponse>> OnStatusUserChanged { get; set; }
        [Parameter] public EventCallback<MyEventArgs<ThirdPartyDtoResponse>> OnStatusThirdPartyChanged { get; set; }
        [Parameter] public EventCallback<MyEventArgs<List<object>>> OnStatusMultipleUsersChanged { get; set; }

        [Parameter] public EventCallback<MyEventArgs<List<object>>> OnStatusChangedMultipleSelection { get; set; }

        [Parameter] public EventCallback<MyEventArgs<List<PersonInRadication>>> OnStatusChangeRadication { get; set; }

        [Parameter] public int ConfigurationInUse { get; set; } = 1;

        private List<PersonInRadication> radicationRecievedToReturn { get; set; } = new();
        private UserSearchComponent _userSearchComponet = new();
        private ThirdPartySearchComponent _thirdUserSearchComponet = new();
        private ModalNotificationsComponent notificationModal = new();

        #endregion Parameters

        private bool modalStatus { get; set; } = false;

        public void UpdateModalStatus(bool newValue)
        {
            modalStatus = newValue;
            StateHasChanged();
        }

        private void HandleModalClosed(bool value)
        {
            UpdateModalStatus(value);
        }

        #region UserModal

        private async Task UserModalOneSelection(MyEventArgs<VUserDtoResponse> user)
        {
            await OnStatusUserChanged.InvokeAsync(user);
        }

        private async Task UserModalMultipleSelection(MyEventArgs<List<object>> users)
        {
            await OnStatusMultipleUsersChanged.InvokeAsync(users);
        }

        private async Task OnClickAssignUserData()
        {
            await _userSearchComponet.HandleModalClosed(false);
        }

        #endregion UserModal

        #region ThirdPartyModal

        private async Task ThirdPartyModalOneSelection(MyEventArgs<ThirdPartyDtoResponse> ThirdParty)
        {
            await OnStatusThirdPartyChanged.InvokeAsync(ThirdParty);
        }

        private async Task ThirdPartyMultipleSelection(MyEventArgs<List<object>> ThirdParties)
        {
            await OnStatusChangedMultipleSelection.InvokeAsync(ThirdParties);
        }

        private async Task OnClickAssignThirdPartyData()
        {
            await _thirdUserSearchComponet.HandleModalClosed(false);
        }

        #endregion ThirdPartyModal

        #region RadicationModal

        #region RadicationRecieved

        private void SenderList(MyEventArgs<List<object>> listToManipulate)
        {
            if (listToManipulate.Data.Count == 2)
            {
                List<VUserDtoResponse> userSelected = (List<VUserDtoResponse>)(listToManipulate.Data[0] ?? new());
                List<VUserDtoResponse> userCopies = (List<VUserDtoResponse>)(listToManipulate.Data[1] ?? new());

                if (userSelected.Count != 0)
                {
                    transformUserToModel(userSelected, "Sender");
                }

                if (userCopies.Count != 0)
                {
                    transformUserToModel(userCopies, "Copy");
                }
            }
            else
            {
                List<ThirdPartyDtoResponse> ThirdPartySelected = (List<ThirdPartyDtoResponse>)(listToManipulate.Data[0] ?? new());
                List<ThirdPartyDtoResponse> ThirdPartyCopies = (List<ThirdPartyDtoResponse>)(listToManipulate.Data[1] ?? new());

                List<ThirdUserDtoResponse> ThirdUserSelected = (List<ThirdUserDtoResponse>)(listToManipulate.Data[2] ?? new());
                List<ThirdUserDtoResponse> ThirdUserCopies = (List<ThirdUserDtoResponse>)(listToManipulate.Data[3] ?? new());

                if (ThirdPartySelected.Count != 0)
                {
                    transformThirdPartyToModel(ThirdPartySelected, "Sender");
                }

                if (ThirdPartyCopies.Count != 0)
                {
                    transformThirdPartyToModel(ThirdPartyCopies, "Copy");
                }
                if (ThirdUserSelected.Count != 0)
                {
                    transformThirdUserToModel(ThirdUserSelected, "Sender");
                }

                if (ThirdUserCopies.Count != 0)
                {
                    transformThirdUserToModel(ThirdUserCopies, "Copy");
                }
            }
        }

        private void RecipientList(MyEventArgs<List<object>> listToManipulate)
        {
            if (listToManipulate.Data.Count == 2)
            {
                List<VUserDtoResponse> userSelected = (List<VUserDtoResponse>)(listToManipulate.Data[0] ?? new());
                List<VUserDtoResponse> userCopies = (List<VUserDtoResponse>)(listToManipulate.Data[1] ?? new());

                if (userSelected.Count != 0)
                {
                    transformUserToModel(userSelected, "Recipient");
                }

                if (userCopies.Count != 0)
                {
                    transformUserToModel(userCopies, "Copy");
                }
            }
            else
            {
                List<ThirdPartyDtoResponse> ThirdPartySelected = (List<ThirdPartyDtoResponse>)(listToManipulate.Data[0] ?? new());
                List<ThirdPartyDtoResponse> ThirdPartyCopies = (List<ThirdPartyDtoResponse>)(listToManipulate.Data[1] ?? new());

                List<ThirdUserDtoResponse> ThirdUserSelected = (List<ThirdUserDtoResponse>)(listToManipulate.Data[2] ?? new());
                List<ThirdUserDtoResponse> ThirdUserCopies = (List<ThirdUserDtoResponse>)(listToManipulate.Data[3] ?? new());

                if (ThirdPartySelected.Count != 0)
                {
                    transformThirdPartyToModel(ThirdPartySelected, "Recipient");
                }

                if (ThirdPartyCopies.Count != 0)
                {
                    transformThirdPartyToModel(ThirdPartyCopies, "Copy");
                }
                if (ThirdUserSelected.Count != 0)
                {
                    transformThirdUserToModel(ThirdUserSelected, "Recipient");
                }

                if (ThirdUserCopies.Count != 0)
                {
                    transformThirdUserToModel(ThirdUserCopies, "Copy");
                }
            }
        }

        private void transformUserToModel(List<VUserDtoResponse> listToTransform, string typeOfPerson)
        {
            foreach (var item in listToTransform)
            {
                PersonInRadication personInRadication = new()
                {
                    TypeOfPersonInRadication = typeOfPerson ?? "",
                    Id = (item.UserId ?? 0),
                    CompanyId = (item.CompanyId ?? 0),

                    AddressId = default,

                    FullName = item.FullName ?? "",
                    IdentificationType = item.IdentificationTypeCode ?? "",
                    IdentificationTypeName = item.IdentificationType ?? "",

                    IdentificationNumber = item.Identification ?? "",

                    AdministrativeUnitId = item.AdministrativeUnitId,

                    AdministrativeUnitCode = item.AdministrativeUnitCode ?? "",
                    AdministrativeUnitName = item.AdministrativeUnitName ?? "",

                    ProductionOfficeId = (item.ProductionOfficeId ?? 0),

                    ProductionOfficeCode = item.ProductionOfficeCode ?? "",

                    ProductionOfficeName = item.ProductionOfficeName ?? "",

                    Email1 = item.Email ?? "",

                    Email2 = "",
                    ChargeCode = item.ChargeCode ?? "",

                    Charge = item.Charge ?? "",

                    Phone1 = item.PhoneNumber ?? "",

                    Phone2 = item.CellPhoneNumber ?? "",
                };

                radicationRecievedToReturn.Add(personInRadication);
            }
        }

        private void transformThirdPartyToModel(List<ThirdPartyDtoResponse> listToTransform, string typeOfPerson)
        {
            foreach (var item in listToTransform)
            {
                PersonInRadication personInRadication = new()
                {
                    TypeOfPersonInRadication = typeOfPerson ?? "",
                    Id = item.ThirdPartyId,
                    CompanyId = item.CompanyId,

                    AddressId = default,

                    FullName = item.FullName ?? "",
                    IdentificationType = "",
                    IdentificationTypeName = "",

                    IdentificationNumber = item.IdentificationNumber ?? "",

                    AdministrativeUnitId = default,

                    AdministrativeUnitCode = "",
                    AdministrativeUnitName = default,

                    ProductionOfficeId = default,

                    ProductionOfficeCode = default,

                    ProductionOfficeName = default,

                    Email1 = item.Email1 ?? "",

                    Email2 = "",
                    ChargeCode = item.ChargeCode ?? "",

                    Charge = item.ChargeName ?? "",

                    Phone1 = item.Phone1 ?? "",

                    Phone2 = item.Phone2 ?? "",
                };

                radicationRecievedToReturn.Add(personInRadication);
            }
        }

        private void transformThirdUserToModel(List<ThirdUserDtoResponse> listToTransform, string typeOfPerson)
        {
            foreach (var item in listToTransform)
            {
                PersonInRadication personInRadication = new()
                {
                    TypeOfPersonInRadication = typeOfPerson ?? "",
                    Id = item.ThirdUserId,
                    CompanyId = default,

                    AddressId = default,

                    FullName = item.Names ?? "",
                    IdentificationType = item.IdentificationType,
                    IdentificationTypeName = item.IdentificationTypeName,

                    IdentificationNumber = item.IdentificationNumber ?? "",

                    AdministrativeUnitId = default,

                    AdministrativeUnitCode = "",
                    AdministrativeUnitName = "",

                    ProductionOfficeId = default,

                    ProductionOfficeCode = "",

                    ProductionOfficeName = "",

                    Email1 = item.Email ?? "",

                    Email2 = "",
                    ChargeCode = "",

                    Charge = "",

                    Phone1 = "",

                    Phone2 = "",
                };

                radicationRecievedToReturn.Add(personInRadication);
            }
        }

        private async Task OnClickAssignRadication()
        {
            radicationRecievedToReturn = new();
            await _userSearchComponet.HandleModalClosed(false);
            await _thirdUserSearchComponet.HandleModalClosed(false);
            notificationModal.UpdateModal(ModalNotificationsComponent.ModalType.Delete, "¿Esta seguro de su selección?", "Aceptar", true);
        }

        private async Task HandleModalNotiClose(ModalClosedEventArgs args)
        {
            if (args.IsAccepted)
            {
                var eventArgs = new MyEventArgs<List<PersonInRadication>>
                {
                    Data = radicationRecievedToReturn,
                    ModalStatus = false
                };
                await OnStatusChangeRadication.InvokeAsync(eventArgs);
            }
        }

        #endregion RadicationRecieved

        #endregion RadicationModal
    }
}