using EFTask.Utilties;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EFTask.Models.ViewModel
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUSe",controller:"Account")]
        [ValidDomain(AllowedDomain:"gmail.com" ,ErrorMessage ="Email Domain must be  gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Provide your country")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.Password)]   
        public string Password { get; set; }


        [Display(Name = "Confrim Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="password and confrimation password do not match.")]
        public string ConfrimPassword { get; set; }


    }
}
