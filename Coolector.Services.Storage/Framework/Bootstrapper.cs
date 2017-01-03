using System.Collections.Generic;
using System.Globalization;
using Autofac;
using Coolector.Common.Exceptionless;
using Coolector.Common.Mongo;
using Coolector.Common.Nancy;
using Coolector.Common.Nancy.Serialization;
using Coolector.Common.Security;
using Coolector.Services.Storage.Framework.IoC;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Newtonsoft.Json;
using NLog;
using RawRabbit.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Coolector.Common.Extensions;
using Coolector.Common.Services;
using Coolector.Common.RabbitMq;
using Coolector.Services.Storage.Cache;
using Coolector.Services.Storage.Providers.Operations;
using Coolector.Services.Storage.Providers.Remarks;
using Coolector.Services.Storage.Providers.Statistics;
using Coolector.Services.Storage.Providers.Users;
using Coolector.Services.Storage.Services;
using Coolector.Services.Storage.Services.Operations;
using Coolector.Services.Storage.Services.Remarks;
using Coolector.Services.Storage.Services.Statistics;
using Coolector.Services.Storage.Services.Users;

namespace Coolector.Services.Storage.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IExceptionHandler _exceptionHandler;
        private static readonly string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string InvalidDecimalSeparator = DecimalSeparator == "." ? "," : ".";
        private readonly IConfiguration _configuration;

        public static ILifetimeScope LifeTimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

#if DEBUG
        public override void Configure(INancyEnvironment environment)
        {
            base.Configure(environment);
            environment.Tracing(enabled: false, displayErrorTraces: true);
        }
#endif

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            Logger.Info("Coolector.Services.Storage Configuring application container");
            base.ConfigureApplicationContainer(container);

            container.Update(builder =>
            {
                builder.RegisterType<CustomJsonSerializer>().As<JsonSerializer>().SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<GeneralSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<MongoDbSettings>()).SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<ProviderSettings>()).SingleInstance();
                builder.RegisterModule<MongoDbModule>();
                builder.RegisterType<MongoDbInitializer>().As<IDatabaseInitializer>();
                builder.RegisterType<DatabaseSeeder>().As<IDatabaseSeeder>();
                builder.RegisterType<OperationRepository>().As<IOperationRepository>();
                builder.RegisterType<RemarkRepository>().As<IRemarkRepository>();
                builder.RegisterType<RemarkCategoryRepository>().As<IRemarkCategoryRepository>();
                builder.RegisterType<TagRepository>().As<ITagRepository>();
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<UserSessionRepository>().As<IUserSessionRepository>();
                builder.RegisterType<CustomHttpClient>().As<IHttpClient>();
                builder.RegisterType<ServiceClient>().As<IServiceClient>();
                builder.RegisterType<ProviderClient>().As<IProviderClient>();
                builder.RegisterType<RemarkServiceClient>().As<IRemarkServiceClient>();
                builder.RegisterType<RemarkProvider>().As<IRemarkProvider>();
                builder.RegisterType<UserServiceClient>().As<IUserServiceClient>();
                builder.RegisterType<UserProvider>().As<IUserProvider>();
                builder.RegisterType<OperationServiceClient>().As<IOperationServiceClient>();
                builder.RegisterType<OperationProvider>().As<IOperationProvider>();
                builder.RegisterType<StatisticsServiceClient>().As<IStatisticsServiceClient>();
                builder.RegisterType<StatisticsProvider>().As<IStatisticsProvider>();
                builder.RegisterInstance(_configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
                builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();
                builder.RegisterModule<EventHandlersModule>();
                builder.RegisterModule<RedisModule>();
                SecurityContainer.Register(builder, _configuration);
                RabbitMqContainer.Register(builder, _configuration.GetSettings<RawRabbitConfiguration>());
            });
            LifeTimeScope = container;
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                _exceptionHandler.Handle(ex, ctx.ToExceptionData(),
                    "Request details", "Coolector", "Service", "Storage");

                return ctx.Response;
            });
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var databaseSettings = container.Resolve<MongoDbSettings>();
            var databaseInitializer = container.Resolve<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync();
            var databaseSeeder = container.Resolve<IDatabaseSeeder>();
            databaseSeeder.SeedAsync();

            pipelines.BeforeRequest += (ctx) =>
            {
                FixNumberFormat(ctx);

                return null;
            };
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,PUT,GET,OPTIONS,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers",
                    "Authorization, Origin, X-Requested-With, Content-Type, Accept");
            };
            pipelines.SetupTokenAuthentication(container);
            _exceptionHandler = container.Resolve<IExceptionHandler>();
            Logger.Info("Coolector.Services.Storage API has started.");
        }

        private void FixNumberFormat(NancyContext ctx)
        {
            if (ctx.Request.Query == null)
                return;

            var fixedNumbers = new Dictionary<string, double>();
            foreach (var key in ctx.Request.Query)
            {
                var value = ctx.Request.Query[key].ToString();
                if (!value.Contains(InvalidDecimalSeparator))
                    continue;

                var number = 0;
                if (int.TryParse(value.Split(InvalidDecimalSeparator[0])[0], out number))
                    fixedNumbers[key] = double.Parse(value.Replace(InvalidDecimalSeparator, DecimalSeparator));
            }
            foreach (var fixedNumber in fixedNumbers)
            {
                ctx.Request.Query[fixedNumber.Key] = fixedNumber.Value;
            }
        }
    }
}