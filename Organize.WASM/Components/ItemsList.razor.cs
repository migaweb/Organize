using Microsoft.AspNetCore.Components;
using Organize.Shared.Contracts;
using Organize.Shared.Entities;
using Organize.WASM.ItemEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Organize.WASM.Components
{
  public partial class ItemsList : ComponentBase, IDisposable
  {
    [Inject]
    private ICurrentUserService CurrentUserService { get; set; }
    //[Inject]
    //private ItemEditService ItemEditService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected ObservableCollection<BaseItem> UserItems { get; set; } = new ObservableCollection<BaseItem>();

    protected override void OnInitialized()
    {
      base.OnInitialized();
      UserItems = CurrentUserService.CurrentUser.UserItems;
      UserItems.CollectionChanged += HandleUserItemsCollectionChanged;
    }

    private void HandleUserItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      StateHasChanged();
    }

    private void OnBackgroundClicked()
    {
      //ItemEditService.EditItem = null;
      NavigationManager.NavigateTo("/items");
    }

    public void Dispose()
    {
      UserItems.CollectionChanged -= HandleUserItemsCollectionChanged;
    }
  }
}
