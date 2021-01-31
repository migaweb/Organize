using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Organize.Shared.Contracts;
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

    public bool UseShortNavText { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      var width = await JSRuntime.InvokeAsync<int>("blazorDimension.getWidth");
      CheckUsesShortNavText(width);

      _dotNetReference = DotNetObjectReference.Create(this);
      await JSRuntime.InvokeVoidAsync("blazorResize.registerReferenceForResizeEvent", _dotNetReference);
    }

    protected void SignOut()
    {

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

    public void Dispose()
    {
      // .NET references needs to be disposed. Not so important just here.
      _dotNetReference?.Dispose();
    }
  }
}
