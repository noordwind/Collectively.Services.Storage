using Collectively.Common.NancyFx;
using Nancy.Security;

namespace Collectively.Services.Storage.Modules
{
    public abstract class ModuleBase : ApiModuleBase
    {
        protected ModuleBase(string modulePath = "", bool requireAuthentication = true) : base(modulePath)
        {
            if (requireAuthentication)
            {
                this.RequiresAuthentication();
            }
        }
    }
}