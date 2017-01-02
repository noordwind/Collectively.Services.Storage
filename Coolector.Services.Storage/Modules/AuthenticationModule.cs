using Coolector.Common.Security;
using Nancy;

namespace Coolector.Services.Storage.Modules
{
    public class AuthenticationModule : ModuleBase
    {
        public AuthenticationModule(IServiceAuthentication serviceAuthentication)
        {
            Post("authenticate", args => 
            {
                var credentials = this.BindRequest<Credentials>();
                var token = serviceAuthentication.CreateToken(credentials);
                if (token.HasNoValue)
                {
                    return HttpStatusCode.Unauthorized;
                }
                
                return new { token };
            });
        }        
    }
}