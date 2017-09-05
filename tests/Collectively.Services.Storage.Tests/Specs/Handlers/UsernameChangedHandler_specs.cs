using Collectively.Services.Storage.Handlers;
using Collectively.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using Collectively.Common.Services;
using Collectively.Services.Storage.Models.Users;
using Collectively.Messages.Events.Users;
using It = Machine.Specifications.It;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Tests.Specs.Handlers
{
    public abstract class UsernameChangedHandler_specs
    {
        protected static IHandler Handler;
        protected static UsernameChangedHandler UsernameChangedHandler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static Mock<IUserCache> UserCacheMock;
        protected static UsernameChanged Event;
        protected static User User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            UserRepositoryMock = new Mock<IUserRepository>();
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            UserCacheMock = new Mock<IUserCache>();
            UsernameChangedHandler = new UsernameChangedHandler(Handler,
                UserRepositoryMock.Object, RemarkRepositoryMock.Object,
                UserCacheMock.Object);
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
        }

        protected static void InitializeEvent()
        {
            Event = new UsernameChanged(Guid.NewGuid(), User?.UserId, User?.Name, User?.State);
        }
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_username_changed_handle_async : UsernameChangedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => UsernameChangedHandler.HandleAsync(Event).Await();

        It should_call_user_repository_get_by_id_async = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(User.UserId), Times.Once);
        };

        It should_call_user_repository_edit_async = () =>
        {
            UserRepositoryMock.Verify(x => x.EditAsync(Moq.It.IsAny<User>()), Times.Once);
        };
    }
}