using Collectively.Services.Storage.Handlers;
using Collectively.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using Collectively.Common.Services;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Messages.Events.Remarks;
using It = Machine.Specifications.It;
using Collectively.Messages.Events;

namespace Collectively.Services.Storage.Tests.Specs.Handlers
{
    public abstract class RemarkDeletedHandler_specs
    {
        protected static IHandler Handler;
        protected static RemarkDeletedHandler RemarkDeletedHandler;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static Remark Remark;
        protected static RemarkDeleted Event;

        protected static void Initialize()
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            RemarkRepositoryMock = new Mock<IRemarkRepository>();

            Remark = new Remark
            {
                Id = Guid.NewGuid(),
                Photos = new List<File>()
            };
            Event = new RemarkDeleted(Guid.NewGuid(), Resource.Create("test", "test"), "userId", Remark.Id);
            RemarkDeletedHandler = new RemarkDeletedHandler(Handler, RemarkRepositoryMock.Object);
        }
    }

    [Subject("RemarkDeletedHandler HandleAsync")]
    public class when_invoking_remark_deleted_handle_async : RemarkDeletedHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(Remark);
        };

        Because of = () =>
        {
            RemarkDeletedHandler.HandleAsync(Event).Await();
        };

        It should_call_remark_repository_delete_async = () =>
        {
            RemarkRepositoryMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<Remark>()), Times.Once);
        };
    }
}