using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;

namespace VinylStore.Catalog.InfrastructureSP
{
    public class ItemRepository // : IItemRepository
    {
        private readonly SqlConnection _sqlConnection;

        public ItemRepository(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        public async Task<IList<Item>> GetAsync()
        {
            var result = await _sqlConnection.QueryAsync<Item>("GetAllItems", commandType: CommandType.StoredProcedure);
            return result.AsList();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            return await _sqlConnection.ExecuteScalarAsync<Item>("GetAllItems", new {Id = id.ToString()},
                commandType: CommandType.StoredProcedure);
        }

        public Item Add(Item order)
        {
            var result =
                _sqlConnection.ExecuteScalar<Item>("InsertItem", order, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Item Update(Item item)
        {
            var result =
                _sqlConnection.ExecuteScalar<Item>("UpdateItem", item, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Item Delete(Item item)
        {
            throw new NotImplementedException();
        }
    }
}