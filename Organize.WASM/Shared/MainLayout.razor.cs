using GeneralUi.BusyOverlay;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Organize.Shared.Contracts;
using Organize.WASM.OrganizeAuthenticationStateProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Shared
{
  public partial class MainLayout: LayoutComponentBase, IDisposable
  {
    private DotNetObjectReference<MainLayout> _dotNetReference;

    [Inject]
    private ICurrentUserService CurrentUserService { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    [Inject]
    private IUserItemManager UserItemManager { get; set; }

    [Inject]
    private IAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    public BusyOverlayService BusyOverlayService { get; set; }

    private bool IsAuthenticated { get; set; } = false;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    public bool UseShortNavText { get; set; }

    protected override async Task OnParametersSetAsync()
    {
      
      await base.OnParametersSetAsync();
      var authState = await AuthenticationStateTask;
      IsAuthenticated = authState.User.Identity.IsAuthenticated;

      if (!IsAuthenticated || CurrentUserService.CurrentUser.IsUserItemsPropertyLoaded)
      {
        return;
      }

      try
      {
        BusyOverlayService.SetBusyState(BusyEnum.Busy);
        await UserItemManager.RetrieveAllUserItemsOfUserAndSetToUserAsync(CurrentUserService.CurrentUser);
      }
      finally
      {
        BusyOverlayService.SetBusyState(BusyEnum.NotBusy);
      }
    }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      var width = await JSRuntime.InvokeAsync<int>("blazorDimension.getWidth");
      CheckUsesShortNavText(width);

      _dotNetReference = DotNetObjectReference.Create(this);
      await JSRuntime.InvokeVoidAsync("blazorResize.registerReferenceForResizeEvent", nameof(MainLayout), _dotNetReference);
    }

    protected void SignOut()
    {
      AuthenticationStateProvider.UnsetUser();
    }

    [JSInvokable]
    public void HandleResize(int width, int height)
    {
      CheckUsesShortNavText(width);
    }

    [JSInvokable]
    public static void OnResize()
    {

    }

    private void CheckUsesShortNavText(int width)
    {
      var oldvalue = UseShortNavText;
      UseShortNavText = width < 700;
      if (oldvalue != UseShortNavText)
      {
        StateHasChanged();
      }
    }

    public async void Dispose()
    {
      // .NET references needs to be disposed. Not so important just here.
      _dotNetReference?.Dispose();
      await JSRuntime.InvokeVoidAsync("blazorResize.unRegister", nameof(MainLayout));
    }
  }
}
