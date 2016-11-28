using Coolector.Services.Storage.Mappers;
using Machine.Specifications;
using System;
using Coolector.Common.Dto.Users;
using It = Machine.Specifications.It;
using FluentAssertions;

namespace Coolector.Services.Storage.Tests.Specs.Mappers
{
    [Subject("UserMapper Map")]
    public class when_invoking_user_mapper_map : Mapper_specs<UserDto>
    {
        Establish context = () =>
        {
            Initialize(new UserMapper());
            Source.id = Guid.NewGuid();
            Source.userId = Guid.NewGuid().ToString();
            Source.email = "user1@email.com";
            Source.name = "user1";
            Source.pictureUrl = "url";
            Source.role = "user";
            Source.state = "active";
            Source.createdAt = DateTime.UtcNow;
            Source.provider = "coolector";
            Source.externalUserId = "extId";
        };

        Because of = () => Map();

        It should_map_user_object = () =>
        {
            Result.ShouldNotBeNull();
            Result.Id.ShouldBeEquivalentTo((Guid)Source.id);
            Result.UserId.ShouldBeEquivalentTo((string)Source.userId);
            Result.Email.ShouldBeEquivalentTo((string)Source.email);
            Result.Name.ShouldBeEquivalentTo((string)Source.name);
            Result.PictureUrl.ShouldBeEquivalentTo((string)Source.pictureUrl);
            Result.Role.ShouldBeEquivalentTo((string)Source.role);
            Result.State.ShouldBeEquivalentTo((string)Source.state);
            Result.CreatedAt.ShouldBeEquivalentTo((DateTime)Source.createdAt);
            Result.Provider.ShouldBeEquivalentTo((string)Source.provider);
            Result.ExternalUserId.ShouldBeEquivalentTo((string)Source.externalUserId);
        };
    }
}