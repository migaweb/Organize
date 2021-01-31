using Microsoft.AspNetCore.Components;
using Organize.Shared.Contracts;
using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Components
{
  public partial class ChildItemEdit : ComponentBase
  {
    [Inject]
    private IUserItemManager UserItemManager { get; set; }

    [Parameter]
    public ParentItem ParentItem { get; set; }

    private async Task AddNewchildToParentAsync()
    {
      await UserItemManager.CreateNewChildItemAndAddItParentItemAsync(ParentItem);
    }
  }
}
