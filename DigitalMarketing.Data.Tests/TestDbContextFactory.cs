using DigitalMarketing.DigitalMarketing.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.Tests
{

    /// <summary>
    /// یه Base Class مشترک برای راه اندازی SQLite
    /// (جلوگیری از تکرار کد)
    /// </summary>
    public class TestDbContextFactory : IDisposable
    {
        private readonly SqliteConnection _connection;
        public MyDbContext Context { get; }

        public TestDbContextFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new MyDbContext(options);
            Context.Database.EnsureCreated();
        }


        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
