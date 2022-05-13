using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UserServiceWebApi.Helpers
{
    public static class AuthServiceHelper
    {
        public static SymmetricSecurityKey GetKey(string originKey) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(originKey));
    }
}
