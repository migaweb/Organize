using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorExample.Entities
{
  public class StateManager
  {
    public string Property { get; set; }
    public int Counter { get; set; }

    public event Action OnChange;

    public void SetProperty(string property)
    {
      Property = property;
      Counter++;
      NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
  }
}
