using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using System.Collections.Generic;
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
        protected static RemarkCreatedHandler Handler;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IUserRepository> UserRepositoryMock;
        protected static RemarkCreated Event;
        protected static UserDto User;
        protected static Exception Exception;

        protected static void Initialize(Action setup)
        {
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            UserRepositoryMock = new Mock<IUserRepository>();
            Handler = new RemarkCreatedHandler(UserRepositoryMock.Object,
                RemarkRepositoryMock.Object, new GeneralSettings());
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
                new RemarkCreated.RemarkLocation(string.Empty, 1, 1), new List<RemarkFile>(),
                "test", DateTime.Now);
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

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_user_repository_get_by_id_async = () =>
        {
            UserRepositoryMock.Verify(x => x.GetByIdAsync(User.UserId), Times.Once);
        };

        It should_call_remark_repository_add_async = () =>
        {
            RemarkRepositoryMock.Verify(x => x.AddAsync(Moq.It.IsAny<RemarkDto>()), Times.Once);
        };
    }

    [Subject("RemarkCreatedHandler HandleAsync")]
    public class when_invoking_remark_created_handle_async_without_user : RemarkCreatedHandler_specs
    {
        Establish context = () => Initialize(() =>
        {
            InitializeEvent();
        });

        Because of = () => Exception = Catch.Exception(() => Handler.HandleAsync(Event).Await());

        It should_fail = () => Exception.ShouldBeOfExactType<InvalidOperationException>();

        It should_have_a_specific_reason = () => Exception.Message.ShouldContain("Operation is not valid");
    }
}