using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorExample.Entities
{
  public static class Extensions
  {
    public static ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message)
    {
      return jsRuntime.InvokeAsync<bool>("confirm", message);
    }
  }
}
