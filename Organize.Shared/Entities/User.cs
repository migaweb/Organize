using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Organize.Shared.Entities
{
  public class User
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
  }
}
