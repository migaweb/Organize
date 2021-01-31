using Microsoft.AspNetCore.Components;
using Organize.Shared.Entities;
using Organize.WASM.ItemEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Components
{
  public partial class ItemElement<TItem> : ComponentBase, IDisposable where TItem : BaseItem
  {
    [CascadingParameter]
    public int TotalNumber { get; set; }
    [CascadingParameter]
    public string ColorPrefix { get; set; }
    [Parameter]
    public RenderFragment MainFragment { get; set; }
    [Parameter]
    public RenderFragment DetailFragment { get; set; }
    [Parameter]
    public TItem Item { get; set; }
    public string DetailAreaId { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    //[Inject]
    //public ItemEditService ItemEditService { get; set; }

    private void OpenItemInEditMode()
    {
      //ItemEditService.EditItem = Item;
      Uri.TryCreate("/items/" + Item.ItemTypeEnum + "/" + Item.Id, UriKind.Relative, out var uri);
      NavigationManager.NavigateTo(uri.ToString());
    }

    protected override void OnAfterRender(bool firstRender)
    {
      base.OnAfterRender(firstRender);
      if (firstRender)
      {
        Item.PropertyChanged += HandleItemPropertyChanged;
      }
    }

    private void HandleItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      StateHasChanged();
    }

    protected override void OnParametersSet()
    {
      base.OnParametersSet();
      DetailAreaId = "detailArea" + Item.Position;
    }

    public void Dispose()
    {
      Item.PropertyChanged -= HandleItemPropertyChanged;
    }
  }
}
