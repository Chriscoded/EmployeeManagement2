using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object? value)
        {
            //lets get the value of the domain inputed by using @ as a spliting point for the arrays
            //Strings[1] is the second array which has the domain name
            String[] Strings = value.ToString().Split("@");
            //if the domain name matches the domain name we sacified we want then return bool(true/false)
           return  Strings[1].ToUpper() == allowedDomain.ToUpper(); 
        }
    }
}
