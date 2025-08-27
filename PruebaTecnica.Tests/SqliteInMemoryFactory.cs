using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Data;

public sealed class SqliteInMemoryFactory : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;

    public SqliteInMemoryFactory()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var ctx = new AppDbContext(_options);
        ctx.Database.EnsureCreated();
    }

    public AppDbContext CreateContext() => new AppDbContext(_options);

    public void Dispose() => _connection.Dispose();
}
