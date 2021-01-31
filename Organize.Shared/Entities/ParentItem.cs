using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Organize.Shared.Entities
{
  public class ParentItem : BaseItem
  {
    private ObservableCollection<ChildItem> _childItems;

    public ObservableCollection<ChildItem> ChildItems 
    { 
      get => _childItems;
      set => SetProperty(ref _childItems, value);
    }
  }
}
