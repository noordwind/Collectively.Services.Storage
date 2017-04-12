using Autofac;
using Collectively.Common.ServiceClients;
using Collectively.Services.Storage.ServiceClients;

namespace Collectively.Services.Storage.Framework.IoC
{
    public class ServiceClientsModule : ServiceClientsModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterService<OperationServiceClient, IOperationServiceClient>(builder, "operations");
            RegisterService<RemarkServiceClient, IRemarkServiceClient>(builder, "remarks");
            RegisterService<UserServiceClient, IUserServiceClient>(builder, "users");
            RegisterService<StatisticsServiceClient, IStatisticsServiceClient>(builder, "statistics");
            RegisterService<NotificationServiceClient, INotificationServiceClient>(builder, "notification");
        }
    }
}