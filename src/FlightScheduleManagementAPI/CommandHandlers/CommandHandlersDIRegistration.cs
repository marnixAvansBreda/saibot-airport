using Microsoft.Extensions.DependencyInjection;

namespace FlightScheduleManagementAPI.CommandHandlers
{
    public static class CommandHandlersDIRegistration
    {
        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<IScheduleFlightCommandHandler, ScheduleFlightCommandHandler>();
        }
    }
}