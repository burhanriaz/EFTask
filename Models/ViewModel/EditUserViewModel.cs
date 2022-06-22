using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFTask.Models.ViewModel
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();

        }
        public string Id { get; set; }
        [Required(ErrorMessage ="Provide User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Provide Email Address")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Provide Country Name")]

        public string Country { get; set; }

        public List<string> Roles { get; set; }
        public List<string> Claims { get; set; }


    }
}
