using Autofac;
using Collectively.Messages.Commands;
using System.Reflection;
using Module = Autofac.Module;

namespace Collectively.Services.Storage.Framework.IoC
{
    public class CommandHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var coreAssembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
        }
    }
}