using Microsoft.AspNetCore.Components.Authorization;
using Organize.Shared.Contracts;
using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Organize.WASM.OrganizeAuthenticationStateProvider
{
  public class SimpleAuthenticationStateProvider : AuthenticationStateProvider, IAuthenticationStateProvider
  {
    private readonly ICurrentUserService _currentUserService;

    public SimpleAuthenticationStateProvider(ICurrentUserService currentUserService)
    {
      _currentUserService = currentUserService;
    }
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      if (_currentUserService.CurrentUser == null)
      {
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
      }

      var user = _currentUserService.CurrentUser;
      var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { 
        new Claim("id", user.Id.ToString()),
        new Claim(ClaimTypes.Role, "admin")
      }, "apiAuth"));
      return Task.FromResult(new AuthenticationState(authenticatedUser));

    }
    public void SetAuthenticationState(User user)
    {
      var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { 
        new Claim("id", user.Id.ToString()), new Claim(ClaimTypes.Role, "admin") }, "apiAuth"));
      var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
      NotifyAuthenticationStateChanged(authState);
    }

    public void UnsetUser()
    {
      var unsetUser = new ClaimsPrincipal(new ClaimsIdentity());
      var authState = Task.FromResult(new AuthenticationState(unsetUser));
      NotifyAuthenticationStateChanged(authState);
    }
  }
}
