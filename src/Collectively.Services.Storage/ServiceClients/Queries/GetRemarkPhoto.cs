﻿using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class GetRemarkPhoto : IQuery
    {
        public Guid Id { get; set; }
    }
}