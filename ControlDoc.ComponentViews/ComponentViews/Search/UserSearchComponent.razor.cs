using ControlDoc.Components.Components.Input;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Models.Models.SystemFields;
using ControlDoc.Services.Interfaces;
using ControlDoc.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ControlDoc.ComponentViews.ComponentViews.Search
{
    public partial class UserSearchComponent : ComponentBase
    {
#nullable enable

        [Inject]
        private IJSRuntime Js { get; set; }

        private Meta meta { get; set; } = new() { pageSize = 10 };
        private Dictionary<string, dynamic> headers { get; set; } = new();

        #region Parameters

        [Parameter] public bool showParamTrdDdl { get; set; } = new();

        [Parameter] public bool showCopiesColumn { get; set; } = new();
        [Parameter] public bool showCarge { get; set; } = new();
        [Parameter] public bool multipleSelection { get; set; } = new();

        [Parameter] public EventCallback<MyEventArgs<VUserDtoResponse>> OnStatusChanged { get; set; } = new();

        [Parameter] public EventCallback<MyEventArgs<List<object>>> OnStatusChangedMultipleSelection { get; set; } = new();
        public string? firstName { get; set; }
        public string? lastName { get; set; }

        public string? IdModalIdentifier { get; set; }
        private bool selectAllManager { get; set; } = new();
        private bool selectAllCopies { get; set; } = new();

        #endregion Parameters

        #region administrativeunidFields

        private List<AdministrativeUnitDtoResponse>? administrativeUnitList { get; set; } = new();
        private bool isEnableAdministriveUnit { get; set; } = new();
        private bool isFilterableAdministriveUnit { get; set; } = new();
        private int selectAdministriveUnitId { get; set; } = new();

        #endregion administrativeunidFields

        #region productionOfficeFields

        private List<ProductionOfficesDtoResponse>? productionOfficeList { get; set; } = new();
        private bool isEnableProductionOffice { get; set; } = new();
        private bool isFilterableProductionOffice { get; set; } = new();

        private int selectProductionOfficetId { get; set; } = new();

        #endregion productionOfficeFields

        #region chargeFields

        private List<SystemFieldDtoResponse>? chargeList { get; set; }
        private bool isEnableCharge { get; set; }
        private bool isFilterableCharge { get; set; }

        private string? selectChargeCode { get; set; }

        #endregion chargeFields

        #region NameFields

        [Parameter] public bool showNameField { get; set; } = new();
        [Parameter] public bool showLastNameField { get; set; } = new();

        public InputModalComponent? firstNameInput { get; set; } = new();
        public InputModalComponent? lastNameInput { get; set; } = new();

        #endregion NameFields

        #region gridFields

        public List<VUserDtoResponse>? UserList { get; set; } = new();
        private bool dataChargue = false;

        #endregion gridFields

        #region FieldsToReturn

        private List<VUserDtoResponse> usersInManagerToReturn { get; set; } = new();
        private List<VUserDtoResponse> usersInCopiesToReturn { get; set; } = new();

        public VUserDtoResponse? userToReturn { get; set; } = new();

        #endregion FieldsToReturn

        #region FillingData

        private async Task FillAdministrativeUnitDdl()
        {
            var response = await CallService.Get<List<AdministrativeUnitDtoResponse>>($"paramstrd/AdministrativeUnit/ByAdministrativeUnits");

            administrativeUnitList = response.Data;
        }

        private async Task FillChargeDdl()
        {
            headers = new()
            {
                { "ParamCode", "CAR" }
            };
            var response = await CallService.Get<List<SystemFieldDtoResponse>>($"params/SystemFields/ByFilter", headers);

            response.Data!.ForEach(x => { x.Code = $"CAR,{x.Code}"; });

            chargeList = response.Data;
        }

        private async Task FillPoductionOfficeDdl()
        {
            headers = new()
            {
                { "AdministrativeUnitId", selectAdministriveUnitId.ToString() }
            };

            var response = await CallService.Get<List<ProductionOfficesDtoResponse>>($"paramstrd/ProductionOffice/ByProductionOffices", headers);
            productionOfficeList = response.Data ?? default;
        }

        #endregion FillingData

        [Inject] private ICallService CallService { get; set; }

        #region OnActions

       

        public async Task OnClickButton()
        {
            selectAllManager = false;
            selectAllCopies = false;

            headers = new()
                  {
                        { "firstName", firstNameInput?.InputValue ?? ""},
                         { "lastName",lastNameInput?.InputValue ?? "" },
                         { "administrativeUnitId",selectAdministriveUnitId.ToString()},
                         { "productionOfficeId",selectProductionOfficetId.ToString()},
                         {"ChargeCode",selectChargeCode??"" }
                  };

            var response = await CallService.Get<List<VUserDtoResponse>>($"generalviews/VUser/ByFilter", headers);

            if (response != null || response.Data.Count != 0)
            {
                UserList = response.Data;
                meta = response.Meta;
                dataChargue = true;
                reactiveExistingData(UserList);
                StateHasChanged();
            }
            else
            {
                UserList = new();
                meta = new() { pageSize = 10 };
            }
        }

        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
            if (showParamTrdDdl)
            {
                isEnableAdministriveUnit = true;
                isFilterableAdministriveUnit = true;
                await FillAdministrativeUnitDdl();
            }

            if (showCarge)
            {
                isEnableCharge = true;
                isFilterableCharge = true;

                await FillChargeDdl();
            }
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }

        public async Task Reset()
        {
            usersInManagerToReturn = new();
            usersInCopiesToReturn = new();
            userToReturn = null;

            administrativeUnitList = new();
            isEnableAdministriveUnit = false;
            isFilterableAdministriveUnit = false;
            selectAdministriveUnitId = 0;

            productionOfficeList = new();
            isEnableProductionOffice = false;
            isFilterableProductionOffice = false;
            selectProductionOfficetId = 0;

            chargeList = new();
            isEnableCharge = false;
            isFilterableCharge = false;
            selectChargeCode = "";

            firstNameInput = new();
            lastNameInput = new();
            UserList = new();

            chargeList = new();
            await OnInitializedAsync();

            StateHasChanged();
        }

        public async Task OnChangeUA(int value)
        {
            selectAdministriveUnitId = value;
            productionOfficeList = new();
            if (selectAdministriveUnitId != 0)
            {
                await FillPoductionOfficeDdl();
            }

            if (productionOfficeList.Count != 0)
            {
                isEnableProductionOffice = true;
                isFilterableProductionOffice = true;
            }
            else
            {
                isEnableProductionOffice = false;
                isFilterableProductionOffice = false;
            }
        }

        public void OnChangeOP(int value)
        {
            selectProductionOfficetId = value;
        }

        public void OnChangeCh(string value)
        {
            selectChargeCode = value;
        }

        private void SelecteUser(VUserDtoResponse user)
        {
            UserList.Where(x => x.UserId != user.UserId).ToList().ForEach(x => { x.Selected = false; });
            userToReturn = user;
        }

        private async Task ChangeStateManager(VUserDtoResponse user)
        {
            if (!multipleSelection)
            {
                SelecteUser(user);
                await HandleModalClosed(false);
            }
            else
            {
                selectAllManager = false;

                if (user.Selected && user.Copy)
                {
                    user.Copy = false;
                    await ChangeStateCopies(user);
                }

                var allUsersSavedInManger = usersInManagerToReturn.Select(x => x.UserId).ToList();
                if (allUsersSavedInManger.Contains(user.UserId) && !user.Selected)
                {
                    var elementToErrase = usersInManagerToReturn.Find(x => x.UserId == user.UserId);
                    usersInManagerToReturn.Remove(elementToErrase!);
                }
                else if (!allUsersSavedInManger.Contains(user.UserId) && user.Selected)
                {
                    usersInManagerToReturn.Add(user);
                }
            }
        }

        private async Task ChangeStateCopies(VUserDtoResponse user)
        {
            selectAllCopies = false;

            if (user.Selected && user.Copy)
            {
                user.Selected = false;
                await ChangeStateManager(user);
            }

            var allUsersSavedInCopies = usersInCopiesToReturn.Select(x => x.UserId).ToList();
            if (allUsersSavedInCopies.Contains(user.UserId) && !user.Copy)
            {
                var elementToErrase = usersInCopiesToReturn.Find(x => x.UserId == user.UserId);
                usersInCopiesToReturn.Remove(elementToErrase!);
            }
            else if (!allUsersSavedInCopies.Contains(user.UserId) && user.Copy)
            {
                usersInCopiesToReturn.Add(user);
            }
        }

        public async Task HandleModalClosed(bool status)
        {
            if (multipleSelection)
            {
                List<object> ListOfUserAndCopiesToReturn = new() {
                    usersInManagerToReturn,
                    usersInCopiesToReturn
                };

                var eventArgs = new MyEventArgs<List<object>>
                {
                    Data = ListOfUserAndCopiesToReturn,
                    ModalStatus = status
                };
                await OnStatusChangedMultipleSelection.InvokeAsync(eventArgs);
            }
            else
            {
                var eventArgs = new MyEventArgs<VUserDtoResponse>
                {
                    Data = userToReturn!,
                    ModalStatus = status
                };
                await OnStatusChanged.InvokeAsync(eventArgs);
            }

            StateHasChanged();
        }

        #endregion OnActions

        private void ChangeAllStateManager()
        {
            usersInManagerToReturn = new();
            usersInCopiesToReturn = new();
            if (selectAllCopies) { selectAllCopies = !selectAllManager; }
            UserList?.ForEach(x => { x.Selected = selectAllManager; x.Copy = selectAllCopies; });
            if (selectAllManager)
            {
                usersInManagerToReturn = UserList ?? new();
            }
        }

        private void ChangeAllStateCopies()
        {
            usersInManagerToReturn = new();
            usersInCopiesToReturn = new();

            if (selectAllManager) { selectAllManager = !selectAllCopies; }

            UserList?.ForEach(x => { x.Copy = selectAllCopies; x.Selected = selectAllManager; });
            if (selectAllCopies)
            {
                usersInCopiesToReturn = UserList ?? new();
            }
        }

        private void reactiveExistingData(List<VUserDtoResponse> usersToReactive)
        {
            var allUsersSavedInCopies = usersInCopiesToReturn.Select(x => x.UserId).ToList();
            var allUsersSavedInManger = usersInManagerToReturn.Select(x => x.UserId).ToList();
            usersToReactive.Where(x => allUsersSavedInManger.Contains(x.UserId)).ToList().ForEach(x => { x.Selected = true; });
            usersToReactive.Where(x => allUsersSavedInCopies.Contains(x.UserId)).ToList().ForEach(x => { x.Copy = true; });
        }

        private void HandlePaginationGrid(List<VUserDtoResponse> newDataList)
        {
            PageLoadService.MostrarSpinnerReadLoad(Js);
            UserList = newDataList;
            reactiveExistingData(UserList);
            PageLoadService.OcultarSpinnerReadLoad(Js);
        }


    }
}