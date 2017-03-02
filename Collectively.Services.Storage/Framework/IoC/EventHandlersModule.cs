using Autofac;
using Collectively.Messages.Events;
using System.Reflection;
using Module = Autofac.Module;

namespace Collectively.Services.Storage.Framework.IoC
{
    public class EventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var coreAssembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}