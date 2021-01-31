using GeneralUi.DropdownControl;
using Microsoft.AspNetCore.Components;
using Organize.Shared.Contracts;
using Organize.Shared.Enums;
using Organize.WASM.ItemEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Pages
{
  public partial class ItemsOverview : ComponentBase
  {
    [Inject]
    private IUserItemManager UserItemManager { get; set; }

    [Inject]
    private ICurrentUserService CurrentUserService { get; set; }
    //[Inject]
    //public ItemEditService ItemEditService { get; set; }

    [Parameter]
    public string TypeString { get; set; }

    [Parameter]
    public int? Id { get; set; }

    private DropdownItem<ItemTypeEnum> SelectedDropdownType { get; set; }

    private IList<DropdownItem<ItemTypeEnum>> DropdownTypes { get; set; }

    private bool ShowEdit { get; set; }

    private async Task AddNewAsync()
    {
      if (SelectedDropdownType == null) return;

      await UserItemManager.CreateNewUserItemAndAddItToUserAsync(
        CurrentUserService.CurrentUser, SelectedDropdownType.ItemObject);
    }

    protected override void OnInitialized()
    {
      base.OnInitialized();
      //ItemEditService.EditeItemChanged += HandleEditItemChanged;

      DropdownTypes = new List<DropdownItem<ItemTypeEnum>>();

      var item = new DropdownItem<ItemTypeEnum>();
      item.ItemObject = ItemTypeEnum.Text;
      item.DisplayText = "Text";
      DropdownTypes.Add(item);

      item = new DropdownItem<ItemTypeEnum>();
      item.ItemObject = ItemTypeEnum.Url;
      item.DisplayText = "Url";
      DropdownTypes.Add(item);

      item = new DropdownItem<ItemTypeEnum>();
      item.ItemObject = ItemTypeEnum.Parent;
      item.DisplayText = "Parent";
      DropdownTypes.Add(item);
    }

    protected override void OnParametersSet()
    {
      base.OnParametersSet();

      if (Id != null && Enum.TryParse(typeof(ItemTypeEnum), TypeString, out _))
      {
        ShowEdit = true;
      }
      else
      {
        ShowEdit = false;
      }
    }

    private void HandleEditItemChanged(object sender, ItemEditEventArgs e)
    {
      ShowEdit = e.Item != null;
      StateHasChanged();
    }
  }
}
