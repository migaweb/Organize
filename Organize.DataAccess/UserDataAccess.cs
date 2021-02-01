using Organize.Shared.Contracts;
using Organize.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organize.DataAccess
{
  public class UserDataAccess : IUserDataAccess
  {
    private IPersistenceService _persistanceService;

    public UserDataAccess(IPersistenceService persistanceService)
    {
      _persistanceService = persistanceService;
    }

    public async Task<bool> IsUserWithNameAvailableAsync(User user)
    {
      var users = await _persistanceService.GetAsync<User>(u => u.Username == user.Username);
      return users.FirstOrDefault() != null;
    }

    public async Task InsertUserAsync(User user)
    {
      await _persistanceService.InsertAsync(user);
    }

    public async Task<User> AuthenticateAndGetUserAsync(User user)
    {
      var users = await _persistanceService.GetAsync<User>(u => u.Username == user.Username && u.Password == user.Password);

      return users.FirstOrDefault();
    }
  }
}
