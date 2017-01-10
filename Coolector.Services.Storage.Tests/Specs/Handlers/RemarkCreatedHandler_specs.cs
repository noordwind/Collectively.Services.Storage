using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Remarks.Shared.Events.Models;
using Coolector.Services.Storage.Settings;
using Coolector.Services.Users.Shared.Dto;
using It = Machine.Specifications.It;

namespace Coolector.Services.Storage.Tests.Specs.Handlers
{
    public abstract class RemarkCreatedHandler_specs
    {
        protected static IHandler Handler;
        protected static RemarkCreatedHandler RemarkCreatedHandler;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static RemarkCreated Event;
        protected static UserDto User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            Handler = new Handler();
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            UserRepositoryMock = new Mock<IUserRepository>();
            RemarkCreatedHandler = new RemarkCreatedHandler(Handler, 
                UserRepositoryMock.Object,
                RemarkRepositoryMock.Object);
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
            Event = new RemarkCreated(Guid.NewGuid(), Guid.NewGuid(), User?.UserId, User?.Name,
                new RemarkCreated.RemarkCategory(Guid.NewGuid(), "litter"), 
                new RemarkCreated.RemarkLocation(string.Empty, 1, 1),
                "test", new List<string>{ "tag1" }, DateTime.Now);
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

        It should_call_user_repository_get_by_id_async = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(User.UserId), Times.Once);
        };

        It should_call_remark_repository_add_async = () =>
        {
            RemarkRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<RemarkDto>()), Times.Once);
        };
    }
}