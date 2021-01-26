﻿using Organize.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Organize.Shared.Entities
{
  public class User : BaseEntity
  {
    [Required]
    [StringLength(10, ErrorMessage = "User name is too long")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    public GenderTypeEnum GenderType { get; set; }

    public ObservableCollection<BaseItem> UserItems { get; set; }

    public override string ToString()
    {
      var salutation = string.Empty;

      if (GenderType == GenderTypeEnum.Male)
        salutation = "Mr";

      if (GenderType == GenderTypeEnum.Female)
        salutation = "Mrs";

      return $"{salutation} {FirstName} {LastName}";
    }
  }
}
