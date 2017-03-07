using System;
using Autofac;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients;
using Collectively.Services.Storage.ServiceClients;

namespace Collectively.Services.Storage.Framework.IoC
{
    public class ServiceClientsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterService<OperationServiceClient, IOperationServiceClient>(builder, "operations");
            RegisterService<RemarkServiceClient, IRemarkServiceClient>(builder, "remarks");
            RegisterService<UserServiceClient, IUserServiceClient>(builder, "users");
            RegisterService<StatisticsServiceClient, IStatisticsServiceClient>(builder, "statistics");
        }

        private void RegisterService<TService, TInterface>(ContainerBuilder builder, string title) where TService : TInterface
        {
            var settingsKey = $"{title}-settings";
            builder.Register(x => (TService)Activator.CreateInstance(typeof(TService), 
                new object[]{x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(settingsKey)}))
                .As<TInterface>()
                .SingleInstance();
        }
    }
}