using System;
using Collectively.Common.Extensions;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Framework
{
    public static class Extensions
    {
        public static string ToCodename(this string value)
        => value.Empty() ? string.Empty : value.TrimToLower().Replace(" ", "-");

        public static bool IsLocationProvided(this BrowseRemarks query)
        => (Math.Abs(query.Latitude) <= 0.0000000001 || 
            Math.Abs(query.Longitude) <= 0.0000000001) == false;
    }
}