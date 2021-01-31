﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Organize.Shared.Entities
{
  public class UrlItem : BaseItem
  {
    private string _url;

    public string Url 
    { 
      get => _url;
      set => SetProperty(ref _url, value);
    }
  }
}
