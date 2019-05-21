using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.BaggageSetManagementEventHandler.DataAccess
{
    public static class DBInitializer
    {
        public static void Initialize(BaggageSetManagementDBContext context)
        {
              Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => context.Database.Migrate());
        }
    }
}
