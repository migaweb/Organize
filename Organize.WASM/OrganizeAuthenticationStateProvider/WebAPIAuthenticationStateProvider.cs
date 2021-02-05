using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Organize.Shared.Contracts;
using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Organize.WASM.OrganizeAuthenticationStateProvider
{
  public class WebAPIAuthenticationStateProvider : AuthenticationStateProvider, IAuthenticationStateProvider
  {
    private readonly ICurrentUserService _currentUserService;
    private readonly HttpClient _httpClient;
    private readonly WebAPIAccess.WebAPIAccess _webAPIAccess;
    private readonly ILocalStorageService _localStorageService;

    public WebAPIAuthenticationStateProvider(
      ICurrentUserService currentUserService, HttpClient httpClient, 
      IPersistenceService persistenceService, ILocalStorageService localStorageService)
    {
      _currentUserService = currentUserService;
      _httpClient = httpClient;
      _webAPIAccess = (WebAPIAccess.WebAPIAccess)persistenceService;
      _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var savedToken = await _localStorageService.GetItemAsync<string>("authToken");
      
      if (string.IsNullOrEmpty(savedToken))
      {
        // Return empty identity
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
      }

      try
      {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
        var user = await _webAPIAccess.GetUserByTokenAsync();
        user.Token = savedToken;
        return await CreateAuthenticationState(user);
      } 
      catch
      {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
      }
    }

    public void SetAuthenticationState(User user)
    {
      var authStateTask = CreateAuthenticationState(user);
      NotifyAuthenticationStateChanged(authStateTask);
    }

    private async Task<AuthenticationState> CreateAuthenticationState(User user)
    {
      await _localStorageService.SetItemAsync("authToken", user.Token);
      _currentUserService.CurrentUser = user;
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", user.Token);

      var handler = new JwtSecurityTokenHandler();
      var token = handler.ReadJwtToken(user.Token);

      var claims = token.Claims.ToList();
      var roleClaim = claims.FirstOrDefault(e => e.Type == "role");
      if (roleClaim != null)
      {
        var newRoleClaim = new Claim(ClaimTypes.Role, roleClaim.Value);
        claims.Add(newRoleClaim);
      }

      //var claimsPrincipal = new ClaimsPrincipal(
      //  new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }, "apiAuth"));

      var claimsPrincipal = new ClaimsPrincipal(
        new ClaimsIdentity(claims, "apiAuth"));

      Console.WriteLine(JsonSerializer.Serialize(claims));

      return new AuthenticationState(claimsPrincipal);
    }

    public void UnsetUser()
    {
      NotifyAuthenticationStateChanged(CreateUnsetUserAuthenticationStateAsync());
    }

    private async Task<AuthenticationState> CreateUnsetUserAuthenticationStateAsync()
    {
      await _localStorageService.RemoveItemAsync("authToken");
      var unsetUser = new ClaimsPrincipal(new ClaimsIdentity());
      return new AuthenticationState(unsetUser);
    }
  }
}
