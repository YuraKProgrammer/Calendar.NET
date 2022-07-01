using System;
using Calendar.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Calendar.SqlStorage
{
    public class EventsDbContext : DbContext
    {
        private readonly string _connectionString;

        public EventsDbContext()
        {
        }

        public EventsDbContext(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                if (string.IsNullOrEmpty(_connectionString))
                    options.UseSqlServer();
                else
                    options.UseSqlServer(_connectionString);
            }

            base.OnConfiguring(options);
        }

        public DbSet<EventRecord> Events { get; set; }
    }
}
