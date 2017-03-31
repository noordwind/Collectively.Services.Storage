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
    public class FavoriteRemarkDeletedHandler : IEventHandler<FavoriteRemarkDeleted>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IRemarkRepository _remarkRepository;

        public FavoriteRemarkDeletedHandler(IHandler handler, IUserRepository userRepository,
            IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(FavoriteRemarkDeleted @event)
        {
            await _handler
                .Run(async () =>
                {
                    var user = await _userRepository.GetByIdAsync(@event.UserId);
                    if (user.HasNoValue)
                    {
                        throw new ServiceException(OperationCodes.UserNotFound,
                            $"Favorite remark cannot be deleted because user: {@event.UserId} does not exist");
                    }
                    user.Value.FavoriteRemarks.Remove(@event.RemarkId);
                    await _userRepository.EditAsync(user.Value);
                    
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }
                    remark.Value.UserFavorites.Remove(@event.UserId);
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