using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.ItemEdit
{
  public class ItemEditService
  {
    public event EventHandler<ItemEditEventArgs> EditeItemChanged;

    private BaseItem _editItem;

    public BaseItem EditItem
    {
      get { return _editItem; }
      set
      {
        if (_editItem == value)
        {
          return;
        }

        _editItem = value;
        var args = new ItemEditEventArgs(_editItem);
        OnEditItemChanged(args);
      }
    }

    protected virtual void OnEditItemChanged(ItemEditEventArgs e)
    {
      EventHandler<ItemEditEventArgs> handler = EditeItemChanged;
      if (handler != null)
      {
        handler(this, e);
      }
    }
  }
}
