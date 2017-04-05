using Collectively.Common.Extensions;
using Collectively.Common.Types;
using Collectively.Services.Storage.Providers;
using Machine.Specifications;
using Moq;
using Collectively.Services.Storage.Tests.Framework;
using It = Machine.Specifications.It;
using Collectively.Common.ServiceClients;

namespace Collectively.Services.Storage.Tests.Specs.Providers
{
    public abstract class ProviderClient_specs
    {
        protected static IProviderClient ProviderClient;
        protected static Mock<IServiceClient> ServiceClientMock;
        protected static Mock<IStorage> StorageMock;
        protected static Mock<IStorage> EmptyStorageMock;

        protected static void Initialize()
        {
            ServiceClientMock = new Mock<IServiceClient>();
            StorageMock = new Mock<IStorage>();
            EmptyStorageMock = new Mock<IStorage>();

            ProviderClient = new ProviderClient();

            var obj = new object();
            var collection = new[] {new object(), new object()}.PaginateWithoutLimit();
            StorageMock.Setup(x => x.FetchAsync()).ReturnsAsync(obj);
            StorageMock.Setup(x => x.FetchCollectionAsync()).ReturnsAsync(collection);
            EmptyStorageMock.Setup(x => x.FetchAsync()).ReturnsAsync(Maybe<object>.Empty);
            EmptyStorageMock.Setup(x => x.FetchCollectionAsync()).ReturnsAsync(Maybe<PagedResult<object>>.Empty);
        }
    }

    [Subject("ProviderClient GetAsync")]
    public class when_get_async : ProviderClient_specs
    {
        protected static Maybe<object> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ProviderClient.GetAsync(async () => await StorageMock.Object.FetchAsync()).Result;

        It should_not_be_null = () => Result.ShouldNotBeNull();

        It should_return_value = () => Result.HasValue.ShouldBeTrue();
    }

    [Subject("ProviderClient GetAsync")]
    public class when_get_async_with_two_handlers_and_first_returns_value : ProviderClient_specs
    {
        protected static Maybe<object> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ProviderClient.GetAsync(
            async () => await StorageMock.Object.FetchAsync(),
            async () => await EmptyStorageMock.Object.FetchAsync()).Result;

        It should_not_be_null = () => Result.ShouldNotBeNull();
        It should_return_value = () => Result.HasValue.ShouldBeTrue();
        It should_not_call_second_handler = () => EmptyStorageMock.Verify(x => x.FetchAsync(), Times.Never);
    }

    [Subject("ProviderClient GetAsync")]
    public class when_get_async_with_two_handlers_and_second_returns_value : ProviderClient_specs
    {
        protected static Maybe<object> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ProviderClient.GetAsync(
            async () => await EmptyStorageMock.Object.FetchAsync(),
            async () => await StorageMock.Object.FetchAsync()).Result;

        It should_not_be_null = () => Result.ShouldNotBeNull();
        It should_return_value = () => Result.HasValue.ShouldBeTrue();
        It should_call_first_handler = () => EmptyStorageMock.Verify(x => x.FetchAsync(), Times.Once);
        It should_call_second_handler = () => StorageMock.Verify(x => x.FetchAsync(), Times.Once);
    }

    [Subject("ProviderClient GetCollectionAsync")]
    public class when_get_collection_async : ProviderClient_specs
    {
        protected static Maybe<PagedResult<object>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ProviderClient.GetCollectionAsync(
            async () => await StorageMock.Object.FetchCollectionAsync()).Result;

        It should_not_be_null = () => Result.ShouldNotBeNull();
        It should_return_value = () => Result.HasValue.ShouldBeTrue();
    }

    [Subject("ProviderClient GetCollectionAsync")]
    public class when_get_collection_async_with_two_handlers_and_first_returns_value : ProviderClient_specs
    {
        protected static Maybe<PagedResult<object>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ProviderClient.GetCollectionAsync(
            async () => await StorageMock.Object.FetchCollectionAsync(),
            async () => await EmptyStorageMock.Object.FetchCollectionAsync()).Result;

        It should_not_be_null = () => Result.ShouldNotBeNull();
        It should_return_value = () => Result.HasValue.ShouldBeTrue();
        It should_not_call_second_handler = () => EmptyStorageMock.Verify(x => x.FetchCollectionAsync(), Times.Never);
    }

    [Subject("ProviderClient GetCollectionAsync")]
    public class when_get_collection_async_with_two_handlers_and_second_returns_value : ProviderClient_specs
    {
        protected static Maybe<PagedResult<object>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ProviderClient.GetCollectionAsync(
            async () => await EmptyStorageMock.Object.FetchCollectionAsync(),
            async () => await StorageMock.Object.FetchCollectionAsync()).Result;

        It should_not_be_null = () => Result.ShouldNotBeNull();
        It should_return_value = () => Result.HasValue.ShouldBeTrue();
        It should_call_first_handler = () => EmptyStorageMock.Verify(x => x.FetchCollectionAsync(), Times.Once);
        It should_call_second_handler = () => StorageMock.Verify(x => x.FetchCollectionAsync(), Times.Once);
    }
}