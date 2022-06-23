using EFTask.Models;
using EFTask.Models.ViewModel;

namespace EFTask.Repository
{
    public interface ITokenService
    {
      
            string BuildToken(string key, string issuer, ApplicationUser user);
            bool ValidateToken(string key, string issuer, string audience, string token);
        
    }
}
