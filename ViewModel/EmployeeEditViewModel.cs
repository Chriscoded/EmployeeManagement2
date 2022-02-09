
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2.ViewModel
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public int Id { get; set; }
        public string encryptedId { get; set; }
        public string? ExistingPhotoPath { get; set; }
       

    }
}
