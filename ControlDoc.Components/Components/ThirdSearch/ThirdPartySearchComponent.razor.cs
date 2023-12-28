using ControlDoc.Components.Components.Input;
using ControlDoc.Models.Models.Administration.Response;
using ControlDoc.Models.Models.Generic;
using ControlDoc.Models.Models.Pagination;
using ControlDoc.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace ControlDoc.Components.Components.ThirdSearch
{
    public partial class ThirdPartySearchComponent
    {
        #region Properties
        [Inject]
        private ICallService CallService { get; set; }
        private int companyId { get; set; } = 0;

        private bool searchByPN { get; set; } = true;
        private bool searchByPJ { get; set; } = false;

        private bool disableButton { get; set; } = true;

        private TelerikSwitch<bool> SwitchRefPN { get; set; } = new();
        private TelerikSwitch<bool> SwitchRefPJ { get; set; } = new();

        private InputModalComponent inputNames { get; set; } = new();

        private InputModalComponent inputEmail { get; set; } = new();

        private InputModalComponent inputIdentificcation { get; set; } = new();

        private List<ThirdPartyDtoResponse> thirdPartyList { get; set; } = new();

        private List<ThirdPartyDtoResponse> thirdPartiesManagerToReturn { get; set; } = new();
        private List<ThirdPartyDtoResponse> thirdPartiesCopiesToReturn { get; set; } = new();

        private List<ThirdUserDtoResponse> thirdUsersManagerToReturn { get; set; } = new();
        private List<ThirdUserDtoResponse> thirdUsersCopiesToReturn { get; set; } = new();

        private ThirdPartyDtoResponse thirdPartyToReturn { get; set; } = new();
        private ThirdUserDtoResponse thirdUserToReturn { get; set; } = new();
        private Meta meta = new() { pageSize = 10 };

        #endregion Properties

        #region Parameters

        [Parameter] public bool multipleSelection { get; set; } = true;
        [Parameter] public bool showCopiesColumn { get; set; } = true;

        [Parameter] public EventCallback<MyEventArgs<ThirdPartyDtoResponse>> OnStatusChanged { get; set; }

        [Parameter] public EventCallback<MyEventArgs<ThirdUserDtoResponse>> OnStatusThirdUserChanged { get; set; }

        [Parameter] public EventCallback<MyEventArgs<List<object>>> OnStatusChangedMultipleSelection { get; set; }

        #endregion Parameters

        #region Event Handlers

        #region Switch Events

        private void OnChangeSwitchPJ()
        {
            searchByPJ = !searchByPJ;
            searchByPN = !searchByPJ;
            thirdPartyList = new();
            EnableButton();
        }

        private void OnChangeSwitchPN()
        {
            searchByPN = !searchByPN;
            searchByPJ = !searchByPN;
            thirdPartyList = new();
            EnableButton();
        }

        private void EnableButton()
        {
            if (searchByPJ != searchByPN)
            {
                disableButton = false;
            }
        }

        #endregion Switch Events

        #region Button Click Event

        private async Task OnClickButton()
        {
            try
            {
                string typePersonToSearch = "";
                if (searchByPJ && !searchByPN) { typePersonToSearch = "PJ"; }
                else if (!searchByPJ && searchByPN) { typePersonToSearch = "PN"; }

                Dictionary<string, dynamic> headers = new()
                {
                    { "companyId", companyId},
                    { "personType",typePersonToSearch??""},
                    { "names",inputNames.InputValue??""},
                    { "email",inputEmail.InputValue??""},
                    {"identificationNumber",inputIdentificcation.InputValue??"" }
                };

                var response = await CallService.Get<List<ThirdPartyDtoResponse>>($"administration/ThirdParty/ByFilterWithUsers", headers);

                thirdPartyList = response.Data ?? new();

                ReactiveExistingData(thirdPartyList);
            }
            catch { thirdPartyList = new(); }
        }

        #endregion Button Click Event

        #region Modal Event

        protected override async Task OnInitializedAsync()
        {
            EnableButton();

            StateHasChanged();
        }

        public async Task HandleModalClosed(bool status)
        {
            if (!multipleSelection && thirdPartyToReturn != null)
            {
                var eventArgs = new MyEventArgs<ThirdPartyDtoResponse>
                {
                    Data = thirdPartyToReturn,
                    ModalStatus = status
                };
                await OnStatusChanged.InvokeAsync(eventArgs);
            }
            else if (!multipleSelection && thirdUserToReturn != null)
            {
                var eventArgs = new MyEventArgs<ThirdUserDtoResponse>
                {
                    Data = thirdUserToReturn,
                    ModalStatus = status
                };
                await OnStatusThirdUserChanged.InvokeAsync(eventArgs);
            }
            else
            {
                List<object> ListOfThirdPartiesAndCopiesToReturn = new() {
                    thirdPartiesManagerToReturn,
                    thirdPartiesCopiesToReturn,
                    thirdUsersManagerToReturn,
                    thirdUsersCopiesToReturn
                };

                var eventArgs = new MyEventArgs<List<object>>
                {
                    Data = ListOfThirdPartiesAndCopiesToReturn,
                    ModalStatus = status
                };
                await OnStatusChangedMultipleSelection.InvokeAsync(eventArgs);
            }
        }

        #endregion Modal Event

        #region Selection Events

        private void SelectThirdParty(ThirdPartyDtoResponse thirdParty)
        {
            thirdPartyList.Where(x => x.ThirdPartyId != thirdParty.ThirdPartyId).ToList().ForEach(x => { x.Selected = false; });
            thirdPartyToReturn = thirdParty;
        }

        private void SelectThirdUser(ThirdUserDtoResponse thirdUser)
        {
            thirdPartyList?
                .Where(x => x.ThirdPartyId == thirdUser.ThirdPartyId)
                .FirstOrDefault()?
                .ThirdUsers?
                .Where(y => y.ThirdUserId != thirdUser.ThirdUserId)
                .ToList()
                .ForEach(x => x.Selected = false);

            thirdUserToReturn = thirdUser;
        }

        public async Task changeStateThirdParty(ThirdPartyDtoResponse thirdParty)
        {
            if (!multipleSelection)
            {
                SelectThirdParty(thirdParty);
                await HandleModalClosed(false);
            }
            else
            {
                if (thirdParty.Selected && thirdParty.Copy)
                {
                    thirdParty.Copy = false;
                    await ChangeStateThirdPartyCopies(thirdParty);
                }

                var allUsersSavedInManger = thirdPartiesManagerToReturn.Select(x => x.ThirdPartyId).ToList();
                if (allUsersSavedInManger.Contains(thirdParty.ThirdPartyId) && !thirdParty.Selected)
                {
                    var elementToErrase = thirdPartiesManagerToReturn.Find(x => x.ThirdPartyId == thirdParty.ThirdPartyId);
                    thirdPartiesManagerToReturn.Remove(elementToErrase!);
                }
                else if (!allUsersSavedInManger.Contains(thirdParty.ThirdPartyId) && thirdParty.Selected)
                {
                    thirdPartiesManagerToReturn.Add(thirdParty);
                }
            }
        }

        public async Task ChangeStateThirdPartyCopies(ThirdPartyDtoResponse thirdParty)
        {
            if (thirdParty.Selected && thirdParty.Copy)
            {
                thirdParty.Selected = false;
                await changeStateThirdParty(thirdParty);
            }

            var allUsersSavedInCopies = thirdPartiesCopiesToReturn.Select(x => x.ThirdPartyId).ToList();
            if (allUsersSavedInCopies.Contains(thirdParty.ThirdPartyId) && !thirdParty.Copy)
            {
                var elementToErrase = thirdPartiesCopiesToReturn.Find(x => x.ThirdPartyId == thirdParty.ThirdPartyId);
                thirdPartiesCopiesToReturn.Remove(elementToErrase!);
            }
            else if (!allUsersSavedInCopies.Contains(thirdParty.ThirdPartyId) && thirdParty.Copy)
            {
                thirdPartiesCopiesToReturn.Add(thirdParty);
            }
        }

        public async Task changeStateThirdUser(ThirdUserDtoResponse thirdUser)
        {
            if (!multipleSelection)
            {
                SelectThirdUser(thirdUser);
                await HandleModalClosed(false);
            }
            else
            {
                if (thirdUser.Selected && thirdUser.Copy)
                {
                    thirdUser.Copy = false;
                    await ChangeStateThirdUserCopies(thirdUser);
                }

                var allThirdUsersSavedInManger = thirdUsersManagerToReturn.Select(x => x.ThirdUserId).ToList();
                if (allThirdUsersSavedInManger.Contains(thirdUser.ThirdUserId) && !thirdUser.Selected)
                {
                    var elementToErrase = thirdUsersManagerToReturn.Find(x => x.ThirdUserId == thirdUser.ThirdUserId);
                    thirdUsersManagerToReturn.Remove(elementToErrase!);
                }
                else if (!allThirdUsersSavedInManger.Contains(thirdUser.ThirdUserId) && thirdUser.Selected)
                {
                    thirdUsersManagerToReturn.Add(thirdUser);
                }

                EnableCheckBox(thirdUser.ThirdPartyId, 1);
            }
        }

        #endregion Selection Events

        #region Checkbox Events

        private void EnableCheckBox(int thirdPartyId, int SelectionOrCopies)
        {
            var thirdparty = thirdPartyList.Find(x => x.ThirdPartyId == thirdPartyId);

            List<bool> selectionBools;
            if (SelectionOrCopies == 1)
            {
                selectionBools = thirdparty.ThirdUsers.Select(x => x.Selected).Distinct().ToList();

                if (selectionBools.Count() != 1)
                {
                    thirdPartyList.Find(x => x.ThirdPartyId == thirdPartyId).EnableSelection = false;
                }
                else if (selectionBools.Count() == 1 && !selectionBools[0])
                {
                    thirdPartyList.Find(x => x.ThirdPartyId == thirdPartyId).EnableSelection = true;
                }
            }
            else if (SelectionOrCopies == 2)
            {
                selectionBools = thirdparty.ThirdUsers.Select(x => x.Copy).Distinct().ToList();

                if (selectionBools.Count() != 1)
                {
                    thirdPartyList.Find(x => x.ThirdPartyId == thirdPartyId).EnableCopy = false;
                }
                else if (selectionBools.Count() == 1 && !selectionBools[0])
                {
                    thirdPartyList.Find(x => x.ThirdPartyId == thirdPartyId).EnableCopy = true;
                }
            }
        }

        #endregion Checkbox Events

        #region Copies Events

        public async Task ChangeStateThirdUserCopies(ThirdUserDtoResponse thirdUser)
        {
            if (thirdUser.Selected && thirdUser.Copy)
            {
                thirdUser.Selected = false;
                await changeStateThirdUser(thirdUser);
            }

            var allUsersSavedInCopies = thirdUsersCopiesToReturn.Select(x => x.ThirdUserId).ToList();
            if (allUsersSavedInCopies.Contains(thirdUser.ThirdUserId) && !thirdUser.Copy)
            {
                var elementToErrase = thirdUsersCopiesToReturn.Find(x => x.ThirdUserId == thirdUser.ThirdUserId);
                thirdUsersCopiesToReturn.Remove(elementToErrase!);
            }
            else if (!allUsersSavedInCopies.Contains(thirdUser.ThirdUserId) && thirdUser.Copy)
            {
                thirdUsersCopiesToReturn.Add(thirdUser);
            }

            EnableCheckBox(thirdUser.ThirdPartyId, 2);
        }

        #endregion Copies Events

        #region Reactive Data

        private void ReactiveExistingData(List<ThirdPartyDtoResponse> thirdPartyToReactive)
        {
            var allThirdPartiesSavedInCopies = new HashSet<int>(thirdPartiesCopiesToReturn.Select(x => x.ThirdPartyId));
            var allThirdPartiesSavedInManager = new HashSet<int>(thirdPartiesManagerToReturn.Select(x => x.ThirdPartyId));
            var allThirdUsersSaveInManager = new HashSet<int>(thirdUsersManagerToReturn.Select(x => x.ThirdUserId));
            var allThirdUsersSaveInCopies = new HashSet<int>(thirdUsersCopiesToReturn.Select(x => x.ThirdUserId));

            thirdPartyToReactive
                .Where(x => allThirdPartiesSavedInManager.Contains(x.ThirdPartyId))
                .ToList()
                .ForEach(x => x.Selected = true);

            thirdPartyToReactive
                .Where(x => allThirdPartiesSavedInCopies.Contains(x.ThirdPartyId))
                .ToList()
                .ForEach(x => x.Copy = true);

            // Reactivar ThirdUsers en Manager y Copies
            foreach (var item in thirdPartyToReactive)
            {
                item.ThirdUsers
                    .Where(x => allThirdUsersSaveInManager.Contains(x.ThirdUserId))
                    .ToList()
                    .ForEach(x => x.Selected = true);

                item.ThirdUsers
                    .Where(x => allThirdUsersSaveInCopies.Contains(x.ThirdUserId))
                    .ToList()
                    .ForEach(x => x.Copy = true);
            }
        }

        #endregion Reactive Data

        #endregion Event Handlers
    }
}