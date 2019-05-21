using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Pitstop.FlightScheduleManagementAPI.Repositories.Model;

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
            List<Flight> customers = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var flightsSelection = await conn.QueryAsync<Flight>("select * from Flight");

                if (flightsSelection != null)
                {
                    customers.AddRange(flightsSelection);
                }
            }

            return customers;
        }

        public async Task<Flight> GetFlightAsync(int flightId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<Flight>("select * from Flight where FlightId = @FlightId",
                    new { FlightId = flightId });
            }
        }
    }
}
