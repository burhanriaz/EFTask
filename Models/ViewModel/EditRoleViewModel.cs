using System.Collections.Generic;

namespace EFTask.Models.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new  List<string>();
        }
        public string Id { get; set; }
        public string RoleName { get; set; }

        public ICollection<string> Users { get; set; }
    }
}
