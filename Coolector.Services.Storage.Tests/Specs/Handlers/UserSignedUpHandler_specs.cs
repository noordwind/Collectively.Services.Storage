using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using Coolector.Services.Users.Shared.Dto;
using Coolector.Services.Users.Shared.Events;
using It = Machine.Specifications.It;

namespace Coolector.Services.Storage.Tests.Specs.Handlers
{
    public abstract class UserSignedUpHandler_specs
    {
        protected static UserSignedUpHandler Handler;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static UserSignedUp Event;
        protected static UserDto User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            Handler = new UserSignedUpHandler(UserRepositoryMock.Object);
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
            Event = new UserSignedUp(Guid.NewGuid(), User?.UserId, User?.Email, User?.Name,
                User?.PictureUrl, User?.Role, User?.State, "coolector",
                string.Empty, User?.CreatedAt ?? DateTime.UtcNow);
        }
    }

    [Subject("UserNameChangedHandler HandleAsync")]
    public class when_invoking_user_signed_up_handle_async : UserSignedUpHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
        });

        Because of = () => Handler.HandleAsync(Event).Await();

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
    public class when_invoking_user_signed_up_for_existing_user : UserSignedUpHandler_specs
    {
        private Establish context = () => Initialize(() =>
        {
            InitializeUser();
            InitializeEvent();
            UserRepositoryMock.Setup(x => x.ExistsAsync(User.UserId)).ReturnsAsync(true);
        });

        Because of = () => Handler.HandleAsync(Event).Await();

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