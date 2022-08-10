#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // need this when doing NotMapped
namespace LoginAndRegistration.Models;
public class User
{
    [Key]
    public int UserId {get;set;}

    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters long")]
    [Display(Name = "First Name")]
    public string FirstName {get;set;}

    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long")]
    [Display(Name = "Last Name")]
    public string LastName {get;set;}

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email {get;set;}

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [DataType(DataType.Password)]
    public string Password {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;

    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    // Need the following for confirm password
    [NotMapped] // use NotMapped so that it will not be put into your database
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword {get;set;}
}