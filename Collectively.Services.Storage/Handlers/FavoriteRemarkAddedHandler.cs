using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Remarks;

namespace Collectively.Services.Storage.Handlers
{
    public class FavoriteRemarkAddedHandler : IEventHandler<FavoriteRemarkAdded>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IRemarkRepository _remarkRepository;

        public FavoriteRemarkAddedHandler(IHandler handler, IUserRepository userRepository,
            IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(FavoriteRemarkAdded @event)
        {
            await _handler
                .Run(async () =>
                {
                    var user = await _userRepository.GetByIdAsync(@event.UserId);
                    if (user.HasNoValue)
                    {
                        throw new ServiceException(OperationCodes.UserNotFound,
                            $"Favorite remark cannot be added because user: {@event.UserId} does not exist");
                    }
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }
                    if (user.Value.FavoriteRemarks == null) 
                    {
                        user.Value.FavoriteRemarks = new HashSet<Guid>();
                    }
                    if (remark.Value.UserFavorites == null) 
                    {
                        remark.Value.UserFavorites = new HashSet<string>();
                    }
                    user.Value.FavoriteRemarks.Add(@event.RemarkId);
                    remark.Value.UserFavorites.Add(@event.UserId);
                    await _userRepository.EditAsync(user.Value);
                    await _remarkRepository.UpdateAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}