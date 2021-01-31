using System;
using System.Collections.Generic;
using System.Text;

namespace Organize.Shared.Entities
{
  public class BaseEntity : NotifyingObject
  {
    private int _id;
    public int Id
    {
      get { return _id; }
      set
      {
        if (_id == value)
        {
          return;
        }

        _id = value;
        NotifyPropertyChanged();
      }
    }
  }
}
