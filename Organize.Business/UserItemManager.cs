using Organize.Shared.Contracts;
using Organize.Shared.Entities;
using Organize.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Organize.Business
{
  public class UserItemManager : IUserItemManager
  {
    public async Task<ChildItem> CreateNewChildItemAndAddItParentItemAsync(ParentItem parent)
    {
      var childtItem = new ChildItem();
      childtItem.ParentId = parent.Id;
      childtItem.ItemTypeEnum = Shared.Enums.ItemTypeEnum.Child;

      parent.ChildItems.Add(childtItem);
      return await Task.FromResult(childtItem);
    }

    public async Task<BaseItem> CreateNewUserItemAndAddItToUserAsync(User user, ItemTypeEnum typeEnum)
    {
      BaseItem item = null;

      switch (typeEnum)
      {
        case ItemTypeEnum.Text:
          item = await CreateAndInsertItemAsync<TextItem>(user, typeEnum);
          break;
        case ItemTypeEnum.Url:
          item = await CreateAndInsertItemAsync<UrlItem>(user, typeEnum);
          break;
        case ItemTypeEnum.Parent:
          var parent = await CreateAndInsertItemAsync<ParentItem>(user, typeEnum);
          parent.ChildItems = new ObservableCollection<ChildItem>();
          break;
      }

      user.UserItems.Add(item);
      return item;
    }

    private async Task<T> CreateAndInsertItemAsync<T>(User user, ItemTypeEnum typeEnum) where T : BaseItem, new()
    {
      var item = new T();
      item.ItemTypeEnum = typeEnum;
      item.Position = user.UserItems.Count + 1;
      item.ParentId = user.Id;
      item.Id = user.UserItems.Count + 1;
      // TODO: Add to storage provider

      return await Task.FromResult(item);
    }
  }
}
