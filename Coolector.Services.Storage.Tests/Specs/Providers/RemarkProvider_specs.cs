using Coolector.Common.Types;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;
using Machine.Specifications;
using Moq;
using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.Remarks;
using It = Machine.Specifications.It;

namespace Coolector.Services.Storage.Tests.Specs.Providers
{
    public abstract class RemarkProvider_specs
    {
        protected static IRemarkProvider RemarkProvider;
        protected static Mock<IRemarkRepository> RemarkRepositoryMock;
        protected static Mock<IRemarkCategoryRepository> RemarkCategoryRepositoryMock;
        protected static Mock<IProviderClient> ProviderClientMock;
        protected static ProviderSettings ProviderSettings;

        protected static void Initialize()
        {
            RemarkRepositoryMock = new Mock<IRemarkRepository>();
            RemarkCategoryRepositoryMock = new Mock<IRemarkCategoryRepository>();
            ProviderClientMock = new Mock<IProviderClient>();
            ProviderSettings = new ProviderSettings
            {
                UsersApiUrl = "apiUrl"
            };
            RemarkProvider = new RemarkProvider(RemarkRepositoryMock.Object, RemarkCategoryRepositoryMock.Object,
                ProviderClientMock.Object, ProviderSettings);
        }
    }

    [Subject("RemarkProvider BrowseAsync")]
    public class when_invoking_remark_provider_browse_async : RemarkProvider_specs
    {
        Establish context = () => Initialize();

        Because of = () => RemarkProvider.BrowseAsync(Moq.It.IsAny<BrowseRemarks>()).Await();

        It should_call_get_collection_using_storage_async = () =>
        {
            ProviderClientMock.Verify(x => x.GetFilteredCollectionUsingStorageAsync(
                Moq.It.IsAny<BrowseRemarks>(),
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<Func<Task<Maybe<PagedResult<RemarkDto>>>>>(),
                Moq.It.IsAny<Func<PagedResult<RemarkDto>, Task>>()), Times.Once);
        };
    }

    [Subject("RemarkProvider GetAsync")]
    public class when_invoking_remark_provider_get_async : RemarkProvider_specs
    {
        Establish context = () => Initialize();

        Because of = () => RemarkProvider.GetAsync(Moq.It.IsAny<Guid>()).Await();

        It should_call_get_async = () =>
        {
            ProviderClientMock.Verify(x => x.GetUsingStorageAsync(
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<string>(),
                Moq.It.IsAny<Func<Task<Maybe<RemarkDto>>>>(),
                Moq.It.IsAny<Func<RemarkDto, Task>>()), Times.Once);
        };
    }
}