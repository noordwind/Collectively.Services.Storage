using Collectively.Common.Host;
using Collectively.Messages.Events.Operations;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Framework;
using Collectively.Messages.Events.Users;

namespace Collectively.Services.Storage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10000)
                .UseAutofac(Bootstrapper.LifeTimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<SignedUp>()
                .SubscribeToEvent<UsernameChanged>()
                .SubscribeToEvent<AvatarUploaded>()
                .SubscribeToEvent<AvatarRemoved>()
                .SubscribeToEvent<RemarkCreated>()
                .SubscribeToEvent<RemarkDeleted>()
                .SubscribeToEvent<RemarkResolved>()
                .SubscribeToEvent<RemarkProcessed>()
                .SubscribeToEvent<RemarkRenewed>()
                .SubscribeToEvent<RemarkCanceled>()
                .SubscribeToEvent<PhotosToRemarkAdded>()
                .SubscribeToEvent<PhotosFromRemarkRemoved>()
                .SubscribeToEvent<RemarkVoteSubmitted>()
                .SubscribeToEvent<RemarkVoteDeleted>()   
                .SubscribeToEvent<CommentAddedToRemark>()
                .SubscribeToEvent<CommentEditedInRemark>()
                .SubscribeToEvent<CommentDeletedFromRemark>()
                .SubscribeToEvent<RemarkCommentVoteSubmitted>()
                .SubscribeToEvent<RemarkCommentVoteDeleted>()             
                .SubscribeToEvent<OperationCreated>()
                .SubscribeToEvent<OperationUpdated>()
                .Build()
                .Run();
        }
    }
}
