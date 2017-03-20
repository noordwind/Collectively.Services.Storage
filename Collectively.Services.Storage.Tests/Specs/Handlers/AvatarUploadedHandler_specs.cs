using Collectively.Services.Storage.Handlers;
using Collectively.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using Collectively.Common.Services;
using Collectively.Services.Storage.Models.Users;
using Collectively.Messages.Events.Users;
using It = Machine.Specifications.It;

namespace Collectively.Services.Storage.Tests.Specs.Handlers
{
    public abstract class AvatarUploadedHandler_specs
    {
        protected static IHandler Handler;
        protected static AvatarUploadedHandler AvatarUploadedHandler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static AvatarChanged Event;
        protected static User User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            UserRepositoryMock = new Mock<IUserRepository>();
            AvatarUploadedHandler = new AvatarUploadedHandler(Handler, UserRepositoryMock.Object);
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
            Event = new AvatarChanged(Guid.NewGuid(), User?.UserId, User?.AvatarUrl);
        }
    }

    [Subject("AvatarChangedHandler HandleAsync")]
    public class when_invoking_avatar_changed_handle_async : AvatarUploadedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => AvatarChangedHandler.HandleAsync(Event).Await();

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