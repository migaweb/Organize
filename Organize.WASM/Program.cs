using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Organize.Business;
using Organize.DataAccess;
using Organize.Shared.Contracts;
using Organize.TestFake;
using Organize.WASM.ItemEdit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Organize.WASM
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");

      builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

      //builder.Services.AddSingleton<IUserManager, UserManager>();
      builder.Services.AddScoped<IUserManager, UserManagerFake>();
      builder.Services.AddScoped<IUserItemManager, UserItemManager>();
      builder.Services.AddScoped<IItemDataAccess, ItemDataAccess>();
      builder.Services.AddScoped<IPersistenceService, InMemoryStorage.InMemoryStorage>();
      builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
      builder.Services.AddScoped<ItemEditService>();

      var host = builder.Build();

      var persistenceService = host.Services.GetRequiredService<IPersistenceService>();
      await persistenceService.InitAsync();

      var currentUserService = host.Services.GetRequiredService<ICurrentUserService>();
      var userItemManager = host.Services.GetRequiredService<IUserItemManager>();

      if (persistenceService is InMemoryStorage.InMemoryStorage)
      {
        Console.WriteLine("Ja");
        TestData.CreateTestUser(userItemManager);
        currentUserService.CurrentUser = TestData.TestUser;
        Console.WriteLine("Test User Items: " + TestData.TestUser.UserItems.Count);
        Console.WriteLine("Items: " + currentUserService.CurrentUser.UserItems.Count);

      }

      await host.RunAsync();
    }
  }
}
