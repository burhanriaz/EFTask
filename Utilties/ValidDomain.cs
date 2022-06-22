using System.ComponentModel.DataAnnotations;

namespace EFTask.Utilties
{
    public class ValidDomainAttribute: ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidDomainAttribute( string AllowedDomain)
        {
            allowedDomain = AllowedDomain;
        }
        public override bool IsValid(object value)
        {
            value.ToString();
            string[] Domain = value.ToString().Split('@');
            return allowedDomain.ToLower() == Domain[1].ToLower();
           
        }
    }
}
