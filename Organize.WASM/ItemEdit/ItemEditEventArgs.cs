using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.ItemEdit
{
  public class ItemEditEventArgs : EventArgs
  {
    public ItemEditEventArgs()
    {

    }
    public ItemEditEventArgs(BaseItem item)
    {
      Item = item;
    }
    public BaseItem Item { get; set; }
  }
}
