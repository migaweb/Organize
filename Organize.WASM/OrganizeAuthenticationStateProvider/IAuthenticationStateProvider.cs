﻿using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize.WASM.OrganizeAuthenticationStateProvider
{
  public interface IAuthenticationStateProvider
  {
    void SetAuthenticationState(User user);
    void UnsetUser();
  }
}
