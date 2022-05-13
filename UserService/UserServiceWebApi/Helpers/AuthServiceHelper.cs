using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UserServiceWebApi.Helpers
{
    public static class AuthServiceHelper
    {
        public static SymmetricSecurityKey GetKey(string originKey) => 
            string.IsNullOrEmpty(originKey) ? null : new SymmetricSecurityKey(Encoding.ASCII.GetBytes(originKey));
    }
}
