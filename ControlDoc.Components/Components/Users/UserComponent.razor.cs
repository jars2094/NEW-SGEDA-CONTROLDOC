using Microsoft.AspNetCore.Components;

namespace ControlDoc.Components.Components.Users
{
    public partial class UserComponent
    {
        #region Parameters
        [Parameter] public string? UserName { get; set; }
        [Parameter] public string? UserImage { get; set; }
        #endregion
    }
}
