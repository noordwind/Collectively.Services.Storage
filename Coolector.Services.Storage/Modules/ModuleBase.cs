using Coolector.Common.Nancy;
using Nancy.Security;

namespace Coolector.Services.Storage.Modules
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