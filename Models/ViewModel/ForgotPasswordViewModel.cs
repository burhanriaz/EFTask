using System.ComponentModel.DataAnnotations;

namespace EFTask.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required (ErrorMessage ="Please provide a Email")]
        [EmailAddress (ErrorMessage ="Please provide valid Email")]
        public string Email { get; set; }
    }
}
