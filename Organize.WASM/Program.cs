using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using GeneralUi.BusyOverlay;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Organize.Business;
using Organize.DataAccess;
using Organize.Shared.Contracts;
using Organize.TestFake;
using Organize.WASM.ItemEdit;
using Organize.WASM.OrganizeAuthenticationStateProvider;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Organize.WASM
{
  public class Program
  {
    private static bool _isApiPerstance = false;

    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");

      builder.Services.AddOptions();
      builder.Services.AddAuthorizationCore();

      builder.Services.AddBlazoredLocalStorage();
      builder.Services.AddBlazoredModal();
      builder.Services.AddBlazoredToast();
      builder.Services.AddScoped<BusyOverlayService>();

      if (_isApiPerstance)
      {
        builder.Services.AddScoped(sp => 
        new HttpClient { BaseAddress = new Uri(builder.Configuration["apiAddress"]) });

        builder.Services.AddScoped<IPersistenceService, WebAPIAccess.WebAPIAccess>();
        builder.Services.AddScoped<IUserDataAccess, WebAPIAccess.WebAPIUserDataAccess>();
        builder.Services.AddScoped<WebAPIAuthenticationStateProvider>();
        builder.Services.AddScoped<IAuthenticationStateProvider>(
          provider => provider.GetRequiredService<WebAPIAuthenticationStateProvider>());
        // For the cascading state provider
        builder.Services.AddScoped<AuthenticationStateProvider>(
          provider => provider.GetRequiredService<WebAPIAuthenticationStateProvider>());
      }
      else
      {
        builder.Services.AddScoped<IPersistenceService, InMemoryStorage.InMemoryStorage>();
        //builder.Services.AddScoped<IPersistenceService, IndexedDB.IndexedDB>();
        builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();

        builder.Services.AddScoped<SimpleAuthenticationStateProvider>();
        builder.Services.AddScoped<IAuthenticationStateProvider>(
          provider => provider.GetRequiredService<SimpleAuthenticationStateProvider>());
        // For the cascading state provider
        builder.Services.AddScoped<AuthenticationStateProvider>(
          provider => provider.GetRequiredService<SimpleAuthenticationStateProvider>());
      }


      builder.Services.AddScoped<IUserManager, UserManager>();
      //builder.Services.AddScoped<IUserManager, UserManagerFake>();
      builder.Services.AddScoped<IUserItemManager, UserItemManager>();
      builder.Services.AddScoped<IItemDataAccess, ItemDataAccess>();
      
      builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
      
      builder.Services.AddScoped<ItemEditService>();

      var host = builder.Build();

      var persistenceService = host.Services.GetRequiredService<IPersistenceService>();
      await persistenceService.InitAsync();

      var currentUserService = host.Services.GetRequiredService<ICurrentUserService>();
      var userItemManager = host.Services.GetRequiredService<IUserItemManager>();
      var userManager = host.Services.GetRequiredService<IUserManager>();

      if (persistenceService is InMemoryStorage.InMemoryStorage)
      {
        Console.WriteLine("Ja");
        TestData.CreateTestUser(userItemManager, userManager);
        currentUserService.CurrentUser = TestData.TestUser;
        Console.WriteLine("Test User Items: " + TestData.TestUser.UserItems.Count);
        Console.WriteLine("Items: " + currentUserService.CurrentUser.UserItems.Count);

      }

      await host.RunAsync();
    }
  }
}
