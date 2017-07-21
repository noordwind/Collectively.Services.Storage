using Collectively.Common.Security;
using Nancy;

namespace Collectively.Services.Storage.Modules
{
    public class AuthenticationModule : ModuleBase
    {
        public AuthenticationModule(IServiceAuthenticatorHost serviceAuthenticatorHost) 
            : base(requireAuthentication: false)
        {
            Post("authenticate", args => 
            {
                var credentials = this.BindRequest<Credentials>();
                var token = serviceAuthenticatorHost.CreateToken(credentials);
                if (token.HasNoValue)
                {
                    return HttpStatusCode.Unauthorized;
                }
                
                return token.Value;
            });
        }        
    }
}