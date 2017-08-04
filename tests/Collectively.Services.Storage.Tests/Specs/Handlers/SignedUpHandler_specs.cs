using Collectively.Services.Storage.Handlers;
using Collectively.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using Collectively.Common.Services;
using Collectively.Services.Storage.Models.Users;
using Collectively.Messages.Events.Users;
using It = Machine.Specifications.It;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Messages.Events;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Tests.Specs.Handlers
{
    public abstract class SignedUpHandler_specs
    {
        protected static IHandler Handler;
        protected static SignedUpHandler SignedUpHandler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static Mock<IUserServiceClient> UserServiceClientMock;
        protected static Mock<IAccountStateService> AccountStateServiceMock;
        protected static SignedUp Event;
        protected static User User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            UserRepositoryMock = new Mock<IUserRepository>();
            UserServiceClientMock = new Mock<IUserServiceClient>();
            SignedUpHandler = new SignedUpHandler(Handler, UserRepositoryMock.Object, 
                UserServiceClientMock.Object, AccountStateServiceMock.Object);
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
            UserRepositoryMock.Setup(x => x.GetByIdAsync(User.UserId))
                .ReturnsAsync(User);
            UserServiceClientMock.Setup(x => x.GetAsync<User>(User.UserId))
                .ReturnsAsync(User);            
        }

        protected static void InitializeEvent()
        {
            Event = new SignedUp(Guid.NewGuid(), Resource.Create("test", "test"), 
                User?.UserId, "collectively");
        }
    }

    [Subject("SignedUpHandler HandleAsync")]
    public class when_invoking_user_signed_up_handle_async : SignedUpHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => SignedUpHandler.HandleAsync(Event).Await();
        
        It should_call_user_repository_add_async = () =>
        {
            UserRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<User>()), Times.Once);
        };
    }
}