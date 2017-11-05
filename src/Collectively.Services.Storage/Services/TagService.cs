using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task AddOrUpdateAsync(IEnumerable<Tag> tags)
        {
            var allTags = await _tagRepository.BrowseAsync(new BrowseTags
            {
                Results = int.MaxValue
            });
            var newTags = new List<Tag>();
            foreach (var tag in tags)
            {
                var existingTag = allTags.Value.Items.SingleOrDefault(x => x.Name == tag.Name);
                if (existingTag != null)
                {
                    continue;
                }
                if (newTags.Any(x => x.Name == tag.Name))
                {
                    continue;
                }
                newTags.Add(tag);
            }
            await _tagRepository.AddManyAsync(newTags);
        }
    }
}