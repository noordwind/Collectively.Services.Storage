using Coolector.Common.Dto.General;
using Coolector.Common.Dto.Users;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Providers.Users;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Services.Users;

namespace Coolector.Services.Storage.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(IUserProvider userProvider) : base("users")
        {
            Get("", async args => await FetchCollection<BrowseUsers, UserDto>
                (async x => await userServiceClient.BrowseAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetUser, UserDto>
                (async x => await userServiceClient.GetAsync(x.Id)).HandleAsync());

            Get("{name}/account", async args => await Fetch<GetUserByName, UserDto>
                (async x => await userServiceClient.GetByNameAsync(x.Name)).HandleAsync());

            Get("{name}/available", async args => await Fetch<GetNameAvailability, AvailableResourceDto>
                (async x => await userServiceClient.IsAvailableAsync(x.Name)).HandleAsync());
        }
    }
}