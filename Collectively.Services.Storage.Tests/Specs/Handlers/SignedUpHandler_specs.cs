using Collectively.Services.Storage.Handlers;
using Collectively.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using Collectively.Common.Services;

using Collectively.Messages.Events.Users;
using It = Machine.Specifications.It;

namespace Collectively.Services.Storage.Tests.Specs.Handlers
{
    public abstract class SignedUpHandler_specs
    {
        protected static IHandler Handler;
        protected static SignedUpHandler SignedUpHandler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static SignedUp Event;
        protected static UserDto User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            UserRepositoryMock = new Mock<IUserRepository>();
            SignedUpHandler = new SignedUpHandler(Handler, UserRepositoryMock.Object);
            setup();
        }

        protected static void InitializeUser()
        {
            User = new UserDto
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
            Event = new SignedUp(Guid.NewGuid(), User?.UserId, User?.Email, User?.Name,
                User?.PictureUrl, User?.Role, User?.State, "collectively",
                string.Empty, User?.CreatedAt ?? DateTime.UtcNow);
        }
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_user_signed_up_handle_async : SignedUpHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => SignedUpHandler.HandleAsync(Event).Await();

        It should_call_user_repository_exists_async = () =>
        {
            UserRepositoryMock.Verify(x => x.ExistsAsync(User.UserId), Times.Once);
        };

        It should_call_user_repository_add_async = () =>
        {
            UserRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<UserDto>()), Times.Once);
        };
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_user_signed_up_for_existing_user : SignedUpHandler_specs
    {
        private Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
            UserRepositoryMock.Setup(x => x.ExistsAsync(User.UserId)).ReturnsAsync(true);
        });

        Because of = () => SignedUpHandler.HandleAsync(Event).Await();

        It should_call_user_repository_exists_async = () =>
        {
            UserRepositoryMock.Verify(x => x.ExistsAsync(User.UserId), Times.Once);
        };

        It should_not_call_user_repository_add_async = () =>
        {
            UserRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<UserDto>()), Times.Never);
        };
    }
}