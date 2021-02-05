using Blazored.Modal;
using Blazored.Modal.Services;
using GeneralUi.BusyOverlay;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Organize.Business;
using Organize.Shared.Contracts;
using Organize.WASM.Components;
using Organize.WASM.OrganizeAuthenticationStateProvider;
using System;
using System.Threading.Tasks;

namespace Organize.WASM.Pages
{
  public class SigninBase : SignBase
  {
    protected string Day { get; } = DateTime.Now.DayOfWeek.ToString();

    [Inject]
    private ICurrentUserService CurrentUserService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IUserManager UserManager { get; set; }

    [Inject]
    private IAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    public BusyOverlayService BusyOverlayService { get; set; }

    [Inject]
    public IModalService ModalService { get; set; }

    public bool ShowPassword { get; set; }

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

      try
      {
        BusyOverlayService.SetBusyState(BusyEnum.Busy);
        var foundUser = await UserManager.TrySignInAndGetUserAsync(User);

        if (foundUser != null)
        {
          AuthenticationStateProvider.SetAuthenticationState(foundUser);
          CurrentUserService.CurrentUser = foundUser;
          NavigationManager.NavigateTo("items");
        }
        else
        {
          var parameters = new ModalParameters();
          parameters.Add(nameof(ModalMessage.Message), "User not found!");
          ModalService.Show<ModalMessage>("Error", parameters);
        }
      }
      catch (Exception ex)
      {
        var parameters = new ModalParameters();
        parameters.Add(nameof(ModalMessage.Message), ex.Message);
        ModalService.Show<ModalMessage>("Error", parameters);
      }
      finally
      {
        BusyOverlayService.SetBusyState(BusyEnum.NotBusy);
        
      }
    }
  }
}
