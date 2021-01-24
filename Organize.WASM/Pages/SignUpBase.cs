using GeneralUi.DropdownControl;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Organize.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Pages
{
  public class SignUpBase : SignBase
  {
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected IList<DropdownItem<GenderTypeEnum>> GenderTypeDropdownItems { get; } = new List<DropdownItem<GenderTypeEnum>>();

    protected DropdownItem<GenderTypeEnum> SelectedGenderTypeDropdownItem { get; set; }

    [Parameter]
    public string Username { get; set; }

    protected override void OnInitialized()
    {
      base.OnInitialized();

      var male = new DropdownItem<GenderTypeEnum> { ItemObject = GenderTypeEnum.Male, DisplayText = "Male" };
      var female = new DropdownItem<GenderTypeEnum> { ItemObject = GenderTypeEnum.Female, DisplayText = "Female" };
      var neutral = new DropdownItem<GenderTypeEnum> { ItemObject = GenderTypeEnum.Neutral, DisplayText = "Other" };

      GenderTypeDropdownItems.Add(male);
      GenderTypeDropdownItems.Add(female);
      GenderTypeDropdownItems.Add(neutral);

      SelectedGenderTypeDropdownItem = female;

      // Using query parameters
      TryGetUsernameFromUri();
      // Using route parameters
      //User.Username = Username;
    }

    private void TryGetUsernameFromUri()
    {
      var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
      StringValues sv;

      if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("username", out sv))
      {
        User.Username = sv;
      }
    }

    protected void OnValidSubmit()
    {
      // TODO: create user

      NavigationManager.NavigateTo("signin");
    }
  }
}
