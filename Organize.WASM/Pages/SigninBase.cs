using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Organize.Business;
using Organize.Shared.Contracts;
using System;
using System.Threading.Tasks;

namespace Organize.WASM.Pages
{
  public class SigninBase : SignBase
  {
    protected string Day { get; } = DateTime.Now.DayOfWeek.ToString();

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IUserManager UserManager { get; set; }

    protected override void OnInitialized()
    {
      base.OnInitialized();
      User = new Organize.Shared.Entities.User() { 
        FirstName = "c",
        LastName = "c",
        PhoneNumber = "123"
      };

      EditContext = new EditContext(User);
    }

    protected async void OnSubmit()
    {
      if (!EditContext.Validate())
      {
        return;
      }

      var foundUser = await UserManager.TrySignInAndGetUserAsync(User);

      if (foundUser != null)
      {
        NavigationManager.NavigateTo("items");
      }
    }
  }
}
