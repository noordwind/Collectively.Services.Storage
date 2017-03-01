using System;
using System.Threading.Tasks;
using Collectively.Common.Types;

using Collectively.Services.Storage.Queries;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.Services.Remarks;

namespace Collectively.Services.Storage.Providers.Remarks
{
    public class RemarkProvider : IRemarkProvider
    {
        private readonly IProviderClient _provider;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkCategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IRemarkServiceClient _serviceClient;

        public RemarkProvider(IProviderClient provider,
            IRemarkRepository remarkRepository,
            IRemarkCategoryRepository categoryRepository,
            ITagRepository tagRepository,
            IRemarkServiceClient serviceClient)
        {
            _provider = provider;
            _remarkRepository = remarkRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<RemarkDto>> GetAsync(Guid id)
            => await _provider.GetAsync(
                async () => await _remarkRepository.GetByIdAsync(id),
                async () => await _serviceClient.GetAsync(id));

        public async Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query)
            => await _provider.GetCollectionAsync(async () => await _remarkRepository.BrowseAsync(query));

        public async Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await _provider.GetCollectionAsync(
                async () => await _categoryRepository.BrowseAsync(query),
                async () => await _serviceClient.BrowseCategoriesAsync(query));

        public async Task<Maybe<PagedResult<TagDto>>> BrowseTagsAsync(BrowseRemarkTags query)
            => await _provider.GetCollectionAsync(
                async () => await _tagRepository.BrowseAsync(query),
                async () => await _serviceClient.BrowseTagsAsync(query));
    }
}