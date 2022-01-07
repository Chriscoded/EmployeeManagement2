//this model is serving like artificial model just for processing amd at the end it will be transfered to Employee model
// that is the model serve as middle man the arm is to ensure that our images are properly processed
using EmployeeManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModel
{
    public class EmployeeCreateViewModel
    {
        
        [Required]
        [MaxLength(50, ErrorMessage = "Name can not exceeed 10 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Invalid Email")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }

        //[Required]
        public Dept? Department { get; set; }
        //public string PhotoPath { get; set; }
        public IFormFile Photos { get; set; }    
    }
}
