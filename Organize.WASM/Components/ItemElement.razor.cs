﻿using Microsoft.AspNetCore.Components;
using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.Components
{
  public partial class ItemElement<TItem> : ComponentBase where TItem : BaseItem
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

    protected override void OnParametersSet()
    {
      base.OnParametersSet();
      DetailAreaId = "detailArea" + Item.Position;
    }
  }
}
