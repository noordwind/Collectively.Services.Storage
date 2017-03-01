using System.Linq;
using Autofac;
using Collectively.Common.Security;
using Collectively.Services.Storage.Services;
using Collectively.Services.Storage.Services.Operations;
using Collectively.Services.Storage.Services.Remarks;
using Collectively.Services.Storage.Services.Statistics;
using Collectively.Services.Storage.Services.Users;

namespace Collectively.Services.Storage.Framework.IoC
{
    public class ServiceClientModule : Module
    {
        private readonly static string OperationsSettingsKey = "operations-settings";
        private readonly static string RemarksSettingsKey = "remarks-settings";
        private readonly static string StatisticsSettingsKey = "statistics-settings";
        private readonly static string UsersSettingsKey = "users-settings";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "operations"))
                .Named<ServiceSettings>(OperationsSettingsKey)
                .SingleInstance();

            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "remarks"))
                .Named<ServiceSettings>(RemarksSettingsKey)
                .SingleInstance();

            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "statistics"))
                .Named<ServiceSettings>(StatisticsSettingsKey)
                .SingleInstance();

            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "users"))
                .Named<ServiceSettings>(UsersSettingsKey)
                .SingleInstance();

            builder.RegisterType<ServiceClient>()
                .As<IServiceClient>();

            builder.Register(x => new OperationServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(OperationsSettingsKey)))
                .As<IOperationServiceClient>()
                .SingleInstance();

            builder.Register(x => new RemarkServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(RemarksSettingsKey)))
                .As<IRemarkServiceClient>()
                .SingleInstance();

            builder.Register(x => new StatisticsServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(StatisticsSettingsKey)))
                .As<IStatisticsServiceClient>()
                .SingleInstance();

            builder.Register(x => new UserServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(UsersSettingsKey)))
                .As<IUserServiceClient>()
                .SingleInstance();
        }
    }
}