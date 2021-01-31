﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Organize.Business;
using Organize.Shared.Contracts;
using Organize.Shared.Entities;
using Organize.Shared.Enums;
using Organize.WASM.ItemEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Components
{
  public partial class ItemEdit : ComponentBase, IDisposable
  {
    private BaseItem Item { get; set; } = new BaseItem();
    private int TotalNumber { get; set; }

    //[Inject]
    //private ItemEditService ItemEditService { get; set; }
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    [Inject]
    private ICurrentUserService CurrentUserService { get; set; }

    protected override void OnInitialized()
    {
      base.OnInitialized();
      //ItemEditService.EditeItemChanged += HandleEditItemChanged;
      //Item = ItemEditService.EditItem;
      SetDataFromUri();
    }

    private void SetDataFromUri()
    {
      var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
      var segmentCount = uri.Segments.Length;

      if (segmentCount > 2 && 
        Enum.TryParse(typeof(ItemTypeEnum), uri.Segments[segmentCount-2].Trim('/'), out var typeEnum) &&
        int.TryParse(uri.Segments[segmentCount-1], out var id))
      {
        var userItem = CurrentUserService.CurrentUser
                                         .UserItems
                                         .Where(e => e.ItemTypeEnum == (ItemTypeEnum)typeEnum)
                                         .Where(e => e.Id == id)
                                         .SingleOrDefault();

        // Not found? Redirect to items
        if (userItem == null)
        {
          NavigationManager.LocationChanged -= HandleLocationChanged;
          NavigationManager.NavigateTo("/items");
        }
        else
        {
          Item = userItem;
          NavigationManager.LocationChanged += HandleLocationChanged;
          StateHasChanged();
        }
      }
    }

    private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
    {
      SetDataFromUri();
    }

    public void Dispose()
    {
      NavigationManager.LocationChanged -= HandleLocationChanged;
    }

    //private void HandleEditItemChanged(object sender, ItemEditEventArgs e)
    //{
    //  Item = e.Item;
    //  StateHasChanged();
    //}
  }
}
