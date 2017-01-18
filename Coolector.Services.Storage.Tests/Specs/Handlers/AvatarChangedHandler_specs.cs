using Coolector.Common.Domain;
using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using Coolector.Common.Services;
using Coolector.Services.Users.Shared.Dto;
using Coolector.Services.Users.Shared.Events;
using It = Machine.Specifications.It;

namespace Coolector.Services.Storage.Tests.Specs.Handlers
{
    public abstract class AvatarChangedHandler_specs
    {
        protected static IHandler Handler;
        protected static AvatarChangedHandler AvatarChangedHandler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static AvatarChanged Event;
        protected static UserDto User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            UserRepositoryMock = new Mock<IUserRepository>();
            AvatarChangedHandler = new AvatarChangedHandler(Handler, UserRepositoryMock.Object);
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
            Event = new AvatarChanged(Guid.NewGuid(), User?.UserId, User?.PictureUrl);
        }
    }

    [Subject("AvatarChangedHandler HandleAsync")]
    public class when_invoking_avatar_changed_handle_async : AvatarChangedHandler_specs
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
            UserRepositoryMock.Verify(x => x.EditAsync(Moq.It.IsAny<UserDto>()), Times.Once);
        };
    }
}