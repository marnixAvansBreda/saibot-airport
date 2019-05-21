using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Pitstop.FlightScheduleManagementAPI.Repositories.Model;
using Serilog;

namespace Pitstop.FlightScheduleManagementAPI.Repositories
{
    public class SqlServerFlightDataRepository : IFlightRepository
    {
        private string _connectionString;

        public SqlServerFlightDataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            List<Flight> flights = new List<Flight>();
            Log.Information("sqlserverrepo " + _connectionString);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Log.Error("connection " + conn.DataSource + " " + conn.Database + " " + conn.ClientConnectionId);
                Log.Error("Test log line 25");
                var flightsSelection = await conn.QueryAsync<Flight>("select * from Flight");
                Log.Information("flightselection " + flightsSelection);
                if (flightsSelection != null)
                {
                    flights.AddRange(flightsSelection);
                }
            }
            Log.Error("flights " + flights.Count);
            return flights;
        }

        public async Task<Flight> GetFlightAsync(string flightId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<Flight>("select * from Flight where FlightId = @FlightId",
                    new { FlightId = flightId });
            }
        }
    }
}
