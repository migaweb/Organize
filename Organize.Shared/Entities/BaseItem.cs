using Organize.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Organize.Shared.Entities
{
  public class BaseItem : BaseEntity
  {
    private int _parentId;
    private ItemTypeEnum _itemTypeEnum;
    private int _position;
    private bool _isDone;
    private string _title;

    public int ParentId
    {
      get => _parentId;
      set => SetProperty(ref _parentId, value);
    }

    public ItemTypeEnum ItemTypeEnum
    {
      get => _itemTypeEnum;
      set => SetProperty(ref _itemTypeEnum, value);
    }

    public int Position
    {
      get => _position;
      set => SetProperty(ref _position, value);
    }

    public bool IsDone 
    { 
      get => _isDone;
      set => SetProperty(ref _isDone, value);
    }

    public string Title 
    { 
      get => _title;
      set => SetProperty(ref _title, value);
    }
  }
}
