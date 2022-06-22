using System.ComponentModel.DataAnnotations;

namespace EFTask.Models.ViewModel
{
    public class CreateRoleViewModel
    
    {
        [Required(ErrorMessage ="Provide Role Name")]
        public string RoleName { get; set; }
    }

}
