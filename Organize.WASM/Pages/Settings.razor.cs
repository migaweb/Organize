using Microsoft.AspNetCore.Components;
using Organize.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Pages
{
  public partial class Settings : ComponentBase
  {
    [Inject]
    private IUserItemManager UserItemManager { get; set; }

    [Inject]
    private ICurrentUserService CurrentUserService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private async void DeleteAllDone()
    {
      await UserItemManager.DeleteAllDoneAsync(CurrentUserService.CurrentUser);
      NavigationManager.NavigateTo("/items");
    }
  }
}
