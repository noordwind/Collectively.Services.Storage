using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;
using System.Linq;

namespace Collectively.Services.Storage.Providers
{
    public class RemarkProvider : IRemarkProvider
    {
        private readonly IProviderClient _provider;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkCategoryRepository _categoryRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IRemarkServiceClient _serviceClient;

        public RemarkProvider(IProviderClient provider,
            IRemarkRepository remarkRepository,
            IRemarkCategoryRepository categoryRepository,
            IGroupRepository groupRepository,
            ITagRepository tagRepository,
            IReportRepository reportRepository,
            IRemarkServiceClient serviceClient)
        {
            _provider = provider;
            _remarkRepository = remarkRepository;
            _categoryRepository = categoryRepository;
            _groupRepository = groupRepository;
            _tagRepository = tagRepository;
            _reportRepository = reportRepository;
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<Remark>> GetAsync(Guid id)
        { 
            var remark = await _provider.GetAsync(
                async () => await _remarkRepository.GetByIdAsync(id),
                async () => await _serviceClient.GetAsync<Remark>(id));
            if(remark.HasNoValue)
            {
                return null;
            }
            if(remark.Value.Group == null)
            {
                return remark;
            }
            var group = await _groupRepository.GetAsync(remark.Value.Group.Id);
            remark.Value.Group.Criteria = group.Value.Criteria;
            remark.Value.Group.Members = group.Value.Members.ToDictionary(x => x.UserId, x => x.Role);

            return remark;
        }

        public async Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query)
            => await _provider.GetCollectionAsync(async () => await _remarkRepository.BrowseAsync(query));

        public async Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await _provider.GetCollectionAsync(
                async () => await _categoryRepository.BrowseAsync(query),
                async () => await _serviceClient.BrowseCategoriesAsync<RemarkCategory>(query));

        public async Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query)
            => await _provider.GetCollectionAsync(
                async () => await _tagRepository.BrowseAsync(query),
                async () => await _serviceClient.BrowseTagsAsync<Tag>(query));

        public async Task<Maybe<PagedResult<Report>>> BrowseReportsAsync(BrowseReports query)
            => await _provider.GetCollectionAsync(async () => await _reportRepository.BrowseAsync(query));
    }
}