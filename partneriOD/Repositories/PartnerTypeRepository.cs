using Dapper;
using partneriOD.Interfaces;
using partneriOD.Models;
using System.Data.SqlClient;

namespace partneriOD.Repositories
{
    public class PartnerTypeRepository : IPartnerTypeRepository
    {

        private readonly string _connectionString;

        public PartnerTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<PartnerType>> GetAllPartnerTypesAsync()
        {
            var sql = GetPartnerTypeSql(false);
            var partnerTypes = await QueryPartnerTypesAsync(sql);
            return partnerTypes;
        }

        private string GetPartnerTypeSql(bool withWhereClause)
        {
            var sql = @"
                    SELECT * FROM partnerTypes";

            if (withWhereClause)
                sql += " WHERE Id = @Id";

            return sql;
        }

        private async Task<IEnumerable<PartnerType>> QueryPartnerTypesAsync(string sql, object? parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
//                var policyDictionary = new Dictionary<int, Policy>();

                var partnerTypes = await connection.QueryAsync<PartnerType>(
                    sql,
                    parameters);

                return partnerTypes;
            }
        }

    }
}

