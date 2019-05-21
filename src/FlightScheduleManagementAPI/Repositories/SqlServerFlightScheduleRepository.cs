using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Pitstop.FlightScheduleManagementAPI.Domain;
using Pitstop.FlightScheduleManagementAPI.Repositories.Model;
using Pitstop.Infrastructure.Messaging;
using Polly;
using Serilog;

namespace Pitstop.FlightScheduleManagementAPI.Repositories
{
    public class SqlServerFlightScheduleRepository : IFlightScheduleRepository
    {
        private static readonly JsonSerializerSettings _serializerSettings;
        private static readonly Dictionary<DateTime, string> _store = new Dictionary<DateTime, string>();
        private string _connectionString;
        
        static SqlServerFlightScheduleRepository()
        {
            _serializerSettings = new JsonSerializerSettings();
            _serializerSettings.Formatting = Formatting.Indented;
            _serializerSettings.Converters.Add(new StringEnumConverter
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            });
        }
        public SqlServerFlightScheduleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void EnsureDatabase()
        {
            // init db
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("FlightScheduleEventStore", "master")))
            {
                conn.Open();

                // create database
                string sql = "if DB_ID('FlightScheduleEventStore') IS NULL CREATE DATABASE FlightScheduleEventStore;";

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) =>
                    { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                    .Execute(() => conn.Execute(sql));

                conn.ChangeDatabase("FlightScheduleEventStore");
                sql = @" 
                    if OBJECT_ID('FlightSchedule') IS NULL 
                    CREATE TABLE FlightSchedule (
                        [Id] varchar(50) NOT NULL,
                        [CurrentVersion] int NOT NULL,
                    PRIMARY KEY([Id]));
                   
                    if OBJECT_ID('FlightScheduleEvent') IS NULL
                    CREATE TABLE FlightScheduleEvent (
                        [Id] varchar(50) NOT NULL REFERENCES FlightSchedule([Id]),
                        [Version] int NOT NULL,
                        [Timestamp] datetime2(7) NOT NULL,
                        [MessageType] varchar(75) NOT NULL,
                        [EventData] text,
                    PRIMARY KEY([Id], [Version]));";

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) =>
                    { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                    .Execute(() => conn.Execute(sql));
            }
        }

        public Task<ScheduledFlight> GetScheduledFlightAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public async Task SaveScheduledFlightAsync(int id, int originalVersion, int newVersion, IEnumerable<Event> newEvents)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Log.Error("id" + id + " original version " + originalVersion + " newVersion " + newVersion + " events " + newEvents.FirstOrDefault().ToString());
                // update eventstore
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    // store aggregate
                    int affectedRows = 0;
                    var aggregate = await conn
                        .QuerySingleOrDefaultAsync<Aggregate>(
                            "select * from FlightSchedule where Id = @Id",
                            new { Id = id },
                            transaction);
                    Log.Error("aggregate line 99 " + aggregate);
                    if (aggregate != null)
                    {
                        Log.Error("Updating existing aggregate");
                        // update existing aggregate
                        affectedRows = await conn.ExecuteAsync(
                            @"update FlightSchedule
                              set [CurrentVersion] = @NewVersion
                              where [Id] = @Id
                              and [CurrentVersion] = @CurrentVersion;",
                            new
                            {
                                Id = id,
                                NewVersion = newVersion,
                                CurrentVersion = originalVersion
                            },
                            transaction);
                    }
                    else
                    {
                        Log.Error("Insert new aggregate");
                        // insert new aggregate
                        affectedRows = await conn.ExecuteAsync(
                            "insert FlightSchedule ([Id], [CurrentVersion]) values (@Id, @CurrentVersion)",
                            new { Id = id, CurrentVersion = newVersion },
                            transaction);
                    }

                    // check concurrency
                    if (affectedRows == 0)
                    {
                        Log.Error("0 affected rows");
                        transaction.Rollback();
                        throw new ConcurrencyException();
                    }

                    // store events
                    int eventVersion = originalVersion;
                    foreach (var e in newEvents)
                    {
                        eventVersion++;
                        await conn.ExecuteAsync(
                            @"insert FlightScheduleEvent ([Id], [Version], [Timestamp], [MessageType], [EventData])
                              values (@Id, @NewVersion, @Timestamp, @MessageType,@EventData);",
                            new
                            {
                                Id = id,
                                NewVersion = eventVersion,
                                Timestamp = DateTime.Now,
                                MessageType = e.MessageType,
                                EventData = SerializeEventData(e)
                            }, transaction);
                    }

                    // commit
                    transaction.Commit();
                }
            }
        }
        private string SerializeEventData(Event eventData)
        {
            return JsonConvert.SerializeObject(eventData, _serializerSettings);
        }
    }
}
