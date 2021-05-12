using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Npgsql;

namespace WebApp.Controllers
{
    public class ImageRepository
    {
        private readonly NpgsqlConnection _connection;

        private const string ConnectionString =
            "Server=34.204.178.238;Port=5432;UserId=postgres;Password=erlemaido;Include Error Detail=true";

        public ImageRepository()
        {
            _connection = new NpgsqlConnection(ConnectionString);
            _connection.OpenAsync();
        }

        private NpgsqlConnection GetConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                return _connection;
            }

            var newConnection = new NpgsqlConnection(ConnectionString);
            newConnection.OpenAsync();
            return newConnection;
        }

        public byte[] GetPictureBytes(uint oid)
        {
            var currentConnection = GetConnection();
            var manager = new NpgsqlLargeObjectManager(currentConnection);

            byte[] image;
            using var transaction = currentConnection.BeginTransaction();
            using (var stream = manager.OpenRead(oid))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    image = ms.ToArray();
                }
            }

            transaction.Commit();

            return image;
        }

        public Task<uint> SaveFile(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyToAsync(ms);
            return SaveFile(ms.ToArray());
        }

        private async Task<uint> SaveFile(byte[] fileBytes)
        {
            var currentConnection = GetConnection();

            var manager = new NpgsqlLargeObjectManager(currentConnection);
            // Create a new empty file, returning the identifier to later access it
            uint oid;
            try
            {
                oid = await manager.CreateAsync(0);
            }
            catch (Exception e)
            {
                if (e is NullReferenceException)
                {
                }

                currentConnection = new NpgsqlConnection(ConnectionString);
                await currentConnection.OpenAsync();
                manager = new NpgsqlLargeObjectManager(currentConnection);
                oid = await manager.CreateAsync(0);
            }

            // Reading and writing Large Objects requires the use of a transaction
            await using var transaction = await currentConnection.BeginTransactionAsync();
            // Open the file for reading and writing
            await using (var stream = await manager.OpenReadWriteAsync(oid))
            {
                await stream.WriteAsync(fileBytes.AsMemory(0, fileBytes.Length));
                await stream.SeekAsync(0, System.IO.SeekOrigin.Begin);
            }

            // Save the changes to the object
            await transaction.CommitAsync();
            await currentConnection.CloseAsync();

            return oid;
        }
    }
}