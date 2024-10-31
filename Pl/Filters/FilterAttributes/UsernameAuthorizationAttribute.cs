using Microsoft.AspNetCore.Mvc;

namespace UpDownCryptorollBackend.Filters.FilterAttributes;

public class UsernameAuthorizationAttribute : TypeFilterAttribute
{
    public UsernameAuthorizationAttribute(string parameterName = "username") 
        : base(typeof(UsernameAuthorizationFilter))
    {
        Arguments = [parameterName];
    }
}