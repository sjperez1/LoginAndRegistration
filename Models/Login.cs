#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // need this when doing NotMapped
namespace LoginAndRegistration.Models;

[NotMapped]
public class Login
{
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    [EmailAddress]
    public string LoginEmail {get;set;}

    [Required(ErrorMessage = "Password is required")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string LoginPassword {get;set;}
}