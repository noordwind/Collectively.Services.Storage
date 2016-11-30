using Coolector.Common.Dto.Users;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Services.Users;

namespace Coolector.Services.Storage.Modules
{
    public class UserSessionModule : ModuleBase
    {
        public UserSessionModule(IUserServiceClient userServiceClient) : base("user-sessions")
        {
            Get("{id}", async args => await Fetch<GetUserSession, UserSessionDto>
                (async x => await userServiceClient.GetSessionAsync(x.Id)).HandleAsync());
        }
    }
}