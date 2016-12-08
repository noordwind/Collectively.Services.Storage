﻿using Coolector.Common.Events.Remarks;
using Coolector.Common.Host;
using Coolector.Services.Storage.Framework;
using Coolector.Services.Users.Shared.Events;

namespace Coolector.Services.Storage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10000)
                .UseAutofac(Bootstrapper.LifeTimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<UserSignedUp>()
                .SubscribeToEvent<UserNameChanged>()
                .SubscribeToEvent<AvatarChanged>()
                .SubscribeToEvent<RemarkCreated>()
                .SubscribeToEvent<RemarkDeleted>()
                .SubscribeToEvent<RemarkResolved>()
                .SubscribeToEvent<OperationCreated>()
                .SubscribeToEvent<OperationUpdated>()
                .Build()
                .Run();
        }
    }
}
