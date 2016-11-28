using Coolector.Common.Dto.Users;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Modules
{
    public class UserSessionModule : ModuleBase
    {
        public UserSessionModule(IUserProvider userProvider) : base("user-sessions")
        {
            Get("{id}", async args => await Fetch<GetUserSession, UserSessionDto>
                (async x => await userProvider.GetSessionAsync(x.Id)).HandleAsync());
        }
    }
}