using Collectively.Services.Storage.Handlers;
using Collectively.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using Collectively.Common.Services;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Users;
using It = Machine.Specifications.It;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Messages.Events;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Tests.Specs.Handlers
{
    public abstract class RemarkCreatedHandler_specs
    {
        protected static IHandler Handler;
        protected static RemarkCreatedHandler RemarkCreatedHandler;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static Mock<IRemarkServiceClient> RemarkServiceClientMock;
        protected static Mock<IRemarkCache> RemarkCacheMock;
        protected static RemarkCreated Event;
        protected static Guid RemarkId = Guid.NewGuid();
        protected static User User;
        protected static Remark Remark;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            RemarkServiceClientMock = new Mock<IRemarkServiceClient>();
            RemarkCacheMock = new Mock<IRemarkCache>();
            Remark = new Remark();
            RemarkServiceClientMock
                .Setup(x => x.GetAsync<Remark>(RemarkId))
                .ReturnsAsync(Remark);
            RemarkCreatedHandler = new RemarkCreatedHandler(Handler, 
                RemarkRepositoryMock.Object, 
                RemarkServiceClientMock.Object,
                RemarkCacheMock.Object);
            setup();
        }

        protected static void InitializeUser()
        {
            User = new User
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid().ToString(),
                Name = "user"
            };
        }

        protected static void InitializeEvent()
        {
            Event = new RemarkCreated(Guid.NewGuid(), Resource.Create("test", "test"), User.UserId, RemarkId);
        }
    }

    [Subject("RemarkCreatedHandler HandleAsync")]
    public class when_invoking_remark_created_handler_async : RemarkCreatedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => RemarkCreatedHandler.HandleAsync(Event).Await();

        It should_call_remark_repository_add_async = () =>
        {
            RemarkRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<Remark>()), Times.Once);
        };
    }
}