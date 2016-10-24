using Coolector.Common.Events.Remarks;
using Coolector.Dto.Common;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Handlers;
using Coolector.Services.Storage.Repositories;
using Machine.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using It = Machine.Specifications.It;

namespace Coolector.Services.Storage.Tests.Specs.Handlers
{
    public abstract class RemarkDeletedHandler_specs
    {
        protected static RemarkDeletedHandler Handler;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;

        protected static RemarkDto RemarkDto;
        protected static RemarkDeleted Event;

        protected static void Initialize()
        {
            RemarkRepositoryMock = new Mock<IRemarkRepository>();

            var guid = Guid.NewGuid();
            RemarkDto = new RemarkDto
            {
                Id = guid,
                Photos = new List<FileDto>()
            };
            Event = new RemarkDeleted(guid);

            Handler = new RemarkDeletedHandler(RemarkRepositoryMock.Object);
        }
    }

    [Subject("RemarkDeletedHandler HandleAsync")]
    public class when_invoking_remark_deleted_handle_async : RemarkDeletedHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            RemarkRepositoryMock.Setup(x => x.GetByIdAsync(Moq.It.IsAny<Guid>()))
                .ReturnsAsync(RemarkDto);
        };

        Because of = () =>
        {
            Handler.HandleAsync(Event).Await();
        };

        It should_call_remark_repository_delete_async = () =>
        {
            RemarkRepositoryMock.Verify(x => x.DeleteAsync(Moq.It.IsAny<RemarkDto>()), Times.Once);
        };
    }
}